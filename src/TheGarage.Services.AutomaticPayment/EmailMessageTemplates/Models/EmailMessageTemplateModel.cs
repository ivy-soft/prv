namespace TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Models
{
    public class EmailMessageTemplateModel
    {
        private readonly string subject;

        private readonly string body;

        public EmailMessageTemplateModel(string subject, string body)
        {
            this.subject = subject;
            this.body = body;
        }

        public string Subject
        {
            get
            {
                return this.subject;
            }
        }

        public string Body
        {
            get
            {
                return this.body;
            }
        }
    }
}
