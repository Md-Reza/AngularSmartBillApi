using SmartBill.APIService.Entities;
using System.Threading.Tasks;

namespace SmartBill.APIService.Interface
{
    public interface IHistoryRepo : IDisposable
    {
        Task CreateLoginHistoryAsync(LoginHistory loginHistory);
        Task DeleteUserHistoryAsync(long userID);
    }
}
