using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelUploader.DTO;
using NovelUploader.Models;
using NovelUploader.Service;

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
        public async Task<ActionResult<List<NovelTitlesDTO>>> GetNovelTitles()
        {
            var novels = await _context.Novels.ToListAsync();
            var titles = new List<NovelTitlesDTO>();
            novels.ForEach(_ =>
            {
                titles.Add(new NovelTitlesDTO()
                {
                    ChapterNumber = _.CapNumber,
                    Title = _.Title
                });
            });

            return titles;
        }

        // GET: api/Novel/5
        [HttpGet("{chapter}")]
        public async Task<ActionResult<Novel>> GetNovelChapter(int chapter)
        {
            var novelModel = await _context.Novels.FirstOrDefaultAsync(_=>_.CapNumber == chapter);

            if (novelModel == null)
            {
                return NotFound();
            }

            return novelModel;
        }

        // PUT: api/Novel/5        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNovelModel(int id, Novel novelModel)
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
        [HttpPost]
        public async Task<ActionResult<Novel>> PostNovelModel(Novel novelModel)
        {
            _context.Novels.Add(novelModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNovelModel", new { id = novelModel.Id }, novelModel);
        }

        // DELETE: api/Novel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Novel>> DeleteNovelModel(int id)
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

        
        [HttpPost]
        [Route("FillDatabase")]
        public async Task<ActionResult<IEnumerable<Novel>>> PostFillNovelDatabase([FromBody]string filepath)
        {
            var novelList = await new NovelParserService().Run(filepath);            

            await _context.Novels.AddRangeAsync(novelList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNovelModel", novelList);

        }



        private bool NovelModelExists(int id)
        {
            return _context.Novels.Any(e => e.Id == id);
        }
    }
}
