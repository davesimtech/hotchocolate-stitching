using Api.User.Queries;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.User.Types
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.BindFieldsExplicitly();

            descriptor
                .Field(f => f.GetUserAccount(default))
                .Type<UserAccountType>();
        }
    }
}
