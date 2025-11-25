using System.Collections.Generic;
using System.Net.Mime;
using AgendaTelefonicaApi.Models;
using AgendaTelefonicaApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AgendaTelefonicaApi.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AgendaTelefonicaController : ControllerBase
    {
        private readonly IContatoRepository contatoRepository;
        public AgendaTelefonicaController(IContatoRepository contatoRepository)
            => this.contatoRepository = contatoRepository;

        /// <summary>
        /// Endpoint para retornar todos os contatos da agenda telefônica.
        /// </summary>
        /// <returns>
        /// Retorna uma lista com todos os contatos cadastrados no banco de dados.
        /// </returns>
        /// <response code="200">A requisição foi bem-sucedida e a resposta contém a lista de contatos.</response>
        [HttpGet("/api/contatos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contato>))]
        public IActionResult ObterTodosContatos()
        {
            return Ok(contatoRepository.ObterTodos());
        }

        /// <summary>
        /// Endpoint para consultar um contato pelo ID.
        /// </summary>
        /// <returns>
        /// Retorna o contato, caso ele exista, ou 404 caso ele não tenha sido encontrado.
        /// </returns>
        /// <param name="id">ID do contato a ser consultado.</param>
        /// <response code="200">O contato foi encontrado e seu conteúdo está disponível na resposta.</response>
        /// <response code="404">O contato não foi encontrado.</response>
        [HttpGet("/api/contatos/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contato))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ObterContatoPorId(int id)
        {
            var contato = contatoRepository.ObterPorId(id);
            if (contato == null)
                return NotFound();

            return Ok(contato);
        }

        /// <summary>
        /// Endpoint para criar um contato.
        /// </summary>
        /// <returns>
        /// Retorna 409 (Conflict) caso um contato com o ID especificado já exista,
        /// ou 201 (Created) caso a criação tenha sido bem-sucedida.
        /// </returns>
        /// <param name="contato">Os dados do contato a ser criado.</param>
        /// <response code="201">O contato foi criado com sucesso.</response>
        /// <response code="409">O contato não pode ser criado porque já existe outro contato com o mesmo ID.</response>
        [HttpPost("/api/contatos")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Contato))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult CriarContato([FromBody] Contato contato)
        {
            if (string.IsNullOrWhiteSpace(contato.Nome))
                return BadRequest();
            
            if (contatoRepository.ObterPorNome(contato.Nome) != null)
                return Conflict();
            
            contato = contatoRepository.Criar(contato);

            var url = Url.Action(
                action: nameof(ObterContatoPorId),
                controller: "AgendaTelefonica",
                values: new { id = contato.Id },
                protocol: Request.Scheme);

            return Created(url, contato);
        }

        /// <summary>
        /// Endpoint para atualizar um contato.
        /// </summary>
        /// <returns>
        /// Retorna 400 caso os ids sejam divergentes,
        /// 404 caso o contato com o ID especificado não tenha sido encontrado,
        /// ou 200 caso o contato tenha sido encontrado e atualizado com sucesso.
        /// </returns>
        /// <param name="contato">Os dados do contato a ser atualizado.</param>
        /// <param name="id">O ID do contato a ser atualizado.</param>
        /// <response code="200">O contato foi atualizado com sucesso.</response>
        /// <response code="400">Os IDs informados são divergentes.</response>
        /// <response code="404">O contato não foi encontrado.</response>
        [HttpPut("/api/contatos/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contato))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AtualizarContato([FromBody] Contato contato, int id)
        {
            if (id != contato.Id || string.IsNullOrWhiteSpace(contato.Nome))
                return BadRequest();
            
            var existingContato = contatoRepository.ObterPorId(contato.Id);
            if (existingContato == null)
                return NotFound();

            if (contatoRepository.ObterPorNome(contato.Nome) != null)
                return Conflict();
            
            contatoRepository.Atualizar(existingContato, contato);
            return Ok(existingContato);
        }

        /// <summary>
        /// Endpoint para remover um contato.
        /// </summary>
        /// <returns>
        /// Retorna 404 caso o contato com o ID especificado não tenha sido encontrado,
        /// ou 204 caso o contato tenha sido removido com sucesso.
        /// </returns>
        /// <param name="id">O ID do contato a ser removido.</param>
        /// <response code="404">O contato não foi encontrado.</response>
        /// <response code="204">O contato foi removido com sucesso.</response>
        [HttpDelete("/api/contatos/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoverContato(int id)
        {
            var existingContato = contatoRepository.ObterPorId(id);
            if (existingContato == null)
                return NotFound();
            
            contatoRepository.Remover(id);
            return NoContent();
        }

        /// <summary>
        /// Endpoint para retornar uma lista de contatos cujo nome contenha a string (nome) fornecida.
        /// </summary>
        /// <returns>
        /// Retorna uma lista com todos os contatos cujo nome contenha a string (nome) fornecida.
        /// </returns>
        /// <response code="200">A requisição foi bem-sucedida e a resposta contém a lista de contatos.</response>
        [HttpGet("/api/contatos/pesquisar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contato>))]
        public IActionResult BuscarContatoPorNome([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("O parâmetro de busca 'nome' é obrigatório.");

            return Ok(contatoRepository.BuscarPorNome(nome));
        }

    }
    
}
