using CourseSearch.Domain.Services.EdxCourses;
using CourseSearch.Domain.Services.EdxCourses.HttpClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace CourseSearch.Infrastructure.Services.Worker;
public class CourseSyncWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CourseSyncWorker> _logger;
    private const int PageSize = 20; // Quantos cursos buscar por vez

    public CourseSyncWorker(IServiceProvider serviceProvider, ILogger<CourseSyncWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var techKeywords = new List<string>
        {
            // 1. Fundamental & General Concepts
            "Information Technology",
            "Computer Science",
            "Systems Analysis",
            "Programming Logic",
            "Algorithm",
            "Data Structures",
            "Code",
            "Coding",
            "IT",
            "Tech",
            "Computing",
            "Developer",
    
            // 2. Programming Languages
            "Programming",
            "Language",
            "Python",
            "Java",
            "JavaScript",
            "TypeScript",
            "C#",
            "C++",
            "C",
            "Go",
            "Golang",
            "Rust",
            "PHP",
            "Ruby",
            "Swift",
            "Kotlin",
            "Scala",
            "SQL",
            "R",
            "Shell Script",
            "Bash",
            "PowerShell",
            "Scripting",

            // 3. Web Development (Frontend & Backend)
            "Web Development",
            "Web",
            "Frontend",
            "Front-end",
            "Backend",
            "Back-end",
            "Fullstack",
            "Full-stack",
            "HTML",
            "CSS",
            "React",
            "Angular",
            "Vue",
            "Svelte",
            "Next.js",
            "Node.js",
            "NodeJS",
            "Express.js",
            "ASP.NET",
            ".NET",
            "Django",
            "Flask",
            "Ruby on Rails",
            "Laravel",
            "Spring Boot",
            "API",
            "REST",
            "RESTful",
            "GraphQL",
            "WebSockets",

            // 4. Data Science, AI & Machine Learning
            "Data Science",
            "Data Analysis",
            "Data Analyst",
            "Business Intelligence",
            "BI",
            "Artificial Intelligence",
            "AI",
            "Machine Learning",
            "ML",
            "Deep Learning",
            "Neural Networks",
            "Computer Vision",
            "Natural Language Processing",
            "NLP",
            "Big Data",
            "Pandas",
            "NumPy",
            "Scikit-learn",
            "TensorFlow",
            "PyTorch",
            "Keras",
            "Apache Spark",
            "Hadoop",
            "Power BI",
            "Tableau",
            "Data",

            // 5. Cloud Computing & DevOps
            "Cloud Computing",
            "Cloud",
            "AWS",
            "Amazon Web Services",
            "Azure",
            "Google Cloud",
            "GCP",
            "DevOps",
            "Infrastructure as Code",
            "IaC",
            "CI/CD",
            "Continuous Integration",
            "Continuous Delivery",
            "Continuous Deployment",
            "Docker",
            "Kubernetes",
            "K8s",
            "Ansible",
            "Terraform",
            "Jenkins",
            "Git",
            "GitHub",
            "GitLab",
            "Serverless",
            "Observability",
            "SRE",
            "Site Reliability Engineering",

            // 6. Databases
            "Database",
            "SQL",
            "NoSQL",
            "MySQL",
            "PostgreSQL",
            "Postgres",
            "SQL Server",
            "Oracle",
            "MongoDB",
            "Redis",
            "Cassandra",
            "Firebase",
            "DynamoDB",
            "SQLite",

            // 7. Cybersecurity
            "Cybersecurity",
            "Information Security",
            "InfoSec",
            "Ethical Hacking",
            "Penetration Testing",
            "Pentest",
            "Malware",
            "Firewall",
            "Cryptography",
            "Security",

            // 8. Mobile Development
            "Mobile Development",
            "Mobile",
            "iOS",
            "Android",
            "Swift",
            "Kotlin",
            "React Native",
            "Flutter",
            "Xamarin",
            "App Development",

            // 9. Software Engineering & Architecture
            "Software Engineering",
            "Software Architecture",
            "Design Patterns",
            "Agile",
            "Scrum",
            "Kanban",
            "Microservices",
            "Clean Code",
            "Clean Architecture",
            "SOLID",
            "TDD",
            "Test Driven Development",
            "BDD",
            "Software Testing",
            "QA",
            "Quality Assurance",

            // 10. Other Tech Areas
            "Game Development",
            "Gaming",
            "Unity",
            "Unreal Engine",
            "Internet of Things",
            "IoT",
            "Arduino",
            "Raspberry Pi",
            "Embedded Systems",
            "Blockchain",
            "Linux",
            "Networking",
            "Computer Networks",
        };

        var exclusionKeywords = new List<string>
        {
            "History", "Music", "Art", "Philosophy", "Literature", "Law", "Biology",
            "Medicine", "Health", "Marketing", "Business", "Management", "Finance",
            "Accounting", "Economics", "Gastronomy", "Cooking", "Writing", "Language Learning",
            "Psychology", "Sociology", "Ethics"
        };

        string pattern = @"\b(" + string.Join("|", techKeywords.Select(Regex.Escape)) + @")\b";

        var techRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);


        _logger.LogInformation("Worker de sincronização de cursos iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Iniciando ciclo de sincronização completa dos cursos...");

            // Criamos um "escopo" de serviços para cada ciclo de sincronização.
            // Isso é essencial porque o BackgroundService é um Singleton, mas o DbContext e outros serviços são Scoped.
            using (var scope = _serviceProvider.CreateScope())
            {
                var apiClient = scope.ServiceProvider.GetRequiredService<IEdxApiClient>();
                var etlService = scope.ServiceProvider.GetRequiredService<IEdxCourseEtlService>();

                var currentPage = 1;
                var hasNextPage = true;

                while (hasNextPage && !stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Buscando página {CurrentPage} de cursos...", currentPage);

                    var response = await apiClient.GetCoursesByPageAsync(currentPage, PageSize);

                    if (response?.Results == null || !response.Results.Any())
                    {
                        _logger.LogWarning("Não foram encontrados resultados na página {CurrentPage}. Encerrando ciclo.", currentPage);
                        hasNextPage = false;
                        continue;
                    }

                    var technologyCourses = response.Results.Where(course =>
                    {
                        if (string.IsNullOrEmpty(course.Name))
                        {
                            return false;
                        }

                        // Filtro Positivo: O nome do curso CORRESPONDE a uma de nossas palavras-chave de tecnologia como uma palavra inteira?
                        bool isTechMatch = techRegex.IsMatch(course.Name);

                        // Filtro Negativo: Se for um match de tecnologia, o nome NÃO PODE CONTER uma palavra da lista de exclusão.
                        bool isExcluded = exclusionKeywords.Any(keyword => course.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));

                        return isTechMatch && !isExcluded;

                    }).ToList();

                    _logger.LogInformation("Processando cursos da página {CurrentPage}.", currentPage);
                    foreach (var courseDto in technologyCourses)
                    {
                        try
                        {
                            await etlService.SynchronizeCourseFromDtoAsync(courseDto);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Falha ao sincronizar o curso {CourseId}: {ErrorMessage}", courseDto.CourseId, ex.Message);
                            // Continua para o próximo curso mesmo se um falhar
                        }
                    }

                    // Verifica se existe uma próxima página
                    hasNextPage = !string.IsNullOrEmpty(response.Pagination.Next);
                    currentPage++;

                    // Pequena pausa para não sobrecarregar a API da edX
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }

        _logger.LogInformation("Worker de sincronização de cursos finalizando.");
    }
}
