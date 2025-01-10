using SmartBill.APIService.Entities;
using SmartBillApi.DataTransferObject.DtoModel;

namespace SmartBill.APIService.Interface
{
    public interface IMasterServiceRepo : IDisposable
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task ExecuteCategoryAsync(CategoryDto categoryDto,string createdBy);
        Task<Category> GetCategorieAsync(string categoryIDName);
        Task<IEnumerable<Supplier>> GetAllSupplierAsync();
        Task ExcuteSupplierAsync(SupplierDto supplier, string changedBy);
        Task<IEnumerable<SubCategory>> GetSubCategoriesAsync();
        Task ExecuteSubCategoryAsync(SubCategoryDto subCategoryDto, string createdBy);
        Task<SubCategory> GetSubCategorieAsync(string subCategoryIDName);
    }
}
