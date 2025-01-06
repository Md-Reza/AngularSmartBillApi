using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartBill.APIService.Handlers;
using SmartBill.APIService.Interface;
using SmartBill.APIService.Security;
using SmartBillApi.DataTransferObject.DtoModel;
using SmartBillApi.DataTransferObject.ViewModel;
using System.Transactions;

namespace SmartBill.APIService.Controllers
{
    [Route("SBILL/[controller]")]
    [ApiController]
    public class UserServiceController(IMapper mapper, STokenService sTokenService, IUserRepo userRepo) : ControllerBase
    {
        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUserAsync(UserEntryDto userEntryDto)
        {
            if (!ModelState.IsValid)
                return this.SBadRequest(ModelState);

            // string changedBy = this.GetDisplayName();
            string changedBy = "Rezwan";

            if (userEntryDto.UserID <= 0)
            {
                //check by user name
                var userByUserName = await userRepo.GetUserAsync(userEntryDto.UserName);
                if (userByUserName != null)
                    return this.SBadRequest($"Username: {userEntryDto.UserName} already exists.");

                //check by Employee ID
                if (!string.IsNullOrEmpty(userEntryDto.EmployeeID))
                {
                    var userByEmployeeID = await userRepo.GetUserAsync(userEntryDto.EmployeeID);
                    if (userByEmployeeID != null)
                        return this.SBadRequest($"Employee ID: {userEntryDto.EmployeeID} already exists.");
                }
            }

            try
            {
                using (TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled))
                {
                    long userID = await userRepo.ExecuteUserAsync(userEntryDto, changedBy);

                    if (userEntryDto.UserID <= 0)
                    {
                        //user security
                        sTokenService.GetSecurityHash(userEntryDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        await userRepo.ExecuteUserSecurityAsync(userID, passwordHash, passwordSalt);
                    }

                    transaction.Complete();
                }
                return this.SSuccess(SMessageHandler.RecordSaved);
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            try
            {
                var users = await userRepo.GetUsersAsync();
                return !users.Any() ? this.SBadRequest(SMessageHandler.NoRecord()) : this.SSuccess(mapper.Map<List<UserViewModel>>(users));
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }
    }
}
