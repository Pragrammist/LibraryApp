using System;

namespace LibraryApp.Services.Dtos.Book
{
    public class BookWithStatusDto : BookDto
    {
        public int Status { get; set; }

        public DateTime WriteDate { get; set; }
    }

}
