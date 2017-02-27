using System;
using System.Data.SqlClient;

namespace Introduction_To_DB_Apps
{
    class App
    {
        /* Change connection string with your Server name here */
        public static string connectionStr = @"Server=NICK-PC\SQLEXPRESS;Database=MinionsDB;Integrated Security=true";
        static void Main(string[] args)
        {
            while (true)
            {
                PrintMenu();
                string command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        while (true)
                        {
                            Console.Write("Confirm delete database?(Y/N): ");
                            switch (Console.ReadLine().ToLower())
                            {
                                case "y":
                                    Initialize.InitializeDatabase();
                                    break;
                                case "n":
                                    break;
                                default:
                                    Console.WriteLine("Invalid input!");
                                    continue;
                            }
                            break;
                        }
                        break;
                    case "2":
                        Villains.GetVillainsNames();
                        break;
                    case "3":
                        Console.Write("Enter villain Id: ");
                        int villainId = int.Parse(Console.ReadLine());
                        Villains.GetVillainMinions(villainId);
                        break;
                    case "4":
                        Minions.AddMinion();
                        break;
                    case "5":
                        Towns.ChangeTownNamesCasing();
                        break;
                    case "6":
                        Villains.RemoveVillain();
                        break;
                    case "7":
                        Minions.PrintAllMinionsNames();
                        break;
                    case "8":
                        Minions.IncreaseAge();
                        break;
                    case "9":
                        Minions.GetOlder();
                        break;
                    case "10":
                        return;
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }
                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
        }
        static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Task list:");
            Console.WriteLine("1  --> Task01 - Initial Setup(Drop old database, create fresh one and insert 5 entries)");
            Console.WriteLine("2  --> Task02 - Get Villain's names");
            Console.WriteLine("3  --> Task03 - Get Minion Names");
            Console.WriteLine("4  --> Task04 - Add new minion");
            Console.WriteLine("5  --> Task05 - Change Town Names Casing");
            Console.WriteLine("6  --> Task06 - Remove villain");
            Console.WriteLine("7  --> Task07 - Print All Minion Names");
            Console.WriteLine("8  --> Task08 - Increase Minions Age");
            Console.WriteLine("9  --> Task09 - Increase Age Stored Procedure");
            Console.WriteLine("10 --> Exit");
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
