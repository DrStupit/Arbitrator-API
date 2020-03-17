using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using couchbase_rest_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace couchbase_rest_api.Services
{
    public interface ICouncilService
    {
        IEnumerable<Council> GetAll();
        IEnumerable<Council> GetByUser(Guid userId);

    }
    public class CouncilService: ICouncilService
    {
        private List<Council> _councils = new List<Council>();
        private IBucket _bucket;

        public CouncilService()
        {
            _bucket = ClusterHelper.GetBucket("lawyermanagementdb");
        }

        public void GetAllCouncils()
        {
            var n1ql = @"SELECT c.*, META(c).id
                FROM lawyermanagementdb c
                WHERE c.type = 'Council';";
            var query = QueryRequest.Create(n1ql);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            var result = _bucket.Query<Council>(query);

            foreach (Council council in result)
            {
                _councils.Add(council);
            }
        }

        public void GetCouncilByUser(Guid userId)
        {
            var n1ql = @$"SELECT c.*, META(c).id
                FROM lawyermanagementdb c
                WHERE c.type = 'Council'AND c.userId = '{userId}';";
            var query = QueryRequest.Create(n1ql);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            var result = _bucket.Query<Council>(query);
            
            foreach (Council council in result)
            {
                _councils.Add(council);
            }

        }
        
        public IEnumerable<Council> GetAll()
        {
            GetAllCouncils();
            return _councils;
        }

        public IEnumerable<Council> GetByUser(Guid userId)
        {
            GetCouncilByUser(userId);
            return _councils;
        }
    }
}
