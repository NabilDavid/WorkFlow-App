using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;



public static class ArabicNumeralHelper
{
    public static string ConvertNumerals(this string input)
    {

        {
            return input.Replace('0', '\u0660')
                    .Replace('1', '\u0661')
                    .Replace('2', '\u0662')
                    .Replace('3', '\u0663')
                    .Replace('4', '\u0664')
                    .Replace('5', '\u0665')
                    .Replace('6', '\u0666')
                    .Replace('7', '\u0667')
                    .Replace('8', '\u0668')
                    .Replace('9', '\u0669');

        }
    }
}