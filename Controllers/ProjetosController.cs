using GERTAR.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
#nullable enable
namespace GERTAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetosController : ControllerBase
    {
        private readonly GERTARContext _context;

        public ProjetosController(GERTARContext context)
        {
            _context = context;
        }

        // GET: api/Projetos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TB_PROJETO>>> GetProjetos()
        {
            return await _context.TB_PROJETOS.ToListAsync();
        }

        [HttpGet("{IdProjeto}")]
        public async Task<ActionResult<TB_PROJETO>> GetProjeto(int IdProjeto)
        {
            var tB_PROJETO = await _context.TB_PROJETOS.FindAsync(IdProjeto);

            if (tB_PROJETO == null)
            {
                return NotFound();
            }

            return tB_PROJETO;
        }

        [HttpPut("{IdProjeto}/{NmProjeto}")]
        public async Task<ActionResult<TB_PROJETO>> Put(int IdProjeto, string NmProjeto)
        {
            if (string.IsNullOrWhiteSpace(NmProjeto))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Nome não preenchido", Detail = "O projeto precisa de um nome." });
            }

            TB_PROJETO? Projeto = _context.TB_PROJETOS.Where(x => x.ID_PROJETO == IdProjeto).FirstOrDefault();
            if (Projeto != null)
            {
                try
                {
                    Projeto.NM_PROJETO = NmProjeto;
                    await _context.SaveChangesAsync();
                    return Ok(Projeto);
                }
                catch(DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                    {
                        return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Projeto existente", Detail = "Já existe um projeto com este nome." });
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Erro interno", Detail = "Um erro ocorreu. Contate o administrador" });
                    }
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("{NmProjeto}")]
        public async Task<ActionResult<TB_PROJETO>> Post(string NmProjeto)
        {
            if (string.IsNullOrWhiteSpace(NmProjeto))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Nome não preenchido", Detail = "O projeto precisa de um nome." });
            }
            TB_PROJETO NovoProjeto = new TB_PROJETO { NM_PROJETO = NmProjeto };
           
            _context.TB_PROJETOS.Add(NovoProjeto);
            try
            {
                await _context.SaveChangesAsync();
                TB_PROJETO ProjetoSalvo = _context.TB_PROJETOS.OrderByDescending(o => o.ID_PROJETO).First();
                return Ok(ProjetoSalvo);
            }
            catch(DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Projeto existente", Detail = "Já existe um projeto com este nome." });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Erro interno", Detail = "Um erro ocorreu. Contate o administrador" });
                }
            }
            

        }

        [HttpDelete("{IdProjeto}")]
        public async Task<IActionResult> DeleteTB_PROJETO(int IdProjeto)
        {
            var tB_PROJETO = await _context.TB_PROJETOS.FindAsync(IdProjeto);
            if (tB_PROJETO == null)
            {
                return NotFound();
            }
            var Pendencias = _context.TB_PROJETO_TAREFAS.Where(x => x.ID_PROJETO == IdProjeto
                                                               && x.STATUS_TAREFA == "pendente").ToList();
            if (Pendencias.Any())
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Ação não permitida", Detail = "É necessário concluir ou remover as tarefas pendentes para excluir este projeto" });
            }
            _context.TB_PROJETOS.Remove(tB_PROJETO);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
