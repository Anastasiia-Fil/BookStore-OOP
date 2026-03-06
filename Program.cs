using System;
using System.Collections.Generic;
using System.Linq;
using Assignment_01_StarterCode;

namespace Assignment_01_StarterCode
{
    /* ------------------- Book Class -------------------
       Note: This class is complete. Do not change it.
       It contains all properties and methods for a Book object.
    --------------------------------------------------- */
    public class Book
    {
        public string BookName { get; set; }  // Book title (not unique alone)
        public int Serial { get; set; }       // Unique with BookName
        public bool Status { get; set; } = false; // false = available, true = rented

        public Book(string name, int serial)
        {
            BookName = name;
            Serial = serial;
        }

        // Returns true if the book is available
        public bool Available()
        {
            return !Status;
        }

        // Rent the book. Returns true if successful
        public bool Rent()
        {
            if (!Status)
            {
                Status = true;
                return true;
            }
            return false;
        }

        // Return the book. Returns true if successful
        public bool Return()
        {
            if (Status)
            {
                Status = false;
                return true;
            }
            return false;
        }

        // Display information about the book
        public void BookInfo()
        {
            Console.WriteLine("Book Name: {0}, Serial: {1}, Status: {2}",
                BookName, Serial, Status ? "Rented" : "Available");
        }
    }

    /* ------------------- Reader Class -------------------
       Each reader has a name and a list of books rented.
       Learners will implement the RentABook and ReturnABook methods.
    ---------------------------------------------------- */
    public class Reader
    {
        public string ReaderName { get; set; }
        public List<Book> ReadersBookList { get; private set; } // Good practice: property starts with capital letter

        public Reader(string name)
        {
            ReaderName = name;
            ReadersBookList = new List<Book>(); // Start with no rented books
        }

        // Rent a book for the reader
        public void RentABook(Book book)
        {
            // ---------------- Hints for Learners ----------------
            // 1. Check if the reader already has 2 books
            // 2. If so, display "You cannot rent more than two books!" and exit
            // 3. Otherwise, try to rent the book using book.Rent()
            // 4. If successful, add it to readersBookList and display confirmation
            // 5. If book is already rented, display "Sorry! The book '{book}' is already rented."
            // -----------------------------------------------------

            if (ReadersBookList.Count >= 2)
            {
                Console.WriteLine("You cannot rent more than two books!");
                return;
            }

            if (book.Rent())
            {
                ReadersBookList.Add(book);
                Console.WriteLine("Book: '{0}' successfully rented by {1}.", book.BookName, ReaderName);
            }
            else
            {
                Console.WriteLine("Sorry! The book '{0}' is not Available for renting.", book.BookName);
            }
        }

        // Return a book for the reader
        public void ReturnABook(Book book)
        {
            // ---------------- Hints for Learners ----------------
            // 1. Check if readersBookList contains the book
            // 2. If yes, call book.Return() to mark as available
            // 3. Remove the book from readersBookList
            // 4. Display confirmation message
            // 5. If not in list, display "Error! You have not rented the book '{book}'"
            // -----------------------------------------------------

            if (ReadersBookList.Contains(book))
            {
                book.Return();
                ReadersBookList.Remove(book);
                Console.WriteLine("Book: '{0}' successfully returned by {1}.", book.BookName, ReaderName);
            }
            else
            {
                Console.WriteLine("Error! You have not rented the book '{0}'", book.BookName);
            }
        }

        // Display reader info and list of rented books
        public void ReaderInfo()
        {
            Console.WriteLine("Reader {0} rented the following books:", ReaderName);

            if (ReadersBookList.Count == 0)
            {
                Console.WriteLine("No books rented yet!");
                return;
            }

            foreach (var book in ReadersBookList)
            {
                book.BookInfo();
            }
        }
    }

    /* ------------------- BookStore Class -------------------
       This class manages all books and readers.
       Learners will implement most methods.
    -------------------------------------------------------- */
    public class BookStore
    {
        public List<Book> BookStoreBooksList { get; private set; }
        public List<Reader> BookStoreReadersList { get; private set; }

        public BookStore()
        {
            BookStoreBooksList = new List<Book>();
            BookStoreReadersList = new List<Reader>();
        }

        // Add a new reader to the store (already implemented)
        public void AddAReader(string name)
        {
            Reader reader = new Reader(name);
            BookStoreReadersList.Add(reader);
        }

        // Remove a reader from the store
        public void RemoveAReader(string name)
        {
            // ---------------- Hints for Learners ----------------
            // 1. Find the reader in BookStoreReadersList by name
            // 2. If not found, display error message
            // 3. If found, return all rented books first
            // 4. Then remove reader from BookStoreReadersList
            // -----------------------------------------------------

            Reader foundReader = BookStoreReadersList.FirstOrDefault(reader => reader.ReaderName == name);

            if (foundReader == null)
            {
                Console.WriteLine("Error! Reader '{0}' does not exist.", name);
                return;
            }

            while (foundReader.ReadersBookList.Count > 0)
            {
                foundReader.ReturnABook(foundReader.ReadersBookList[0]);
            }

            BookStoreReadersList.Remove(foundReader);
            Console.WriteLine("Reader {0} successfully removed from bookstore.", name);
        }

        // Add a new book to the store
        public void AddABook(string name, int serial)
        {
            // ---------------- Hints for Learners ----------------
            // 1. Create a new Book object with name and serial
            // 2. Add it to BookStoreBooksList
            // 3. Display confirmation message
            // -----------------------------------------------------

            Book newBook = new Book(name, serial);
            BookStoreBooksList.Add(newBook);
            Console.WriteLine("Book: '{0}', Serial: {1} added to bookstore.", name, serial);
        }

        // Remove a book from the store
        public void RemoveABook(string name, int serial)
        {
            // ---------------- Hints for Learners ----------------
            // 1. Find the book in BookStoreBooksList using name & serial
            // 2. If not found, display error
            // 3. If found, check book.Status
            // 4. If true (rented), cannot remove, display error
            // 5. If false, remove book and display confirmation
            // -----------------------------------------------------

            Book foundBook = BookStoreBooksList.FirstOrDefault(book => book.BookName == name && book.Serial == serial);

            if (foundBook == null)
            {
                Console.WriteLine("Error! Book '{0}' with serial '{1}' does not exist.", name, serial);
                return;
            }

            if (!foundBook.Available())
            {
                Console.WriteLine("Sorry! '{0}' is already rented. System cannot remove a rented book!", name);
                return;
            }

            BookStoreBooksList.Remove(foundBook);
            Console.WriteLine("The book: '{0}', Serial: {1}, is successfully removed from the bookstore.", name, serial);
        }

        // Rent a book to a reader
        public void RentABook(string readerName, string bookName)
        {
            // ---------------- Hints for Learners ----------------
            // 1. Find the reader in BookStoreReadersList
            // 2. If reader not found, display error
            // 3. Find the first available copy of the book
            // 4. If no copy is available, display error
            // 5. Otherwise, call reader.RentABook(book)
            // -----------------------------------------------------

            Reader foundReader = BookStoreReadersList.FirstOrDefault(reader => reader.ReaderName == readerName);

            if (foundReader == null)
            {
                Console.WriteLine("{0}, you are not a registered reader of this bookstore!", readerName);
                return;
            }

            Book availableBook = BookStoreBooksList.FirstOrDefault(book => book.BookName == bookName && book.Available());

            if (availableBook == null)
            {
                Console.WriteLine("Sorry! The book '{0}' is not Available for renting.", bookName);
                return;
            }

            foundReader.RentABook(availableBook);
        }

        // Return a book from a reader
        public void ReturnABook(string readerName, string bookName, int serial)
        {
            // ---------------- Hints for Learners ----------------
            // 1. Find the reader in BookStoreReadersList
            // 2. If reader not found, display error
            // 3. Find the book in reader.readersBookList by name & serial
            // 4. If not found, display error
            // 5. Otherwise, call reader.ReturnABook(book)
            // -----------------------------------------------------

            Reader foundReader = BookStoreReadersList.FirstOrDefault(reader => reader.ReaderName == readerName);

            if (foundReader == null)
            {
                Console.WriteLine("{0}, you are not a registered reader of this bookstore!", readerName);
                return;
            }

            Book rentedBook = foundReader.ReadersBookList.FirstOrDefault(book => book.BookName == bookName && book.Serial == serial);

            if (rentedBook == null)
            {
                Console.WriteLine("Error! The book '{0}' with serial '{1}' was not rented by {2}.", bookName, serial, readerName);
                return;
            }

            foundReader.ReturnABook(rentedBook);
        }

        // Display all available books in the store
        public void ShowBookInformation()
        {
            // ---------------- Hints for Learners ----------------
            // 1. Loop through BookStoreBooksList
            // 2. Display each book using book.BookInfo()
            // 3. Optional: Only display books that are available
            // -----------------------------------------------------

            Console.WriteLine("The bookstore has the following books available:");

            foreach (var book in BookStoreBooksList)
            {
                if (book.Available())
                {
                    book.BookInfo();
                }
            }
        }

        // Display all readers and their rented books (already implemented)
        public void ShowReaderInformation()
        {
            if (BookStoreReadersList.Count == 0)
            {
                Console.WriteLine("The bookstore currently have no readers.");
                return;
            }
            Console.WriteLine("The bookstore has the following readers:");
            foreach (var reader in BookStoreReadersList)
            {
                reader.ReaderInfo();
            }
            Console.WriteLine();
        }
    }
}

/* ------------------- Main Program -------------------
   Do not change the Main method Code.
   This will automatically test your bookstore system.
----------------------------------------------------- */
internal class Program
{
    static void Main(string[] args)
    {
        //Do not change the Main method Code.
        BookStore bs = new BookStore();

        // Add readers
        bs.AddAReader("Mahbub Murshed");
        bs.AddAReader("David Alwright");
        bs.AddAReader("Susan Harper");

        // Add books
        bs.AddABook("Object Oriented Programming", 1);
        bs.AddABook("Object Oriented Programming", 2);
        bs.AddABook("Object Oriented Programming", 3);
        bs.AddABook("Programming Fundamentals", 1);
        bs.AddABook("Programming Fundamentals", 2);
        bs.AddABook("Let us C#", 1);
        bs.AddABook("Programming is Fun", 1);
        bs.AddABook("Life is Beautiful", 1);
        bs.AddABook("Let's Talk About the Logic", 1);
        bs.AddABook("How to ace a job interview", 1);

        bs.ShowBookInformation();
        bs.ShowReaderInformation();

        // Rent books
        bs.RentABook("Salma Hayek", "Object Oriented Programming");

        bs.RentABook("Mahbub Murshed", "Object Oriented Programming");
        bs.RentABook("Mahbub Murshed", "How to ace a job interview");
        bs.RentABook("Mahbub Murshed", "Life is Beautiful");

        Console.WriteLine();

        bs.RentABook("David Alwright", "Object Oriented Programming");
        bs.RentABook("David Alwright", "Programming Fundamentals");
        Console.WriteLine();

        bs.RentABook("Susan Harper", "Let's Talk About the Logic");
        bs.RentABook("Susan Harper", "How to ace a job interview");
        Console.WriteLine();

        bs.ShowBookInformation();
        bs.ShowReaderInformation();

        // Return books
        bs.ReturnABook("Mahbub Murshed", "Object Oriented Programming", 1);
        bs.RentABook("Mahbub Murshed", "Life is Beautiful");
        Console.WriteLine();

        bs.ReturnABook("Mahbub Murshed", "Programming Fundamentals", 1);

        // Remove books & readers
        bs.RemoveABook("Let us C#", 1);
        bs.RemoveABook("Let's Talk About the Logic", 1);

        bs.ShowReaderInformation();
        bs.RemoveAReader("Mahbub Murshed");

        bs.ShowBookInformation();
        bs.ShowReaderInformation();

        Console.Read();
    }
}


/*
//When implemented correctly, 
//the program should produce the following exact output:

Book: 'Object Oriented Programming', Serial: 1 added to bookstore.
Book: 'Object Oriented Programming', Serial: 2 added to bookstore.
Book: 'Object Oriented Programming', Serial: 3 added to bookstore.
Book: 'Programming Fundamentals', Serial: 1 added to bookstore.
Book: 'Programming Fundamentals', Serial: 2 added to bookstore.
Book: 'Let us C#', Serial: 1 added to bookstore.
Book: 'Programming is Fun', Serial: 1 added to bookstore.
Book: 'Life is Beautiful', Serial: 1 added to bookstore.
Book: 'Let's Talk About the Logic', Serial: 1 added to bookstore.
Book: 'How to ace a job interview', Serial: 1 added to bookstore.
The bookstore has the following books available:
Book Name: Object Oriented Programming, Serial: 1, Status: Available
Book Name: Object Oriented Programming, Serial: 2, Status: Available
Book Name: Object Oriented Programming, Serial: 3, Status: Available
Book Name: Programming Fundamentals, Serial: 1, Status: Available
Book Name: Programming Fundamentals, Serial: 2, Status: Available
Book Name: Let us C#, Serial: 1, Status: Available
Book Name: Programming is Fun, Serial: 1, Status: Available
Book Name: Life is Beautiful, Serial: 1, Status: Available
Book Name: Let's Talk About the Logic, Serial: 1, Status: Available
Book Name: How to ace a job interview, Serial: 1, Status: Available
The bookstore has the following readers:
Reader Mahbub Murshed rented the following books:
No books rented yet!
Reader David Alwright rented the following books:
No books rented yet!
Reader Susan Harper rented the following books:
No books rented yet!

Salma Hayek, you are not a registered reader of this bookstore!
Book: 'Object Oriented Programming' successfully rented by Mahbub Murshed.
Book: 'How to ace a job interview' successfully rented by Mahbub Murshed.
You cannot rent more than two books!

Book: 'Object Oriented Programming' successfully rented by David Alwright.
Book: 'Programming Fundamentals' successfully rented by David Alwright.

Book: 'Let's Talk About the Logic' successfully rented by Susan Harper.
Sorry! The book 'How to ace a job interview' is not Available for renting.

The bookstore has the following books available:
Book Name: Object Oriented Programming, Serial: 3, Status: Available
Book Name: Programming Fundamentals, Serial: 2, Status: Available
Book Name: Let us C#, Serial: 1, Status: Available
Book Name: Programming is Fun, Serial: 1, Status: Available
Book Name: Life is Beautiful, Serial: 1, Status: Available
The bookstore has the following readers:
Reader Mahbub Murshed rented the following books:
Book Name: Object Oriented Programming, Serial: 1, Status: Rented
Book Name: How to ace a job interview, Serial: 1, Status: Rented
Reader David Alwright rented the following books:
Book Name: Object Oriented Programming, Serial: 2, Status: Rented
Book Name: Programming Fundamentals, Serial: 1, Status: Rented
Reader Susan Harper rented the following books:
Book Name: Let's Talk About the Logic, Serial: 1, Status: Rented

Book: 'Object Oriented Programming' successfully returned by Mahbub Murshed.
Book: 'Life is Beautiful' successfully rented by Mahbub Murshed.

Error! The book 'Programming Fundamentals' with serial '1' was not rented by Mahbub Murshed.
The book: 'Let us C#', Serial: 1, is successfully removed from the bookstore.
Sorry! 'Let's Talk About the Logic' is already rented. System cannot remove a rented book!
The bookstore has the following readers:
Reader Mahbub Murshed rented the following books:
Book Name: How to ace a job interview, Serial: 1, Status: Rented
Book Name: Life is Beautiful, Serial: 1, Status: Rented
Reader David Alwright rented the following books:
Book Name: Object Oriented Programming, Serial: 2, Status: Rented
Book Name: Programming Fundamentals, Serial: 1, Status: Rented
Reader Susan Harper rented the following books:
Book Name: Let's Talk About the Logic, Serial: 1, Status: Rented

Book: 'How to ace a job interview' successfully returned by Mahbub Murshed.
Book: 'Life is Beautiful' successfully returned by Mahbub Murshed.
Reader Mahbub Murshed successfully removed from bookstore.
The bookstore has the following books available:
Book Name: Object Oriented Programming, Serial: 1, Status: Available
Book Name: Object Oriented Programming, Serial: 3, Status: Available
Book Name: Programming Fundamentals, Serial: 2, Status: Available
Book Name: Programming is Fun, Serial: 1, Status: Available
Book Name: Life is Beautiful, Serial: 1, Status: Available
Book Name: How to ace a job interview, Serial: 1, Status: Available
The bookstore has the following readers:
Reader David Alwright rented the following books:
Book Name: Object Oriented Programming, Serial: 2, Status: Rented
Book Name: Programming Fundamentals, Serial: 1, Status: Rented
Reader Susan Harper rented the following books:
Book Name: Let's Talk About the Logic, Serial: 1, Status: Rented

*/