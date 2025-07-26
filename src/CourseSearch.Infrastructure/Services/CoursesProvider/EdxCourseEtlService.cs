using CourseSearch.Domain.Dtos.EdxCourses;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Domain.Services.EdxCourses;
using CourseSearch.Domain.Services.EdxCourses.HttpClient;

namespace CourseSearch.Infrastructure.Services.EdxCourses;
public class EdxCourseEtlService : IEdxCourseEtlService
{
    private readonly IEdxApiClient _edxApiClient;
    private readonly ICourseReadOnlyRepository _courseReadOnlyRepository;
    private readonly ICourseWriteOnlyRepository _courseWriteOnlyRepository;
    private readonly ICourseUpdateOnlyRepository _courseUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private const int PlataformId = 1;

    public EdxCourseEtlService(
        IEdxApiClient edxApiClient,
        ICourseReadOnlyRepository courseReadOnlyRepository,
        ICourseWriteOnlyRepository courseWriteOnlyRepository,
        ICourseUpdateOnlyRepository courseUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _edxApiClient = edxApiClient;
        _courseWriteOnlyRepository = courseWriteOnlyRepository;
        _courseReadOnlyRepository = courseReadOnlyRepository;
        _courseUpdateOnlyRepository = courseUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task SynchronizeCourseFromDtoAsync(EdxCourseDto edxCourseDto)
    {
        // 2. TRANSFORM: Converter o DTO para nossa Entidade de Domínio
        var existingCourse = await _courseReadOnlyRepository.GetByExternalIdAsync(edxCourseDto.CourseId);

        if (existingCourse != null)
        {
            // Curso já existe, vamos atualizar
            //existingCourse.UpdateDetails(
            //    edxCourseDto.Name, edxCourseDto.Org, edxCourseDto.Number,
            //    edxCourseDto.Start, edxCourseDto.End, edxCourseDto.Pacing
            //);

            //if (edxCourseDto.Media?.Image != null)
            //{
            //    var image = new CourseImage(edxCourseDto.Media.Image.Small, edxCourseDto.Media.Image.Large, edxCourseDto.Media.Image.Raw);
            //    existingCourse.SetImage(image);
            //}

            _courseUpdateOnlyRepository.Update(existingCourse);
        }
        else
        {
            // Curso é novo, vamos criar
            var newCourse = new Course()
            {
                Id = Guid.NewGuid(),
                ExternalId = edxCourseDto.CourseId,
                Title = edxCourseDto.Name ?? "Título indisponível",
                Author = edxCourseDto.Org,
                Description = "",
                CourseUrl = $"https://courses.edx.org/courses/{edxCourseDto.CourseId}/about",
                UpdatedAt= DateTime.UtcNow,
                PlatformId = PlataformId,
            };

            //if (edxCourseDto.Media?.Image != null)
            //{
            //    var image = new CourseImage(edxCourseDto.Media.Image.Small, edxCourseDto.Media.Image.Large, edxCourseDto.Media.Image.Raw);
            //    newCourse.SetImage(image);
            //}

            await _courseWriteOnlyRepository.Add(newCourse);
        }

        // 3. LOAD: Persistir as mudanças
        await _unitOfWork.Commit();
    }
}
