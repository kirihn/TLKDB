using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace CourseProjectDB.Clases
{
    internal class ProcedureHelper
    {

        public ProcedureHelper() { }

        public void CheckLoginPassword(string login, string password, OracleConnection connection)
        {
            using (OracleCommand command = new OracleCommand("CheckUserLoginPassword", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("p_Login", OracleDbType.NVarchar2).Value = login;
                command.Parameters.Add("p_Password", OracleDbType.NVarchar2).Value = password;

                OracleParameter userRoleParam = new OracleParameter("p_UserRole", OracleDbType.Int32);
                userRoleParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(userRoleParam);

                OracleParameter userMyIdParam = new OracleParameter("p_UserMyId", OracleDbType.Int32);
                userMyIdParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(userMyIdParam);

                command.ExecuteNonQuery();

                int userRole = Convert.ToInt32(userRoleParam.Value);
                int userMyId = Convert.ToInt32(userMyIdParam.Value);
            }


        }
    }
}
