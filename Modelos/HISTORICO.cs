namespace GERTAR.Modelos
{
    public class HISTORICO
    {
        public int ID_PROJETO { get; set; }

        public int ID_TAREFA { get; set; }

        public string TITULO { get; set; } = null!;

        public string? DESCRICAO { get; set; }

        public string PRIORIDADE { get; set; } = null!;

        public DateTime VENCIMENTO { get; set; }

        public string? COMENTARIO { get; set; }

        public string STATUS_TAREFA { get; set; } = null!;

        public DateTime DT_ATUALIZACAO { get; set; }

        public int ID_USUARIO { get; set; }

    }
}
