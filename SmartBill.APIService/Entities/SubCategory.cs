using SmartBillApi.DataTransferObject.ViewModel;

namespace SmartBill.APIService.Entities
{
    public class SubCategory:BaseEntityViewModel
    {
        public long SubCategoryID { get; set; }
        public long CategoryID { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
