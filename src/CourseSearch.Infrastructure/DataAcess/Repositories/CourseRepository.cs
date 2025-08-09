using CourseSearch.Domain.Dtos.IASuggestions;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.Course;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CourseSearch.Infrastructure.DataAcess.Repositories;
internal class CourseRepository : ICourseReadOnlyRepository, ICourseWriteOnlyRepository, ICourseUpdateOnlyRepository
{
    private readonly CourseSearchDbContext _dbcontext;

    public CourseRepository(CourseSearchDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    public async Task AddOrUpdateCourse(Course course, string plataformName)
    {
        var existingCourse = await GetByExternalIdAsync(course.ExternalId);

        if (existingCourse != null)
        {
            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            existingCourse.Author = course.Author;
            existingCourse.CourseUrl = course.CourseUrl;
            existingCourse.UpdatedAt = course.UpdatedAt;
            existingCourse.PlatformId = course.PlatformId;
            existingCourse.CourseLevels = course.CourseLevels;
            existingCourse.Locale = course.Locale;

            if (course.Rating != null)
            {
                if (existingCourse.Rating != null)
                {
                    existingCourse.Rating.Count = course.Rating.Count;
                    existingCourse.Rating.Average = course.Rating.Average;
                }
                else
                {
                    existingCourse.Rating = course.Rating;
                }
            }

            Update(existingCourse);
            return;
        }

        await _dbcontext.Courses.AddAsync(course);
    }
    public async Task<Course?> GetByExternalIdAsync(string externalId)
    {
        return await _dbcontext.Courses
            .FirstOrDefaultAsync(c => c.ExternalId == externalId);
    }

    async Task<Course?> ICourseReadOnlyRepository.GetById(Guid id)
    {
        return await _dbcontext.Courses
            .Include(c => c.Tags)
            .Include(c => c.Platform)
            .Include(c => c.Rating)
            .AsNoTracking()
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

        // Usamos um dicionário para armazenar os cursos encontrados e suas pontuações,
        // evitando duplicatas e facilitando a atribuição de pontos.
        var courseScores = new Dictionary<Guid, (Course Course, int Score)>();

        // --- PASSO 1: Busca por Tags (Alta Prioridade, +10 pontos) ---
        var coursesByTag = await _dbcontext.Courses
            .Include(c => c.Tags) // Inclui as tags para podermos filtrar por elas
            .Where(c => c.Tags.Any(t => topics.Contains(t.Name)))
            .ToListAsync();

        foreach (var course in coursesByTag)
        {
            // Calcula quantas tags o curso tem em comum com a lista de tópicos
            int matchingTagsCount = course.Tags.Count(t => topics.Contains(t.Name));

            // Adiciona ao dicionário com uma pontuação alta por cada tag correspondente
            if (!courseScores.ContainsKey(course.Id))
            {
                courseScores[course.Id] = (course, 0);
            }
            courseScores[course.Id] = (course, courseScores[course.Id].Score + (10 * matchingTagsCount));
        }

        // --- PASSO 2: Busca por Título (Média Prioridade, +3 pontos) ---
        // Construímos uma query dinâmica para buscar por múltiplos tópicos no título
        var queryBuilder = _dbcontext.Courses.AsQueryable();
        var predicate = PredicateBuilder.New<Course>(false); // Começa com 'false' para poder usar OR

        foreach (var topic in topics)
        {
            predicate = predicate.Or(c => c.Title.Contains(topic));
        }

        var coursesByTitle = await queryBuilder.Where(predicate).Include(c => c.Tags).ToListAsync();

        foreach (var course in coursesByTitle)
        {
            // Se o curso ainda não foi adicionado pela busca de tags, adicione-o com uma pontuação menor.
            if (!courseScores.ContainsKey(course.Id))
            {
                courseScores[course.Id] = (course, 3);
            }
            else // Se já existe, apenas adiciona os pontos extras por ter o título correspondente
            {
                courseScores[course.Id] = (course, courseScores[course.Id].Score + 3);
            }
        }

        // --- PASSO 3: Ordenar por pontuação e retornar a lista de cursos ---
        var relevantCourses = courseScores.Values
            .OrderByDescending(x => x.Score)
            .Select(x => x.Course)
            .Take(100)
            .ToList();

        return relevantCourses;
    }
}
