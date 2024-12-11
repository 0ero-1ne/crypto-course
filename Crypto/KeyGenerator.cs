using System;
using System.Linq;

namespace CourseProject.Crypto
{
    public class KeyGenerator
    {
        private static readonly string AlphabetLowerCase = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string AlphabetUpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string Numbers = "0123456789";
        private static readonly string SpecialChars = "@#!`'\"\\|/?$;:%^&*()-=_+.,~<>[]{}";
        private static readonly string AllChars = AlphabetLowerCase + AlphabetUpperCase + Numbers + SpecialChars;

        public static string GenerateKey(int keyLength)
        {
            Random random = new();
            return string.Concat(
                Enumerable.Repeat(0, keyLength).Select(c => AllChars[random.Next(AllChars.Length)])
            );
        }
    }
}
