namespace TheGarage.EmailService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using TheGarage.Data.Models;
    using TheGarage.EmailService.Attachments.Contracts;
    using TheGarage.EmailService.Contracts;
    using TheGarage.EmailService.Models.Contracts;

    public class EmailMessageService : IEmailMessageService
    {
        private readonly ISmtpClientService smtpClientService;
        private readonly ISanitizeService sanitizeService;
        private readonly IMessageModel messageModel;
        private readonly Lazy<IPdfAttachment> pdfAttachment;
        private ICollection<byte[]> pdfStreams;

        public EmailMessageService(Lazy<IPdfAttachment> pdfAttachment, ISmtpClientService smtpClientService, ISanitizeService sanitizeService, IMessageModel messageModel)
        {
            this.pdfAttachment = pdfAttachment;
            this.smtpClientService = smtpClientService;
            this.sanitizeService = sanitizeService;
            this.messageModel = messageModel;
        }

        public ICollection<byte[]> PdfStreams
        {
            get
            {

                return this.pdfStreams;
            }
        }

        public EmailMessage Send(string userId, string toEmail, string fromEmail, string subject, string body, string sender)
        {
            var decodedBody = this.sanitizeService.HtmlDecode(body);
            var model = this.messageModel.CreateEmailMessage(userId, toEmail, fromEmail, subject, decodedBody, sender);

            var sanitizeBody = this.sanitizeService.HtmlSanitize(decodedBody);
            var mailMessage = this.messageModel.CreateMailMessage(fromEmail, toEmail, subject, sanitizeBody);

            if (this.PdfStreams != null && this.PdfStreams.Count > 0)
            {
                var collection = this.PdfStreams.ToList();
                foreach (var stream in collection)
                {
                    var memoryStream = this.pdfAttachment.Value.Create(stream);
                    mailMessage.Attachments.Add(new Attachment(memoryStream, "filename.pdf", "application/pdf"));
                }
            }

            try
            {
                this.smtpClientService.Send(mailMessage);
                model.Status = "success";
            }
            catch (Exception ex)
            {
                // TODO ADD LOGGER
                model.Status = ex.ToString();
            }
            finally
            {
                if (this.PdfStreams != null && this.PdfStreams.Count > 0)
                {
                    this.DisposeAttachments(mailMessage);
                }
            }

            return model;
        }

        public ICollection<byte[]> AddAttachment(byte[] pdfStream)
        {
            if (pdfStream == null)
            {
                throw new ArgumentNullException("Null is not allowed!!!");
            }

            this.PdfStreams.Add(pdfStream);

            return this.pdfStreams;
        }

        internal void DisposeAttachments(MailMessage mailMessage)
        {
            foreach (Attachment attachment in mailMessage.Attachments)
            {
                attachment.Dispose();
            }

            mailMessage.Attachments.Dispose();
            mailMessage = null;
        }
    }
}
