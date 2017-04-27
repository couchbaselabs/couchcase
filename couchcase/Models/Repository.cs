using System;
using System.Collections.Generic;
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
            var n1ql = $"SELECT COUNT(1) AS numDocuments FROM `{_bucket.Name}` WHERE type = 'Magic';";
            var query = QueryRequest.Create(n1ql);
            query.Timeout(new TimeSpan(0, 0, 5));
            var queryResult =  _bucket.Query<dynamic>(query);
            return queryResult.Rows[0].numDocuments;
        }

        public void CreateMagic10()
        {
            for (int i = 0; i < 10; i++)
                _bucket.Upsert(new Document<dynamic> {
                    Id = "doc" + i,
                    Content = new {
                        Title = $"Document #{i}",
                        Type = "Magic"
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
                        Updated = GetDateTimeNowFormatted(),
                        Type = "Magic"
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

        public void TouchDocument(string id)
        {
            var doc = _bucket.Get<dynamic>(id).Value;
            doc.updated = GetDateTimeNowFormatted();
            _bucket.Replace(new Document<dynamic> {
                Id = id,
                Content = doc
            });
        }

        private string GetDateTimeNowFormatted()
        {
            return DateTime.Now.ToString("M/d/yy h:mm:ss.fff");
        }
    }
}