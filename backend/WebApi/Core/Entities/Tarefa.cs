using WebApi.Core.Enums.Tarefa;

namespace WebApi.Core.Entities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public StatusTarefa Status { get; set; }
    }
}
