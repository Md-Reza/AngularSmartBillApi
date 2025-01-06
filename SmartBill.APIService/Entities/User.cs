using SmartBillApi.DataTransferObject.ViewModel;

namespace SmartBill.APIService.Entities
{
    public class User : BaseEntityViewModel
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string EmployeeID { get; set; }
        public bool IsAdmin { get; set; }
        public string OfficeLoc { get; set; }
    }
}
