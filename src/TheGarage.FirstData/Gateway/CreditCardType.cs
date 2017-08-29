using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGarage.Services.Payment.Gateway
{
    public enum CreditCardType
    {
        Invalid,
        Diners,
        AmericanExpress,
        Visa,
        MasterCard,
        Discover
    }
}