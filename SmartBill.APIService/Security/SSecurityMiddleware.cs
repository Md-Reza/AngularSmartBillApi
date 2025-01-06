
using SmartBill.APIService.Interface;
using SmartBillApi.DataTransferObject.ViewModel;

namespace SmartBill.APIService.Security
{
    internal class SSecurityMiddleware(RequestDelegate requestDelegate)
    {
        public async Task Invoke(HttpContext httpContext, IUserRepo userRepo, STokenService sTokenService)
        {
            var requestPath = httpContext.Request.Path;
            var applicationHelper = ApplicationHelper.Deserialize();

            if (requestPath.Value == "/")
            {
                httpContext.Response.Redirect("/welcome", true);
                return;
            }

            if (requestPath.Value == "/welcome")
            {
                httpContext.Response.StatusCode = 200;
                await httpContext.Response.WriteAsync($"Welcome to {applicationHelper.ServiceName}{Environment.NewLine}");
                return;
            }

            //string clientID = httpContext.Request.Headers["ClientID"];
            //string clientVersion = httpContext.Request.Headers["ClientVersion"];
            string token = httpContext.Request.Headers.Authorization;
           // string deviceName = httpContext.Request.Headers["DeviceName"];

            //Check User
            if (!string.IsNullOrEmpty(token))
            {

                var claims = sTokenService.ParseClaimsFromJwtToken(token);
                var userName = claims.Find(x => x.Type == "name").Value;

                var user = await userRepo.GetUserAsync(userName);

                if (user == null)
                {
                    httpContext.Response.StatusCode = 400;
                    ResponseViewModel responseViewModel = new()
                    {
                        StatusCode = 400,
                        Message = $"Invalid credentials. Please login to the system."
                    };
                    await httpContext.Response.WriteAsJsonAsync(responseViewModel);
                    return;
                }

                var userSecurity = await userRepo.GetUserSecurityAsync(userName);
            }
            await requestDelegate.Invoke(httpContext);
        }
    }
}
