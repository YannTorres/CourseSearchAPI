using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.Course;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace CourseSearch.Infrastructure.DataAcess.Repositories;
internal class CourseRepository : ICourseReadOnlyRepository, ICourseWriteOnlyRepository, ICourseUpdateOnlyRepository
{
    private readonly CourseSearchDbContext _dbcontext;
    private readonly string _connectionString;

    public CourseRepository(CourseSearchDbContext dbContext, IConfiguration configuration)
    {
        _dbcontext = dbContext;
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
   
    public async Task AddOrUpdateCourse(Course course)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();
        try
        {
            string? courseLevelsJson = null;
            if (course.CourseLevels != null && course.CourseLevels.Any())
            {
                courseLevelsJson = JsonSerializer.Serialize(course.CourseLevels);
            }

            // 1. Upsert do Course (por ExternalId)
            await connection.ExecuteAsync(@"
            MERGE INTO Courses AS target
            USING (VALUES (
                @ExternalId,
                @Title,
                @Description,
                @CourseUrl,
                @Author,
                @IconUrl,
                @DurationsInMinutes,
                @Locale,
                @Popularity,
                @UpdatedAt,
                @PlatformId,
                @CourseLevels
            )) AS source (
                ExternalId, Title, Description, CourseUrl, Author, IconUrl, DurationsInMinutes,
                Locale, Popularity, UpdatedAt, PlatformId, CourseLevels 
            )
            ON target.ExternalId = source.ExternalId
            WHEN MATCHED THEN
                UPDATE SET Title = source.Title,
                           Description = source.Description,
                           CourseUrl = source.CourseUrl,
                           Author = source.Author,
                           IconUrl = source.IconUrl,
                           DurationsInMinutes = source.DurationsInMinutes,
                           Locale = source.Locale,
                           Popularity = source.Popularity,
                           UpdatedAt = source.UpdatedAt,
                           PlatformId = source.PlatformId,
                           CourseLevels = source.CourseLevels
            WHEN NOT MATCHED THEN
                INSERT (Id, ExternalId, Title, Description, CourseUrl, Author, IconUrl, DurationsInMinutes,
                        Locale, Popularity, UpdatedAt, PlatformId, Active, CreatedAt, CourseLevels)
                VALUES (NEWID(), source.ExternalId, source.Title, source.Description, source.CourseUrl, 
                        source.Author, source.IconUrl, source.DurationsInMinutes, source.Locale,
                        source.Popularity, source.UpdatedAt, source.PlatformId, 1, SYSUTCDATETIME(), source.CourseLevels);
        ", new
            {
                course.ExternalId,
                course.Title,
                course.Description,
                course.CourseUrl,
                course.Author,
                course.IconUrl,
                course.DurationsInMinutes,
                course.Locale,
                course.Popularity,
                course.UpdatedAt,
                course.PlatformId,
                CourseLevels = courseLevelsJson // Passando a string serializada
            }, transaction);

            // Buscar o ID do curso (se já existia ou acabou de ser criado)
            var courseId = await connection.QuerySingleAsync<Guid>(
                "SELECT Id FROM Courses WHERE ExternalId = @ExternalId",
                new { course.ExternalId }, transaction);

            // 3. Tags (evita duplicadas por curso)
            if (course.Tags != null && course.Tags.Any())
            {
                foreach (var tag in course.Tags)
                {
                    await connection.ExecuteAsync(@"
                    IF NOT EXISTS (SELECT 1 FROM Tags WHERE Name = @Name AND CourseId = @CourseId)
                    BEGIN
                        INSERT INTO Tags (Name, CourseId) VALUES (@Name, @CourseId);
                    END
                ", new { tag.Name, CourseId = courseId }, transaction);
                }
            }

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }
    public async Task<Course?> GetByExternalIdAsync(string externalId)
    {
        return await _dbcontext.Courses
            .Include(c => c.Tags)
            .Include(c => c.Platform)
            .Include(c => c.Rating)
            .FirstOrDefaultAsync(c => c.ExternalId == externalId);
    }

    async Task<Course?> ICourseReadOnlyRepository.GetById(Guid id)
    {
        return await _dbcontext.Courses
            .Include(c => c.Tags)
            .Include(c => c.Rating)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    async Task<Course?> ICourseUpdateOnlyRepository.GetById(Guid id)
    {
        return await _dbcontext.Courses
            .Include(c => c.Rating)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public IQueryable<Course> GetAll()
    {
        return _dbcontext.Courses
            .Include(c => c.Tags)
            .Include(c => c.Platform)
            .Include(c => c.Rating)
            .AsNoTracking();
    }
    public void Update(Course course)
    {
        _dbcontext.Courses.Update(course);
    }

    public async Task<List<Course>?> FindCoursesByTopics(List<string> topics)
    {
        if (topics == null || topics.Count == 0)
        {
            return [];
        }

        var lowerTopics = topics.Select(t => t.ToLower()).ToList();
        var courseScores = new Dictionary<Guid, (Course Course, int Score)>();

        // Busca por Tags (Alta Prioridade, +10 pontos)
        var coursesByTag = await _dbcontext.Courses
            .Include(c => c.Tags)
            .Where(c => c.Tags.Any(t => lowerTopics.Contains(t.Name.ToLower())))
            .ToListAsync();

        foreach (var course in coursesByTag)
        {
            // Calcula quantas tags o curso tem em comum com a lista de tópicos
            int matchingTagsCount = course.Tags.Count(t => lowerTopics.Contains(t.Name.ToLower()));

            if (!courseScores.ContainsKey(course.Id))
            {
                courseScores[course.Id] = (course, 0);
            }
            courseScores[course.Id] = (course, courseScores[course.Id].Score + (10 * matchingTagsCount));
        }

        var searchTerms = string.Join(" ", topics);

        //var coursesByTitle = await _dbcontext.Courses
        //    .Where(c => EF.Functions.FreeText(c.Title, searchTerms))
        //    .Include(c => c.Tags)
        //    .ToListAsync();

        var coursesByTitle = await _dbcontext.Courses
            .Where(c => lowerTopics.Any(t => c.Title.ToLower().Contains(t)))
            .Include(c => c.Tags)
            .ToListAsync();

        foreach (var course in coursesByTitle)
        {
            if (!courseScores.ContainsKey(course.Id))
            {
                courseScores[course.Id] = (course, 3);
            }
            else
            {
                courseScores[course.Id] = (course, courseScores[course.Id].Score + 3);
            }
        }

        var relevantCourses = courseScores.Values
            .OrderByDescending(x => x.Score)
            .Select(x => x.Course)
            .Take(100)
            .ToList();

        return relevantCourses;
    }

    public async Task<List<Course>> GetCourseBySharedTags(List<string> tagNames, Guid excludeCourseId)
    {
        return await _dbcontext.Courses
                             .Include(c => c.Tags)
                             .Include(c => c.Platform)
                             .Include(c => c.Rating)
                             .Where(c => c.Id != excludeCourseId &&
                                         c.Tags.Any(t => tagNames.Contains(t.Name)))
                             .ToListAsync();
    }

    public async Task<UserCourseRating?> GetUserRating(Guid courseId, Guid userId)
    {
        return await _dbcontext.UserCourseRating.FirstOrDefaultAsync(ucr => ucr.CourseId == courseId && ucr.UserId == userId);
    }

    public async Task AddUserRating(UserCourseRating courseRating)
    {
        await _dbcontext.UserCourseRating.AddAsync(courseRating);
    }

    public void UpdateUserRating(UserCourseRating courseRating)
    {
        _dbcontext.UserCourseRating.Update(courseRating);
    }

    public void UpdateRating(Rating rating)
    {
        _dbcontext.Ratings.Update(rating);
    }

    public async Task AddRating(Rating rating)
    {
        await _dbcontext.Ratings.AddAsync(rating);
    }
}
