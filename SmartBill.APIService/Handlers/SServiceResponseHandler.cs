using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartBillApi.DataTransferObject.ViewModel;
using System.Security.Claims;

namespace SmartBill.APIService.Handlers
{
    public static class SServiceResponseHandler
    {
        public static BadRequestObjectResult SBadRequest(this ControllerBase controllerBase, string message)
        {
            if (string.IsNullOrEmpty(message) || message.Length <= 0)
                message = "Invalid exception message catch by SBILL Service";

            ResponseViewModel responseViewModel = new()
            {
                Message = message,
                StatusCode = StatusCodes.Status400BadRequest
            };
            return controllerBase.BadRequest(responseViewModel);
        }
        public static BadRequestObjectResult SBadRequest(this ControllerBase controllerBase, ModelStateDictionary modelState)
        {
            return controllerBase.BadRequest(modelState);
        }
        public static OkObjectResult SSuccess(this ControllerBase controllerBase, string message)
        {
            if (string.IsNullOrEmpty(message) || message.Length <= 0)
                message = "Invalid exception message catch by SBILL Service";

            ResponseViewModel responseViewModel = new()
            {
                Message = message,
                StatusCode = StatusCodes.Status200OK
            };
            return controllerBase.Ok(responseViewModel);
        }
        public static OkObjectResult SSuccess(this ControllerBase controllerBase, object value)
        {
            return controllerBase.Ok(value);
        }
        public static string GetDisplayName(this ControllerBase controllerBase)
        {
            return controllerBase.User.FindFirst(ClaimTypes.GivenName).Value.ToUpper();
        }
        public static long GetUserID(this ControllerBase controllerBase)
        {
            return Convert.ToInt64(controllerBase.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public static string GetEmployeeID(this ControllerBase controllerBase)
        {
            return controllerBase.User.FindFirst(x => x.Type == "sid").Value.ToString();
        }
        public static string GetUserPlantID(this ControllerBase controllerBase)
        {
            return controllerBase.User.FindFirst(x => x.Type == "sid").Value.ToString();
        }
    }
}
