namespace TheGarage.EmailService.Attachments
{
    using System.IO;
    using TheGarage.EmailService.Attachments.Contracts;

    public class PdfAttachment : IPdfAttachment
    {
        public MemoryStream Create(byte[] buffer)
        {
            var stream = new MemoryStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
