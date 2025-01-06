using Dapper;
using Microsoft.Data.SqlClient;
using SmartBill.APIService.Entities;
using SmartBill.APIService.Interface;
using SmartBill.APIService.Security;
using System.Data;

namespace SmartBill.APIService.Repository
{
    public sealed class HistoryRepository : IHistoryRepo
    {
        private SqlConnection sqlConnection { get; }
        public HistoryRepository() => sqlConnection = new SqlConnection(ConnectionExchanger.GetConnection());
        public void Dispose() => sqlConnection.Dispose();
        public async Task CreateLoginHistoryAsync(LoginHistory loginHistory)
        {
            string sql = @"INSERT History.LoginHistory ( UserID,  SessionKey)
	                        VALUES ( @UserID, @SessionKey)";
            await sqlConnection.ExecuteAsync(sql, param: new
            {
                @UserID = loginHistory.UserID,
                @SessionKey = loginHistory.SessionKey,
            });
            sqlConnection.Close();
        }
        public async Task DeleteUserHistoryAsync(long userID)
        {
            await sqlConnection.ExecuteAsync("System.usp_DeleteUserHistory",
                param: new { @UserID = userID },
                commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            sqlConnection.Close();
        }
    }
}
