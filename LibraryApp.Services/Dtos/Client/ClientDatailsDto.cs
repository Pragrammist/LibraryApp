using System;

namespace LibraryApp.Services.Dtos.Client
{
    public class ClientDatailsDto
    {
        public int ClientId { get; set; }

        public string Name { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }

        public DateTime DateCreated { get; set; }
    }

    public class ChangeClientDataDto
    {
        public int ClientId { get; set; }

        public string Name { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }
    }

}
