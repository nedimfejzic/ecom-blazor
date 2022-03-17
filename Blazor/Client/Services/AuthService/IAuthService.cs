namespace Blazor.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLoginDTO request);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePasswordDTO request);
    }
}
