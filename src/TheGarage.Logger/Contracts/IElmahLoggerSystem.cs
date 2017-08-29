namespace TheGarage.Logger.Contracts
{
    using System;

    public interface IElmahLoggerSystem
    {
        void Log(Exception ex);
    }
}
