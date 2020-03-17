using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using couchbase_rest_api.Helpers;
using couchbase_rest_api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace couchbase_rest_api.Services
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        IEnumerable<User> GetAll();
    }
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>();
        private readonly AppSettings _appSettings;
        private IBucket _bucket;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _bucket = ClusterHelper.GetBucket("lawyermanagementdb");
        }

        public void GetAllUsers()
        {
            var n1ql = @"SELECT u.*, META(u).id
                FROM lawyermanagementdb u
                WHERE u.type = 'User';";
            var query = QueryRequest.Create(n1ql);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            var result = _bucket.Query<User>(query);

            foreach (User user in result)
            {
                _users.Add(user);
            }
        }

        public User Authenticate(string email, string password)
        {
            GetAllUsers();
            var user = _users.SingleOrDefault(x => x.Email == email && x.Password == password);

            if (user == null)
            {
                return null;
            }
            // authentication succesfull so generate token. 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();

        }

        public IEnumerable<User> GetAll()
        {
            GetAllUsers();
            return _users.WithoutPasswords();
        }
    }
}
