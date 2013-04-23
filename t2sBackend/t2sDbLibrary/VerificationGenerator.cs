using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2sDbLibrary
{
    /// <summary>
    /// Random String Verification String Generator
    /// </summary>
    public class VerificationGenerator
    {
        /// <summary>
        /// Default bag for random string generation
        /// </summary>
        public static readonly String DEFAULT_BAG = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        /// <summary>
        /// Generates a random string using the DEFAULT_BAG
        /// </summary>
        /// <param name="length">Length of the string to generate</param>
        /// <returns>A random string</returns>
        public static String GenerateString(int length)
        {
            return GenerateString(length, VerificationGenerator.DEFAULT_BAG);
        }

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length">Length of the string to generate</param>
        /// <param name="bag">Bag of characters to use</param>
        /// <returns>A random string</returns>
        public static String GenerateString(int length, String bag)
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; ++i)
            {
                sb.Append(bag[r.Next(bag.Length)]);
            }

            return sb.ToString();
        }
    }
}
