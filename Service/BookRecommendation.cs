using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.model;
using LibraryManagementSystem.Service;

namespace LibraryManagementSystem.Service
{
    class BookRecommendation
    {
        public float[] WeightedAverageVectors(Dictionary<string, float> wordWeights, Dictionary<string, float[]> vectors, int dimension)
        {
            var averageVector = new float[dimension];
            float totalWeight = 0.0f;

            foreach (var pairThing in wordWeights)
            {
                string word = pairThing.Key;
                float weight = pairThing.Value;

                if (vectors.TryGetValue(word, out var vector))
                {
                    for (int i = 0; i < dimension; i++)
                    {
                        averageVector[i] += vector[i] * weight;
                    }

                    totalWeight += weight;
                }
            }

            if (totalWeight > 0.0f)
            {
                for (int i = 0; i < dimension; i++)
                {
                    averageVector[i] /= totalWeight;
                }
            }

            return averageVector;
        }

        public static float CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
            {
                return 0.0f;
            }

            float dot = 0.0f;
            float normA = 0.0f;
            float normB = 0.0f;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dot += vectorA[i] * vectorB[i];
                normA += vectorA[i] * vectorA[i];
                normB += vectorB[i] * vectorB[i];
            }

            if (normA == 0.0f || normB == 0.0f)
            {
                return 0.0f;
            }

            return dot / (float)(Math.Sqrt(normA) * Math.Sqrt(normB));
        }

        /*
         * Not that many books loaded. Could add more csv files found in the archivedirectory if needed but works well with only this many.
         * Could also add more filters such as ratings.
         */
        public IEnumerable<(Book, float)> getTop10RecommendedBooks(Dictionary<string, float> keywordsAndWeights)
        {
            /*
             * Keep dimension 50 because glove.6B.50D.txt has vectors of 50 floats.
             */
            int dimension = 50;
            string word2vecFilePath = "..\\..\\..\\Service\\files\\glove.6B.50d.txt";
            string bookCSVPath = "..\\..\\..\\Service\\files\\archive\\book600k-700k.csv";

            var wordVectors = Word2VecLoader.LoadWordVectors(word2vecFilePath, dimension);

            var queryWeights = new Dictionary<string, float>();

            foreach (var (keyword, importance) in keywordsAndWeights)
            {
                queryWeights.Add(keyword, importance);
            };

            var queryVector = WeightedAverageVectors(queryWeights, wordVectors, dimension);


            var loader = new LoadCSV();
            var books = loader.ReadCsv(bookCSVPath);

            List<(Book book, float similarity)> booksWithScores = new List<(Book book, float similarity)>();

            foreach (var book in books)
            {
                book.Description = book.Description.ToLower();

                /*
                 * Replace useless things from the csv (maybe there are some I missed) 
                 */
                string cleaned = book.Description
                                     .Replace("<p>", "")
                                     .Replace("<p />", "")
                                     .Replace("<i>", "")
                                     .Replace("<i />", "")
                                     .Replace("<br>", "")
                                     .Replace("<br />", "")
                                     .Replace("--", "")
                                     .Replace(".", "")
                                     .Replace(",", "");

                var words = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 1)
                {
                    /*
                     * We put g.Count as the value of the dictionary entry because if let's say adventure appears 5 times in a description it is
                     * more important than robot(ofcourse if robot appears fewer times than the word adventure).
                     */
                    var keywordWeights = words
                        .GroupBy(w => w)
                        .ToDictionary(g => g.Key, g => (float)g.Count());

                    var bookVector = WeightedAverageVectors(keywordWeights, wordVectors, dimension);

                    var similarity = CosineSimilarity(queryVector, bookVector);

                    var bookToAdd = new Book
                    {
                        id = Guid.NewGuid(),
                        title = book.Name,
                        author = book.Authors,
                        genre = "",
                        quantity = 0,
                        lentQuantity = 0
                    };

                    booksWithScores.Add((
                        bookToAdd,
                        similarity
                    ));
                }
            }

            var sortedBooks = booksWithScores.OrderByDescending(b => b.similarity);

            /*
             * For debugging purposes 
             */
            //Console.WriteLine("Recommended books:");
            //foreach (var (title, score) in sortedBooks.Take(10))
            //{
            //    Console.WriteLine($"{title} with similarity: {score}");
            //}

            return sortedBooks.Take(10);
        }
    }
}
