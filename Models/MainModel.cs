using System.Collections.Generic;

namespace Couchcase.Models
{
    public class MainModel
    {
        public Dictionary<string, MagicDoc> MagicTen { get; set; }
        public IDictionary<string, string> Errors { get; set; }
    }
}