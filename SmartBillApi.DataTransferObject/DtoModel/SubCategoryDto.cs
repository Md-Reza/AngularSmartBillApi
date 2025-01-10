using System.ComponentModel.DataAnnotations;

namespace SmartBillApi.DataTransferObject.DtoModel
{
    public class SubCategoryDto
    {
        [Required]
        public long SubCategoryID { get; set; }
        [Required]
        public long CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Inactive { get; set; }
    }
}
