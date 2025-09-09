namespace CourseSearch.Domain.Extensions;
public static class IntExtensions
{
    public static string ToAvaliacoesString(this int count)
    {
        return count switch
        {
            0 => "Sem avaliações",
            1 => "1 avaliação",
            _ => $"{count} avaliações"
        };
    }
}
