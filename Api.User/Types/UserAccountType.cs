using Api.Models;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.User.Types
{
    public class UserAccountType : ObjectType<UserAccount>
    {
        protected override void Configure(IObjectTypeDescriptor<UserAccount> descriptor)
        {
            descriptor.BindFieldsExplicitly();

            descriptor
                .Field(f => f.Id)
                .Type<UuidType>();

            descriptor
                .Field(f => f.FirstName)
                .Type<StringType>();

            descriptor
                .Field(f => f.LastName)
                .Type<StringType>();
        }
    }
}