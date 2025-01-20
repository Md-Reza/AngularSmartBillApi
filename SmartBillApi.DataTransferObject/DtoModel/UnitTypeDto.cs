using System.ComponentModel.DataAnnotations;

namespace SmartBillApi.DataTransferObject.DtoModel
{
    public class UnitTypeDto
    {
        public long UnitTypeID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string MeasurementUnit { get; set; }
        public int MeasurementQty { get; set; }
        public bool Inactive { get; set; }
    }
}
