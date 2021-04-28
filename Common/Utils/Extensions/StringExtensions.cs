using System;
using System.Globalization;
using System.Text;

namespace MRL.SSL.Common.Utils.Extensions
{
    public static class StringExtensions
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        private static string ChangeCase(string str, bool pascal)
        {
            var sb = new StringBuilder(str.Length);

            bool isNextUpper = pascal, isPrevLower = false;
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
        public static string ToPascalCase(this String str)
        {
            return ChangeCase(str, true);
        }
        public static string ToCamelCase(this String str)
        {
            return ChangeCase(str, false);
        }

    }
}