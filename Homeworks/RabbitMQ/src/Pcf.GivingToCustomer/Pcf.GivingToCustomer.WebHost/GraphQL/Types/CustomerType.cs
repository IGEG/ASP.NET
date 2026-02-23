using HotChocolate.Types;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.WebHost.GraphQL.Types
{
    public class CustomerType : ObjectType<Customer>
    {
        protected override void Configure(IObjectTypeDescriptor<Customer> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(c => c.FirstName).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.LastName).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Email).Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Preferences)
                .Type<ListType<CustomerPreferenceType>>()
                .Name("preferences");

            descriptor.Field(c => c.PromoCodes)
                .Type<ListType<PromoCodeCustomerType>>()
                .Name("promoCodes");
        }
    }

    public class CustomerPreferenceType : ObjectType<CustomerPreference>
    {
        protected override void Configure(IObjectTypeDescriptor<CustomerPreference> descriptor)
        {
            descriptor.Field(cp => cp.PreferenceId).Type<NonNullType<UuidType>>();
            descriptor.Field(cp => cp.Preference).Type<PreferenceType>();
        }
    }

    public class PreferenceType : ObjectType<Preference>
    {
        protected override void Configure(IObjectTypeDescriptor<Preference> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(p => p.Name).Type<NonNullType<StringType>>();
        }
    }

    public class PromoCodeCustomerType : ObjectType<PromoCodeCustomer>
    {
        protected override void Configure(IObjectTypeDescriptor<PromoCodeCustomer> descriptor)
        {
            descriptor.Field(pc => pc.PromoCodeId).Type<NonNullType<UuidType>>();
            descriptor.Field(pc => pc.PromoCode).Type<PromoCodeType>();
        }
    }

    public class PromoCodeType : ObjectType<PromoCode>
    {
        protected override void Configure(IObjectTypeDescriptor<PromoCode> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(p => p.Code).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.ServiceInfo).Type<StringType>();
            descriptor.Field(p => p.BeginDate).Type<NonNullType<DateTimeType>>();
            descriptor.Field(p => p.EndDate).Type<NonNullType<DateTimeType>>();
            descriptor.Field(p => p.PartnerId).Type<UuidType>();
            descriptor.Field(p => p.Preference).Type<PreferenceType>();
        }
    }
}