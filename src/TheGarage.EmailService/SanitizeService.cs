namespace TheGarage.EmailService
{
    using Ganss.XSS;
    using System.Web;
    using TheGarage.EmailService.Contracts;

    public class SanitizeService : ISanitizeService
    {
        public string HtmlDecode(string text)
        {
            var decodedData = HttpUtility.HtmlDecode(text);
            return decodedData;
        }

        public string HtmlSanitize(string text)
        {
            var htmlSanitizer = new HtmlSanitizer();
            var sanitizedData = htmlSanitizer.Sanitize(text);

            return sanitizedData;
        }
    }
}
