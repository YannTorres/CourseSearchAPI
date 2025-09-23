using CourseSearch.Communication.Responses.Courses;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Extensions;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Exception.ExceptionBase;
using System.Data.Entity;

namespace CourseSearch.Application.UseCases.Course.Similar;
public class GetSimilarCoursesUseCase : IGetSimilarCoursesUseCase
{
    private readonly ICourseReadOnlyRepository _courseRepository;
    public GetSimilarCoursesUseCase(ICourseReadOnlyRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }
    public async Task<ResponseSimilarCoursesJson> Execute(Guid id)
    {
        var activeCourse = await _courseRepository.GetById(id);

        if (activeCourse == null)
            return new ResponseSimilarCoursesJson();

        var tags = activeCourse.Tags
            .Select(tag => tag.Name).ToList();

        var courseList = await _courseRepository.GetCourseBySharedTags(tags, activeCourse.Id);

        var scores = new List<Tuple<Domain.Entities.Course, double>>();

        foreach (var course in courseList)
        {
            if (course.Id == activeCourse.Id)
                continue;

            double similaridadeCosseno = CalcularSimilaridadeCosseno(activeCourse.Tags, course.Tags);

            if (similaridadeCosseno > 0)
            {
                scores.Add(Tuple.Create(course, similaridadeCosseno));
            }
        }

        var recommendedCourses = scores.OrderByDescending(s => s.Item2)
                               .Take(5)
                               .Select(s => s.Item1)
                               .ToList();

        List<ResponseShortSimilarCourseJson> shortCourses = [];

        foreach (var curso in recommendedCourses)
        {
            var course = new ResponseShortSimilarCourseJson
            {
                Id = curso.Id,
                Title = curso.Title,
                RatingAverage = curso?.Rating?.Average.ToString("F1") ?? "N/A",
                RatingCount = (curso?.Rating?.Count ?? 0).ToAvaliacoesString()
            };

            shortCourses.Add(course);
        }

        if (shortCourses.Count == 0)
            throw new NotFoundException("Nenhum curso relacionado foi encontrado.");

        return new ResponseSimilarCoursesJson()
        {
            SimilarCourses = shortCourses,
        };
    }

    private static double CalcularSimilaridadeCosseno(ICollection<Tag> tagsA, ICollection<Tag> tagsB)
    {
        var colecaoTagsA = tagsA ?? new List<Tag>();
        var colecaoTagsB = tagsB ?? new List<Tag>();

        var nomesTagsA = colecaoTagsA
            .Select(tag => tag.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var nomesTagsB = colecaoTagsB
            .Select(tag => tag.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var vocabulario = nomesTagsA.Union(nomesTagsB).ToList();

        var vetorA = new List<int>();
        var vetorB = new List<int>();

        foreach (var nomeDaTag in vocabulario)
        {
            vetorA.Add(nomesTagsA.Contains(nomeDaTag) ? 1 : 0);
            vetorB.Add(nomesTagsB.Contains(nomeDaTag) ? 1 : 0);
        }

        double produtoEscalar = 0;
        double magnitudeA = 0;
        double magnitudeB = 0;

        for (int i = 0; i < vocabulario.Count; i++)
        {
            produtoEscalar += vetorA[i] * vetorB[i];
            magnitudeA += Math.Pow(vetorA[i], 2);
            magnitudeB += Math.Pow(vetorB[i], 2);
        }

        if (magnitudeA == 0 || magnitudeB == 0)
            return 0.0;

        magnitudeA = Math.Sqrt(magnitudeA);
        magnitudeB = Math.Sqrt(magnitudeB);

        return produtoEscalar / (magnitudeA * magnitudeB);
    }
}
