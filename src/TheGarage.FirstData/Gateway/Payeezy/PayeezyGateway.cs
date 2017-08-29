using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace TheGarage.Services.Payment.Gateway.Payeezy
{
    public enum PayeezyEnvironment
    {
        Certification,
        Production
    }

    /// <summary>
    /// Wraps the newer REST based API for First Data.
    /// </summary>
    /// <see cref="https://developer.payeezy.com/payeezy-api-reference/apis"/>
    /// <seealso cref="https://developer.payeezy.com/"/>
    public partial class PayeezyGateway : BaseGateway
    {
        #region Constants

        protected const string SandboxBaseUrl = "https://api-cert.payeezy.com/v1/";
        protected const string LiveBaseUrl = "https://api.payeezy.com/v1/";
        protected const string TransactionsController = "transactions";
        protected const string SecurityTokenController = "securitytokens";
        protected const string EventsController = "events";

        #endregion

        #region Properties

        public string ApiKey { get; protected set; }

        public string ApiSecret { get; protected set; }

        /// <summary>ISO 4217 currency code. Defaulted to USD.</summary>
        public string CurrencyCode { get; protected set; }

        public string Token { get; protected set; }

        public string BaseUrl { get; protected set; }

        #endregion

        public PayeezyGateway(string apiKey, string apiSecret, PayeezyEnvironment environment = PayeezyEnvironment.Certification)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
            CurrencyCode = new RegionInfo("US").ISOCurrencySymbol;

            switch (environment)
            {
                case PayeezyEnvironment.Production:
                    BaseUrl = LiveBaseUrl;
                    break;

                default:
                    BaseUrl = SandboxBaseUrl;
                    break;
            }
        }

        #region Common methods

        protected string GenerateHmac(string apiKey, string apiSecret, string token, int nonce, string currentTimestamp, string payload)
        {
            string message = apiKey + nonce.ToString() + currentTimestamp + token + payload;
            HMAC hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            byte[] hmacData = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(message));

            // updated per comment by blakeshadle5159
            // https://developer.payeezy.com/content/how-create-hmac-code-c#comment-402
            string hex = BitConverter.ToString(hmacData);
            hex = hex.Replace("-", "").ToLower();
            byte[] hexArray = Encoding.UTF8.GetBytes(hex);
            return Convert.ToBase64String(hexArray);
        }

        protected HttpWebRequest CreateRequest(string apiKey, string apiSecret, string token, string url, string payloadString)
        {
            string currentTimestamp = GetEpochTimestampInMilliseconds();
            int nonce = GetNonce();

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = MimeTypes.ApplicationJson;
            webRequest.Accept = MimeTypes.ApplicationJson;
            webRequest.Headers.Add("apikey", apiKey);
            webRequest.Headers.Add("token", token);
            webRequest.Headers.Add("nonce", nonce.ToString());
            webRequest.Headers.Add("timestamp", currentTimestamp);
            webRequest.Headers.Add("Authorization", GenerateHmac(apiKey, apiSecret, token, nonce, currentTimestamp, payloadString));
            webRequest.ContentLength = payloadString.Length;

            // write and send request data
            using (StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                streamWriter.Write(payloadString);
            }

            return webRequest;
        }

        public Response ParseResponse(string requestString, string responseString)
        {
            dynamic responseObject = JObject.Parse(responseString);

            Response response = new Response
            {
                MethodType = responseObject.method,
                ParsedMethodType = MethodType.Unknown,
                Amount = responseObject.amount,
                Currency = responseObject.currency,
                //...
                //TODO: avs/cvv2/card/token 
                //...
                TransactionStatus = responseObject.transaction_status,
                ParsedTransactionStatus = TransactionStatus.Unknown,
                ValidationStatus = responseObject.validation_status,
                TransactionType = responseObject.transaction_type,
                ParsedTransactionType = TransactionType.Unknown,
                TransactionId = responseObject.transaction_id,
                TransactionTag = responseObject.transaction_tag,
                BankResponseCode = responseObject.bank_resp_code,
                ParsedBankResponseCode = BankResponseCode.Unknown,
                BankMessage = responseObject.bank_message,
                GatewayResponseCode = responseObject.gateway_resp_code,
                ParsedGatewayResponseCode = GatewayResponseCode.Unknown,
                GatewayMessage = responseObject.gateway_message,
                CorrelationId = responseObject.correlation_id,
                ErrorMessages = new System.Collections.Generic.List<Response.ErrorMessage>(),
                RawRequest = requestString
            };

            #region Parse error messages (if response has any)

            if (responseObject.Error != null && responseObject.Error.messages != null)
            {
                foreach (dynamic error in responseObject.Error.messages)
                {
                    Response.ErrorMessage msg = new Response.ErrorMessage
                    {
                        Code = error.code,
                        Description = error.description
                    };

                    response.ErrorMessages.Add(msg);
                }
            }

            #endregion

            #region Convert response fields into an enum (if possible)

            if (!string.IsNullOrWhiteSpace(response.MethodType) && MethodTypeByString.ContainsKey(response.MethodType))
            {
                response.ParsedMethodType = MethodTypeByString[response.MethodType];
            }

            if (!string.IsNullOrWhiteSpace(response.TransactionStatus) && TransactionStatusByString.ContainsKey(response.TransactionStatus.ToLower()))
            {
                response.ParsedTransactionStatus = TransactionStatusByString[response.TransactionStatus.ToLower()];
            }

            if (!string.IsNullOrWhiteSpace(response.TransactionType) && TransactionTypeByString.ContainsKey(response.TransactionType))
            {
                response.ParsedTransactionType = TransactionTypeByString[response.TransactionType];
            }

            if (!string.IsNullOrWhiteSpace(response.BankResponseCode) && BankResponseCodeByString.ContainsKey(response.BankResponseCode))
            {
                response.ParsedBankResponseCode = BankResponseCodeByString[response.BankResponseCode];
            }

            if (!string.IsNullOrWhiteSpace(response.GatewayResponseCode) && GatewayResponseCodeByString.ContainsKey(response.GatewayResponseCode))
            {
                response.ParsedGatewayResponseCode = GatewayResponseCodeByString[response.GatewayResponseCode];
            }

            #endregion

            return response;
        }
        
        protected Response ProcessRequest(dynamic payload, string relativeUrl)
        {
            string resourceUrl = string.Format("{0}{1}", BaseUrl, relativeUrl);
            string payloadString = JsonConvert.SerializeObject(payload);
            HttpWebRequest webRequest = CreateRequest(this.ApiKey, this.ApiSecret, this.Token, resourceUrl, payloadString);
            string responseString = string.Empty;

            string requestString = JsonConvert.SerializeObject(payload, Formatting.Indented);

            try
            {
                
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader responseStream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        responseString = responseStream.ReadToEnd();
                        //return ParseResponse(requestString, responseString);
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
                    {
                        using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                        {


                            responseString = reader.ReadToEnd();

                            //return ParseResponse(requestString, exceptionResponse);
                        }
                    }
                }
                //throw;
            }

            return ParseResponse(requestString, responseString);
        }

        #endregion

        #region Make Payments

        #region Credit Card Payments

        /// <see cref="https://developer.payeezy.com/creditcardpayment/apis/post/transactions"/>
        /// <seealso cref="https://developer.payeezy.com/payeezy_new_docs/apis/post/transactions%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20-1"/>
        public Response CreditCardAuthorize(string cardNumber, string expirationMonth, string expirationYear, string dollarAmount, string cardHoldersName, string cardVerificationValue, string referenceNumber)
        {
           // DateTime parsedExpirationDate;

            //CreditCardType cardType = ValidateAndParseCardDetails(cardNumber, expirationMonth, expirationYear, out parsedExpirationDate);
            //cardVerificationValue = ValidateCardSecurityCode(cardType, cardVerificationValue);
            dollarAmount = GetUsDollarAmountAsCents(dollarAmount);

            dynamic payload = new
            {
                merchant_ref = referenceNumber,
                transaction_type = TransactionTypeToString[TransactionType.Authorize],
                method = MethodTypeToString[MethodType.CreditCard],
                amount = dollarAmount,
                currency_code = CurrencyCode,
                credit_card = new
                {
                    //type = CardTypeToString[cardType],
                    cardholder_name = cardHoldersName,
                    card_number = cardNumber,
                    //exp_date = FormatCardExpirationDate(parsedExpirationDate),
                    cvv = cardVerificationValue
                }
            };

            return ProcessRequest(payload, TransactionsController);
        }

        /// <see cref="https://developer.payeezy.com/creditcardpayment/apis/post/transactions"/>
        /// <seealso cref="https://developer.payeezy.com/payeezy_new_docs/apis/post/transactions%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20-1"/>
        public Response CreditCardPurchase(string merchantToken, string cardType, string tokenValue, string expirationMonth, string expirationYear, string dollarAmount, string cardHoldersName, string referenceNumber)
        {
            //TEST merchantToken
            this.Token = merchantToken;

            //TODO
            //var parsedExpirationDate = ValidateExpDate(expirationMonth, expirationYear);
            dollarAmount = GetUsDollarAmountAsCents(dollarAmount);

            dynamic payload = new
            {
                merchant_ref = referenceNumber,
                transaction_type = TransactionTypeToString[TransactionType.Purchase],
                method = "token",
                //eci_indicator = "2",
                amount = dollarAmount,
                currency_code = CurrencyCode,
                token = new
                {
                    token_type = "FDToken",
                    token_data = new
                    {
                        type = cardType,
                        value = tokenValue,
                        cardholder_name = cardHoldersName,
                        // TODO
                        //exp_date = FormatCardExpirationDate(parsedExpirationDate)
                        exp_date = string.Format("{0}{1}", expirationMonth, expirationYear)
                    }
                }
            };

            return ProcessRequest(payload, TransactionsController);
        }

        #endregion

        #region Capture or Reverse a Payment

        /// <see cref="https://developer.payeezy.com/capturereversepayment/apis/post/transactions/%7Bid%7D"/>
        /// <seealso cref="https://developer.payeezy.com/payeezy_new_docs/apis/post/transactions/%7Bid%7D-2"/>
        public Response CreditCardCapture(string transactionId, string referenceNumber, string transactionTag, string dollarAmount)
        {
            dollarAmount = GetUsDollarAmountAsCents(dollarAmount);

            dynamic payload = new
            {
                merchant_ref = referenceNumber,
                transaction_tag = transactionTag,
                transaction_type = TransactionTypeToString[TransactionType.Capture],
                method = MethodTypeToString[MethodType.CreditCard],
                amount = dollarAmount,
                currency_code = CurrencyCode
            };

            return ProcessRequest(payload, string.Format("{0}/{1}", TransactionsController, transactionId));
        }

        /// <see cref="https://developer.payeezy.com/capturereversepayment/apis/post/transactions/%7Bid%7D"/>
        public Response CreditCardRefund(string merchantToken, string transactionId, string cardNumber, string expirationMonth, string expirationYear, string dollarAmount, string cardHoldersName, string cardVerificationValue, string referenceNumber)
        {
            // MerchantToken;
            this.Token = merchantToken;


            //CreditCardType cardType = ValidateAndParseCardDetails(cardNumber, expirationMonth, expirationYear, out parsedExpirationDate);
            //cardVerificationValue = ValidateCardSecurityCode(cardType, cardVerificationValue);
            dollarAmount = GetUsDollarAmountAsCents(dollarAmount);

            dynamic payload = new
            {
                merchant_ref = referenceNumber,
                transaction_type = TransactionTypeToString[TransactionType.Refund],
                method = MethodTypeToString[MethodType.CreditCard],
                amount = dollarAmount,
                currency_code = CurrencyCode,
                credit_card = new
                {
                    //type = CardTypeToString[cardType],
                    cardholder_name = cardHoldersName,
                    card_number = cardNumber,
                    //exp_date = FormatCardExpirationDate(parsedExpirationDate),
                    cvv = cardVerificationValue
                }
            };

            return ProcessRequest(payload, string.Format("{0}/{1}", TransactionsController, transactionId));
        }

        /// <see cref="https://developer.payeezy.com/capturereversepayment/apis/post/transactions/%7Bid%7D"/>
        /// <seealso cref="https://developer.payeezy.com/payeezy_new_docs/apis/post/transactions/%7Bid%7D-2"/>
        public Response CreditCardRefund(string merchantToken, string transactionId, string referenceNumber, string transactionTag, string dollarAmount, string cardType, string tokenValue, string cardHoldersName, string expirationMonth, string expirationYear)
        {
            this.Token = merchantToken;
            dollarAmount = GetUsDollarAmountAsCents(dollarAmount);

            dynamic payload = new
            {
                merchant_ref = referenceNumber,
                transaction_type = TransactionTypeToString[TransactionType.Refund],
                transaction_tag = transactionTag,
                method = "token",
                amount = dollarAmount,
                currency_code = CurrencyCode,
                token = new
                {
                    token_type = "FDToken",
                    token_data = new
                    {
                        type = cardType,
                        value = tokenValue,
                        cardholder_name = cardHoldersName,
                        // TODO
                        //exp_date = FormatCardExpirationDate(parsedExpirationDate)
                        exp_date = string.Format("{0}{1}", expirationMonth, expirationYear)
                    }
                }
            };

            return ProcessRequest(payload, string.Format("{0}/{1}", TransactionsController, transactionId));
        }

        /// <see cref="https://developer.payeezy.com/capturereversepayment/apis/post/transactions/%7Bid%7D"/>
        /// <seealso cref="https://developer.payeezy.com/payeezy_new_docs/apis/post/transactions/%7Bid%7D-2"/>
        public Response CreditCardVoid(string transactionId, string referenceNumber, string transactionTag, string dollarAmount)
        {
            dollarAmount = GetUsDollarAmountAsCents(dollarAmount);

            dynamic payload = new
            {
                merchant_ref = referenceNumber,
                transaction_tag = transactionTag,
                transaction_type = TransactionTypeToString[TransactionType.Void],
                method = MethodTypeToString[MethodType.CreditCard],
                amount = dollarAmount,
                currency_code = CurrencyCode
            };

            return ProcessRequest(payload, string.Format("{0}/{1}", TransactionsController, transactionId));
        }

        #endregion

        #endregion

        #region Create Tokens

        #endregion

        #region Reporting

        #endregion

        #region Event Notifications

        /// <see cref="https://developer.payeezy.com/searchforevents/apis/get/events"/>
        public Response SearchForEvents(DateTime dateFrom, DateTime dateTo, int pageSize, int pageNumber)
        {
            dynamic payload = new
            {
                eventType = "TRANSACTION_STATUS",
                from = dateFrom.ToString("yyyy-MM-dd"),
                to = dateTo.ToString("yyyy-MM-dd"),
                offset = pageNumber,
                limit = pageSize
            };

            return ProcessRequest(payload, EventsController);
        }

        /// <see cref="https://developer.payeezy.com/searchforevents/apis/get/events/%7Bid%7D"/>
        public Response GetEventById(string id)
        {
            dynamic payload = new { };

            return ProcessRequest(payload, string.Format("{0}/{1}", EventsController, id));
        }

        #endregion
    }
}