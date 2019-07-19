using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreWithToken.Models
{
    public static class CryptoHelper
    {
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // Generate a random password of a given length (optional)  
        public static string GenerateOtp(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(2, true));
            builder.Append(RandomNumber(10, 99));
            builder.Append(RandomString(2, false));
            return builder.ToString();

        }

        public static string GenerateToken(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(8, true));
            builder.Append(RandomNumber(1000000, 99999999));
            builder.Append(RandomString(8, false));
            builder.Append(RandomNumber(1000000, 99999999));
            builder.Append(RandomString(8, true));
            builder.Append(RandomNumber(1000000, 99999999));
            builder.Append(RandomString(8, false));
            builder.Append(RandomNumber(1000000, 99999999));
            return builder.ToString();
        }

    }
}

