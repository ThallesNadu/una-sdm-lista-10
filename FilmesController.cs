using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OscarFilmeApi.Data;
using OscarFilmeApi.Models;

namespace OscarFilmeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filme>>> GetFilmes()
        {
            return await _context.Filmes.ToListAsync();
        }

        [HttpGet("vencedores")]
        public async Task<ActionResult<IEnumerable<Filme>>> GetVencedores()
        {
            return await _context.Filmes.Where(f => f.Venceu).ToListAsync();
        }

        [HttpGet("estatisticas")]
        public async Task<ActionResult<object>> GetEstatisticas()
        {
            var total = await _context.Filmes.CountAsync();
            var vencedores = await _context.Filmes.CountAsync(f => f.Venceu);

            return new {
                TotalFilmes = total,
                TotalVencedores = vencedores
            };
        }

        [HttpPost]
        public async Task<ActionResult<Filme>> PostFilme(Filme filme)
        {
            if (filme.AnoLancamento < 1929)
            {
                return BadRequest("O ano de lançamento não pode ser menor que 1929 (ano do primeiro Oscar).");
            }

            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFilmes), new { id = filme.Id }, filme);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilme(int id, Filme filme)
        {
            if (id != filme.Id)
            {
                return BadRequest("O ID da URL não corresponde ao ID do corpo da requisição.");
            }

            _context.Entry(filme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                if (filme.Venceu)
                {
                    Console.WriteLine($" Temos um novo vencedor: {filme.Titulo}!");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilme(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
            {
                return NotFound();
            }

            _context.Filmes.Remove(filme);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FilmeExists(int id)
        {
            return _context.Filmes.Any(e => e.Id == id);
        }
    }
}