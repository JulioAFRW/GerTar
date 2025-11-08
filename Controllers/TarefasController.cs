using GERTAR.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GERTAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly GERTARContext _context;

        public TarefasController(GERTARContext context)
        {
            _context = context;
        }

        [HttpGet("{IdProjeto}")]
        public async Task<ActionResult<IEnumerable<TB_PROJETO_TAREFA>>> Get(int IdProjeto)
        {
            TB_PROJETO ProjetoPai = _context.TB_PROJETOS.Where(x => x.ID_PROJETO == IdProjeto).First();
            TB_PROJETO_TAREFA NovoProjeto = new TB_PROJETO_TAREFA
            {
                ID_PROJETO = 5,
                ID_TAREFA = 1,
                TITULO = "Novo Projeto",
                DESCRICAO = "Teste Inclusão",
                PRIORIDADE = "Alta",
                VENCIMENTO = new DateTime(2025, 11, 10),
                COMENTARIO = "API Teste",
                STATUS_TAREFA = "Não iniciada",
                ID_USUARIO = 2
            };
            string jSonTarefa = JsonConvert.SerializeObject(NovoProjeto);
            return await _context.TB_PROJETO_TAREFAS.Where(x => x.ID_PROJETO == IdProjeto).ToListAsync();
        }

        [HttpGet("{IdProjeto}/{IdTarefa}")]
        public async Task<ActionResult<TB_PROJETO_TAREFA>> Get(int IdProjeto, int IdTarefa)
        {
            var tB_PROJETO_TAREFA = await _context.TB_PROJETO_TAREFAS.Where(x => x.ID_PROJETO == IdProjeto
                                                                            && x.ID_TAREFA == IdTarefa).FirstOrDefaultAsync();

            if (tB_PROJETO_TAREFA == null)
            {
                return NotFound();
            }

            return tB_PROJETO_TAREFA;
        }

        [HttpPut]
        public async Task<IActionResult> Put(TB_PROJETO_TAREFA tB_PROJETO_TAREFA)
        {
            TB_PROJETO_TAREFA? Tarefa = _context.TB_PROJETO_TAREFAS.Where(x => x.ID_PROJETO == tB_PROJETO_TAREFA.ID_PROJETO
                                                                          && x.ID_TAREFA == tB_PROJETO_TAREFA.ID_TAREFA).FirstOrDefault(); 
            if (Tarefa == null)
            {
                return NotFound();
            }
            else
            {
                if (Tarefa.PRIORIDADE != tB_PROJETO_TAREFA.PRIORIDADE)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Ação não permitida", Detail = "Não é permitido alterar a prioridade." });
                }
            }
            Tarefa.COMENTARIO = tB_PROJETO_TAREFA.COMENTARIO;
            Tarefa.DESCRICAO = tB_PROJETO_TAREFA.DESCRICAO;
            Tarefa.ID_USUARIO = tB_PROJETO_TAREFA.ID_USUARIO;
            Tarefa.DT_ATUALIZACAO = DateTime.Now;
            Tarefa.STATUS_TAREFA = tB_PROJETO_TAREFA.STATUS_TAREFA;
            Tarefa.TITULO = tB_PROJETO_TAREFA.TITULO;
            Tarefa.VENCIMENTO = tB_PROJETO_TAREFA.VENCIMENTO;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Erro interno", Detail = "Um erro ocorreu. Contate o administrador" });
            }

            return Ok(Tarefa);
        }

        [HttpPost]
        public async Task<ActionResult<TB_PROJETO_TAREFA>> Post(TB_PROJETO_TAREFA tB_PROJETO_TAREFA)
        {
            if (!(tB_PROJETO_TAREFA.PRIORIDADE.ToLower() == "alta" || tB_PROJETO_TAREFA.PRIORIDADE.ToLower() == "média") || (tB_PROJETO_TAREFA.PRIORIDADE.ToLower() == "baixa"))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Prioridade obrigatória", Detail = "A prioridade deve ser alta, média ou baixa." });          
            }
            if (_context.TB_PROJETO_TAREFAS.Where(x => x.ID_PROJETO == tB_PROJETO_TAREFA.ID_PROJETO).Count() == 20)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                return StatusCode((int)HttpStatusCode.NotAcceptable, new ProblemDetails { Title = "Limite tarefas", Detail = "O projeto já atingiu o limite de 20 tarefas." });
            }
            _context.TB_PROJETO_TAREFAS.Add(tB_PROJETO_TAREFA);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Erro interno", Detail = "Um erro ocorreu. Contate o administrador" });
            }

            return Ok(tB_PROJETO_TAREFA);
        }

        [HttpDelete("{IdProjeto}/{IdTarefa}")]
        public async Task<IActionResult> Delete(int IdProjeto, int IdTarefa)
        {
            var tB_PROJETO_TAREFA = await _context.TB_PROJETO_TAREFAS.Where(x => x.ID_PROJETO == IdProjeto
                                                                            && x.ID_TAREFA == IdTarefa).FirstOrDefaultAsync();
            if (tB_PROJETO_TAREFA == null)
            {
                return NotFound();
            }
            _context.TB_PROJETO_TAREFAS.Remove(tB_PROJETO_TAREFA);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
