﻿using System.Text.RegularExpressions;

namespace Models.Utils
{
    public static class StringUtils
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string FirstCharToUpper(this string input)
        {

            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException($"{nameof(input)} cannot be empty", nameof(input));
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
