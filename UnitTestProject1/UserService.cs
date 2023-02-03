using System;

namespace UnitTestProject1
{
    internal class UserService
    {
        internal static object UpdatePassword(int id, string oldPassword, string newPassword)
        {
            var sql = string.Format(@"
                UPDATE [dbo].[User]
                SET [Password] = CASE
                     WHEN '{0}' = [Password] THEN '{1}'
                     ELSE [Password]
                 END
                 WHERE [Id] = {2};" , oldPassword, newPassword, id);

            var reader = 
            throw new NotImplementedException();
        }
    }
}