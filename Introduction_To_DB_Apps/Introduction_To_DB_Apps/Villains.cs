using System;
using System.Data.SqlClient;

namespace Introduction_To_DB_Apps
{
    class Villains
    {

        public static void GetVillainsNames()
        {
            SqlConnection connection = App.EstablishConnection();
            using (connection)
            {
                string query = @"SELECT Name, COUNT(*) as MinionsCount FROM Villains as v
	                                JOIN MinionsVillains as mv ON mv.VillainId = v.Id
                                GROUP BY Name
                                HAVING COUNT(*) > 3";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0]} {reader[1]}");
                }
            }
        }

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

        public static void RemoveVillain()
        {
            SqlConnection dbConn = App.EstablishConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn;
            SqlTransaction transaction = dbConn.BeginTransaction("Remove Villain");
            cmd.Transaction = transaction;
            using (dbConn)
            {
                try
                {
                    Console.Write("Enter villain ID: ");
                    int villainId = int.Parse(Console.ReadLine());
                    cmd.CommandText = @"SELECT COUNT(*) FROM Villains WHERE Id = @villainId";
                    cmd.Parameters.AddWithValue("@villainId", villainId);
                    if ((int)cmd.ExecuteScalar() < 1)
                    {
                        Console.WriteLine("No such villain was found");
                        return;
                    }
                    cmd.CommandText = @"SELECT Name FROM Villains WHERE Id = @villainId";
                    string villainName = cmd.ExecuteScalar().ToString();

                    cmd.CommandText = @"DELETE FROM MinionsVillains WHERE VillainId = @villainId";
                    int minionsReleased = cmd.ExecuteNonQuery();
                    
                    cmd.CommandText = @"DELETE FROM Villains WHERE Id = @villainId";
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine($"{villainName} was deleted");
                    Console.WriteLine($"{minionsReleased} minions released");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
