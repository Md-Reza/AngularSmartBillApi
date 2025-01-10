using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBill.APIService.Handlers;
using SmartBill.APIService.Interface;
using SmartBillApi.DataTransferObject.DtoModel;
using SmartBillApi.DataTransferObject.ViewModel;
using System.Transactions;

namespace SmartBill.APIService.Controllers
{
    [Authorize]
    [Route("SBILL/[controller]")]
    [ApiController]
    public class MasterServiceController(IMapper mapper, IMasterServiceRepo masterServiceRepo) : ControllerBase
    {
        [HttpPost("SaveCategory")]
        public async Task<IActionResult> SaveCategoryAsync(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return this.SBadRequest(ModelState);
            string changedBy = this.GetDisplayName();
            if (categoryDto == null)
                return this.SBadRequest("No category data found to save");

            if (categoryDto.CategoryID == 0)
            {
                var category = await masterServiceRepo.GetCategorieAsync(categoryDto.Name);
                if (category != null)
                    return this.SBadRequest($"Already Exists category name {categoryDto.Name}");
            }

            using (TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled))
            {
                await masterServiceRepo.ExecuteCategoryAsync(categoryDto, changedBy);
                transaction.Complete();
            }
            return this.SSuccess(SMessageHandler.RecordSaved);
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            try
            {
                var data = await masterServiceRepo.GetCategoriesAsync();
                if (!data.Any())
                    this.SBadRequest(SMessageHandler.NoRecord());
                return this.SSuccess(mapper.Map<List<CategoryViewModel>>(data));
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }

        [HttpGet("Suppliers")]
        public async Task<IActionResult> GetSuppliersAsync()
        {
            try
            {
                var data = await masterServiceRepo.GetAllSupplierAsync();
                if (!data.Any())
                    this.SBadRequest(SMessageHandler.NoRecord());
                return this.SSuccess(mapper.Map<List<SupplierViewModel>>(data));
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }

        [HttpPost("SaveSupplier")]
        public async Task<IActionResult> SaveSupplierAsync(SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
                return this.SBadRequest(ModelState);
            string changedBy = this.GetDisplayName();
            if (supplierDto == null)
                return this.SBadRequest("No supplier data found to save");
            using (TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled))
            {
                await masterServiceRepo.ExcuteSupplierAsync(supplierDto, changedBy);
                transaction.Complete();
            }
            return this.SSuccess(SMessageHandler.RecordSaved);
        }

        [HttpPost("SaveSubCategory")]
        public async Task<IActionResult> SaveSubCategoryAsync(SubCategoryDto subCategoryDto)
        {
            if (!ModelState.IsValid)
                return this.SBadRequest(ModelState);
            string changedBy = this.GetDisplayName();
            if (subCategoryDto == null)
                return this.SBadRequest("No sub category data found to save");

            if (subCategoryDto.SubCategoryID == 0)
            {
                var data = await masterServiceRepo.GetSubCategorieAsync(subCategoryDto.Name);
                if (data != null)
                    return this.SBadRequest($"Already Exists sub category name {subCategoryDto.Name}");
            }

            using (TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled))
            {
                await masterServiceRepo.ExecuteSubCategoryAsync(subCategoryDto, changedBy);
                transaction.Complete();
            }
            return this.SSuccess(SMessageHandler.RecordSaved);
        }

        [HttpGet("SubCategories")]
        public async Task<IActionResult> GetSubCategoriesAsync()
        {
            try
            {
                var data = await masterServiceRepo.GetSubCategoriesAsync();
                if (!data.Any())
                    this.SBadRequest(SMessageHandler.NoRecord());
                return this.SSuccess(mapper.Map<List<SubCategoryViewModel>>(data));
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }
    }
}
