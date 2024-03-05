
using System;

namespace LibraryApp.DataAccess.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string Name {  get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
