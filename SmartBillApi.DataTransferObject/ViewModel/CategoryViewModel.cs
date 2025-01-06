namespace SmartBillApi.DataTransferObject.ViewModel
{
    public class CategoryViewModel:BaseEntityViewModel
    {
        public long CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
