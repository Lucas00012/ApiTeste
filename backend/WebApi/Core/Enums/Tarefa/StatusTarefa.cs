using System.ComponentModel;

namespace WebApi.Core.Enums.Tarefa
{
    public enum StatusTarefa
    {
        [Description("Pendente")]
        Pendente,

        [Description("Concluída")]
        Concluida,

        [Description("Cancelada")]
        Cancelada
    }
}
