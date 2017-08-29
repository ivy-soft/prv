namespace TheGarage.Services.AutomaticPayment.Logic.Contracts
{
    public interface IInvoiceNumberGenerator
    {
        string Generate(int garageCompanySequenceNumber, int garageUniqueNumber, int garageSequenceInvoiceNumber);
    }
}
