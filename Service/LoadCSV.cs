using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using LibraryManagementSystem.model;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Service
{
    class LoadCSV
    {
        public List<CSVBook> ReadCsv(string pathOfFile)
        {
            List<CSVBook> books = new List<CSVBook>();

            using (var reader = new StreamReader(pathOfFile)) 
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    CSVBook record = csv.GetRecord<CSVBook>();
                    books.Add(record);
                }
            }

            return books;
        }
    }
}
