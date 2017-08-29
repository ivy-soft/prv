namespace TheGarage.Services.AutomaticPayment.Core.Events.EventHandler.Contracts
{
    public interface IHandler
    {
        bool CheckInvoiceDate();

        bool CheckDueDate();

        bool CheckNextToDueDateWithoutGrace();

        bool CheckNextToDueDateWithGrace();

        bool CheckLastPaymentDay();
    }
}
