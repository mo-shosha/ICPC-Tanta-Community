using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Validation
{
    public class SafeTextAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string input = value.ToString().Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Input cannot be empty or whitespace.";
                return false;
            }

            string pattern = @"^(?!.*<.*?>)([a-zA-Z0-9\s\-_.]+|https?:\/\/[^\s]+)$";
            if (!Regex.IsMatch(input, pattern))
            {
                ErrorMessage = "Input contains invalid characters.";
                return false;
            }

            return true;
        }
    }
}
