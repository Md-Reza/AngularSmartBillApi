namespace SmartBillApi.DataTransferObject.ViewModel
{
    public sealed class LoginSessionViewModel
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string EmployeeID { get; set; }
        public string DisplayName { get; set; }
        public string AccessToken { get; set; }
    }
}
