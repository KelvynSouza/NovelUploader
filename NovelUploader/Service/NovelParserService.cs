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
        public async Task<bool> Run(string path)
        {
            string novel;
            try
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    novel = await sr.ReadToEndAsync();
                }
                string pattern = @"^<.*\s\S*[^>]*> 끝$";

                // Regex rx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                Regex rx = new Regex(pattern, RegexOptions.Multiline, TimeSpan.FromHours(1));
                

                var matches = rx.Matches(novel);

                var array = matches.Count;

            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }
    }
}
