using Dapper;
using Microsoft.Data.SqlClient;
using SmartBill.APIService.Entities;
using SmartBill.APIService.Interface;
using SmartBill.APIService.Security;
using SmartBillApi.DataTransferObject.DtoModel;
using System.Data;

namespace SmartBill.APIService.Repository
{
    public class MasterServiceRepository : IMasterServiceRepo
    {
        private SqlConnection sqlConnection { get; }

        public MasterServiceRepository() => sqlConnection = new SqlConnection(ConnectionExchanger.GetConnection());
        public void Dispose() => sqlConnection.Dispose();

        public async Task ExecuteCategoryAsync(CategoryDto categoryDto, string createdBy)
        {
            await sqlConnection.ExecuteScalarAsync("MasterData.usp_Category", new
            {
                @CategoryID = categoryDto.CategoryID,
                @Name = categoryDto.Name,
                @Description = categoryDto.Description,
                @Inactive = categoryDto.Inactive,
                @CreatedBy = createdBy
            }, commandType: CommandType.StoredProcedure);
            sqlConnection.Close();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var sqlQuery = @"Select * FROM MasterData.Category";
            var data = await sqlConnection.QueryAsync<Category>(sqlQuery).ConfigureAwait(false);
            sqlConnection.Close();
            return data;
        }

        public async Task<IEnumerable<Supplier>> GetAllSupplier()
        {
            var sqlQuery = @"Select * FROM MasterData.Category";
            var data = await sqlConnection.QueryAsync<Supplier>(sqlQuery).ConfigureAwait(false);
            sqlConnection.Close();
            return data;
        }

        public async Task ExcuteSupplier(SupplierDto supplier, string changedBy)
        {
           await sqlConnection.ExecuteScalarAsync("MasterData.usp_Supplier", new
            {
                @SupplierID = supplier.SupplierID,
                @Name = supplier.Name,
                @Address = supplier.Address,
                @ContactNumber = supplier.ContactNumber,
                @OpeningReceivable = supplier.OpeningReceivable,
                @OpeningPayable = supplier.OpeningPayable,
                @Inactive = supplier.Inactive,
                @CreatedBy = changedBy
            }, commandType: CommandType.StoredProcedure);
            sqlConnection.Close();
        }
    }
}
