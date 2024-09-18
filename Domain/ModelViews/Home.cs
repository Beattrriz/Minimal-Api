namespace MinimalAPI.Domain.ModelViews
{
    public struct Home
    {
        public string Message { get => "Bem-vindo à API de Veículos -Minimal API"; }
        //public string Doc { get => "/swagger"; }
        public string SwaggerUrl { get => "http://localhost:5285/swagger/index.html"; }
    }
}