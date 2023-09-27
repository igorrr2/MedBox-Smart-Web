namespace CaixaINteligenteWeb.Models
{
    public class RemedioModel
    {
        public string IdUsuario { get; set; }

        public string Id { get; set; }

        public string NomeRemedio { get; set; }

        public string Descricao { get; set; }

        public DateTime DataHoraCadastro { get; set; }

        public string HorarioInicio { get; set; }

        public string HorarioProximoRemedio { get; set; }

        public int Frequencia { get; set; }

        public int Recipiente { get; set; }
    }
}
