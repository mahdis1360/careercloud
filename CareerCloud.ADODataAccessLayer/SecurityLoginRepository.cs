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
    public class SecurityLoginRepository : IDataRepository<SecurityLoginPoco>
    {
        private readonly string _connStr;
        public SecurityLoginRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SecurityLoginPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (SecurityLoginPoco poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"INSERT INTO [dbo].[Security_Logins]
                                       ([Id]
                                       ,[Login]
                                       ,[Password]
                                       ,[Created_Date]
                                       ,[Password_Update_Date]
                                       ,[Agreement_Accepted_Date]
                                       ,[Is_Locked]
                                       ,[Is_Inactive]
                                       ,[Email_Address]
                                       ,[Phone_Number]
                                       ,[Full_Name]
                                       ,[Force_Change_Password]
                                       ,[Prefferred_Language])
                                 VALUES
                                       (@Id
                                       ,@Login
                                       ,@Password
                                       ,@Created_Date
                                       ,@Password_Update_Date
                                       ,@Agreement_Accepted_Date
                                       ,@Is_Locked
                                       ,@Is_Inactive
                                       ,@Email_Address
                                       ,@Phone_Number
                                       ,@Full_Name
                                       ,@Force_Change_Password
                                       ,@Prefferred_Language)";


                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    comm.Parameters.AddWithValue("@Login", poco.Login );
                    comm.Parameters.AddWithValue("@Password", poco.Password );
                    comm.Parameters.AddWithValue("@Created_Date", poco.Created );
                    comm.Parameters.AddWithValue("@Password_Update_Date", poco.PasswordUpdate );
                    comm.Parameters.AddWithValue("@Agreement_Accepted_Date", poco.AgreementAccepted );
                    comm.Parameters.AddWithValue("@Is_Locked", poco.IsLocked );
                    comm.Parameters.AddWithValue("@Is_Inactive", poco.IsInactive );
                    comm.Parameters.AddWithValue("@Email_Address", poco.EmailAddress );
                    comm.Parameters.AddWithValue("@Phone_Number", poco.PhoneNumber );
                    comm.Parameters.AddWithValue("@Full_Name", poco.FullName );
                    comm.Parameters.AddWithValue("@Force_Change_Password", poco.ForceChangePassword );
                    comm.Parameters.AddWithValue("@Prefferred_Language", poco.PrefferredLanguage );



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

        public IList<SecurityLoginPoco> GetAll(params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {

                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id]
                                  ,[Login]
                                  ,[Password]
                                  ,[Created_Date]
                                  ,[Password_Update_Date]
                                  ,[Agreement_Accepted_Date]
                                  ,[Is_Locked]
                                  ,[Is_Inactive]
                                  ,[Email_Address]
                                  ,[Phone_Number]
                                  ,[Full_Name]
                                  ,[Force_Change_Password]
                                  ,[Prefferred_Language]
                                  ,[Time_Stamp]
                              FROM [dbo].[Security_Logins]";
                connection.Open();
                SqlDataReader reader = comm.ExecuteReader();
                SecurityLoginPoco[] pocos = new SecurityLoginPoco[500];
                int index = 0;
                while (reader.Read())
                {
                    SecurityLoginPoco poco = new SecurityLoginPoco();
                    poco.Id = reader.GetGuid(0);
                    poco.Login  = reader.GetString(1);
                    poco.Password  = reader.GetString(2);
                    poco.Created  = reader.GetDateTime(3);
                    poco.PasswordUpdate = reader.IsDBNull(4) ? (DateTime ?)null : reader.GetDateTime (4);
                    poco.AgreementAccepted  = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5);
                    poco.IsLocked = reader.GetBoolean (6);
                    poco.IsInactive = reader.GetBoolean(7);
                    poco.EmailAddress  = reader.GetString(8);
                    poco.PhoneNumber = reader.IsDBNull(9) ? (string)null : reader.GetString(9);
                    poco.FullName  = reader.GetString(10);
                    poco.ForceChangePassword = reader.GetBoolean(11);
                    poco.PrefferredLanguage  = reader.IsDBNull(12) ? (string)null : reader.GetString(12);
                    poco.TimeStamp = (byte[])reader[13];

                    pocos[index] = poco;
                    index++;

                }
                connection.Close();
                return pocos.Where(a => a != null).ToList();
            }
        }

        public IList<SecurityLoginPoco> GetList(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginPoco GetSingle(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (SecurityLoginPoco poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"DELETE FROM[dbo].[Security_Logins]
                                   WHERE  [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SecurityLoginPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (var poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"UPDATE [dbo].[Security_Logins]
                                       SET
                                          [Login] = @Login
                                          ,[Password] = @Password
                                          ,[Created_Date] = @Created_Date
                                          ,[Password_Update_Date] = @Password_Update_Date
                                          ,[Agreement_Accepted_Date] = @Agreement_Accepted_Date
                                          ,[Is_Locked] =@Is_Locked
                                          ,[Is_Inactive] = @Is_Inactive
                                          ,[Email_Address] = @Email_Address
                                          ,[Phone_Number] = @Phone_Number
                                          ,[Full_Name] = @Full_Name
                                          ,[Force_Change_Password] = @Force_Change_Password
                                          ,[Prefferred_Language] = @Prefferred_Language
                                          WHERE  [Id]= @Id";


                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    comm.Parameters.AddWithValue("@Login", poco.Login);
                    comm.Parameters.AddWithValue("@Password", poco.Password);
                    comm.Parameters.AddWithValue("@Created_Date", poco.Created);
                    comm.Parameters.AddWithValue("@Password_Update_Date", poco.PasswordUpdate);
                    comm.Parameters.AddWithValue("@Agreement_Accepted_Date", poco.AgreementAccepted);
                    comm.Parameters.AddWithValue("@Is_Locked", poco.IsLocked);
                    comm.Parameters.AddWithValue("@Is_Inactive", poco.IsInactive);
                    comm.Parameters.AddWithValue("@Email_Address", poco.EmailAddress);
                    comm.Parameters.AddWithValue("@Phone_Number", poco.PhoneNumber);
                    comm.Parameters.AddWithValue("@Full_Name", poco.FullName);
                    comm.Parameters.AddWithValue("@Force_Change_Password", poco.ForceChangePassword);
                    comm.Parameters.AddWithValue("@Prefferred_Language", poco.PrefferredLanguage);

                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
        }
    }
}
