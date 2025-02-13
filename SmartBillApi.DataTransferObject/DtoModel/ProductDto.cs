using System.ComponentModel.DataAnnotations;

namespace SmartBillApi.DataTransferObject.DtoModel
{
    public class ProductDto
    {
        [Required]
        public long ProductID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SKUID { get; set; }
        [Required]
        public long SupplierID { get; set; }
        [Required]
        public long CategoryID { get; set; }
        public long SizeID { get; set; }
        [Required]
        public long UnitTypeID { get; set; }
        public int AlertQty { get; set; }
        [Required]
        public decimal PurchasePrice { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public bool Inactive { get; set; }
    }
}
