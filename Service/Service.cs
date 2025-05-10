using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibraryManagementSystem.model;
using LibraryManagementSystem.repository;

namespace LibraryManagementSystem.Service
{
    class Service
    {
        Repository repository;

        public Service(Repository repository)
        {
            this.repository = repository;
        }

        public int addBook(string title, string author, string genre, int quantity)
        {
            List<Book>? books = this.repository.getBooks();

            foreach (Book book in books)
            {
                if (book.title == title)
                {
                    return 0;
                }
            }

            int returnValue = this.repository.addBook(title, author, genre, quantity);
            return returnValue;
        }

        public Book? getBookById(Guid id)
        {
            Book? book = this.repository.getBookById(id);
            return book;
        }

        public List<Book>? getBooks()
        {
            List<Book>? books = this.repository.getBooks();
            return books;
        }

        public List<Book>? getLentBooks()
        {
            List<Book>? books = this.repository.getBooks().Where(book => book.lentQuantity > 0).ToList();
            return books;
        }

        public int updateBook(Guid id, string title, string author, string genre, int quantity, string availability)
        {
            Book? book = this.repository.getBookById(id);

            if (book == null)
            {
                return 0;
            }

            string newTitle = book.title;
            if (title != book.title && title != "")
            {
                newTitle = title;
            }

            string newAuthor = book.author;
            if (author != book.author && author != "" && !Regex.IsMatch(author, @"^\d+$"))
            {
                newAuthor = author;
            }

            string newGenre = book.genre;
            if (genre != book.genre && genre != "" && !Regex.IsMatch(genre, @"^\d+$"))
            {
                newGenre = genre;
            }

            int newQuantity = book.quantity;
            if (quantity != book.quantity && quantity != -1)
            {
                newQuantity = quantity;
            }

            string newAvailability = book.available;
            if (availability != book.available && availability != "")
            {
                newAvailability = availability;
            }

            Console.WriteLine($"{newTitle}");

            int returnValue = this.repository.updateBook(id, newTitle, newAuthor, newGenre, newQuantity, newAvailability);
            
            return returnValue;
        }

        public int removeBook(Guid id)
        {
            int returnValue = this.repository.removeBook(id);
            return returnValue;
        }

        public List<Book>? searchBook(string chars, int searchBy)
        {
            List<Book>? books = this.repository.searchBook(chars, searchBy);

            return books;
        }

        public int lendBooks(Guid id, int amount)
        {
            Book? book = this.repository.getBookById(id);

            if (book == null)
            {
                return 0;
            }

            if (book.quantity - (book.lentQuantity + amount) < 0)
            {
                book.lentQuantity = book.quantity;
                this.repository.lendBooks(id, book.lentQuantity);
                return Math.Abs(book.quantity - (book.lentQuantity + amount)) + 1;
            }

            book.lentQuantity += amount;

            this.repository.lendBooks(id, book.lentQuantity);

            return 1;
        }

        public int returnBooks(Guid id, int amount)
        {
            Book? book = this.repository.getBookById(id);

            if (book == null)
            {
                return 0;
            }

            if (amount > book.quantity)
            {
                book.lentQuantity = 0;
                this.repository.returnBooks(id, book.lentQuantity);
                return Math.Abs(book.quantity - amount) + 1;
            }

            if (book.lentQuantity - amount < 0)
            {
                book.lentQuantity = 0;
                return Math.Abs(amount - book.lentQuantity);
            }

            book.lentQuantity -= amount;
            this.repository.returnBooks(id, book.lentQuantity);
            return 1;
        }

        public List<Book> getTop10RecommendedBooks(List<string> words, List<float> weights)
        {
            var bookRecommendation = new BookRecommendation();

            if (words.Count > weights.Count)
            {
                for (int i = weights.Count; i < words.Count; i++)
                {
                    weights.Add(1);
                }
            }
            else if (words.Count <  weights.Count)
            {
                for (int i = 0; i < words.Count; i++)
                {
                    weights.RemoveAt(weights.Count - i);
                }
            }

            var keywordsAndWeights = new Dictionary<string, float>();
            for (int i = 0; i <  words.Count; i++)
            {
                keywordsAndWeights[words[i]] = weights[i];
            }

            IEnumerable<(Book, float)> recommendations = bookRecommendation.getTop10RecommendedBooks(keywordsAndWeights);

            List<Book> onlyBooksFromRecommendations = new List<Book>();
            foreach (var (book, similarity) in recommendations)
            {
                onlyBooksFromRecommendations.Add(book);
            }
            return onlyBooksFromRecommendations;
        }
    }
}
