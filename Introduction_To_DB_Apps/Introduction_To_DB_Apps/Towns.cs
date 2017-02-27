using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction_To_DB_Apps
{
    class Towns
    {
        public static void ChangeTownNamesCasing()
        {
            SqlConnection dbConn = App.EstablishConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn;
            using (dbConn)
            {
                try
                {
                    cmd.CommandText = $"SELECT * FROM Countries";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Countries list:");
                        while (reader.Read())
                        {
                            Console.WriteLine(" - " + reader[1]);
                        }
                    }
                    Console.Write("Enter country: ");
                    string countryName = Console.ReadLine();
                    cmd.CommandText = @"SELECT Id From Countries WHERE CountryName = @countryName";
                    cmd.Parameters.AddWithValue("@countryName", countryName);
                    int countryId = (int)cmd.ExecuteScalar();
                    cmd.CommandText = @"UPDATE Towns
                                        SET Name = UPPER(Name) 
                                            WHERE CountryId = @countryId";
                    cmd.Parameters.AddWithValue("@countryId", countryId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    cmd.CommandText = @"SELECT Name FROM Towns WHERE CountryId = @countryId";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<string> towns = new List<string>();
                        while (reader.Read())
                        {
                            towns.Add(reader[0].ToString());
                        }
                        Console.WriteLine($"{rowsAffected} town names were affected.");
                        Console.WriteLine($"[{string.Join(", ", towns)}]");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
