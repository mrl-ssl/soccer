using System;
using System.Globalization;
using System.Text;

namespace MRL.SSL.Common.Utils.Extensions
{
    public static class StringExtensions
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        public static string ToPascalCase(this String str)
        {
            var sb = new StringBuilder(str.Length);

            bool isNextUpper = true, isPrevLower = false;
            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (c == '_')
                {
                    isNextUpper = true;
                }
                else
                {
                    sb.Append(isNextUpper ? char.ToUpper(c, Culture) : isPrevLower ? c : char.ToLower(c, Culture));
                    isNextUpper = false;
                    isPrevLower = char.IsLower(c);
                }
            }
            return sb.ToString();
        }

    }
}