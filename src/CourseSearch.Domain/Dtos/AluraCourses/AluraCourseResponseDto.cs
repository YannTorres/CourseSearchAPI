using CourseSearch.Domain.Dtos.MicrosoftLearningCourses;
using System.Text.Json.Serialization;

namespace CourseSearch.Domain.Dtos.AluraCourses;
public record AluraCourseResponseDTO(
    [property: JsonPropertyName("slug")] string Slug
);

public record AluraCourseDetailsDto(
    [property: JsonPropertyName("showable")] bool Showable,
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("nome")] string Nome,
    [property: JsonPropertyName("metadescription")] string? Metadescription,
    [property: JsonPropertyName("minutos_video")] int Duration,
    [property: JsonPropertyName("categorias")] IEnumerable<CategoriasAluraCourseDto> Categorias,
    [property: JsonPropertyName("subcategorias")] IEnumerable<SubCategoriasAluraCourseDto> SubCategorias,
    [property: JsonPropertyName("quantidade_aulas")] int Aulas,
    [property: JsonPropertyName("data_atualizacao")] DateTime DataAtualizacao,
    [property: JsonPropertyName("nota")] float Nota,
    [property: JsonPropertyName("quantidade_avaliacoes")] int QuantidadeAvaliacoes
);

public record CategoriasAluraCourseDto(
    [property: JsonPropertyName("nome")] string Nome,
    [property: JsonPropertyName("slug")] string Slug
);

public record SubCategoriasAluraCourseDto(
        [property: JsonPropertyName("nome")] string Nome,
    [property: JsonPropertyName("slug")] string Slug
);
