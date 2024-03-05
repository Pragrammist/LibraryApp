using System;


namespace LibraryApp.DataAccess.Models
{
    public class ClientBook
    {
        public int WriteId {  get; set; }
        
        public int Status { get; set; }

        public int ClientId { get; set; }

        public int BookId { get; set; }

        public DateTime WriteDate { get; set; }
    }
}
