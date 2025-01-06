using SmartBillApi.DataTransferObject.ViewModel;

namespace SmartBill.APIService.Entities
{
    public class Category : BaseEntityViewModel
    {
        public long CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
