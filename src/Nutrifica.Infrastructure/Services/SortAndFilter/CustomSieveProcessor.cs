#nullable disable
using Microsoft.Extensions.Options;

using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Models.Users;

using Sieve.Models;
using Sieve.Services;

namespace Nutrifica.Infrastructure.Services.SortAndFilter;

public class CustomSieveProcessor : SieveProcessor
{
    public CustomSieveProcessor(IOptions<SieveOptions> options) : base(options)
    {
    }

    // public CustomSieveProcessor(IOptions<SieveOptions> options, ISieveCustomSortMethods customSortMethods) : base(options, customSortMethods)
    // {
    // }
    //
    // public CustomSieveProcessor(IOptions<SieveOptions> options, ISieveCustomFilterMethods customFilterMethods) : base(options, customFilterMethods)
    // {
    // }
    //
    // public CustomSieveProcessor(IOptions<SieveOptions> options, ISieveCustomSortMethods customSortMethods, ISieveCustomFilterMethods customFilterMethods) : base(options, customSortMethods, customFilterMethods)
    // {
    // }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.Property<UserModel>(x => x.Email).CanFilter().CanSort();
        mapper.Property<UserModel>(x => x.Enabled).CanFilter().CanSort();
        mapper.Property<UserModel>(x => x.FirstName).CanFilter().CanSort();
        mapper.Property<UserModel>(x => x.LastName).CanFilter().CanSort();
        mapper.Property<UserModel>(x => x.MiddleName).CanFilter().CanSort();
        mapper.Property<UserModel>(x => x.PhoneNumber).CanFilter().CanSort();
        mapper.Property<UserModel>(x => x.Role).CanFilter().CanSort();
        mapper.Property<UserModel>(x => x.SupervisorId).CanFilter(); //.CanSort();
        mapper.Property<UserModel>(x => x.Username).CanFilter().CanSort();

        // mapper.Property<ClientModel>(x => x.FirstName).CanFilter().CanSort();
        // mapper.Property<ClientModel>(x => x.Address.Street).CanSort();
        // mapper.Property<ClientModel>(x => x.Address).HasName("address").CanSort();

        // mapper.Property<ClientModel>(x => x.FirstName).CanFilter().CanSort();
        // mapper.Property<ClientModel>(x => x.MiddleName).CanFilter().CanSort();
        // mapper.Property<ClientModel>(x => x.LastName).CanFilter().CanSort();
        mapper.Property<ClientModel>(x => x.PhoneNumber).CanFilter().CanSort();
        mapper.Property<ClientModel>(x => x.Comment).CanFilter().CanSort();
        mapper.Property<ClientModel>(x => x.State).CanFilter().CanSort();

        mapper.Property<PhoneCallModel>(x => x.CreatedOn).CanSort().CanFilter();

        return mapper;
    }
}