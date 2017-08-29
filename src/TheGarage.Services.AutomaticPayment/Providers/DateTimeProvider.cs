namespace TheGarage.Services.AutomaticPayment.Providers
{
    using System;

    public abstract class DateTimeProvider
    {
        private static DateTimeProvider current = DefaultDateTimeProvider.Instance;

        protected DateTimeProvider()
        {
        }

        public static DateTimeProvider Current
        {
            get
            {
                return current;
            }

            set
            {
                current = value ?? throw new ArgumentNullException("DateTimeProvider current value!");
            }
        }

        public abstract DateTime Now { get; }

        public abstract DateTime SetDateTime(int year, int month, int day);

        public abstract DateTime SetDateTime(int year, int month, int day, int hour, int minute, int second);

        public static void ResetToDefault()
        {
            current = DefaultDateTimeProvider.Instance;
        }
    }
}
