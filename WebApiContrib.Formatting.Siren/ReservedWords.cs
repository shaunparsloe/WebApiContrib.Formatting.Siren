using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Formatting.Siren
{
    public static class ReservedWords
    {            

        public static List<string> ListOfWords()
        {
            List<string> reservedWords = new List<string>();

            reservedWords = new List<string>();
            reservedWords.Add("Properties");
            reservedWords.Add("Entities");
            reservedWords.Add("Class");
            reservedWords.Add("Links");
            reservedWords.Add("Actions");
            reservedWords.Add("Rel");
            reservedWords.Add("Title");

            return reservedWords;
        }
    }
}
