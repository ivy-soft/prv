namespace TheGarage.Logger
{
    public class DataBaseLogger
    {
        private readonly string userEmail;
        private readonly string companyName;
        private readonly string garageName;
        private readonly string exception;

        public DataBaseLogger(string userEmail, string companyName, string garageName, string exception)
        {
            this.userEmail = userEmail;

            this.companyName = companyName;
            this.garageName = garageName;
            this.exception = exception;
        }
    }
}
