using CourseSearch.Communication.Requests.Courses;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Exception.ExceptionBase;

namespace CourseSearch.Application.UseCases.Course.AddRating;
public class AddRatingUseCase : IAddRatingUseCase
{
    private readonly ICourseReadOnlyRepository _courseReadOnlyRepository;
    private readonly ICourseUpdateOnlyRepository _courseUpdateOnlyRepository;
    private readonly ICourseWriteOnlyRepository _courseWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    public AddRatingUseCase(
        ICourseReadOnlyRepository courseReadOnlyRepository,
        ICourseUpdateOnlyRepository courseUpdateOnlyRepository,
        ICourseWriteOnlyRepository courseWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _courseReadOnlyRepository = courseReadOnlyRepository;
        _courseUpdateOnlyRepository = courseUpdateOnlyRepository;
        _courseWriteOnlyRepository = courseWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }
    public async Task Execute(Guid courseId, RequestAddRatingJson rating)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(rating);

        var course = await _courseUpdateOnlyRepository.GetById(courseId);
        
        if (course == null)
            throw new NotFoundException("Curso não encontrado");

        course.PlatformId = null;

        if (course.Rating == null)
        {
            course.Rating = new Rating
            {
                CourseId = course.Id,
                Count = 0,
                Average = 0,
            };

            await _courseWriteOnlyRepository.AddRating(course.Rating);

            await _unitOfWork.Commit();
        }

        var userRating = await _courseReadOnlyRepository.GetUserRating(courseId, loggedUser.Id);

        int oldScore = 0;
        if (userRating == null)
        {
            var newUserRating = new UserCourseRating
            {
                UserId = loggedUser.Id,
                CourseId = course.Id,
                Score = rating.Rating
            };

            await _courseWriteOnlyRepository.AddUserRating(newUserRating);
        }
        else
        {
            oldScore = userRating.Score;
            userRating.Score = rating.Rating;

            _courseUpdateOnlyRepository.UpdateUserRating(userRating);
        }

        UpdateAggregatedRating(course.Rating, oldScore, rating.Rating);

        _courseUpdateOnlyRepository.UpdateRating(course.Rating);

        await _unitOfWork.Commit();
    }
    private void Validate(RequestAddRatingJson rating)
    {
        var result = new AddRatingValidator().Validate(rating);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }

    private void UpdateAggregatedRating(Rating rating, int oldScore, int newScore)
    {
        float totalScore = rating.Average * rating.Count;

        if (oldScore == 0) 
        {
            rating.Count++;
            totalScore += newScore;
        }
        else 
        {
            totalScore = totalScore - oldScore + newScore;
        }

        rating.Average = rating.Count > 0 ? totalScore / rating.Count : 0;
    }
}
