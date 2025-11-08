using AutoMapper;
using GERTAR.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GERTAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly GERTARContext _context;

        public RelatorioController(GERTARContext context)
        {
            _context = context;
        }

        // GET: api/Relatorio
        [HttpGet("{IdUsuario}")]
        public async Task<ActionResult<List<HISTORICO>>> Get(int IdUsuario)
        {
            TB_USUARIO? Usuario = _context.TB_USUARIOS.Where(x => x.ID_USUARIO == IdUsuario).FirstOrDefault();
            if (Usuario == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return StatusCode((int)HttpStatusCode.BadRequest, new ProblemDetails { Title = "Usuário não identificado", Detail = "Usuário deve ser identificado." });
            }
            else
            {
                if (Usuario.PERFIL.ToUpper() != "GERENTE")
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    return StatusCode((int)HttpStatusCode.NotAcceptable, new ProblemDetails { Title = "Área gerencial", Detail = "Acesso negado." });
                }
            }
            string SQL = "SELECT * FROM VW_RELATORIO ORDER BY ID_PROJETO, ID_TAREFA, DT_ATUALIZACAO";
            var Historico = await _context.Database.SqlQueryRaw<HISTORICO>(SQL).ToListAsync();
            return Ok(Historico);
        }
    }
}
