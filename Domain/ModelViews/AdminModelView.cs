using MinimalAPI.Domain.Enuns;

namespace MinimalAPI.Domain.ModelViews
{
    public record AdminModelView
    {
        public int Id {get; set;} = default!;
        public string Email {get; set;} = default!;
         public string Profile {get; set;} = default!;
    }
}