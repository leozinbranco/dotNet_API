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
    public class CursoController : Controller
        {
            private readonly EscolaContext _context;
        public CursoController(EscolaContext context)
        {   
            //constructor
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Curso>> GetAll(){
            return _context.Curso.ToList();
        }

        [HttpGet("{codCurso}")]
        public ActionResult<List<Curso>> Get(int id){
            try{
                var result = _context.Curso.Find(id);
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
        public async Task<ActionResult> post(Curso model){
            try{
                _context.Curso.Add(model);
                if(await _context.SaveChangesAsync() == 1)
                {
                    return Created($"/api/curso/{model.CodCurso}", model);
                }
            }
            catch{
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha na conexão com o banco de dados");
            }

            return BadRequest();
        }

        [HttpPut("{codCurso}")]
        public async Task<ActionResult> put(int id, Curso dadosCursoAlt){
            try{
                var result = await _context.Curso.FindAsync(id);
                if(id != result.Id)
                {
                    return BadRequest();
                }
                result.CodCurso = dadosCursoAlt.CodCurso;
                result.NomeCurso = dadosCursoAlt.NomeCurso;
                result.Periodo = dadosCursoAlt.Periodo;
                await _context.SaveChangesAsync();
                //return NoContent();
                return Created($"/api/curso/{dadosCursoAlt.CodCurso}", dadosCursoAlt);
            }
            catch{
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados");
            }
        }

        [HttpDelete("{codCurso}")]
        public async Task<ActionResult> delete(int id){
            try{
                var curso = await _context.Curso.FindAsync(id);
                System.Diagnostics.Debug.WriteLine(curso);
                if(curso == null)
                {
                    return NotFound();
                }
                _context.Remove(curso);
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