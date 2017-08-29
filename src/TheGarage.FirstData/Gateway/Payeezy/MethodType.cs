using System.Collections.Generic;

namespace TheGarage.Services.Payment.Gateway.Payeezy
{
    public partial class PayeezyGateway : BaseGateway
    {
        public enum MethodType
        {
            Unknown,
            CreditCard
        }

        protected Dictionary<MethodType, string> MethodTypeToString = new Dictionary<MethodType, string>() {
            { MethodType.CreditCard, "credit_card" }
        };

        protected Dictionary<string, MethodType> MethodTypeByString = new Dictionary<string, MethodType>() {
            { "credit_card", MethodType.CreditCard }
        };
    }
}
