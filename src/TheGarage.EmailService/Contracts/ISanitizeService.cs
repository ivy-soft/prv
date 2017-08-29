namespace TheGarage.EmailService.Contracts
{
    public interface ISanitizeService
    {
        string HtmlDecode(string text);

        string HtmlSanitize(string text);
    }
}
