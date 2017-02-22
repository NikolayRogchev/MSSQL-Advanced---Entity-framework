using System;
using System.Data.SqlClient;

namespace Introduction_To_DB_Apps
{
    class Villains
    {
        public static void GetVillainMinions(int villainId)
        {
            SqlConnection dbConn = App.EstablishConnection();
            using (dbConn)
            {
                try
                {
                    string query = @"select v.Name, m.Name, m.Age from Villains as v
                                    inner join MinionsVillains as mv on v.Id = mv.VillainId
                                    inner join Minions as m on m.Id = mv.MinionId
                                    where v.Id = @villainId;";
                    SqlCommand cmd = new SqlCommand(query, dbConn);
                    cmd.Parameters.AddWithValue("@villainId", villainId);
                    SqlDataReader result = cmd.ExecuteReader();
                    using (result)
                    {
                        PrintMinionNames(result);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public static void PrintMinionNames(SqlDataReader data)
        {
            int count = 0;
            while (data.Read())
            {
                if (count == 0)
                {
                    Console.WriteLine("Villain: " + data[0]);
                }
                Console.WriteLine($"{++count}. {(string)data[1]} {(int)data[2]}");
            }
        }
    }
}
