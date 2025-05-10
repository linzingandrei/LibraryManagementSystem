using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Service
{
    class Word2VecLoader
    {
        public static Dictionary<string, float[]> LoadWordVectors(string pathOfFile, int vectorSize)
        {
            var wordVectors = new Dictionary<string, float[]>();

            foreach (var line in File.ReadLines(pathOfFile))
            {
                var tokens = line.Split(' ');
                if (tokens.Length != vectorSize + 1)
                {
                    continue;
                }

                var word = tokens[0];
                var vector = new float[vectorSize];
                for (int i = 0; i < vectorSize; i++)
                {
                    vector[i] = float.Parse(tokens[i + 1], System.Globalization.CultureInfo.InvariantCulture);
                }

                wordVectors[word] = vector;
            }

            return wordVectors;
        }
    }
}
