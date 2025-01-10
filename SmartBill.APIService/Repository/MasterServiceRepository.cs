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

        public async Task<IEnumerable<Supplier>> GetAllSupplierAsync()
        {
            var sqlQuery = @"Select * FROM MasterData.Supplier";
            var data = await sqlConnection.QueryAsync<Supplier>(sqlQuery).ConfigureAwait(false);
            sqlConnection.Close();
            return data;
        }

        public async Task ExcuteSupplierAsync(SupplierDto supplier, string changedBy)
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

        public async Task<Category> GetCategorieAsync(string categoryIDName)
        {
            var sql = @"SELECT * FROM MasterData.Category
                    WHERE (Name = @CategoryIDName)
                    OR (CAST(CategoryID AS NVARCHAR(50)) = @CategoryIDName)";
            var data = await sqlConnection.QueryAsync<Category>(sql, param: new { @CategoryIDName = categoryIDName }).ConfigureAwait(false);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync()
        {
            var sqlQuery = @"Select * FROM MasterData.SubCategory";
            var data = await sqlConnection.QueryAsync<SubCategory>(sqlQuery).ConfigureAwait(false);
            sqlConnection.Close();
            return data;
        }

        public async Task ExecuteSubCategoryAsync(SubCategoryDto subCategoryDto, string createdBy)
        {
            await sqlConnection.ExecuteScalarAsync("MasterData.usp_SubCategory", new
            {
                @SubCategoryID = subCategoryDto.SubCategoryID,
                @CategoryID = subCategoryDto.CategoryID,
                @Name = subCategoryDto.Name,
                @Description = subCategoryDto.Description,
                @Inactive = subCategoryDto.Inactive,
                @CreatedBy = createdBy
            }, commandType: CommandType.StoredProcedure);
            sqlConnection.Close();
        }

        public async Task<SubCategory> GetSubCategorieAsync(string subCategoryIDName)
        {
            var sql = @"SELECT * FROM MasterData.SubCategory
                    WHERE (Name = @SubCategoryIDName)
                    OR (CAST(SubCategoryID AS NVARCHAR(50)) = @SubCategoryIDName)";
            var data = await sqlConnection.QueryAsync<SubCategory>(sql, param: new { @SubCategoryIDName = subCategoryIDName }).ConfigureAwait(false);
            return data.FirstOrDefault();
        }
    }
}
