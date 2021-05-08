using NovelUploader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovelUploader.Service
{
    public class NovelParserService
    {
        public async Task<IEnumerable<Novel>> Run(string path)
        {

            var novel = await ReadNovelFile(path);

            var chapters = GetChaptersParsed(novel);

            var novellist = ChapterToNovelModel(chapters);

            return novellist;
        }

        private async Task<string> ReadNovelFile(string path)
        {
            try
            {

                string novel;
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    novel = await sr.ReadToEndAsync();
                }
                return novel;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        private IEnumerable<string> GetChaptersParsed(string novel)
        {
            string pattern = @"^<.*\s\S*[^>]*> 끝";
            Regex rx = new Regex(pattern,
                RegexOptions.Compiled |
                RegexOptions.Multiline |
                RegexOptions.IgnoreCase |
                RegexOptions.CultureInvariant,
                TimeSpan.FromMinutes(5));

            var matches = rx.Matches(novel);

            var chapters = new List<string>();
            foreach (Match match in matches)
                chapters.Add(match.Value);

            return chapters;
        }

        private IEnumerable<Novel> ChapterToNovelModel(IEnumerable<string> chapters)
        {
            string pattern = @"^<.*\s\S*>";
            Regex rx = new Regex(pattern,
                RegexOptions.Compiled |
                RegexOptions.Multiline |
                RegexOptions.IgnoreCase |
                RegexOptions.CultureInvariant,
                TimeSpan.FromMinutes(5));

            List<Novel> novelList = new List<Novel>();
            int num = 1;
            foreach (var chapter in chapters)
            {
                var title = rx.Match(chapter);

                var novel = new Novel()
                {
                    CapNumber = num,
                    Text = chapter,
                    Title = title.Value
                };

                novelList.Add(novel);
                num++;
            }
            return novelList;
        }
    }
}
