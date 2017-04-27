using Couchbase;

namespace Couchcase.Models
{
    public class MagicDoc
    {
        public IOperationResult<dynamic> Active { get; set; }
        public IOperationResult<dynamic> Replica { get; set; }
    }
}