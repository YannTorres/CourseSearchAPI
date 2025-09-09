using CourseSearch.Application.UseCases.Course.AddRating;
using CourseSearch.Application.UseCases.Course.GetAll;
using CourseSearch.Application.UseCases.Course.GetById;
using CourseSearch.Application.UseCases.Course.Similar;
using CourseSearch.Application.UseCases.Login;
using CourseSearch.Application.UseCases.Roadmap.Create;
using CourseSearch.Application.UseCases.Roadmap.GetAll;
using CourseSearch.Application.UseCases.Roadmap.GetById;
using CourseSearch.Application.UseCases.Users.ChangePassword;
using CourseSearch.Application.UseCases.Users.Delete;
using CourseSearch.Application.UseCases.Users.GetProfile;
using CourseSearch.Application.UseCases.Users.Register;
using CourseSearch.Application.UseCases.Users.UpdateProfile;
using Microsoft.Extensions.DependencyInjection;

namespace CourseSearch.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        addUseCases(services);
    }
    private static void addUseCases(IServiceCollection services)
    {

        // User UseCases
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserProfileUseCase, UpdateUserProfileUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();

        // Courses UseCases
        services.AddScoped<IGetAllCoursesUseCase, GetAllCoursesUseCase>();
        services.AddScoped<IGetByIdCourseUseCase, GetByIdCourseUseCase>();
        services.AddScoped<IGetSimilarCoursesUseCase, GetSimilarCoursesUseCase>();
        services.AddScoped<IAddRatingUseCase, AddRatingUseCase>();

        // Roadmap UseCases
        services.AddScoped<ICreateRoadmapUseCase, CreateRoadmapUseCase>();
        services.AddScoped<IGetAllRoadmapUseCase, GetAllRoadmapUseCase>();
        services.AddScoped<IGetByIdRoadmapUseCase, GetByIdRoadmapUseCase>();

    }
}
