using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Entities;
using WebApi.Core.Enums.Tarefa;
using WebApi.Core.Messaging;
using WebApi.Core.Paginator;
using WebApi.Infrastructure.Data;
using System.Linq.Dynamic.Core;
using AutoMapper.QueryableExtensions;

namespace WebApi.Features.Tarefas
{
    public static class ObterTarefas
    {
        public class Command : QueryParams, IRequest<Result<QueryResponse<Response>>>
        {
            public string Pesquisa { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public StatusTarefa Status { get; set; }
        }

        public class Handler : BaseHandler<Command, Result<QueryResponse<Response>>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public override async Task<Result<QueryResponse<Response>>> Handle(Command request, CancellationToken cancellationToken)
            {
                var query = _dbContext.Tarefas
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Pesquisa))
                {
                    query = query
                        .Where(c => c.Descricao.Contains(request.Pesquisa)
                            || c.Nome.Contains(request.Pesquisa)
                            || c.Status.ToString().Contains(request.Pesquisa));
                }

                query = query.ApplyPagination(request, out var count);

                var response = query
                    .ProjectTo<Response>(_mapper.ConfigurationProvider);

                return Success(new QueryResponse<Response>
                {
                    Items = await response.ToListAsync(cancellationToken),
                    Count = count,
                    Page = request.Page.Value,
                    PageSize = request.PageSize
                });
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
