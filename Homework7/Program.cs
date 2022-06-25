using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;

namespace Homework7
{
    internal class Program
    {
        public static List<Movie> GetMoviesFromDatabase(DbCommand cmd)
        {
            List<Movie> results = new List<Movie>();
            cmd.CommandText = "select * from Movie";
            Movie mv;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    mv = new Movie(
                        Convert.ToInt32(reader["ID"]),
                        Convert.ToString(reader["Title"]),
                        Convert.ToString(reader["Genre"]),
                        Convert.ToInt32(reader["Year"]),
                        Convert.ToDouble(reader["Rating"]),
                        Convert.ToDouble(reader["Price"]));

                    results.Add(mv);
                }
            }
            return results;
        }

        public static bool SearchForMovie(int ID, List<Movie> movies)
        {
            bool found = false;
            foreach (Movie m in movies)
            {
                if (m.ID == ID)
                    found = true;
            }
            return found;
        }

        static void Main(string[] args)
        {
            /*
            somehow this won't work in my vs 2022, there is an error with DbProviderFactory
            string provider = ConfigurationManager.AppSettings["provider"];
            string connectionString = ConfigurationManager.AppSettings["connectionString"];
            DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
            */

            // so I am using a little bit different approach
            try
            {
                Console.WriteLine("Welcome to the Movie Database Program!\n");
                string provider = ConfigurationManager.AppSettings["provider"];
                string connectionString = ConfigurationManager.AppSettings["connectionString"];
                List<Movie> movies;

                Console.WriteLine($"--- Connection information --- \n" +
                    $"Provider: {provider}\n" +
                    $"Connection string: {connectionString}\n");

                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from Movie";

                movies = GetMoviesFromDatabase(cmd);
                MovieWriter.WriteMoviesToScreen(movies);
                Console.WriteLine();

                int ID = -1;
                int option = 0;
                string title, genre;
                int year;
                double rating, price;

                while (option != 4)
                {
                    Console.WriteLine("*** MAIN MENU ***");
                    Console.WriteLine("1. List all movies");
                    Console.WriteLine("2. Insert new movie");
                    Console.WriteLine("3. Delete a movie");
                    Console.WriteLine("4. Exit");
                    Console.Write("Please enter the option (1-4): ");
                    option = Convert.ToInt32(Console.ReadLine());
                    switch (option)
                    {
                        case 1:
                            Console.WriteLine();
                            MovieWriter.WriteMoviesToScreen(movies);
                            break;
                        case 2:
                            Console.Write("\nEnter movie ID: ");
                            ID = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter movie title: ");
                            title = Console.ReadLine();
                            Console.Write("Enter movie genre: ");
                            genre = Console.ReadLine();
                            Console.Write("Enter movie year: ");
                            year = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter movie rating (0-10): ");
                            rating = Convert.ToDouble(Console.ReadLine());
                            Console.Write("Enter movie rental price: ");
                            price = Convert.ToDouble(Console.ReadLine());

                            bool found = SearchForMovie(ID, movies);
                            if (!found)
                            {
                                if (ID > 0 && title.Length > 0 && genre.Length > 0 &&
                                    year > 0 && rating > 0 && price > 0)
                                {
                                    cmd.CommandText = String.Format("insert into Movie values " +
                                        "({0}, '{1}', '{2}', {3}, {4}, {5})",
                                        ID, title, genre, year, rating, price);
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("\nThe movie was added successfully!\n" +
                                        "Here's the list now:\n");
                                    movies = GetMoviesFromDatabase(cmd);
                                    MovieWriter.WriteMoviesToScreen(movies);
                                }
                                else
                                {
                                    Console.WriteLine("\nInvalid data entered!\n");
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nMovie with ID = " + ID + " already exists in the database!\n");
                                Console.WriteLine();
                            }
                            break;
                        case 3:
                            Console.Write("\nEnter movie ID: ");
                            ID = Convert.ToInt32(Console.ReadLine());
                            found = SearchForMovie(ID, movies);
                            if (found)
                            {
                                cmd.CommandText = "delete from Movie where ID = " + ID;
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("\nThe movie was deleted successfully!\n" +
                                    "Here's the list now:\n");
                                movies = GetMoviesFromDatabase(cmd);
                                MovieWriter.WriteMoviesToScreen(movies);
                            }
                            else
                            {
                                Console.WriteLine("\nNo movies with ID = " + ID + "\n");
                                Console.WriteLine();
                            }
                            break;
                        case 4:
                            Console.WriteLine("\nGoodbye, thank you for using our program!\n");
                            break;
                        default:
                            Console.WriteLine("\nInvalid option entered, please retry!\n");
                            break;
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nDatabase error. " + ex.Message + "\n");
                Console.ReadLine();
                return;
            }
        }
    }
}
