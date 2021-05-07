using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelUploader.Models;

namespace NovelUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NovelController : ControllerBase
    {
        private readonly NovelContext _context;

        public NovelController(NovelContext context)
        {
            _context = context;
        }

        // GET: api/Novel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NovelModel>>> GetNovels()
        {
            return await _context.Novels.ToListAsync();
        }

        // GET: api/Novel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NovelModel>> GetNovelModel(int id)
        {
            var novelModel = await _context.Novels.FindAsync(id);

            if (novelModel == null)
            {
                return NotFound();
            }

            return novelModel;
        }

        // PUT: api/Novel/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNovelModel(int id, NovelModel novelModel)
        {
            if (id != novelModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(novelModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NovelModelExists(id))
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

        // POST: api/Novel
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<NovelModel>> PostNovelModel(NovelModel novelModel)
        {
            _context.Novels.Add(novelModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNovelModel", new { id = novelModel.Id }, novelModel);
        }

        // DELETE: api/Novel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NovelModel>> DeleteNovelModel(int id)
        {
            var novelModel = await _context.Novels.FindAsync(id);
            if (novelModel == null)
            {
                return NotFound();
            }

            _context.Novels.Remove(novelModel);
            await _context.SaveChangesAsync();

            return novelModel;
        }

        private bool NovelModelExists(int id)
        {
            return _context.Novels.Any(e => e.Id == id);
        }
    }
}
