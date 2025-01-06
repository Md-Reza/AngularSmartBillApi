using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartBill.APIService.Entities;
using SmartBill.APIService.Filters.Action;
using SmartBill.APIService.Handlers;
using SmartBill.APIService.Interface;
using SmartBill.APIService.Security;
using SmartBillApi.DataTransferObject.DtoModel;
using SmartBillApi.DataTransferObject.ViewModel;

namespace SmartBill.APIService.Controllers
{
    [Route("SBILL/[controller]")]
    [ApiController]
    public class AuthServiceController(IMapper mapper, IUserRepo userRepo, STokenService sTokenService,
        IHistoryRepo historyRepo) : ControllerBase
    {
        [HttpPost("Login")]
        [SModelValidation]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            try
            {
                //string clientID = this.Request.Headers["ClientID"];
                //string clientVersion = this.Request.Headers["ClientVersion"];
                //string sessionKey = this.Request.Headers.Authorization;
                //string deviceName = this.Request.Headers["DeviceName"];

                //var applicationHelper = ApplicationHelper.Deserialize();

                #region Check application is enabled or not and version

                //if (string.IsNullOrEmpty(clientID))
                //    return this.SBadRequest("Client ID not found. Please contact system support team.");

                //if (string.IsNullOrEmpty(clientVersion))
                //    return this.SBadRequest("Client version not found. Please contact system support team.");

                //if (clientID == applicationHelper.DesktopClientID)
                //{
                //    if (!applicationHelper.IsDesktopClientEnabled)
                //        return this.SBadRequest($"Client ID: {clientID} is stopped. Please try again back later.");

                //    //check version
                //    if (applicationHelper.EnforceToCheckVersion)
                //        if (clientVersion != applicationHelper.DesktopClientVersion)
                //            return this.SBadRequest($"Invalid client version. Current version is {applicationHelper.DesktopClientVersion}. Please update your system.");
                //}
                //else if (clientID == applicationHelper.MobileClientID)
                //{
                //    if (!applicationHelper.IsDesktopClientEnabled)
                //        return this.SBadRequest($"Client ID: {clientID} is stopped. Please try again back later.");

                //    //check version
                //    if (applicationHelper.EnforceToCheckVersion)
                //        if (clientVersion != applicationHelper.MobileClientVersion)
                //            return this.SBadRequest($"Invalid client version. Current version is {applicationHelper.MobileClientVersion}. Please update your system.");
                //}
                //else if (clientID == applicationHelper.WebClientID)
                //{
                //    if (!applicationHelper.IsWebClientEnabled)
                //        return this.SBadRequest($"Client ID: {clientID} is stopped. Please try again back later.");

                //    loginDto.IPAddress = httpContext.HttpContext.Connection.RemoteIpAddress.ToString();

                //    //check version
                //    if (applicationHelper.EnforceToCheckVersion)
                //        if (clientVersion != applicationHelper.WebClientVersion)
                //            return this.SBadRequest($"Invalid client version. Current version is {applicationHelper.WebClientVersion}. Please update your system.");
                //}
                //else
                //    return this.SBadRequest($"Invalid Client ID: {clientID}");

                #endregion

                //Check user exist or not 
                var user = await userRepo.GetUserAsync(loginDto.UserNameIDEmpID);
                if (user == null)
                    return this.SBadRequest(SMessageHandler.InvalidUser);

                //if (!user.IsAdmin)
                //    if (!applicationHelper.IsServiceEnabled)
                //        return this.SBadRequest("Smart BILL Service is stopped. Please try again back later.");

                //Check mobile app login access
                //if (clientID == applicationHelper.MobileClientID && !user.EnableAppLogin)
                //    if (!user.EnableAppLogin)
                //        return this.SBadRequest($"You have no permission to login mobile app.");

                // Check user activation state
                if (user.Inactive)
                    return this.SBadRequest("Your account has been deactivated. Please contact with system support team.");

                var userSecurity = await userRepo.GetUserSecurityAsync(user.UserName);
                if (userSecurity == null)
                    return this.SBadRequest(SMessageHandler.InvalidUser);

                if (!sTokenService.IsValidCredential(loginDto.Password, userSecurity))
                    return this.SBadRequest(SMessageHandler.InvalidUser);

                var loginSessionViewModel = mapper.Map<LoginSessionViewModel>(user);
                loginSessionViewModel.AccessToken = sTokenService.GenerateAccessToken(userSecurity);

                //Set user login history
                await historyRepo.CreateLoginHistoryAsync(new LoginHistory()
                {
                    UserID = user.UserID,
                    SessionKey = loginSessionViewModel.AccessToken,
                });

                //remove user history
                await historyRepo.DeleteUserHistoryAsync(user.UserID);

                return this.SSuccess(loginSessionViewModel);
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }
    }
}
