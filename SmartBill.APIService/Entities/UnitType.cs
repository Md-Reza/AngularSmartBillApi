using SmartBillApi.DataTransferObject.Core;
using SmartBillApi.DataTransferObject.ViewModel;

namespace SmartBill.APIService.Entities
{
    public class UnitType : BaseEntityViewModel
    {
        public long UnitTypeID { get; set; }
        public string Name { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public int MeasurementQty { get; set; }
    }
}
