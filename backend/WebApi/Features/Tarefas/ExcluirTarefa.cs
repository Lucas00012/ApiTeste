using MediatR;
using System.Net;
using WebApi.Core.Messaging;
using WebApi.Infrastructure.Data;

namespace WebApi.Features.Tarefas
{
    public static class ExcluirTarefa
    {
        public class Command : IRequest<Result>
        {
            public int Id { get; set; }
        }

        public class Handler : BaseHandler<Command, Result>
        {
            private readonly ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public override async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var tarefa = await _dbContext.Tarefas
                    .FindAsync(request.Id);

                if (tarefa is null)
                {
                    AdicionarErro("Tarefa não encontrada");
                    return Error(HttpStatusCode.NotFound);
                }

                _dbContext.Tarefas.Remove(tarefa);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Success();
            }
        }
    }
}
