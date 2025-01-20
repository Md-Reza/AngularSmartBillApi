using System.ComponentModel.DataAnnotations;

namespace SmartBillApi.DataTransferObject.DtoModel
{
    public class SupplierDto
    {
        [Required]
        public long SupplierID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        public int OpeningReceivable { get; set; }
        public int OpeningPayable { get; set; }
        public bool Inactive { get; set; }
    }
}
