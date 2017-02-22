using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduction_To_DB_Apps
{
    class App
    {
        public static string connectionStr = @"Server=NICK-PC\SQLEXPRESS;Database=master;Integrated Security=true";
        static void Main(string[] args)
        {

            while (true)
            {
                PrintMenu();
                string command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        if (Initialize.InitializeDatabase())
                        {
                            Console.WriteLine("Database initialized successfully!");
                            Thread.Sleep(3000);
                        }
                        break;
                    /* Task 02 - Get villain's names */
                    case "2": 
                        //todo
                        break;
                    /* Task 03 - Get all minions by villain id */
                    case "3":
                        Console.Write("Enter villain Id: ");
                        int villainId = int.Parse(Console.ReadLine());
                        Villains.GetVillainMinions(villainId);
                        Console.Write("Press any key to continue ");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Invalid input!");
                        Thread.Sleep(3000);
                        break;
                }
            }
            

            
            
        }
        static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Task list:");
            Console.WriteLine("1 --> Task01 - Initial Setup(Drop old database, create fresh one and insert 5 entries)");
            Console.WriteLine("2 --> Task02 - Get Villain's names");
            Console.WriteLine("3 --> Task03 - Get Minion Names");
            Console.Write("Choose task: ");
        }
        public static SqlConnection EstablishConnection()
        {
            SqlConnection dbConn = new SqlConnection(connectionStr);
            dbConn.Open();
            return dbConn;
        }
    }
}
