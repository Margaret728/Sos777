using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework7
{
    internal class MovieWriter
    {
        public static void WriteMoviesToScreen(List<Movie> movies)
        {
            foreach (Movie mv in movies)
            {
                Console.WriteLine(mv);
            }
        }
    }
}
