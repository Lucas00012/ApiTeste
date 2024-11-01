using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WebApi.Core.Entities;
using WebApi.Core.Enums.Tarefa;
using WebApi.Core.Messaging;
using WebApi.Infrastructure.Data;

namespace WebApi.Features.Tarefas
{
    public static class AdicionarTarefa
    {
        public class Command : BaseRequest<Result<Response>>
        {
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public StatusTarefa Status { get; set; }

            public override bool EhValido()
            {
                ValidationResult = new CommandValidator().Validate(this);
                return ValidationResult.IsValid;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Nome)
                    .NotEmpty()
                    .WithMessage("O nome não pode estar vazio");

                RuleFor(c => c.Descricao)
                    .NotEmpty()
                    .WithMessage("A descrição não pode estar vazia");
            }
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
                if (!request.EhValido())
                {
                    AdicionarErros(request.ValidationResult);
                    return Error<Response>();
                }

                var jaExiste = await _dbContext.Tarefas
                    .AnyAsync(c => c.Nome == request.Nome, cancellationToken);

                if (jaExiste)
                {
                    AdicionarErro("Já existe uma tarefa com esse nome");
                    return Error<Response>(HttpStatusCode.Conflict);
                }

                var tarefa = new Tarefa
                {
                    Descricao = request.Descricao,
                    Nome = request.Nome,
                    Status = request.Status
                };

                await _dbContext.Tarefas.AddAsync(tarefa, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

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
