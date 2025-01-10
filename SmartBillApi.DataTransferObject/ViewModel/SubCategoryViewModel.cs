namespace SmartBillApi.DataTransferObject.ViewModel
{
    public class SubCategoryViewModel:BaseEntityViewModel
    {
        public long SubCategoryID { get; set; }
        public CategoryViewModel CategoryViewModel { get; set; }
        public long CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
