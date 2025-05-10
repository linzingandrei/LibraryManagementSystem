using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.model;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryManagementSystem.repository
{
    class Repository
    {
        AppDbContext appDbContext;

        public Repository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public int addBook(string title, string author, string genre, int quantity)
        {
            var book = new Book
            {
                id = Guid.NewGuid(),
                title = title,
                author = author,
                genre = genre,
                quantity = quantity,
                lentQuantity = 0,
                available = "true"
            };

            try
            {
                this.appDbContext.Books.Add(book);
                this.appDbContext.SaveChanges();

                return 1;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"Failed to add book: {exception.Message}");
                return 0;
            }
        }

        public Book? getBookById(Guid id)
        {
            return this.appDbContext.Books.Find(id);
        }

        public List<Book>? getBooks()
        {
            var books =  this.appDbContext.Books.ToList();
            books.Sort((a, b) => a.title.CompareTo(b.title));
            return books;
        }

        public int updateBook(Guid id, string newTitle, string newAuthor, string newGenre, int newQuantity, string newAvailability)
        {
            Book? bookToUpdate = this.appDbContext.Books.Find(id);

            if (bookToUpdate == null)
            {
                return 0;
            }

            if (bookToUpdate != null)
            {
                if (newAvailability != null && newAvailability != "")
                {
                    bookToUpdate.title = newTitle;
                    bookToUpdate.author = newAuthor;
                    bookToUpdate.genre = newGenre;
                    bookToUpdate.quantity = newQuantity;
                    bookToUpdate.available = newAvailability;
                }
                else
                {
                    bookToUpdate.title = newTitle;
                    bookToUpdate.author = newAuthor;
                    bookToUpdate.genre = newGenre;
                    bookToUpdate.quantity = newQuantity;
                }
            }

            try
            {
                this.appDbContext.SaveChanges();

                return 1;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"Failed to update book: {exception.Message}");
                return 0;
            }
        }

        public int removeBook(Guid id)
        {
            Book? bookToDelete = this.appDbContext.Books.Find(id);

            if (bookToDelete == null)
            {
                return 0;
            }

            try
            {
                this.appDbContext.Remove(bookToDelete);
                this.appDbContext.SaveChanges();

                return 1;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"Failed to delete book: {exception.Message}");
                return 0;
            }
        }

        public List<Book> searchBook(string chars, int searchBy)
        {
            string column = "title";

            switch(searchBy)
            {
                case 1:
                    column = "title";
                    break;

                case 2:
                    column = "author";
                    break;

                case 3:
                    column = "genre";
                    break;

                case 4:
                    column = "quantity";
                    break;

                case 5:
                    column = "availability";
                    break;
            }

            var results = appDbContext.Books.Where($"{column}.Contains(@0)", chars).ToList();
            results.Sort((a, b) => a.title.CompareTo(b.title));

            return results;
        }

        public void lendBooks(Guid id, int amount)
        {
            Book? book = this.appDbContext.Books.Find(id);

            book.lentQuantity = amount;
            this.appDbContext.SaveChanges();
        }

        public void returnBooks(Guid id, int amount)
        {
            Book? book = this.appDbContext.Books.Find(id);

            book.lentQuantity = amount;
            this.appDbContext.SaveChanges();
        }
    }
}
