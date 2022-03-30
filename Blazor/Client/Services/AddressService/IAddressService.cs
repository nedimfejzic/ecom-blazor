namespace Blazor.Client.Services.AddressService
{
    public interface IAddressService
    {
        Task<Address> GetAddress();
        Task<Address> CreateAddress(Address address);
    }
}
