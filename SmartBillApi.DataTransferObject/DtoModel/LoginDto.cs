using System.ComponentModel.DataAnnotations;

namespace SmartBillApi.DataTransferObject.DtoModel
{
    public sealed class LoginDto
    {
        [Required(ErrorMessage = "Username or employee ID is required")]
        [StringLength(maximumLength: 20, MinimumLength = 4)]
        public string UserNameIDEmpID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(maximumLength: 20, MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
