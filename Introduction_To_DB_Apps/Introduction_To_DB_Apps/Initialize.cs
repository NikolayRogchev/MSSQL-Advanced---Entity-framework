using System;
using System.Data.SqlClient;
using System.IO;

namespace Introduction_To_DB_Apps
{
    class Initialize
    {
        

        public static bool InitializeDatabase()
        {
            try
            {
                CreateDatabase(File.ReadAllText("../../CreateDatabase.sql"));
                if (CreateTables(File.ReadAllText("../../CreateTables.sql")))
                {
                    Console.WriteLine("Tables created successfully!");
                }
                int affectedRows = InsertData(File.ReadAllText("../../InsertData.sql"));
                Console.WriteLine("Successfully inserted {0} rows", affectedRows);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        static void CreateDatabase(string script)
        {
            App.connectionStr = @"Server=NICK-PC\SQLEXPRESS;Database=master;Integrated Security=true";
            SqlConnection connection = App.EstablishConnection();
            using (connection)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(script, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }
            }
            App.connectionStr = @"Server=NICK-PC\SQLEXPRESS;Database=MinionsDB;Integrated Security=true";
        }
        static bool CreateTables(string script)
        {
            SqlConnection dbConn = new SqlConnection(App.connectionStr);
            dbConn.Open();
            using (dbConn)
            {
                try
                {
                    SqlCommand command = new SqlCommand(script, dbConn);
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            return true;
        }

        static int InsertData(string script)
        {
            SqlConnection dbConn = new SqlConnection(App.connectionStr);
            dbConn.Open();
            int affectedRows = 0;
            using (dbConn)
            {
                try
                {
                    SqlCommand command = new SqlCommand(script, dbConn);
                    affectedRows = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
            }
            return affectedRows;
        }

        
    }
}
