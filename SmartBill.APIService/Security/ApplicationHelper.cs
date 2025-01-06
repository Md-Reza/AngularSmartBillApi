using System.Text.Json;

namespace SmartBill.APIService.Security
{
    public sealed class ApplicationHelper
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
        public string ServiceName { get; set; }
        public string ServiceVersion { get; set; }
        public bool IsServiceEnabled { get; set; }

        public void Serialize()
        {
            using FileStream fs = new("servicesettings.json", FileMode.Create);
            File.WriteAllText("servicesettings.json", JsonSerializer.Serialize(this));
        }
        public static ApplicationHelper Deserialize()
        {
            using FileStream fs = new("servicesettings.json", FileMode.Open);
            return JsonSerializer.Deserialize<ApplicationHelper>(fs);
        }
    }
}
