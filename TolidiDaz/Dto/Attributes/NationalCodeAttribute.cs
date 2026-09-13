using System.ComponentModel.DataAnnotations;

namespace Dto.Attributes
{
    public class NationalCodeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // بررسی null بودن value
            if (value == null || string.IsNullOrEmpty(value.ToString()) || value.ToString().Length != 10 || !value.ToString().All(char.IsDigit))
            {
                return new ValidationResult("کد ملی معتبر نیست.");
            }

            // بررسی کدهای تکراری
            string[] invalidCodes = {
                "1111111111", "2222222222", "3333333333", "4444444444",
                "5555555555", "6666666666", "7777777777", "8888888888", "9999999999"
            };

            if (invalidCodes.Contains(value.ToString()))
            {
                return new ValidationResult("کد ملی معتبر نیست");
            }

            // محاسبه رقم کنترل
            int controlDigit = int.Parse(value.ToString()[9].ToString());
            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(value.ToString()[i].ToString()) * (10 - i);
            }

            int remainder = sum % 11;

            // اعتبارسنجی رقم کنترل
            if ((remainder < 2 && controlDigit == remainder) || (remainder >= 2 && controlDigit == 11 - remainder))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("کد ملی معتبر نیست.");
            }
        }
    }
}
