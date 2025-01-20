using SmartBillApi.DataTransferObject.Core;

namespace SmartBillApi.DataTransferObject.ViewModel
{
    public class UnitTypeViewModel : BaseEntityViewModel
    {
        public long UnitTypeID { get; set; }
        public string Name { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public int MeasurementQty { get; set; }
    }
}
