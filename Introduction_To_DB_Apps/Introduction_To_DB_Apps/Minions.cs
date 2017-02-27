using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction_To_DB_Apps
{
    class Minions
    {
        public static bool AddMinion()
        {
            bool result = false;
            Console.Write("Input minion name: ");
            string minionName = Console.ReadLine();
            Console.Write("Input minion age: ");
            int minionAge = int.Parse(Console.ReadLine());
            Console.Write("Input minion town: ");
            string minionTown = Console.ReadLine();
            Console.Write("Input master villain: ");
            string villainName = Console.ReadLine();
            SqlConnection dbConn = App.EstablishConnection();
            SqlTransaction transaction = dbConn.BeginTransaction("Add Minion");
            using (dbConn)
            {
                try
                {
                    bool wasVillainAdded = false;
                    bool wasTownAdded = false;
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = dbConn;
                    cmd.Transaction = transaction;
                    //cmd.Parameters.AddWithValue("@villainName", villainName);
                    if (!IsVillainInDb(dbConn, villainName, transaction, cmd)) // check if villain exist
                    {
                        string insertVillainQuery = $"INSERT INTO Villains VALUES ('{villainName}', 'Evil')";
                        cmd.CommandText = insertVillainQuery;
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            wasVillainAdded = true;
                        }
                    }
                    if (!IsTownInDb(dbConn, minionTown, transaction, cmd)) // check if the town exist
                    {
                        cmd.CommandText = $"INSERT INTO TOWNS VALUES (@townName, 1)"; //todo implement logic for each town country
                        cmd.Parameters.AddWithValue("@townName", minionTown);
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            wasTownAdded = true;
                        }
                    }
                    // we can use parameters, because we set when we check for town and villain
                    cmd.CommandText = $"SELECT Id FROM Villains WHERE Name = @villainName";
                    int villainId = (int)cmd.ExecuteScalar();
                    cmd.CommandText = $"SELECT Id FROM Towns WHERE Name = @minionTown";
                    int townId = (int)cmd.ExecuteScalar();
                    cmd.CommandText = $"INSERT INTO Minions VALUES (@minionName, {minionAge}, {townId})"; //insert the new minion
                    cmd.Parameters.AddWithValue("@minionName", minionName);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"SELECT Id FROM Minions WHERE Name = @minionName";
                    int minionId = (int)cmd.ExecuteScalar();
                    cmd.CommandText = $"INSERT INTO MinionsVillains VALUES ({minionId}, {villainId})";
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    if (wasVillainAdded)
                    {
                        Console.WriteLine($"Villain {villainName} was added to the database.");
                    }
                    if (wasTownAdded)
                    {
                        Console.WriteLine($"Town {minionTown} was added to the database.");
                    }
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Operation was rollbacked!");
                    transaction.Rollback();
                    result = false;
                }
            }
            return result;
        }
        
        static bool IsVillainInDb(SqlConnection dbConn, string villainName, SqlTransaction transaction, SqlCommand cmd)
        {
            bool isVillainExist = false;
            string query = $"SELECT COUNT(*) FROM Villains WHERE Name = @villainName"; //todo Parameters
            //SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = dbConn;
            cmd.Transaction = transaction;
            cmd.Parameters.AddWithValue("@villainName", villainName);
            if ((int)cmd.ExecuteScalar() == 1)
            {
                isVillainExist = true;
            }
            return isVillainExist;
        }

        static bool IsTownInDb(SqlConnection dbConn, string townName, SqlTransaction transaction, SqlCommand cmd)
        {
            bool isTownInDb = false;
            string query = $"SELECT COUNT(*) FROM Towns WHERE Name = @minionTown";
            //SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = dbConn;
            cmd.Transaction = transaction;
            cmd.Parameters.AddWithValue("@minionTown", townName);
            if ((int)cmd.ExecuteScalar() == 1)
            {
                isTownInDb = true;
            }
            return isTownInDb;
        }

        public static void PrintAllMinionsNames()
        {
            SqlConnection dbConn = App.EstablishConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn;
            using (dbConn)
            {
                try
                {
                    cmd.CommandText = @"SELECT Name FROM Minions";
                    List<string> minionNames = new List<string>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minionNames.Add(reader[0].ToString());
                        }
                    }
                    for (int i = 0; i < minionNames.Count / 2; i++)
                    {
                        Console.WriteLine(minionNames[i]);
                        Console.WriteLine(minionNames[minionNames.Count - 1 - i]);
                    }
                    if (minionNames.Count % 2 != 0)
                    {
                        Console.WriteLine(minionNames[minionNames.Count / 2]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void IncreaseAge()
        {
            SqlConnection dbConn = App.EstablishConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn;
            using (dbConn)
            {
                try
                {
                    Console.WriteLine("Input minions Id's separated by space");
                    int[] minionsIds = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    cmd.CommandText = $"UPDATE Minions SET Age = Age + 1 WHERE Id IN ({string.Join(", ", minionsIds)})";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "SELECT * FROM Minions";
                    List<List<string>> names = new List<List<string>>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string[] currentName = reader[1].ToString().Split();
                            for (int i = 0; i < currentName.Length; i++)
                            {
                                currentName[i] = currentName[i].Replace(currentName[i][0], Char.ToUpper(currentName[i][0]));
                            }
                            names.Add(new List<string> { reader[0].ToString(), string.Join(" ", currentName) });
                        }
                    }
                    for (int i = 0; i < names.Count; i++)
                    {
                        cmd.CommandText = $"UPDATE Minions SET Name = '{names[i][1]}' WHERE Id = {names[i][0]} ";
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = "SELECT * FROM Minions";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader[1] + " " + reader[2]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        internal static void GetOlder()
        {
            SqlConnection dbConn = App.EstablishConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn;
            using (dbConn)
            {
                try
                {
                    Console.Write("Enter minion Id: ");
                    int minionId = int.Parse(Console.ReadLine());
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_GetOlder";
                    cmd.Parameters.AddWithValue("@minionId", minionId);
                    cmd.ExecuteNonQuery();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"SELECT * FROM Minions WHERE Id = {minionId}";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[1]} {reader[2]}");
                        }
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
