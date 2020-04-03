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
    public class CompanyProfileRepository : IDataRepository<CompanyProfilePoco>
    {
        private readonly string _connStr;
        public CompanyProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (CompanyProfilePoco poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Profiles]
                                       ([Id]
                                       ,[Registration_Date]
                                       ,[Company_Website]
                                       ,[Contact_Phone]
                                       ,[Contact_Name]
                                       ,[Company_Logo])
                                 VALUES
                                       (@Id
                                       ,@Registration_Date
                                       ,@Company_Website
                                       ,@Contact_Phone
                                       ,@Contact_Name
                                       ,@Company_Logo)";


                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    comm.Parameters.AddWithValue("@Registration_Date", poco.RegistrationDate );
                    comm.Parameters.AddWithValue("@Company_Website", poco.CompanyWebsite );
                    comm.Parameters.AddWithValue("@Contact_Phone", poco.ContactPhone );
                    comm.Parameters.AddWithValue("@Contact_Name", poco.ContactName );
                    comm.Parameters.AddWithValue("@Company_Logo", poco.CompanyLogo );
                  

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

        public IList<CompanyProfilePoco> GetAll(params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {

                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id]
                                      ,[Registration_Date]
                                      ,[Company_Website]
                                      ,[Contact_Phone]
                                      ,[Contact_Name]
                                      ,[Company_Logo]
                                      ,[Time_Stamp]
                                  FROM [dbo].[Company_Profiles]";
                connection.Open();
                SqlDataReader reader = comm.ExecuteReader();
                CompanyProfilePoco[] pocos = new CompanyProfilePoco[500];
                int index = 0;
                while (reader.Read())
                {
                    CompanyProfilePoco poco = new CompanyProfilePoco();
                    poco.Id = reader.GetGuid(0);
                    poco.RegistrationDate  = reader.GetDateTime (1);
                    poco.CompanyWebsite = reader.IsDBNull(2) ? (string)null : reader.GetString(2);
                    poco.ContactPhone  = reader.GetString(3);
                    poco.ContactName = reader.IsDBNull(4) ? (string)null : reader.GetString(4); ;
                   
                    poco.CompanyLogo = reader.IsDBNull(5) ? (byte[] )null : (byte[])reader[5];
                    poco.TimeStamp = (byte[])reader[6];

                    pocos[index] = poco;
                    index++;

                }
                connection.Close();
                return pocos.Where(a => a != null).ToList();
            }
        }

        public IList<CompanyProfilePoco> GetList(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyProfilePoco GetSingle(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyProfilePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (CompanyProfilePoco poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"DELETE FROM[dbo].[Company_Profiles]
                                   WHERE  [Id]= @Id";
                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (var poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"UPDATE [dbo].[Company_Profiles]
                                           SET 
                                              [Registration_Date] = @Registration_Date
                                              ,[Company_Website] = @Company_Website
                                              ,[Contact_Phone] = @Contact_Phone
                                              ,[Contact_Name] = @Contact_Name
                                              ,[Company_Logo] = @Company_Logo
                                            WHERE  [Id]= @Id";


                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    comm.Parameters.AddWithValue("@Registration_Date", poco.RegistrationDate);
                    comm.Parameters.AddWithValue("@Company_Website", poco.CompanyWebsite);
                    comm.Parameters.AddWithValue("@Contact_Phone", poco.ContactPhone);
                    comm.Parameters.AddWithValue("@Contact_Name", poco.ContactName);
                    comm.Parameters.AddWithValue("@Company_Logo", poco.CompanyLogo);

                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
        }
    }
}
