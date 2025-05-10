using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.model
{
    [Table("book")]
    class Book
    {
        [Column("book_id")]
        public Guid id { get; set; }

        [Column("book_title")]  
        public string title { get; set; }

        [Column("book_author")]
        public string author { get; set; }

        [Column("book_genre")]
        public string genre { get; set; }

        [Column("book_quantity")]
        public int quantity { get; set; }

        [Column("book_lent_quantity")]
        public int lentQuantity { get; set; }

        /*
         * 
         * Only for recommendations (if false it means it's not been bought by the library yet)
         * 
         */
        [Column("book_to_buy")]
        public string available { get; set; }

        public void toString()
        {
            Console.WriteLine(
                "Id: " + this.id + ", " +
                "Title: " + this.title + ", " + 
                "Author: " + this.author + ", " +
                "Genre: " + this.genre + ", " +
                "Quantity: " + this.quantity.ToString() + ", " +
                "Lent quantity: " + this.lentQuantity.ToString() + ", " +
                "Available:" + this.available
            );
        }
    }
}
