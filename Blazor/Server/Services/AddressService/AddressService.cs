using Blazor.Server.Services.AuthService;
using Microsoft.EntityFrameworkCore;

namespace Blazor.Server.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;

        public AddressService(DataContext dataContext, IAuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
        {
            var response = new ServiceResponse<Address>();
            var addressInDb = (await GetAddress()).Data;
            if (addressInDb == null)
            {
                address.UserId = _authService.GetUserId();
                _dataContext.Addresses.Add(address);
                response.Data = address;
            }
            else
            {
                addressInDb.Street = address.Street;
                addressInDb.City = address.City;
                addressInDb.Country = address.Country;
                addressInDb.FirstName = address.FirstName;
                addressInDb.LastName = address.LastName;
                addressInDb.Zip = address.Zip;
                addressInDb.State = address.State;

                response.Data = addressInDb;
            }

            await _dataContext.SaveChangesAsync();
            return response;

        }

        public async Task<ServiceResponse<Address>> GetAddress()
        {
            int userId = _authService.GetUserId();

            var address = await _dataContext.Addresses.FirstOrDefaultAsync(c=>c.UserId == userId);

            return new ServiceResponse<Address> { Data = address };

        }
    }
}
