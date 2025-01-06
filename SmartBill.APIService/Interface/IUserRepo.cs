using SmartBill.APIService.Entities;
using SmartBillApi.DataTransferObject.DtoModel;

namespace SmartBill.APIService.Interface
{
    public interface IUserRepo:IDisposable
    {
        Task<User> GetUserAsync(string userNameIDEmpID);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<UserSecurity> GetUserSecurityAsync(string userNameIDEmpID);
        Task<long> ExecuteUserAsync(UserEntryDto userEntryDto, string changeBy);
        Task ExecuteUserSecurityAsync(long userID, byte[] passwordHash, byte[] passwordSalt);
    }
}
