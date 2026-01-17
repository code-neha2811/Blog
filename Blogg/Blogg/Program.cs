using Blogg.Demo;
using Microsoft.Data.SqlClient;

string connectionString = "Server=(localdb)\\mssqllocaldb;Database=users;Trusted_Connection=True;TrustServerCertificate=True";
DataAccess db = new DataAccess();

bool running = true;
while (running)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("========================================");
    Console.WriteLine("           BLOGGY DASHBOARD             ");
    Console.WriteLine("========================================");
    Console.ResetColor();

    Console.WriteLine("1. View all posts");
    Console.WriteLine("2. Create new post");
    Console.WriteLine("3. Update post title");

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("4. Delete post");
    Console.ResetColor();

    Console.WriteLine("5. Finish");

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("6. Seed Database (10 Posts)");
    Console.ResetColor();

    Console.Write("\nSelect an option: ");

    string choice = Console.ReadLine();

    if (choice == "1")
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{"ID",-6} | {"AUTHOR",-15} | {"TITLE"}");
        Console.WriteLine(new string('-', 50));
        Console.ResetColor();

        string sql = "SELECT Id, Author, Title FROM BlogPost";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"],-6} | {reader["Author"],-15} | {reader["Title"]}");
                }
            }
        }
        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    if (choice == "2")
    {
        Console.Clear();
        Console.Write("Enter author: ");
        string author = Console.ReadLine();
        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        db.CreateBlogPost(author, title);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nPost saved successfully!");
        Console.ResetColor();
        Console.ReadKey();
    }

    if (choice == "3")
    {
        Console.Clear();
        Console.Write("Enter ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("New Title (leave blank to keep current): ");
            string t = Console.ReadLine();
            Console.Write("New Author (leave blank to keep current): ");
            string a = Console.ReadLine();

            db.UpdateBlogPost(id, t, a);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Update processed!");
            Console.ResetColor();
        }
        Console.ReadKey();
    }

    if (choice == "4")
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Enter ID to delete: ");
        Console.ResetColor();

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            db.DeleteBlogPost(id);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Entry removed.");
            Console.ResetColor();
        }
        Console.ReadKey();
    }

    if (choice == "5")
    {
        running = false;
    }

    if (choice == "6")
    {
        Console.Clear();
        Console.WriteLine("Initializing data...");
        db.SeedDatabase();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success: 10 sample posts added!");
        Console.ResetColor();
        Console.ReadKey();
    }
}