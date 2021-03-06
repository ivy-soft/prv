﻿using System.Collections.Generic;

namespace TheGarage.Services.Payment.Gateway.Payeezy
{
    public partial class PayeezyGateway : BaseGateway
    {
        public enum GatewayResponseCode
        {
            Unknown,
            TransactionNormal,

            CVV2_CID_CVC2_DataNotVerified,
            InvalidCreditCardNumber,
            InvalidExpiryDate,
            InvalidAmount,
            InvalidCardHolder,
            InvalidAuthorizationNo,
            InvalidVerificationStringOrInvalidIssuer,
            InvalidTransactionCode,
            InvalidReferenceNo,
            InvalidAvsString,
            InvalidCustomerReferenceNumber,
            InvalidDuplicate,
            InvalidRefund,
            RestrictedCardNumber,
            InvalidTransactionTag,
            DataWithinTransactionIncorrect,
            InvalidAuthNumberOnPreAuthCompletion,

            InvalidSequenceNo,
            DeclinedOrInvalidOrTimeout,
            BceFunctionError,
            InvalidResponseFromPayeezy,
            InvalidDateFromHost,

            InvalidTransactionDescription,
            InvalidGatewayIdOrInvalidCard,
            InvalidTransactionNumber,
            ConnectionInactive,
            UnmatchedTransaction,
            InvalidReversalResponse,
            UnableToSendSocketTransaction,
            UnableToWriteTransactionToFile,
            UnableToVoidTransaction,
            PaymentTypeNotSupportedByMerchant,
            UnableToConnect,
            UnableToSendLogon,
            UnableToSendTrans,
            InvalidLogonOrStolenCard,
            TerminalNotActivated,
            Terminal_Gateway_Mismatch,
            InvalidProcessingCenterOrExpiredCard,
            NoProcessorsAvailableOrIncorrectPin,
            DatabaseUnavailable,
            SocketError,
            HostNotReadyOrCardLocked,

            AddressNotVerified,
            TransactionPlacedInQueue,
            TransactionReceivedFromBank,
            ReversalPending,
            ReversalComplete,
            ReversalSentToBank,

            FraudSuspected_AddressCheckFailed,
            FraudSuspected_Card_Check_Number_CheckFailed,
            FraudSuspected_CountryCheckFailed,
            FraudSuspected_CustomerReferenceCheckFailed,
            FraudSuspected_EmailAddressCheckFailed,
            FraudSuspected_IpAddressCheckFailed,

            FunctionPerformedErrorFree,
            ReferToCardIssuer,
            InvalidMerchant,
            DoNotHonor,
            InvalidTransactionForTerminal,
            HonorWithId,
            TimeOut,
            UnableToReverse,
            PartialApproval,
            InvalidTransaction_Card_Issuer_Acquirer,
            InvalidCardNumber,
            InvalidCaptureDate,
            SystemErrorReEnterTransaction,
            NoFromAccount,
            NoToAccount,
            NoCheckingAccount,
            NoSavingAccount,
            NoCreditAccount,
            FormatError,
            ImplausibleCardData,
            TransactionNotAllowed,
            LostCard,
            SpecialPickup,
            HotCard,
            PickupCard,
            NotSufficientFunds,
            ExpiredCard,
            IncorrectPinReEnter,
            TransactionNotPermittedOnCard,
            TxnNotPermittedOnTerm,
            SuspectedFraud,
            ExceedsAmountLimit,
            RestrictedCard,
            MAC_KeyError,
            ExceedsFrequencyLimit,
            ExceedsAcquirerLimit,
            RetainCardNoReasonSpecified,
            IndicateReasonForSendingReversal,
            ExceedsPINRetry,
            InvalidAccount,
            IssuerDoesNotParticipateInTheService,
            FunctionNotAvailable,
            KeyValidationError,
            ApprovalForPurchaseAmountOnly,
            UnableToVerifyPin,
            InvalidCardVerificationValue,
            NotDeclinedValidForAllZeroAmountTransactions,
            InvalidLifeCycleOfTransaction,
            NoKeysToUse,
            K_M_E_SyncError,
            PIN_KeyError,
            MAC_SyncError,
            SecurityViolation,
            IssuerNotAvailable,
            InvalidIssuer,
            TransactionCannotBeCompleted,
            InvalidOriginator,
            ContactAcquirer,
            SystemMalfunction,
            NoFundsTransfer,
            DuplicateReversal,
            DuplicateTransaction,
            CashServiceNotAvailable,
            CashBackRequestExceedsIssuerLimit,
            StopPaymentOrder,
            RevocationOfAuthorizationOrder,
            RevocationOfAllAuthorizationsOrder,
            Declined,
            _3dSecureAuthenticationFailed,
            CardholderDidNotReturnFromACS,
            CancelledByUser,
            OrderAlreadyExistsInDatabase,
            PostAuthAlreadyPerformed,
            CardholderDidNotReturnFromPayPal,
            Fraud_CardTemporarilyBlocked,
            CardUsedIsNotPermitted,
            CardholderDidNotReturnFromSOFORT,
            SuspicionOfManipulation,
            CardholderDidNotReturnFromDirekt,
            InvalidBicInSenderaccount_CMCIDEDD,
            TransactionNotVoidable,
            TransactionNotFound_ProbablyCancelledByUser,
            InvalidAccountData,
            ECI_7,
            BrandNotSupported,
            RedirectYourCustomerToPayPal,
            Invalid3dSecureValues,
            SelectedBrandDoesNotMatchCardNumber,
            NoTerminalSetup,
            CardSecurityCodeIsMandatory,
            TimeoutWhileEntering_PIN_TAN,
            CommunicationError,
            OrderTooOldToBeReferenced,
            PaymentRejected_ReferToCardIssuer,
            HostedDataWasNotFound,
            NoAuthorizedPreAuthFound,
            NoFurtherPostAuthPossibleForThisOrderId,
            CantRefund,
            TotalAmountPassedIsMoreThanReturn_Void_Amount,
            InvalidTransaction,
            InternalError,
            UnknownMasterData,
            GiropayFiducia,
            TransactionRejected,
            TransactionTypeNotSupportedForThisEndpoint,
            CardIssuerNotPermitted,
            BankWentOffline,
            ConnectExtendedHashAlreadyUsed,
            InvalidDirectDebitTrackData_TrackThreeMissing
        }

        protected Dictionary<string, GatewayResponseCode> GatewayResponseCodeByString = new Dictionary<string, GatewayResponseCode>() {
            //This response code indicates that the transaction was processed normally.
            //Please refer to the bank and approval response information for bank approval Status.
            { "00", GatewayResponseCode.TransactionNormal },
            //The following response codes indicate invalid data in the transaction.
            //In these cases, the data should be changed before attempting to resend the transaction.
            //These response codes are generated by the remote Plug-In.
            { "08", GatewayResponseCode.CVV2_CID_CVC2_DataNotVerified },
            { "22", GatewayResponseCode.InvalidCreditCardNumber },
            { "25", GatewayResponseCode.InvalidExpiryDate },
            { "26", GatewayResponseCode.InvalidAmount },
            { "27", GatewayResponseCode.InvalidCardHolder },
            { "28", GatewayResponseCode.InvalidAuthorizationNo },
            { "31", GatewayResponseCode.InvalidVerificationStringOrInvalidIssuer },
            { "32", GatewayResponseCode.InvalidTransactionCode },
            { "57", GatewayResponseCode.InvalidReferenceNo },
            { "58", GatewayResponseCode.InvalidAvsString },
            { "60", GatewayResponseCode.InvalidCustomerReferenceNumber },
            { "63", GatewayResponseCode.InvalidDuplicate },
            { "64", GatewayResponseCode.InvalidRefund },
            { "68", GatewayResponseCode.RestrictedCardNumber },
            { "69", GatewayResponseCode.InvalidTransactionTag },
            { "72", GatewayResponseCode.DataWithinTransactionIncorrect },
            { "93", GatewayResponseCode.InvalidAuthNumberOnPreAuthCompletion },
            // The following response codes indicate a problem with the merchant configuration at the financial institution.
            // Please contact Payeezy for further investigation.
            { "11", GatewayResponseCode.InvalidSequenceNo },
            { "12", GatewayResponseCode.DeclinedOrInvalidOrTimeout },
            { "21", GatewayResponseCode.BceFunctionError },
            { "23", GatewayResponseCode.InvalidResponseFromPayeezy },
            { "30", GatewayResponseCode.InvalidDateFromHost },
            // The following response codes indicate a problem with the Global Gateway e4℠ host or an error in the merchant configuration.
            // Please contact Payeezy for further investigation.
            { "10", GatewayResponseCode.InvalidTransactionDescription },
            { "14", GatewayResponseCode.InvalidGatewayIdOrInvalidCard },
            { "15", GatewayResponseCode.InvalidTransactionNumber },
            { "16", GatewayResponseCode.ConnectionInactive },
            { "17", GatewayResponseCode.UnmatchedTransaction },
            { "18", GatewayResponseCode.InvalidReversalResponse },
            { "19", GatewayResponseCode.UnableToSendSocketTransaction },
            { "20", GatewayResponseCode.UnableToWriteTransactionToFile },
            { "24", GatewayResponseCode.UnableToVoidTransaction },
            { "37", GatewayResponseCode.PaymentTypeNotSupportedByMerchant },
            { "40", GatewayResponseCode.UnableToConnect },
            { "41", GatewayResponseCode.UnableToSendLogon },
            { "42", GatewayResponseCode.UnableToSendTrans },
            { "43", GatewayResponseCode.InvalidLogonOrStolenCard },
            { "52", GatewayResponseCode.TerminalNotActivated },
            { "53", GatewayResponseCode.Terminal_Gateway_Mismatch },
            { "54", GatewayResponseCode.InvalidProcessingCenterOrExpiredCard },
            { "55", GatewayResponseCode.NoProcessorsAvailableOrIncorrectPin },
            { "56", GatewayResponseCode.DatabaseUnavailable },
            { "61", GatewayResponseCode.SocketError },
            { "62", GatewayResponseCode.HostNotReadyOrCardLocked },
            // The following response codes indicate the final state of a transaction.
            // In the event of one of these codes being returned, please contact Payeezy for further investigation.
            { "44", GatewayResponseCode.AddressNotVerified },
            { "70", GatewayResponseCode.TransactionPlacedInQueue },
            { "73", GatewayResponseCode.TransactionReceivedFromBank },
            { "76", GatewayResponseCode.ReversalPending },
            { "77", GatewayResponseCode.ReversalComplete },
            { "79", GatewayResponseCode.ReversalSentToBank },
            // The following response codes indicate the final state of a transaction due to custom Fraud Filters created by the Merchant.
            { "F1", GatewayResponseCode.FraudSuspected_AddressCheckFailed },
            { "F2", GatewayResponseCode.FraudSuspected_Card_Check_Number_CheckFailed },
            { "F3", GatewayResponseCode.FraudSuspected_CountryCheckFailed },
            { "F4", GatewayResponseCode.FraudSuspected_CustomerReferenceCheckFailed },
            { "F5", GatewayResponseCode.FraudSuspected_EmailAddressCheckFailed },
            { "F6", GatewayResponseCode.FraudSuspected_IpAddressCheckFailed },
            // Other
            { "17000", GatewayResponseCode.FunctionPerformedErrorFree },
            { "17002", GatewayResponseCode.ReferToCardIssuer },
            { "17003", GatewayResponseCode.InvalidMerchant },
            { "17004", GatewayResponseCode.DoNotHonor },
            { "17005", GatewayResponseCode.DoNotHonor },
            { "17006", GatewayResponseCode.InvalidTransactionForTerminal },
            { "17007", GatewayResponseCode.HonorWithId },
            { "17008", GatewayResponseCode.TimeOut },
            { "17010", GatewayResponseCode.UnableToReverse },
            { "17011", GatewayResponseCode.PartialApproval },
            { "17012", GatewayResponseCode.InvalidTransaction_Card_Issuer_Acquirer },
            { "17013", GatewayResponseCode.InvalidAmount },
            { "17014", GatewayResponseCode.InvalidCardNumber },
            { "17017", GatewayResponseCode.InvalidCaptureDate },
            { "17019", GatewayResponseCode.SystemErrorReEnterTransaction },
            { "17020", GatewayResponseCode.NoFromAccount },
            { "17021", GatewayResponseCode.NoToAccount },
            { "17022", GatewayResponseCode.NoCheckingAccount },
            { "17023", GatewayResponseCode.NoSavingAccount },
            { "17024", GatewayResponseCode.NoCreditAccount },
            { "17030", GatewayResponseCode.FormatError },
            { "17034", GatewayResponseCode.ImplausibleCardData },
            { "17039", GatewayResponseCode.TransactionNotAllowed },
            { "17041", GatewayResponseCode.LostCard },
            { "17042", GatewayResponseCode.SpecialPickup },
            { "17043", GatewayResponseCode.HotCard },
            { "17044", GatewayResponseCode.PickupCard },
            { "17051", GatewayResponseCode.NotSufficientFunds },
            { "17054", GatewayResponseCode.ExpiredCard },
            { "17055", GatewayResponseCode.IncorrectPinReEnter },
            { "17057", GatewayResponseCode.TransactionNotPermittedOnCard },
            { "17058", GatewayResponseCode.TxnNotPermittedOnTerm },
            { "17059", GatewayResponseCode.SuspectedFraud },
            { "17061", GatewayResponseCode.ExceedsAmountLimit },
            { "17062", GatewayResponseCode.RestrictedCard },
            { "17063", GatewayResponseCode.MAC_KeyError },
            { "17065", GatewayResponseCode.ExceedsFrequencyLimit },
            { "17066", GatewayResponseCode.ExceedsAcquirerLimit },
            { "17067", GatewayResponseCode.RetainCardNoReasonSpecified },
            { "17068", GatewayResponseCode.IndicateReasonForSendingReversal },
            { "17075", GatewayResponseCode.ExceedsPINRetry },
            { "17076", GatewayResponseCode.InvalidAccount },
            { "17077", GatewayResponseCode.IssuerDoesNotParticipateInTheService },
            { "17078", GatewayResponseCode.FunctionNotAvailable },
            { "17079", GatewayResponseCode.KeyValidationError },
            { "17080", GatewayResponseCode.ApprovalForPurchaseAmountOnly },
            { "17081", GatewayResponseCode.UnableToVerifyPin },
            { "17082", GatewayResponseCode.InvalidCardVerificationValue },
            { "17083", GatewayResponseCode.NotDeclinedValidForAllZeroAmountTransactions },
            { "17084", GatewayResponseCode.InvalidLifeCycleOfTransaction },
            { "17085", GatewayResponseCode.NoKeysToUse },
            { "17086", GatewayResponseCode.K_M_E_SyncError },
            { "17087", GatewayResponseCode.PIN_KeyError },
            { "17088", GatewayResponseCode.MAC_SyncError },
            { "17089", GatewayResponseCode.SecurityViolation },
            { "17091", GatewayResponseCode.IssuerNotAvailable },
            { "17092", GatewayResponseCode.InvalidIssuer },
            { "17093", GatewayResponseCode.TransactionCannotBeCompleted },
            { "17094", GatewayResponseCode.InvalidOriginator },
            { "17095", GatewayResponseCode.ContactAcquirer },
            { "17096", GatewayResponseCode.SystemMalfunction },
            { "17097", GatewayResponseCode.NoFundsTransfer },
            { "17098", GatewayResponseCode.DuplicateReversal },
            { "17099", GatewayResponseCode.DuplicateTransaction },
            { "17243", GatewayResponseCode.CashServiceNotAvailable },
            { "17244", GatewayResponseCode.CashBackRequestExceedsIssuerLimit },
            { "17280", GatewayResponseCode.StopPaymentOrder },
            { "17281", GatewayResponseCode.RevocationOfAuthorizationOrder },
            { "17283", GatewayResponseCode.RevocationOfAllAuthorizationsOrder },
            { "05", GatewayResponseCode.Declined },
            { "-5101", GatewayResponseCode._3dSecureAuthenticationFailed },
            { "-5103", GatewayResponseCode.CardholderDidNotReturnFromACS },
            { "-5993", GatewayResponseCode.CancelledByUser },
            { "-2304", GatewayResponseCode.ExpiredCard },
            { "-5003", GatewayResponseCode.OrderAlreadyExistsInDatabase },
            { "-10501", GatewayResponseCode.PostAuthAlreadyPerformed },
            { "-5104", GatewayResponseCode.CardholderDidNotReturnFromPayPal },
            { "-5005", GatewayResponseCode.Fraud_CardTemporarilyBlocked },
            { "04", GatewayResponseCode.CardUsedIsNotPermitted },
            { "-5106", GatewayResponseCode.CardholderDidNotReturnFromSOFORT },
            { "34", GatewayResponseCode.SuspicionOfManipulation },
            { "-P00001", GatewayResponseCode.CancelledByUser },
            { "33", GatewayResponseCode.ExpiredCard },
            { "-5105", GatewayResponseCode.CardholderDidNotReturnFromDirekt },
            { "1940", GatewayResponseCode.InvalidBicInSenderaccount_CMCIDEDD },
            { "-5019", GatewayResponseCode.TransactionNotVoidable },
            { "100", GatewayResponseCode.DoNotHonor },
            { "-8018", GatewayResponseCode.TransactionNotFound_ProbablyCancelledByUser },
            { "-1", GatewayResponseCode.InvalidAccountData },
            { "-5102", GatewayResponseCode.ECI_7 },
            { "-5002", GatewayResponseCode.BrandNotSupported },
            { "10486", GatewayResponseCode.RedirectYourCustomerToPayPal },
            { "51", GatewayResponseCode.NotSufficientFunds },
            { "3100", GatewayResponseCode.CancelledByUser },
            { "-5100", GatewayResponseCode.Invalid3dSecureValues },
            { "-5994", GatewayResponseCode.SelectedBrandDoesNotMatchCardNumber },
            { "-2303", GatewayResponseCode.InvalidCardNumber },
            { "-30031", GatewayResponseCode.NoTerminalSetup },
            { "96", GatewayResponseCode.IssuerNotAvailable },
            { "-12000", GatewayResponseCode.CardSecurityCodeIsMandatory },
            { "2000", GatewayResponseCode.TimeoutWhileEntering_PIN_TAN },
            { "-30057", GatewayResponseCode.CommunicationError },
            { "-5995", GatewayResponseCode.OrderTooOldToBeReferenced },
            { "02", GatewayResponseCode.PaymentRejected_ReferToCardIssuer },
            { "-10503", GatewayResponseCode.InvalidAmount },
            { "10422", GatewayResponseCode.RedirectYourCustomerToPayPal },
            { "-5010", GatewayResponseCode.HostedDataWasNotFound },
            { "13", GatewayResponseCode.InvalidAmount },
            { "-5004", GatewayResponseCode.NoAuthorizedPreAuthFound },
            { "-10424", GatewayResponseCode.NoFurtherPostAuthPossibleForThisOrderId },
            { "10009", GatewayResponseCode.CantRefund },
            { "-10601", GatewayResponseCode.TotalAmountPassedIsMoreThanReturn_Void_Amount },
            { "-30060", GatewayResponseCode.InternalError },
            { "86", GatewayResponseCode.UnknownMasterData },
            { "955", GatewayResponseCode.GiropayFiducia },
            { "None", GatewayResponseCode.TransactionRejected },
            { "", GatewayResponseCode.TransactionRejected },
            { "01", GatewayResponseCode.Unknown },
            { "-30063", GatewayResponseCode.TransactionTypeNotSupportedForThisEndpoint },
            { "3900", GatewayResponseCode.BankWentOffline },
            { "4900", GatewayResponseCode.Declined },
            { "-5992", GatewayResponseCode.ConnectExtendedHashAlreadyUsed },
            { "902", GatewayResponseCode.InvalidTransaction },
            { "92", GatewayResponseCode.InvalidIssuer },
            { "-5991", GatewayResponseCode.InvalidDirectDebitTrackData_TrackThreeMissing }
        };
    }
}