
DROP TABLE ClientBook;
DROP TABLE Book;
DROP TABLE Client;

USE libraryapp


CREATE TABLE Client(
    ClientId INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    Name NVARCHAR(100)  NOT NULL,
    Telephone NVARCHAR(15)  NOT NULL UNIQUE,
    Address NVARCHAR(200)  NOT NULL,
    DateCreated DATE DEFAULT GETDATE() NOT NULL 
);

CREATE TABLE Book(
    BookId INT PRIMARY KEY  NOT NULL IDENTITY(1,1),
    Description NVARCHAR(300) NOT NULL,
    Name NVARCHAR(100)  NOT NULL UNIQUE,
    DateCreated DATE DEFAULT GETDATE() NOT NULL
);

CREATE TABLE ClientBook(
    WriteId INT PRIMARY KEY  NOT NULL IDENTITY(1,1),
    Status INT  DEFAULT 1  NOT NULL,
    ClientId INT FOREIGN KEY REFERENCES Client(ClientId),
    BookId INT FOREIGN KEY REFERENCES Book(BookId),
    WriteDate DATE DEFAULT GETDATE() NOT NULL
);