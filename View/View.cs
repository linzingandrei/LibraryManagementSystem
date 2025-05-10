using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using LibraryManagementSystem.model;
using LibraryManagementSystem.Service;

namespace LibraryManagementSystem.view
{
    class View
    {
        Service.Service service;

        public View(Service.Service service)
        {
            this.service = service;
        }

        public void printMenu()
        {
            Console.WriteLine("********************************************************");
            Console.WriteLine("********************************************************");
            Console.WriteLine("** 1. Add a new book                                  **");
            Console.WriteLine("** 2. Update a book                                   **");
            Console.WriteLine("** 3. Delete a book                                   **");
            Console.WriteLine("**                                                    **");
            Console.WriteLine("** 4. Display a book                                  **");
            Console.WriteLine("** 5. Display all books                               **");
            Console.WriteLine("**                                                    **");
            Console.WriteLine("** 6. Search for a book                               **");
            Console.WriteLine("** 7. Book lending menu                               **");
            Console.WriteLine("**                                                    **");
            Console.WriteLine("** 8. Get recommendations for new books to add        **");
            Console.WriteLine("**                                                    **");
            Console.WriteLine("** 9. Print the menu again                            **");
            Console.WriteLine("** 10. Exit                                           **");
            Console.WriteLine("********************************************************");
            Console.WriteLine("********************************************************");
        }

        public void run()
        {
            printMenu();

            Console.Write(">");

            while (true) {
                string? userInput = Console.ReadLine();

                if (userInput != null)
                {
                    userInput = userInput.Replace(" ", "");
                }

                switch (userInput)
                {
                    case "1":
                        addBook();
                        break;

                    case "2":
                        updateBook();
                        break;

                    case "3":
                        deleteBook();
                        break;

                    case "4":
                        getDetailsAboutABook();
                        break;

                    case "5":
                        getAllBooks();
                        break;

                    case "6":
                        searchForABook();
                        printMenu();
                        break;

                    case "7":
                        Console.Clear();
                        runLend();
                        Console.Clear();
                        printMenu();
                        break;

                    case "8":
                        Console.Clear();
                        recommendBooks();
                        Console.Clear();
                        printMenu();
                        break;

                    case "9":
                        printMenu();
                        break;

                    case "10":
                        Console.WriteLine("Exiting...");
                        goto exit_program;

                    default:
                        Console.WriteLine("Unknown command");
                        break;

                }

                Console.Write(">");
            }

            exit_program:;
        }

        void recommendBooks()
        {
            Console.WriteLine("Please input keywords(on the same line, separated by a space) that you want the recommendation system to take into mind (minimum of 1)");
            Console.Write(">");

            goto jump;

            error_no_keywords_given:
                Console.WriteLine("Please input at least 1 keyword!!!");
                Console.Write(">");

            jump:
                string? inputKeywords = Console.ReadLine();

            if (inputKeywords == null)
            {
                Console.WriteLine("No input given!");
                goto error_no_keywords_given;
            }

            if (inputKeywords.Split(' ').Length < 1)
            {
                goto error_no_keywords_given;
            }
            else
            {
                var keywords = new List<string>();
                foreach(var keyword in inputKeywords.Split(' '))
                {
                    keywords.Add(keyword);
                }

                Console.WriteLine("Please input weights(on the same line, separated by a space) that you want the recommendation system to take into mind (in the order of the keywords you gave). Otherwise, they will default to 1");
                Console.Write(">");

                string? inputWeights = Console.ReadLine();

                var weights = new List<float>();
                if (inputWeights != null && inputWeights != "")
                {
                    foreach (var number in inputWeights.Split(' '))
                    {
                        weights.Add(int.Parse(number));
                    }
                }

                var books = this.service.getTop10RecommendedBooks(keywords, weights);
                
                Console.WriteLine();

                foreach (var book in books)
                {
                    Console.WriteLine($"{book.title}");
                }

                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("================================================================================================================================================================");
                    Console.WriteLine("Now, if you plan to add a book to the library press the number key corresponding to the line of the book (start the row counting with 1).");
                    Console.WriteLine("Also, give a number corresponding to the quantity of books you would like to get (default is 1)");
                    Console.WriteLine("Otherwise, this menu will close");

                
                    Console.Write(">");
                    string? userInput = Console.ReadLine();

                    Console.WriteLine($"{userInput}");

                    if (userInput != null && userInput != "")
                    {
                        if (!(int.Parse(userInput) > 10 || !Regex.IsMatch(userInput, @"^\d+$")))
                        {
                            Console.WriteLine("Give quantity if you want, press enter directly if you want the default [1]");
                            Console.Write(">");

                            string? userQuantInput = Console.ReadLine();
                            int userQuant = 1;

                            if (userQuantInput != null && userQuantInput != "")
                            {
                                userQuant = int.Parse(userQuantInput);
                            }

                            int digitOfBookToAdd = int.Parse(userInput);

                            Book bookToAdd = books[digitOfBookToAdd - 1];
                            this.service.addBook(
                                books[digitOfBookToAdd - 1].title,
                                books[digitOfBookToAdd - 1].author,
                                books[digitOfBookToAdd - 1].genre,
                                userQuant
                            );
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        void addBook()
        {
            issue_in_title_for_add:
                Console.WriteLine("Title:");
                Console.Write(">");

            string? title = Console.ReadLine();

            if (title == null)
            {
                Console.WriteLine("Issue in input!");
                goto issue_in_title_for_add;
            }
            else
            {
                issue_in_author_for_add:
                    Console.WriteLine("Author:");
                    Console.Write(">");

                string? author = Console.ReadLine();

                if (author == null)
                {
                    Console.WriteLine("Issue in author!");
                    goto issue_in_author_for_add;
                }
                else
                {
                    issue_in_genre_for_add:
                        Console.WriteLine("Genre:");
                        Console.Write(">");

                    string? genre = Console.ReadLine();

                    if (genre == null)
                    {
                        Console.WriteLine("Issue in genre!");
                        goto issue_in_genre_for_add;
                    }
                    else
                    {
                        issue_in_quantity_for_add:
                            Console.WriteLine("Quantity:");
                            Console.Write(">");

                        int quantity = -1;
                        string? quantity_input = Console.ReadLine();

                        if (quantity_input == null)
                        {
                            Console.WriteLine("Issue in quantity (input not a valid number)!");
                            goto issue_in_quantity_for_add;
                        }
                        else if (!Regex.IsMatch(quantity_input, @"^\d+$"))
                        {
                            Console.WriteLine("Issue in quantity (input not a valid number)!");
                            goto issue_in_quantity_for_add;
                        }
                        else
                        {
                            quantity = int.Parse(quantity_input);

                            int returnValueForAdd = this.service.addBook(title, author, genre, quantity);

                            if (returnValueForAdd == 1)
                            {
                                Console.WriteLine("Book added successfully!");
                            }
                            else
                            {
                                Console.WriteLine("There was an issue adding your book");
                            }
                        }
                    }
                }
            }
        }

        void updateBook()
        {
            Console.WriteLine("Enter the id of the book you want to be updated:");
            Console.Write(">");

            string? idInput = Console.ReadLine();

            if (idInput == null)
            {
                Console.WriteLine("The id cannot be null!");
            }

            Console.WriteLine("Enter new title(press enter if you don't want it changed):");
            Console.Write(">");
            string? newTitle = Console.ReadLine();

            Console.WriteLine("Enter new author(press enter if you don't want it changed):");
            Console.Write(">");
            string? newAuthor = Console.ReadLine();

            Console.WriteLine("Enter new genre(press enter if you don't want it changed):");
            Console.Write(">");
            string? newGenre = Console.ReadLine();

            Console.WriteLine("Enter new quantity(press enter if you don't want it changed):");
            Console.Write(">");
            int newQuantity = -1;

            issue_in_quantity_for_update:
                string? newQuantity_input = Console.ReadLine();

            if (newQuantity_input != "")
            {
                if (!Regex.IsMatch(newQuantity_input, @"^\d+$"))
                {
                    Console.WriteLine("Issue in quantity (input not a valid number)!");
                    goto issue_in_quantity_for_update;
                }

                Console.WriteLine("Please enter a valid number");
                Console.Write(">");

                newQuantity = int.Parse(newQuantity_input);
            }

            Console.WriteLine("Change availability too?");
            Console.WriteLine("y(yes) or n(no)");

            Console.Write(">");
            string? userInputForAvailability = Console.ReadLine();
            int returnValueForUpdate;

            if (userInputForAvailability == "yes" || userInputForAvailability == "y")
            {
                issue_in_availability_for_update:
                    Console.WriteLine("Enter new availability");
                    Console.Write(">");

                string? newAvailability = Console.ReadLine();

                if (newAvailability == null)
                {
                    Console.WriteLine("Issue in availability!");
                    goto issue_in_availability_for_update;
                }
                else
                {
                    returnValueForUpdate = this.service.updateBook(Guid.Parse(idInput), newTitle, newAuthor, newGenre, newQuantity, userInputForAvailability);
                }
            }
            else
            {
                returnValueForUpdate = this.service.updateBook(Guid.Parse(idInput), newTitle, newAuthor, newGenre, newQuantity, "");
            }

            if (returnValueForUpdate == 1)
            {
                Console.WriteLine("Book updated successfully!");
            }
            else
            {
                Console.WriteLine("There was an issue updating your book");
            }
        }

        void deleteBook()
        {
            Console.WriteLine("Enter the id of the book you want to be deleted:");
            Console.Write(">");

            string? idInputForDeletion = Console.ReadLine();

            if (idInputForDeletion == null)
            {
                Console.WriteLine("The id cannot be null!");
            }
            else
            {
                int returnValueForDelete = this.service.removeBook(Guid.Parse(idInputForDeletion));

                if (returnValueForDelete == 1)
                {
                    Console.WriteLine("Are you sure you want to delete this book?\nType y(yes) or n(no)");

                    string? response = Console.ReadLine();

                    if (response != null)
                    {
                        if (response.Equals("y") || response.Equals("yes"))
                        {
                            Console.WriteLine("Book was deleted successfully!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Something went wrong while deleting the book!");
                }
            }
        }

        void getDetailsAboutABook()
        {
            not_valid_guid:
                Console.WriteLine("Enter the id of the book you want to see:");
                Console.Write(">");

            string? idInputForPrintingABook = Console.ReadLine();

            if (idInputForPrintingABook == null || idInputForPrintingABook == "")
            {
                Console.WriteLine("The id cannot be null!");
            }
            else if (!Guid.TryParse(idInputForPrintingABook, out Guid result))
            {
                Console.WriteLine("Not a valid guid!");
                goto not_valid_guid;
            }
            else
            {
                Book? book = this.service.getBookById(Guid.Parse(idInputForPrintingABook));

                if (book != null)
                {
                    book.toString();
                }
                else
                {
                    Console.WriteLine("Book with that id doesn't exist!");
                }
            }
        }

        void getAllBooks()
        {
            List<Book>? books = this.service.getBooks();

            if (books == null)
            {
                Console.WriteLine("There are no books!");
            }
            else
            {
                foreach (Book book in books)
                {
                    book.toString();
                }
            }
        }

        void searchForABook()
        {
            exit_search:
                Console.WriteLine("Choose what to search by or exit:");
                Console.WriteLine("1. Title");
                Console.WriteLine("2. Author");
                Console.WriteLine("3. Genre");
                Console.WriteLine("4. Quantity");
                Console.WriteLine("5. Availability");
                Console.WriteLine("6. Exit search");

            while (true)
            {
                issue:
                    string? searchOption_ = Console.ReadLine();

                if (searchOption_ == null || searchOption_ == "")
                {
                    Console.WriteLine("Issue in quantity (input not a valid number)!");
                    goto issue;
                }
                else if (!Regex.IsMatch(searchOption_, @"^\d+$"))
                {
                    Console.WriteLine("Issue in quantity (input not a valid number)!");
                    goto issue;
                }

                int searchOption = int.Parse(searchOption_);
                
                StringBuilder inputBuffer = new StringBuilder();

                switch (searchOption)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        Console.Write(">");
                        while (true)
                        {
                            if (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    goto exit_search;
                                }
                                else if (keyInfo.Key == ConsoleKey.Backspace)
                                {
                                    if (inputBuffer.Length > 0)
                                    {
                                        inputBuffer.Remove(inputBuffer.Length - 1, 1);
                                    }
                                }
                                else
                                {
                                    inputBuffer.Append(keyInfo.KeyChar);
                                }

                                Console.Clear();
                                Console.WriteLine("Press enter to exit");
                                Console.WriteLine();
                                Console.Write(">");
                                Console.WriteLine(inputBuffer.ToString());

                                string searchString = inputBuffer.ToString();

                                if (!string.IsNullOrEmpty(searchString))
                                {
                                    if (searchOption == 4)
                                    {
                                    error_not_number:
                                        foreach (char c in searchString)
                                        {
                                            if (c != '0' && c != '1' && c != '2' && c != '3' && c != '4' && c != '5' && c != '6' && c != '7' && c != '8' && c != 9)
                                            {
                                                Console.WriteLine("Not a valid number!");
                                                goto error_not_number;
                                            }
                                        }
                                    }
                                    List<Book>? books = service.searchBook(searchString, searchOption);

                                    if (books != null && books.Count > 0)
                                    {
                                        Console.WriteLine("\nResults:");
                                        foreach (Book book in books)
                                        {
                                            book.toString();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nNo results.");
                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(20);
                            }
                        }

                    case 6:
                        Console.WriteLine("Exiting the search...");
                        goto exit;

                    default:
                        Console.WriteLine("Unknown option!");
                        break;
                }
            }

            exit:
                Console.Clear();
        }

        public void printLendMenu()
        {
            Console.WriteLine("********************************************************");
            Console.WriteLine("********************************************************");
            Console.WriteLine("** 1. See lent books                                  **");
            Console.WriteLine("** 2. Lend a book                                     **");
            Console.WriteLine("** 3. Return book                                     **");
            Console.WriteLine("**                                                    **");
            Console.WriteLine("** 4. Search for books                                **");
            Console.WriteLine("** 5. Display all books                               **");
            Console.WriteLine("**                                                    **");
            Console.WriteLine("** 6. Print the menu again                            **");
            Console.WriteLine("** 7. Return to the main menu                         **");
            Console.WriteLine("********************************************************");
            Console.WriteLine("********************************************************");
        }

        void runLend()
        {
            printLendMenu();

            while (true)
            {
                Console.Write(">");
                string? userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        List<Book>? lentBooks = this.service.getLentBooks();

                        if (lentBooks == null || lentBooks.Count() == 0)
                        {
                            Console.WriteLine("No lent books.");
                        }
                        else
                        {
                            foreach (Book book in lentBooks)
                            {
                                book.toString();
                            }
                        }

                        break;

                    case "2":
                        Console.WriteLine("Enter the id of the book to lend:");
                        Console.Write(">");
                        string? idForLend = Console.ReadLine();

                        if (idForLend == null || idForLend == "")
                        {
                            Console.WriteLine("Not a valid id!");
                        }
                        else
                        {
                            Console.WriteLine("Enter the amount to lend:");
                            Console.Write(">");
                            string? amount_ = Console.ReadLine();

                            if (amount_ == null || amount_ == "")
                            {
                                Console.WriteLine("Not a valid amount!");
                            }
                            else if (Regex.IsMatch(amount_, @"^\d+$"))
                            {

                                int amount = int.Parse(amount_);

                                int returnValueForLend = this.service.lendBooks(Guid.Parse(idForLend), amount);
                                //Console.WriteLine(returnValueForLend);

                                if (returnValueForLend == 0)
                                {
                                    Console.WriteLine($"Book could not be lent!");
                                }
                                else if (returnValueForLend > 1)
                                {
                                    Console.WriteLine($"{returnValueForLend - 1} books could not be lent!");
                                }
                                else
                                {
                                    Console.WriteLine("Book lent successfully!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Not a valid number!");
                            }
                        }

                        break;

                    case "3":
                        Console.WriteLine("Enter the id of the book to return:");
                        Console.Write(">");
                        string? idForReturn = Console.ReadLine();

                        if (idForReturn == null || idForReturn == "")
                        {
                            Console.WriteLine("Not a valid id!");
                        }
                        else
                        {
                            Console.WriteLine("Enter the amount to return:");
                            Console.Write(">");
                            string? amount_ = Console.ReadLine();

                            if (amount_ == null || amount_ == "")
                            {
                                Console.WriteLine("Not a valid amount!");
                            }
                            else if (Regex.IsMatch(amount_, @"^\d+$"))
                            {

                                int amount = int.Parse(amount_);

                                int returnValueForReturn = this.service.returnBooks(Guid.Parse(idForReturn), amount);

                                //Console.WriteLine(returnValueForReturn);

                                if (returnValueForReturn == 0)
                                {
                                    Console.WriteLine("No books could be returned!");
                                }
                                else if (returnValueForReturn > 1)
                                {
                                    Console.WriteLine($"{returnValueForReturn - 1} books could not be returned!");
                                }
                                else
                                {
                                    Console.WriteLine("Books returned succesfully!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Not a valid number!");
                            }
                        }

                        break;

                    case "4":
                        searchForABook();
                        break;

                    //TODO: maybe remove this after case 4 is fully implemented
                    case "5":
                        List<Book>? books = this.service.getBooks();

                        if (books == null)
                        {
                            Console.WriteLine("No books.");
                        }
                        else
                        {
                            foreach(Book book in books)
                            {
                                book.toString();
                            }
                        }

                        break;

                    case "6":
                        printLendMenu();
                        break;

                    case "7":
                        Console.WriteLine("Exiting...");
                        goto exit;

                    default:
                        Console.WriteLine("Unknown command!");
                        break;
                }
            }

            exit:;
        }
    }
}
