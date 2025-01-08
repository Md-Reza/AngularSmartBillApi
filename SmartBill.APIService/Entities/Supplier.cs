using SmartBillApi.DataTransferObject.ViewModel;

namespace SmartBill.APIService.Entities
{
    public class Supplier : BaseEntityViewModel
    {
        public long SupplierID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public int OpeningReceivable { get; set; }
        public int OpeningPayable { get; set; }
    }
}
