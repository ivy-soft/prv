namespace TheGarage.Services.AutomaticPayment.Providers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using TheGarage.Data;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    public class DataProvider : IDataProvider
    {
        private readonly ITheGarageData data;

        public DataProvider(ITheGarageData data)
        {
            this.data = data;
        }

        public IEnumerable<Garage> GetAllGarages(/*ITheGarageData data*/)
        {
            return this.data.Garages
                .All()
                .Include(c => c.Company)
                .ToList();
        }



        public IEnumerable<User> InvoiceUsers(Garage garage)
        {
            var invoiceUsers = this.data.Garages
                                                .All()
                                                .Where(c => c.Id == garage.Id)
                                                .SelectMany(u => u.Users)
                                                    .Where(c =>
                                                    (!c.IsDeleted && c.IsClient && c.IsApproved && !c.IsRejected && !c.HasMonthlyInvoiceUnpaid && c.IsActive)
                                                    || ((c.HasProrateInvoiceUnpaid && !c.IsActive && !c.IsRejected && c.IsClient && !c.IsDeleted && c.IsApproved)
                                                    && c.Invoices.Any(i => i.Status == "Unpaid" && i.Type == "Monthly" && i.PaidOn == null && !i.IsDeleted)))
                                                    .Include(c => c.Cards.Select(ccode => ccode.ChargeCode))
                                                .Include(c => c.Customer)
                                                .Include(cc => cc.CustomerCompany)
                                                .ToList();

            return invoiceUsers;
        }
    }
}
