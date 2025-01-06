namespace SmartBill.APIService.Entities
{
    public class UserSecurity
    {
        public long UserSecurityID { get; set; }
        public long UserID { get; set; }
        public User User { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime LastChangedDate { get; set; }
    }
}
