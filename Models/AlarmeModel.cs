namespace CaixaINteligenteWeb.Models
{
    public class AlarmeModel
    {
        public bool Ativo { get; set; }
        public string Id { get; set; }
        public string nomeRemedio { get; set; }
        public string IdUsuario { get; set; }
        
        public string IdRemedio { get; set; }

        public DateTime DataHoraCadastro { get; set; }
    }
}
