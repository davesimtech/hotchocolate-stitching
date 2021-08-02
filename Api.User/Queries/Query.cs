using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.User.Queries
{
    public class Query
    {
        public UserAccount GetUserAccount(
            Guid id)
        {
            return new UserAccount()
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User"
            };
        }
    }
}
