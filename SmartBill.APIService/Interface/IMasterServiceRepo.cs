using SmartBill.APIService.Entities;
using SmartBillApi.DataTransferObject.DtoModel;

namespace SmartBill.APIService.Interface
{
    public interface IMasterServiceRepo : IDisposable
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task ExecuteCategoryAsync(CategoryDto categoryDto,string createdBy);
        Task<IEnumerable<Supplier>> GetAllSupplierAsync();
        Task ExcuteSupplierAsync(SupplierDto supplier, string changedBy);
    }
}
