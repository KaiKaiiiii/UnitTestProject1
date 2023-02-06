using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using static DataLogic;


namespace UnitTestProject1
{
    internal class UserService
    {
        internal static User GetUserInformationById(int id)
        {
            var sql = string.Format(@"
                SELECT *
                FROM UserList
                WHERE Id = {0};
                ", id);


            var reader = SqlHelper.ExecuteReader(_connectionString, sql, CommandType.Text);
            var result = new User();
            while (reader.Read())
            {
                 result = new User
                {
                    Id = Int32.Parse(reader[0] + ""),
                    FullName = Convert.ToString(reader[1]),
                    ImageURL= Convert.ToString(reader[2]),
                    PhoneNumber = Int32.Parse(reader[3] + ""),
                    Role = Convert.ToString(reader[4]),
                    Address= Convert.ToString(reader[5]),
                    UserStatus= Convert.ToString(reader[6]),    
                    Facebook = Convert.ToString(reader[7]),
                    Instagram = Convert.ToString(reader[8]),
                    UserName= Convert.ToString(reader[9]),
                    Password  = Convert.ToString(reader[10]),
                };
            }
            //if(result!= null)
            //{
            //    result += "";
            //}

            return result;
        }

        internal static int UpdatePassword(User user, string oldPassword, string newPassword)
        {
            if (oldPassword == "" || oldPassword.Length > 16) { return 0; }

            var sql = string.Format(@"
                UPDATE UserList
                SET [Password] = CASE
                     WHEN '{0}' = [Password] THEN '{1}'
                     ELSE [Password]
                 END
                 WHERE [Id] = {2};", oldPassword, newPassword, user.Id);

            var result = SqlHelper.ExecuteNonQuery(_connectionString, sql, CommandType.Text);
            return result;
        }
    }
}