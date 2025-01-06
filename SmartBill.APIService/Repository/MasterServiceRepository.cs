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
    }
}
