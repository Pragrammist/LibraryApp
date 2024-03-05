
using System;

namespace LibraryApp.DataAccess.Models
{
    public class BookWithStatus : Book
    {
        public int Status { get; set; }

        public DateTime WriteDate { get; set; }
    }
}
