namespace TheGarage.Logger
{
    using System;
    using TheGarage.Logger.Contracts;

    public class ElmahLoggerSystem : IElmahLoggerSystem
    {
        public void Log(Exception ex)
        {
            Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
        }
    }
}
