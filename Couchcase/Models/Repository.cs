using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;

namespace Couchcase.Models
{
    public class Repository
    {
        private readonly IBucket _bucket;

        public Repository(IBucket bucket)
        {
            _bucket = bucket;
        }

        public int GetNumDocuments()
        {
            var n1ql = $"SELECT COUNT(1) AS numDocuments FROM `{_bucket.Name}`;";
            var query = QueryRequest.Create(n1ql);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            return _bucket.Query<dynamic>(query).Rows[0].numDocuments;
        }

        public void CreateMagic10()
        {
            for (int i = 0; i < 10; i++)
                _bucket.Upsert(new Document<dynamic> {
                    Id = "doc" + i,
                    Content = new {
                        Title = $"Document #{i}"
                    }
                });
        }

        public Dictionary<string, MagicDoc> GetMagicTen()
        {
            var dict = new Dictionary<string, MagicDoc>();
            for (int i = 0; i < 10; i++)
            {
                var key = "doc" + i;
                var magicDoc = new MagicDoc
                {
                    Active = _bucket.Get<dynamic>(key),
                    Replica = _bucket.GetFromReplica<dynamic>(key)
                };
                dict.Add(key, magicDoc);
            }
            return dict;
        }

        public void CreateArbitrary(int numDocumentsToCreate)
        {
            for (int i = 0; i < numDocumentsToCreate; i++)
                _bucket.Insert(new Document<dynamic> {
                    Id = Guid.NewGuid().ToString(),
                    Content = new {
                        CreatedAt = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(),
                        SomeRandomText = Path.GetRandomFileName(),
                        Type = "arbitrary"
                    }
                });
        }

        public void DeleteAllArbitrary()
        {
            var n1ql = $"DELETE FROM `{_bucket.Name}` WHERE type='arbitrary';";
            var query = QueryRequest.Create(n1ql);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            _bucket.Query<dynamic>(query);
            Thread.Sleep(1000); // hack because ScanConsistency doesn't seem to work with DELETE?
        }

        public Dictionary<string, string> UpdateMagic10()
        {
            var errorDict = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                var key = "doc" + i;
                var result = _bucket.Replace(new Document<dynamic>
                {
                    Id = key,
                    Content = new
                    {
                        Title = $"Document #{i}",
                        Updated = DateTime.Now
                    }
                });
                if(!result.Success)
                    errorDict.Add(key, result?.Exception?.Message);
            }
            return errorDict;
        }

        public void DeleteDocument(string id)
        {
            _bucket.Remove(id);
        }
    }
}