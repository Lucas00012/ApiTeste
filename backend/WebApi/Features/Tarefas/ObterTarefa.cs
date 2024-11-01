using AutoMapper;
using MediatR;
using System.Net;
using WebApi.Core.Entities;
using WebApi.Core.Enums.Tarefa;
using WebApi.Core.Messaging;
using WebApi.Infrastructure.Data;

namespace WebApi.Features.Tarefas
{
    public static class ObterTarefa
    {
        public class Command : IRequest<Result<Response>>
        {
            public int Id { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public StatusTarefa Status { get; set; }
        }

        public class Handler : BaseHandler<Command, Result<Response>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public override async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var tarefa = await _dbContext.Tarefas
                    .FindAsync(request.Id);

                if (tarefa is null)
                {
                    AdicionarErro("Tarefa não encontrada");
                    return Error<Response>(HttpStatusCode.NotFound);
                }

                var response = _mapper.Map<Response>(tarefa);
                return Success(response);
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Tarefa, Response>();
            }
        }
    }
}
