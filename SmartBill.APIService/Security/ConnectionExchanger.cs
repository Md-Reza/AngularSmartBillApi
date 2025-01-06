namespace SmartBill.APIService.Security
{
    public sealed class ConnectionExchanger
    {
        public static string GetConnection()
        {
            string server = Environment.MachineName;
            return @$"Server={server};Database=SBill;User Id=sa;Password=Crystal@123;TrustServerCertificate=True";
        }
    }
}
