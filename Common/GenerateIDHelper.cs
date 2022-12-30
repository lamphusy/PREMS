using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class GenerateIDHelper
    {
        public static string OrganizationID()
        {
            const int numOfChar = 6;
            var random = new Random();

            string ID = "";
            for(int i =0; i< numOfChar; i++)
            {
                int UpperOrLower = random.Next(0,2);
                if(UpperOrLower == 0)
                {
                    var charRandom = Convert.ToChar(random.Next(97, 123));
                    ID += charRandom;
                }
                else
                {
                    var charRandom = Convert.ToChar(random.Next(65, 91));
                    ID += charRandom;
                }
            }
            return ID;
        }
        public static string ReceiptID()
        {
            const int numOfChar = 9;
            var random = new Random();

            string ID = "";
            for (int i = 0; i < numOfChar; i++)
            {    
                var charRandom = Convert.ToChar(random.Next(48, 58));
                ID += charRandom;
            }
            return ID;
        }
        public static string ClassID(string IDOrganization)
        {
            const int numOfChar = 4;
            var random = new Random();

            string ID = "";
            for (int i = 0; i < numOfChar; i++)
            {
                var charRandom = Convert.ToChar(random.Next(48, 57));
                ID += charRandom;
            }
            ID = IDOrganization +  ID;
            return ID;
        }
    }
}
