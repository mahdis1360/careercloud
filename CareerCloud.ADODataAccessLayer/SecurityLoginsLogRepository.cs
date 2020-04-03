using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CareerCloud.ADODataAccessLayer
{
   public  class SecurityLoginsLogRepository : IDataRepository<SecurityLoginsLogPoco>
    {
        private readonly string _connStr;
        public SecurityLoginsLogRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (SecurityLoginsLogPoco poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"INSERT INTO [dbo].[Security_Logins_Log]
                                       ([Id]
                                       ,[Login]
                                       ,[Source_IP]
                                       ,[Logon_Date]
                                       ,[Is_Succesful])
                                 VALUES
                                       (@Id
                                       ,@Login
                                       ,@Source_IP
                                       ,@Logon_Date
                                       ,@Is_Succesful)";


                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    comm.Parameters.AddWithValue("@Login", poco.Login);
                    comm.Parameters.AddWithValue("@Source_IP", poco.SourceIP );
                    comm.Parameters.AddWithValue("@Logon_Date", poco.LogonDate );
                    comm.Parameters.AddWithValue("@Is_Succesful", poco.IsSuccesful );
                   
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginsLogPoco> GetAll(params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {

            using (SqlConnection connection = new SqlConnection(_connStr))
            {

                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id]
                                  ,[Login]
                                  ,[Source_IP]
                                  ,[Logon_Date]
                                  ,[Is_Succesful]
                              FROM [dbo].[Security_Logins_Log]";
                connection.Open();
                SqlDataReader reader = comm.ExecuteReader();
                SecurityLoginsLogPoco[] pocos = new SecurityLoginsLogPoco[3500];
                int index = 0;
                while (reader.Read())
                {
                    SecurityLoginsLogPoco poco = new SecurityLoginsLogPoco();
                    poco.Id = reader.GetGuid(0);
                    poco.Login  = reader.GetGuid(1);
                    poco.SourceIP  = reader.GetString(2);
                    poco.LogonDate  = reader.GetDateTime (3);
                    poco.IsSuccesful  = reader.GetBoolean (4);
                   

                    pocos[index] = poco;
                    index++;

                }
                connection.Close();
                return pocos.Where(a => a != null).ToList();
            }
        }

        public IList<SecurityLoginsLogPoco> GetList(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsLogPoco GetSingle(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsLogPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsLogPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (SecurityLoginsLogPoco poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"DELETE FROM[dbo].[Security_Logins_Log]
                                   WHERE  [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (var poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"UPDATE [dbo].[Security_Logins_Log]
                                           SET 
                                              [Login] = @Login
                                              ,[Source_IP] = @Source_IP
                                              ,[Logon_Date] = @Logon_Date
                                              ,[Is_Succesful] = @Is_Succesful
                                          WHERE  [Id]= @Id";


                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    comm.Parameters.AddWithValue("@Login", poco.Login);
                    comm.Parameters.AddWithValue("@Source_IP", poco.SourceIP);
                    comm.Parameters.AddWithValue("@Logon_Date", poco.LogonDate);
                    comm.Parameters.AddWithValue("@Is_Succesful", poco.IsSuccesful);

                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
        }
    }
}
