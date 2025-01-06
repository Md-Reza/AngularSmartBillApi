using System;
using System.ComponentModel.DataAnnotations;

namespace SmartBillApi.DataTransferObject.ViewModel
{
    public class BaseEntityViewModel
    {
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss tt}")]
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss tt}")]
        public DateTime? ModifiedDate { get; set; }
    }
}
