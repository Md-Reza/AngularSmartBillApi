namespace SmartBill.APIService.Handlers
{
    public sealed class SMessageHandler
    {
        public static string InvalidUser { get; } = "Invalid username or password.";
        public static string RecordSaved { get; } = "The record has been saved.";
        public static string RecordDeleted { get; } = "The record has been deleted.";

        public static string NoRecord() => "No records were found.";
        public static string NoRecord(object param) => $"No records were found using {param}.";
        public static string InvalidRecord(string objectName, object param) => $"Invalid {objectName}:{param}.";
        public static string RecordAlreadyExist(string objectName, object param) => $"{objectName}:{param} already exist.";
    }
}
