using System.ComponentModel.DataAnnotations;

namespace SmartBillApi.DataTransferObject.DtoModel
{
    public class CategoryDto
    {
        [Required]
        public long CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Inactive { get; set; }
        public string Description { get; set; }
    }
}
