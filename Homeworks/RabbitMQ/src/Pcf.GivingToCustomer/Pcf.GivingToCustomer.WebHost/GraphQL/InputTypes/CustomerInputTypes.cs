using HotChocolate.Types;
using System;
using System.Collections.Generic;

namespace Pcf.GivingToCustomer.WebHost.GraphQL.InputTypes
{
    public class CreateCustomerInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Guid> PreferenceIds { get; set; }
    }

    public class UpdateCustomerInput
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Guid> PreferenceIds { get; set; }
    }

    public class DeleteCustomerInput
    {
        public Guid Id { get; set; }
    }

    // Типы для GraphQL схемы
    public class CreateCustomerInputType : InputObjectType<CreateCustomerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCustomerInput> descriptor)
        {
            descriptor.Field(i => i.FirstName)
                .Type<NonNullType<StringType>>()
                .Description("First name of the customer");

            descriptor.Field(i => i.LastName)
                .Type<NonNullType<StringType>>()
                .Description("Last name of the customer");

            descriptor.Field(i => i.Email)
                .Type<NonNullType<StringType>>()
                .Description("Email address of the customer");

            descriptor.Field(i => i.PreferenceIds)
                .Type<ListType<UuidType>>()
                .Description("List of preference IDs");
        }
    }

    public class UpdateCustomerInputType : InputObjectType<UpdateCustomerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateCustomerInput> descriptor)
        {
            descriptor.Field(i => i.Id)
                .Type<NonNullType<UuidType>>()
                .Description("ID of the customer to update");

            descriptor.Field(i => i.FirstName)
                .Type<StringType>()
                .Description("First name of the customer");

            descriptor.Field(i => i.LastName)
                .Type<StringType>()
                .Description("Last name of the customer");

            descriptor.Field(i => i.Email)
                .Type<StringType>()
                .Description("Email address of the customer");

            descriptor.Field(i => i.PreferenceIds)
                .Type<ListType<UuidType>>()
                .Description("List of preference IDs");
        }
    }

    public class DeleteCustomerInputType : InputObjectType<DeleteCustomerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteCustomerInput> descriptor)
        {
            descriptor.Field(i => i.Id)
                .Type<NonNullType<UuidType>>()
                .Description("ID of the customer to delete");
        }
    }
}