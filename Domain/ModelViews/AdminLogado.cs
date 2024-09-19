using MinimalAPI.Domain.Enuns;

namespace MinimalAPI.Domain.ModelViews
{
    public record AdminLogado
    {
        public string Email {get; set;} = default!;
        public string Profile {get; set;} = default!;
        public string Token {get; set;} = default!;
    }
}