using System.ComponentModel.DataAnnotations;

namespace SmartBillApi.DataTransferObject.DtoModel
{
    public class UserEntryDto
    {
        public long UserID { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string UserFullName { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(maximumLength: 30)]
        public string EmployeeID { get; set; }
        public bool IsAdmin { get; set; }

        [StringLength(maximumLength: 100)]
        public string OfficeLoc { get; set; }

        public bool Inactive { get; set; }
    }
}
