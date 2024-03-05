using System;

namespace LibraryApp.DataAccess.Models
{
    public class Client
    {
        public int ClientId { get; set; }

        public string Name { get; set; }

        public string Telephone {  get; set; }

        public string Address { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
