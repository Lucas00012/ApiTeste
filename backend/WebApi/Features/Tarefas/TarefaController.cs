using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Core.Controllers;
using WebApi.Core.Paginator;

namespace WebApi.Features.Tarefas
{
    [Route("api/tarefas")]
    public class TarefasController : MainController
    {
        /*
         GET - Recuperar informações
        PUT - Atualizar informações
        POST - Adicionar informações
        DELETE - Excluir informações
         */
        private readonly IMediator _mediator;

        public TarefasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(QueryResponse<ObterTarefas.Response>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTarefas(string pesquisa, string orderBy, OrderDirection direction, int? page, int? pageSize)
        {
            var result = await _mediator.Send(new ObterTarefas.Command
            {
                Pesquisa = pesquisa,
                Direction = direction,
                OrderBy = orderBy,
                Page = page,
                PageSize = pageSize
            });

            return CustomResponse(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ObterTarefa.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ObterTarefa(int id)
        {
            var result = await _mediator.Send(new ObterTarefa.Command
            {
                Id = id
            });

            return CustomResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ExcluirTarefa(int id)
        {
            var result = await _mediator.Send(new ExcluirTarefa.Command
            {
                Id = id
            });

            return CustomResponse(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AdicionarTarefa.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AdicionarTarefa(AdicionarTarefa.Command command)
        {
            var result = await _mediator.Send(command);
            return CustomResponse(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AtualizarTarefa.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AtualizarTarefa(int id, AtualizarTarefa.Command command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);
            return CustomResponse(result);
        }
    }
}
