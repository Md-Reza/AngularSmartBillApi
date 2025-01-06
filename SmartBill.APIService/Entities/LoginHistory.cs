namespace SmartBill.APIService.Entities
{
    public sealed class LoginHistory
    {
        public long LogID { get; set; }
        public long UserID { get; set; }
        public User User { get; set; }
        public string SessionKey { get; set; }
        public DateTime LoginDate { get; set; }
    }
}
