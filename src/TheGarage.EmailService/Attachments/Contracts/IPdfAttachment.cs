namespace TheGarage.EmailService.Attachments.Contracts
{
    using System.IO;

    public interface IPdfAttachment
    {
        MemoryStream Create(byte[] buffer);
    }
}
