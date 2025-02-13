namespace SmartBillApi.DataTransferObject.ViewModel
{
    public class ProductViewModel : BaseEntityViewModel
    {
        public long ProductID { get; set; }
        public string Name { get; set; }
        public string SKUID { get; set; }
        public SupplierViewModel SupplierViewModel { get; set; }
        public long SupplierID { get; set; }
        public CategoryViewModel CategoryViewModel { get; set; }
        public long CategoryID { get; set; }
        public long SizeID { get; set; }
        public UnitTypeViewModel UnitTypeViewModel { get; set; }
        public long UnitTypeID { get; set; }
        public int AlertQty { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
    }
}
