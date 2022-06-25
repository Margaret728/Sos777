using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework7
{
    internal class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public double Price { get; set; }

        public Movie()
        {
            ID = 0;
            Title = "A movie";
            Genre = "N/A";
            Year = 2022;
            Rating = 0;
            Price = 0;

        }

        public Movie(int id, string title, string genre, int year, double rating, double price)
        {
            ID = id;
            Title = title;
            Genre = genre;
            Year = year;
            Rating = rating;
            Price = price;
        }

        public override string ToString()
        {
            return $"{ID}  {Title}  {Genre}  {Year}  {Rating:F2}/10.00  {Price:C2}\n";
        }
    }
}
