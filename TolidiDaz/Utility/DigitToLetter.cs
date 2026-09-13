using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
        public static class DigitToLetter
        {
            private static readonly string[] Ones = { "", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
            private static readonly string[] Teens = { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };
            private static readonly string[] Tens = { "", "ده", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
            private static readonly string[] Hundreds = { "", "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
            private static readonly string[] Thousands = { "", "هزار", "میلیون", "میلیارد", "تریلیون", "کوادریلیون", "کویینتیلیون" };

            public static string ConvertNumberToPersianWords(long number)
            {
                if (number == 0) return "صفر";

                List<string> parts = new();
                int thousandCounter = 0;

                while (number > 0)
                {
                    int chunk = (int)(number % 1000);
                    number /= 1000;

                    if (chunk > 0)
                    {
                        string chunkWords = ConvertChunkToWords(chunk);
                        parts.Insert(0, chunkWords + (thousandCounter > 0 ? " " + Thousands[thousandCounter] : ""));
                    }

                    thousandCounter++;
                }

                return string.Join(" و ", parts);
            }

            private static string ConvertChunkToWords(int chunk)
            {
                List<string> words = new();

                if (chunk >= 100)
                {
                    words.Add(Hundreds[chunk / 100]);
                    chunk %= 100;
                }

                if (chunk >= 10 && chunk < 20)
                {
                    words.Add(Teens[chunk - 10]);
                }
                else
                {
                    if (chunk >= 20)
                    {
                        words.Add(Tens[chunk / 10]);
                        chunk %= 10;
                    }

                    if (chunk > 0)
                    {
                        words.Add(Ones[chunk]);
                    }
                }

                return string.Join(" و ", words);
            }
        }

}


