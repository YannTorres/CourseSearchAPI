using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Domain.Repositories.Roadmap;
using CourseSearch.Domain.Repositories.User;
using CourseSearch.Domain.Security.Cryptography;
using CourseSearch.Domain.Security.Tokens;
using CourseSearch.Domain.Services.CourseProvider;
using CourseSearch.Domain.Services.CourseProvider.HttpClient;
using CourseSearch.Domain.Services.IAModelService;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Infrastructure.DataAcess;
using CourseSearch.Infrastructure.DataAcess.Repositories;
using CourseSearch.Infrastructure.Security.Cryptography;
using CourseSearch.Infrastructure.Security.Tokens;
using CourseSearch.Infrastructure.Services.CoursesProvider;
using CourseSearch.Infrastructure.Services.CoursesProvider.HttpClient;
using CourseSearch.Infrastructure.Services.EdxCourses;
using CourseSearch.Infrastructure.Services.EdxCourses.HttpClient;
using CourseSearch.Infrastructure.Services.IAModelService;
using CourseSearch.Infrastructure.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseSearch.Infrastructure;
public static class DependencyInjectionExtension
{

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, PasswordEncripter>();
        services.AddScoped<ILoggedUser, LoggedUser>();

        services.AddScoped<ICourseProvider, EdxCourseProvider>();
        services.AddScoped<ICourseProvider, MicrosoftLearnCourseProvider>();
        services.AddScoped<ICourseProvider, AluraCourseProvider>();

        services.AddScoped<IAIModelService, GeminiModelService>();

        AddHttpClient(services);
        AddRepositories(services);
        AddToken(services, configuration);

        AddDbcContext(services, configuration);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        // User
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

        // Course
        services.AddScoped<ICourseReadOnlyRepository, CourseRepository>();
        services.AddScoped<ICourseUpdateOnlyRepository, CourseRepository>();
        services.AddScoped<ICourseWriteOnlyRepository, CourseRepository>();

        // Roadmap
        services.AddScoped<IRoadmapReadOnlyRepository, RoadmapRepository>();
        services.AddScoped<IRoadmapWriteOnlyRepository, RoadmapRepository>();
        services.AddScoped<IRoadmapUpdateOnlyRepository, RoadmapRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDbcContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<CourseSearchDbContext>(config => config.UseSqlServer(connectionString));
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAcessTokenGenerator>(config => new JwtTokenGenerator(signingKey!, expirationTimeMinutes));
    }

    private static void AddHttpClient(IServiceCollection services)
    {
        services.AddHttpClient<IEdxApiClient, EdxApiClient>(c =>
        {
            c.BaseAddress = new Uri("https://courses.edx.org");
        });

        services.AddHttpClient<IMicrosoftLearnApiClient, MicrosoftLearnApiClient>(c =>
        {
            c.BaseAddress = new Uri("https://learn.microsoft.com");
        });

        services.AddHttpClient<IAluraCourseApiClient, AluraCourseApiClient>(c =>
        {
            c.BaseAddress = new Uri("https://www.alura.com.br");
        });

        services.AddHttpClient<GeminiModelService>();
    }
}
