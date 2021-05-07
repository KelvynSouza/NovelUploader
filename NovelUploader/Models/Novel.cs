using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovelUploader.Models
{
    public class Novel
    {
        public int Id { get; set; }
        public int CapNumber { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
