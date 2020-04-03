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
  public   class SystemCountryCodeRepository : IDataRepository<SystemCountryCodePoco>
    {
        private readonly string _connStr;
        public SystemCountryCodeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SystemCountryCodePoco[] items)
        {

            using SqlConnection connection = new SqlConnection(_connStr);
            foreach (SystemCountryCodePoco poco in items)
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"INSERT INTO [dbo].[System_Country_Codes]
                                       ([Code]
                                       ,[Name])
                                 VALUES
                                       (@Code
                                       ,@Name)";


                comm.Parameters.AddWithValue("@Code", poco.Code);
                comm.Parameters.AddWithValue("@Name", poco.Name);


                connection.Open();
                comm.ExecuteNonQuery();
                connection.Close();

            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SystemCountryCodePoco> GetAll(params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = connection;
                    comm.CommandText = @"SELECT [Code]
                                      ,[Name]
                                  FROM [dbo].[System_Country_Codes]";
                    connection.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    SystemCountryCodePoco[] pocos = new SystemCountryCodePoco[500];
                    int index = 0;
                    while (reader.Read())
                    {
                        SystemCountryCodePoco poco = new SystemCountryCodePoco();
                        poco.Code = reader.GetString(0);
                        poco.Name = reader.GetString(1);

                        pocos[index] = poco;
                        index++;

                    }
                    connection.Close();
                    return pocos.Where(a => a != null).ToList();
                }
            }
        }

        public IList<SystemCountryCodePoco> GetList(Expression<Func<SystemCountryCodePoco, bool>> where, params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SystemCountryCodePoco GetSingle(Expression<Func<SystemCountryCodePoco, bool>> where, params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            IQueryable<SystemCountryCodePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SystemCountryCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (SystemCountryCodePoco poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"DELETE FROM[dbo].[System_Country_Codes]
                                   WHERE  [Code]= @Code";
                    comm.Parameters.AddWithValue("Code", poco.Code );
                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SystemCountryCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (var poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"UPDATE [dbo].[System_Country_Codes]
                                       SET 
                                          [Name] = @Name
                                          WHERE  [Code]= @Code";


                    comm.Parameters.AddWithValue("@Code", poco.Code);
                    comm.Parameters.AddWithValue("@Name", poco.Name);

                    connection.Open();
                    comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
        }

       
    }
}
