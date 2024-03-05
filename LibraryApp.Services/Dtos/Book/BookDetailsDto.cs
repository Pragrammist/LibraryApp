using System;

namespace LibraryApp.Services.Dtos.Book
{
    public class BookDetailsDto
    {
        public int BookId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }

}
