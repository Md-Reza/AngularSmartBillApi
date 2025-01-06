using Dapper;
using Microsoft.Data.SqlClient;
using SmartBill.APIService.Entities;
using SmartBill.APIService.Interface;
using SmartBill.APIService.Security;
using SmartBillApi.DataTransferObject.DtoModel;

namespace SmartBill.APIService.Repository
{
    public class UserRepository : IUserRepo
    {
        private SqlConnection sqlConnection { get; }
        public UserRepository() => sqlConnection = new SqlConnection(ConnectionExchanger.GetConnection());
        public void Dispose() => sqlConnection.Dispose();

        public async Task<User> GetUserAsync(string userNameIDEmpID)
        {
            var sql = @"SELECT * FROM System.[User] AS u
                    WHERE (u.UserName = @UserNameIDEmpID)
                    OR (CAST(u.UserID AS NVARCHAR(50)) = @UserNameIDEmpID)
                    OR (u.EmployeeID = @UserNameIDEmpID)";
            var data = await sqlConnection.QueryAsync<User>(sql, param: new { @UserNameIDEmpID = userNameIDEmpID }).ConfigureAwait(false);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var sql = @"SELECT * FROM System.[User]";
            var data = await sqlConnection.QueryAsync<User>(sql).ConfigureAwait(false);
            return data;
        }

        public async Task<UserSecurity> GetUserSecurityAsync(string userNameIDEmpID)
        {

            var sql = @"SELECT
	                    us.UserSecurityID
                       ,us.PasswordHash
                       ,us.PasswordSalt
                       ,us.LastChangedDate
                       ,us.UserID
                       ,u.UserName
                       ,u.EmployeeID
                       ,u.DisplayName
                       ,u.IsAdmin
                    FROM System.UserSecurity AS us
                    INNER JOIN System.[User] AS u
	                    ON us.UserID = u.UserID
                    WHERE (u.UserName = @UserNameIDEmpID)
                    OR (CAST(u.UserID AS NVARCHAR(50)) = @UserNameIDEmpID)
                    OR (u.EmployeeID = @UserNameIDEmpID)";
            var data = await sqlConnection.QueryAsync(sql,
              map: (UserSecurity us, User u) =>
              {
                  us.User = u;
                  return us;
              },
              splitOn: "UserID",
              param: new { @UserNameIDEmpID = userNameIDEmpID }).ConfigureAwait(false);
            sqlConnection.Close();
            return data.FirstOrDefault();
        }

        public async Task<long> ExecuteUserAsync(UserEntryDto userEntryDto, string changeBy)
        {
            string sql = @"IF EXISTS (SELECT
			                            *
		                            FROM System.[User]
		                            WHERE UserName = @UserName
		                            AND UserID = @UserID)
                            BEGIN

	                            UPDATE System.[User]
                                    SET UserFullName = @UserFullName
                                       ,DisplayName = @DisplayName
                                       ,EmailAddress = @EmailAddress
                                       ,OfficeLoc = @OfficeLoc
                                       ,Inactive = @Inactive
                                       ,ModifiedBy = @ChangedBy
                                       ,ModifiedDate = GETDATE()
                                    WHERE UserName = @UserName
                                    AND UserID = @UserID

	                            SELECT
		                            @UserID;
                            END
                            ELSE
                            BEGIN
	                            EXEC System.usp_GetID 201
						                             ,@UserID OUTPUT
	                            INSERT INTO System.[USER] (UserID, UserName, UserFullName, DisplayName, EmailAddress, EmployeeID, OfficeLoc, Inactive, CreatedBy)
                                VALUES (@UserID, @UserName, @UserFullName, @DisplayName, @EmailAddress, @EmployeeID, @OfficeLoc, @Inactive, @ChangedBy);

	                            SELECT
		                            @UserID;
                            END";
            long userID = await sqlConnection.ExecuteScalarAsync<long>(sql, param: new
            {
                @UserID = userEntryDto.UserID,
                @UserName = userEntryDto.UserName,
                @UserFullName = userEntryDto.UserFullName,
                @DisplayName = userEntryDto.DisplayName.ToUpper(),
                @EmailAddress = userEntryDto.EmailAddress,
                @EmployeeID = userEntryDto.EmployeeID,
                @OfficeLoc = userEntryDto.OfficeLoc,
                @Inactive = userEntryDto.Inactive,
                @ChangedBy = changeBy
            }).ConfigureAwait(false);
            sqlConnection.Close();
            return userID;
        }

        public async Task ExecuteUserSecurityAsync(long userID, byte[] passwordHash, byte[] passwordSalt)
        {
            string sql = @"DECLARE @UserSecurityID BIGINT
	                        EXEC System.usp_GetID 202
						                         ,@UserSecurityID OUTPUT

	                        INSERT INTO System.UserSecurity (UserSecurityID, UserID, PasswordHash, PasswordSalt)
		                        VALUES (@UserSecurityID, @UserID, @PasswordHash, @PasswordSalt);";
            await sqlConnection.ExecuteScalarAsync<long>(sql, param: new
            {
                @UserID = userID,
                @PasswordHash = passwordHash,
                @PasswordSalt = passwordSalt,
            }).ConfigureAwait(false);
            sqlConnection.Close();
        }
    }
}
