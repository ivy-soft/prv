namespace TheGarage.Services.AutomaticPayment.Core.Events.EventHandler
{
    using System;
    using Bytes2you.Validation;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler.Contracts;
    using TheGarage.Services.AutomaticPayment.Providers;

    public class Handler : IHandler
    {
        private readonly int invoiceEvent;
        private readonly int gracePeriod;
        private readonly DateTime dueDate;

        public Handler(int dueDate, int invoiceEvent, int gracePeriod)
        {
            Guard.WhenArgument(dueDate, "dueDate").IsNotEqual(1).Throw();
            Guard.WhenArgument(invoiceEvent, "invoiceEvent").IsLessThan(1).IsGreaterThan(31).Throw();
            Guard.WhenArgument(gracePeriod, "gracePeriod").IsLessThan(0).IsGreaterThan(10).Throw();

            this.invoiceEvent = invoiceEvent;
            this.gracePeriod = gracePeriod;
            this.dueDate = new DateTime(DateTimeProvider.Current.Now.Year, DateTimeProvider.Current.Now.Month, dueDate).Date;
        }

        public bool CheckInvoiceDate()
        {
            var invoiceEventDate = this.dueDate.AddMonths(1);
            invoiceEventDate = invoiceEventDate.AddDays(-this.invoiceEvent);
            bool todayIsEventDay = DateTimeProvider.Current.Now.Date == invoiceEventDate.Date;
            return todayIsEventDay;
        }

        public bool CheckDueDate()
        {
            bool isDueDate = DateTimeProvider.Current.Now.Date == this.dueDate.Date;

            return isDueDate;
        }

        public bool CheckNextToDueDateWithoutGrace()
        {
            bool isNextToDueDateWithoutGrace = (this.gracePeriod == 0) && (DateTimeProvider.Current.Now.Date == this.dueDate.AddDays(1));

            return isNextToDueDateWithoutGrace;
        }

        public bool CheckNextToDueDateWithGrace()
        {
            bool isNextToDueDateWithGrace = (DateTimeProvider.Current.Now.Date >= this.dueDate.AddDays(1)) && (DateTimeProvider.Current.Now.Date <= this.dueDate.AddDays(this.gracePeriod)) && (this.gracePeriod != 0);

            return isNextToDueDateWithGrace;
        }

        public bool CheckLastPaymentDay()
        {
            var isLastPaymentDay = (DateTimeProvider.Current.Now.Date == this.dueDate.AddDays(1 + this.gracePeriod)) && (this.gracePeriod != 0);

            return isLastPaymentDay;
        }
    }
}
