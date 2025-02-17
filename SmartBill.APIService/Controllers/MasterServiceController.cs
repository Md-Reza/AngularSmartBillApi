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
        [HttpGet("DownloadCategoryTemplate")]
        public IActionResult CategoryDownloadExcelAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Category_Upload_Template.xlsx");

            if (!System.IO.File.Exists(filePath))
            {
                return this.SBadRequest("File not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = "Category_Upload_Template.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost("SaveCategorie")]
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

        [HttpPost("SaveCategories")]
        public async Task<IActionResult> SaveCategorysAsync(List<CategoryDto> categoryDto)
        {
            if (!ModelState.IsValid)
                return this.SBadRequest(ModelState);
            string changedBy = this.GetDisplayName();
            if (categoryDto == null)
                return this.SBadRequest("No category data found to save");

            if (!categoryDto.Any())
                return this.SBadRequest($"Nothing found to save.");

            using (TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var item in categoryDto)
                {
                    var category = await masterServiceRepo.GetCategorieAsync(item.Name);
                    if (category != null)
                        item.CategoryID = category.CategoryID;

                    await masterServiceRepo.ExecuteCategoryAsync(item, changedBy);
                }
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

        #region Unit Type

        [HttpGet("UnitTypes")]
        public async Task<IActionResult> GetUnitTypesAsync()
        {
            try
            {
                var data = await masterServiceRepo.GetUnitTypesAsync();
                if (!data.Any())
                    this.SBadRequest(SMessageHandler.NoRecord());
                return this.SSuccess(mapper.Map<List<UnitTypeViewModel>>(data));
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }

        [HttpPost("SaveUnitType")]
        public async Task<IActionResult> SaveUnitTypeAsync(UnitTypeDto unitTypeDto)
        {
            if (!ModelState.IsValid)
                return this.SBadRequest(ModelState);
            string changedBy = this.GetDisplayName();
            if (unitTypeDto == null)
                return this.SBadRequest("No data found to save");

            var checkUnitType = await masterServiceRepo.GetUnitTypeAsync(unitTypeDto.Name);
            if (unitTypeDto != null)
                return this.SBadRequest($"Already Exists Unit Type: {unitTypeDto.Name}");

            using (TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled))
            {
                await masterServiceRepo.ExecuteUnitTypeAsync(unitTypeDto, changedBy);
                transaction.Complete();
            }
            return this.SSuccess(SMessageHandler.RecordSaved);
        }

        #endregion

        #region product 
        [HttpPost("SaveProduct")]
        public async Task<IActionResult> SaveProductAsync(ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return this.SBadRequest(ModelState);
            string changedBy = this.GetDisplayName();
            if (productDto == null)
                return this.SBadRequest("No product data found to save");

            if (productDto.PurchasePrice > productDto.SalePrice)
                return this.SBadRequest("Sale price should not be less than purchase cost.");

            var data =await masterServiceRepo.GetProductAsync(productDto.SKUID);
            if (data != null)
                return this.SBadRequest($"Already Exists this SKU: {productDto.SKUID}");

            using (TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled))
            {

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var uniqueFileName = $"{productDto.SKUID}_{productDto.ImageName}"; 

                var filePath = Path.Combine(folderPath, uniqueFileName);

                // Decode Base64 string to byte array
                var base64Data = productDto.ImagePath.Split(',')[1];
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await formFile.CopyToAsync(stream);
                //}

                productDto.ImagePath = filePath;
                productDto.ImageName = uniqueFileName;

                await masterServiceRepo.ExecuteProductAsync(productDto, changedBy);
                transaction.Complete();
            }
            return this.SSuccess(SMessageHandler.RecordSaved);
        }

        [HttpGet("Products")]
        public async Task<IActionResult> GetProductsAsync()
        {
            try
            {
                var data = await masterServiceRepo.GetProductsAsync();
                if (!data.Any())
                    this.SBadRequest(SMessageHandler.NoRecord());
                return this.SSuccess(mapper.Map<List<ProductViewModel>>(data));
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }

        [HttpGet("Barcode/{prefix}")]
        public async Task<IActionResult> GetBarcodeAsync(long prefix)
        {
            try
            {
                var data = await masterServiceRepo.GetBarcodeTagAsync(prefix);
                if (!data.Any())
                    this.SBadRequest(SMessageHandler.NoRecord());
                return Ok(data);
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }

        #endregion
    }
}
