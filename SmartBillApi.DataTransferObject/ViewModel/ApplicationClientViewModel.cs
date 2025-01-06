namespace SmartBillApi.DataTransferObject.ViewModel
{
    public class ApplicationClientViewModel
    {
        public string DesktopClientID { get; set; }
        public string DesktopClientName { get; set; }
        public string DesktopClientVersion { get; set; }
        public bool IsDesktopClientEnabled { get; set; }
        public string MobileClientID { get; set; }
        public string MobileClientName { get; set; }
        public string MobileClientVersion { get; set; }
        public bool IsMobileClientEnabled { get; set; }
        public string WebClientID { get; set; }
        public string WebClientName { get; set; }
        public string WebClientVersion { get; set; }
        public bool IsWebClientEnabled { get; set; }
        public bool EnforceToCheckVersion { get; set; }
        public bool IsServiceEnabled { get; set; }
    }
}
