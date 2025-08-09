using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Domain.Services.CourseProvider;
using CourseSearch.Infrastructure.Services.CoursesProvider.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CourseSearch.Infrastructure.Services.Worker;
public class CourseSyncWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CourseSyncWorker> _logger;
    private readonly Regex _techRegex;
    private readonly FilterSettings _filterSettings;

    public CourseSyncWorker(
        IServiceProvider serviceProvider,
        ILogger<CourseSyncWorker> logger,
        IOptions<FilterSettings> filterSettings)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _filterSettings = filterSettings.Value;

        string pattern = @"\b(" + string.Join("|", _filterSettings.TechKeywords.Select(Regex.Escape)) + @")\b";
        _techRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker: Iniciando ciclo de sincronização...");


            using (var scope = _serviceProvider.CreateScope())
            {
                var providers = scope.ServiceProvider.GetRequiredService<IEnumerable<ICourseProvider>>();
                var courseSyncService = scope.ServiceProvider.GetRequiredService<ICourseWriteOnlyRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                IEnumerable<ICourseProvider> provideres = providers.Skip(2);

                foreach (var provider in provideres)
                {
                    _logger.LogInformation("Worker: Buscando cursos da plataforma: {PlatformName}", provider.PlatformName);

                    await foreach (var course in provider.FetchAllCoursesAsync(stoppingToken))
                    {
                        bool shouldSync = !provider.RequiresKeywordFiltering;

                        if (provider.RequiresKeywordFiltering)
                        {
                            bool isTechMatch = _techRegex.IsMatch(course.Title);
                            bool isExcluded = _filterSettings.ExclusionKeywords.Any(k => course.Title.Contains(k, StringComparison.OrdinalIgnoreCase));

                            shouldSync = isTechMatch && !isExcluded;
                        }

                        if (shouldSync)
                        {
                            await courseSyncService.AddOrUpdateCourse(course, provider.PlatformName);
                            await unitOfWork.Commit();
                        }
                    }
                }
            }

            _logger.LogInformation("Worker: Ciclo finalizado. Próxima execução em 24 horas.");
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
