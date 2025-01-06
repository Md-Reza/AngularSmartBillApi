using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartBill.APIService.Handlers;
using SmartBill.APIService.Interface;
using SmartBillApi.DataTransferObject.DtoModel;
using SmartBillApi.DataTransferObject.ViewModel;
using System.Transactions;

namespace SmartBill.APIService.Controllers
{
    [Route("SBILL/[controller]")]
    [ApiController]
    public class MasterServiceController(IMapper mapper,IMasterServiceRepo masterServiceRepo) : ControllerBase
    {
        [HttpPost("SaveCategory")]
        public async Task<IActionResult> SaveCategory(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return this.SBadRequest(ModelState);
            // string changedBy = this.GetDisplayName();
            string changedBy = "Rezwan";
            if (categoryDto == null)
                return this.SBadRequest("No category data found to save");
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
                var container = await masterServiceRepo.GetCategoriesAsync();
                if (!container.Any())
                    this.SBadRequest(SMessageHandler.NoRecord());
                return this.SSuccess(mapper.Map<List<CategoryViewModel>>(container));
            }
            catch (Exception ex)
            {
                return this.SBadRequest(ex.Message);
            }
        }
    }
}
