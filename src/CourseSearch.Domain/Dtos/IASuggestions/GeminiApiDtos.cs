namespace CourseSearch.Domain.Dtos.IASuggestions;
// --- Requisição ---
public record GeminiRequest(List<Content> Contents, GenerationConfig GenerationConfig);
public record Content(List<Part> Parts);
public record Part(string Text);
public record GenerationConfig(int CandidateCount, int MaxOutputTokens, decimal Temperature);

// --- Resposta ---
public record GeminiResponse(List<Candidate> Candidates);
public record Candidate(Content Content);
