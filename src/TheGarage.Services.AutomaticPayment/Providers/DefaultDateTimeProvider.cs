namespace TheGarage.Services.AutomaticPayment.Providers
{
    using System;

    public class DefaultDateTimeProvider : DateTimeProvider
    {
        private static DefaultDateTimeProvider instance = new DefaultDateTimeProvider();

        private DefaultDateTimeProvider()
        {
        }

        public static DefaultDateTimeProvider Instance
        {
            get
            {
                return instance;
            }
        }

        public override DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        public override DateTime SetDateTime(int year, int month, int day)
        {
            var sss = new DateTime(year, month, day);
            return sss;
        }

        public override DateTime SetDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}
