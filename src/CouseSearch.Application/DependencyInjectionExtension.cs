using CourseSearch.Application.UseCases.Course.GetAll;
using CourseSearch.Application.UseCases.Login;
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
    }
}
