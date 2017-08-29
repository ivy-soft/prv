using System;
using System.Globalization;
using System.Security.Cryptography;



namespace TheGarage.Services.Payment.Gateway
{
    public static class MimeTypes
    {
        ///<summary>JavaScript Object Notation JSON; Defined in RFC 4627</summary>
        public const string ApplicationJson = "application/json";
    }

    public abstract class BaseGateway
    {
        protected CultureInfo UsCulture
        {
            get { return new CultureInfo("US"); }
        }

        protected string GetEpochTimestampInMilliseconds()
        {
            long millisecondsSinceEpoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return millisecondsSinceEpoch.ToString();
        }

        /// <summary>
        /// Generates a cryptographically strong random number.
        /// </summary>
        /// <see cref="https://en.wikipedia.org/wiki/Cryptographic_nonce"/>
        protected int GetNonce()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }
                return BitConverter.ToInt32(bytes, 0);
            }
        }

        protected string FormatCardExpirationDate(DateTime expirationDate)
        {
            return string.Format("{0}{1}", expirationDate.ToString("MM"), expirationDate.ToString("yy"));
        }

        /// <summary>
        /// Parse a US dollar amount (string) and return the total number of cents.
        /// </summary>
        protected string GetUsDollarAmountAsCents(string dollarAmount)
        {
            if (string.IsNullOrWhiteSpace(dollarAmount))
            {
                throw new DollarAmountNullException("Dollar amount is null / empty");
            }

            string[] dollarParts = dollarAmount.Trim().Split('.');
            if (dollarParts.Length > 2)
            {
                throw new DollarAmountInvalidException("US dollar amounts can only have one decimal point");
            }

            int parsedCentsAmount = 0;
            if (dollarParts.Length == 2)
            {
                if (dollarParts[1].Length != 2)
                {
                    throw new DollarAmountInvalidException("US dollar amounts must have two digit cent amounts");
                }

                if (!int.TryParse(dollarParts[1], out parsedCentsAmount))
                {
                    throw new DollarAmountInvalidException("Cent amount must be numeric");
                }
            }

            int parsedDollarAmount = 0;

            try
            {
                parsedDollarAmount = int.Parse(dollarParts[0], NumberStyles.Currency);
            }
            catch (Exception ex)
            {
                throw new DollarAmountInvalidException("Dollar amount must be numeric", ex);
            }

            parsedDollarAmount *= 100;

            return (parsedDollarAmount + parsedCentsAmount).ToString();
        }

        protected DateTime ValidateExpDate(string expirationMonth, string expirationYear)
        {
            if (string.IsNullOrWhiteSpace(expirationMonth))
            {
                throw new ExpirationNullException("Expiration month is null / empty");
            }

            if (string.IsNullOrWhiteSpace(expirationYear))
            {
                throw new ExpirationNullException("Expiration year is null / empty");
            }

            int parsedExpirationMonth;
            if (!int.TryParse(expirationMonth, out parsedExpirationMonth))
            {
                throw new ExpirationFormatException("Expiration month must be a number");
            }

            if (parsedExpirationMonth < 1 || parsedExpirationMonth > 12)
            {
                throw new ExpirationOutOfRangeException("Expiration month must be between 1 and 12");
            }

            int parsedExpirationYear;
            if (!int.TryParse(expirationYear, out parsedExpirationYear))
            {
                throw new ExpirationFormatException("Expiration year must be a number");
            }

            int fourDigitYear;
            try
            {
                fourDigitYear = UsCulture.Calendar.ToFourDigitYear(parsedExpirationYear);
            }
            catch (Exception ex)
            {
                throw new ExpirationFormatException("Error converting expiration year to a four digit year", ex);
            }

            int daysInMonth = DateTime.DaysInMonth(fourDigitYear, parsedExpirationMonth);
            var parsedExpirationDate = new DateTime(fourDigitYear, parsedExpirationMonth, daysInMonth, 23, 59, 59);

            if (DateTime.Now > parsedExpirationDate)
            {
                throw new CardExpiredException("Card has expired");
            }

            return parsedExpirationDate;
        }
    }
}