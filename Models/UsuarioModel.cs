namespace CaixaINteligenteWeb.Models
{
    public class UsuarioModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime DataHoraCadastro { get; set; }
        public string Id { get; set; }
        public bool IsAdministrador { get; set; }
        public string ChaveEsp32 { get; set; }
    }
}
