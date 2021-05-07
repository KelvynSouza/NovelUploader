using NovelUploader.Service;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NovelUploader.Test
{
    public class RegexTests
    {
        

        [Test]
        public async Task EndingMaker()
        {
            var path = "D:\\Download\\Novel para ler\\Ending Maker\\Ending Maker(Complete+Epilogue).txt";
            var service = new NovelParserService();
            await service.Run(path);
            Assert.Pass();
        }
    }
}