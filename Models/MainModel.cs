using System.Collections.Generic;

namespace Couchcase.Models
{
    public class MainModel
    {
        public int TotalDocuments { get; set; }
        public Dictionary<string, MagicDoc> MagicTen { get; set; }
        public string BucketName { get; set; }
        public IDictionary<string, string> Errors { get; set; }
    }
}