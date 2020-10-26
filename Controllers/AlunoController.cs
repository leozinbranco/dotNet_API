using Microsoft.AspNetCore.Mvc;
using ProjetoEscola_API.Models;
using ProjetoEscola_API.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ProjetoEscola_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : Controller
        {
            private readonly EscolaContext _context;
        public AlunoController(EscolaContext context)
        {   
            //constructor
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<AlunoDotNet>> GetAll(){
            return _context.AlunoDotNet.ToList();
        }

        [HttpGet("{AlunoId}")]
        public ActionResult<List<AlunoDotNet>> Get(int AlunoId){
            try{
                var result = _context.AlunoDotNet.Find(AlunoId);
                if(result == null)
                {
                    return NotFound();
                }   
                return Ok(result);
            }
            catch{
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados");
            }
        }

        [HttpPost]
        public async Task<ActionResult> post(AlunoDotNet model){
            try{
                _context.AlunoDotNet.Add(model);
                if(await _context.SaveChangesAsync() == 1)
                {
                    return Created($"/api/aluno/{model.RA}", model);
                }
            }
            catch{
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha na conexão com o banco de dados");
            }

            return BadRequest();
        }

        [HttpPut("{AlunoId}")]
        public async Task<ActionResult> put(int AlunoId, AlunoDotNet dadosAlunoAlt){
            try{
                var result = await _context.AlunoDotNet.FindAsync(AlunoId);
                if(AlunoId != result.Id)
                {
                    return BadRequest();
                }
                result.RA = dadosAlunoAlt.RA;
                result.Nome = dadosAlunoAlt.Nome;
                result.codCurso = dadosAlunoAlt.codCurso;
                await _context.SaveChangesAsync();
                //return NoContent();
                return Created($"/api/aluno/{dadosAlunoAlt.RA}", dadosAlunoAlt);
            }
            catch{
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados");
            }
        }

        [HttpDelete("{AlunoId}")]
        public async Task<ActionResult> delete(int AlunoId){
            try{
                var aluno = await _context.AlunoDotNet.FindAsync(AlunoId);
                System.Diagnostics.Debug.WriteLine(aluno);
                if(aluno == null)
                {
                    return NotFound();
                }
                _context.Remove(aluno);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados. ");
            }
            return BadRequest();
        }
    }
}