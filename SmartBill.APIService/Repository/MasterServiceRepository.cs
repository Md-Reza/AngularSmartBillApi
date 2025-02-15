using AutoMapper;
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
            var sqlQuery = @"Select 
	                              sc.SubCategoryID
                                  ,sc.Name
                                  ,sc.Description
                                  ,sc.Inactive
                                  ,sc.CreatedBy
                                  ,sc.CreatedDate
                                  ,sc.ModifiedBy
                                  ,sc.ModifiedDate
	                              ,ct.CategoryID
	                              ,ct.Name
                            FROM MasterData.SubCategory AS sc
                            INNER JOIN MasterData.Category AS ct ON sc.CategoryID=ct.CategoryID";
            var data = await sqlConnection.QueryAsync(sqlQuery, map: (SubCategory sc, Category ct) =>
            {
                sc.Category = ct;
                return sc;
            },
                splitOn: "CategoryID").ConfigureAwait(false);
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

        public async Task<IEnumerable<UnitType>> GetUnitTypesAsync()
        {
            var sqlQuery = @"Select * FROM MasterData.UnitType";
            var data = await sqlConnection.QueryAsync<UnitType>(sqlQuery).ConfigureAwait(false);
            sqlConnection.Close();
            return data;
        }

        public async Task<UnitType> GetUnitTypeAsync(string unitTypeIDName)
        {
            var sql = @"SELECT * FROM MasterData.UnitType
                    WHERE (Name = @UnitTypeIDName)
                    OR (CAST(UnitTypeID AS NVARCHAR(50)) = @UnitTypeIDName)";
            var data = await sqlConnection.QueryAsync<UnitType>(sql, param: new { @UnitTypeIDName = unitTypeIDName }).ConfigureAwait(false);
            return data.FirstOrDefault();
        }

        public async Task ExecuteUnitTypeAsync(UnitTypeDto unitTypeDto, string createdBy)
        {
            await sqlConnection.ExecuteScalarAsync("MasterData.usp_UnitType", new
            {
                @UnitTypeID = unitTypeDto.UnitTypeID,
                @Name = unitTypeDto.Name,
                @MeasurementUnit = unitTypeDto.MeasurementUnit,
                @MeasurementQty = unitTypeDto.MeasurementQty,
                @Inactive = unitTypeDto.Inactive,
                @CreatedBy = createdBy
            }, commandType: CommandType.StoredProcedure);
            sqlConnection.Close();
        }

        public async Task ExecuteProductAsync(ProductDto productDto, string createdBy)
        {
            await sqlConnection.ExecuteScalarAsync("MasterData.usp_Product", new
            {
                @ProductID = productDto.ProductID,
                @Name = productDto.Name,
                @SKUID = productDto.SKUID,
                @SupplierID = productDto.SupplierID,
                @CategoryID = productDto.CategoryID,
                @SizeID = productDto.SizeID,
                @UnitTypeID = productDto.UnitTypeID,
                @AlertQty = productDto.AlertQty,
                @PurchasePrice = productDto.PurchasePrice,
                @SalePrice = productDto.SalePrice,
                @Description = productDto.Description,
                @ImagePath = productDto.ImagePath,
                @ImageName = productDto.ImageName,
                @Inactive = productDto.Inactive,
                @CreatedBy = createdBy
            }, commandType: CommandType.StoredProcedure);
            sqlConnection.Close();
        }

        public async Task<Product> GetProductAsync(string productCode)
        {
            var sql = @"Select *
                        from MasterData.Product
                         WHERE (ProductID = @ProductID)
                         OR (CAST(SKUID AS NVARCHAR(50)) = @ProductID)";
            var data = await sqlConnection.QueryAsync<Product>(sql, param: new { @ProductID = productCode }).ConfigureAwait(false);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var sqlQuery = @"Select * FROM MasterData.Product";
            var data = await sqlConnection.QueryAsync<Product>(sqlQuery).ConfigureAwait(false);
            sqlConnection.Close();
            return data;
        }

        public async Task<string> GetBarcodeTagAsync(long prefixID)
        {
            string sqlQuery = @"DECLARE @PrefixID BIGINT
                                EXEC MasterData.usp_GetAutoID 206
				                                  ,@PrefixID OUTPUT
                            SELECT @PrefixID";
            string barcodeTag =await sqlConnection.ExecuteScalarAsync<string>(sqlQuery);
            sqlConnection.Close();
            return barcodeTag;
        }
    }
}
