using Microsoft.Data.SqlClient;

namespace Blogg.Demo
{
    public class DataAccess
    {
        private string connectionString = "Server=(localdb)\\mssqllocaldb;Database=users;Trusted_Connection=True;TrustServerCertificate=True";

        public void CreateBlogPost(string author, string title)
        {
            string sql = "INSERT INTO BlogPost (Author, Title) VALUES (@Author, @Title)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Author", author ?? "");
                command.Parameters.AddWithValue("@Title", title ?? "");
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateBlogPost(int id, string newTitle, string newAuthor)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string currentAuthor = "";
                string currentTitle = "";

                string selectSql = "SELECT Author, Title FROM BlogPost WHERE Id = @Id";
                using (SqlCommand selectCommand = new SqlCommand(selectSql, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentAuthor = reader["Author"].ToString();
                            currentTitle = reader["Title"].ToString();
                        }
                    }
                }

                string finalTitle = string.IsNullOrWhiteSpace(newTitle) ? currentTitle : newTitle;
                string finalAuthor = string.IsNullOrWhiteSpace(newAuthor) ? currentAuthor : newAuthor;

                string updateSql = "UPDATE BlogPost SET Title = @Title, Author = @Author WHERE Id = @Id";
                using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.Parameters.AddWithValue("@Title", finalTitle);
                    updateCommand.Parameters.AddWithValue("@Author", finalAuthor);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public void DeleteBlogPost(int id)
        {
            string sql = "DELETE FROM BlogPost WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void SeedDatabase()
        {
            List<BlogPost> seedList = new List<BlogPost>
            {
                new BlogPost { Author = "Lily", Title = "The sun is bright" },
                new BlogPost { Author = "Ethan", Title = "I will go swimming" },
                new BlogPost { Author = "Maya", Title = "Reflections on a rainy afternoon" },
                new BlogPost { Author = "Noah", Title = "Why I started journaling every day" },
                new BlogPost { Author = "Ava", Title = "Top 5 books that changed my life" },
                new BlogPost { Author = "Lucas", Title = "Exploring the mountains of Peru" },
                new BlogPost { Author = "Emma", Title = "How to stay productive while working remotely" },
                new BlogPost { Author = "Oliver", Title = "The art of brewing the perfect cup of coffee" },
                new BlogPost { Author = "Sophia", Title = "Lessons from my first marathon" },
                new BlogPost { Author = "James", Title = "What photography taught me about patience" }
            };

            foreach (var post in seedList)
            {
                CreateBlogPost(post.Author, post.Title);
            }
        }
    }
}