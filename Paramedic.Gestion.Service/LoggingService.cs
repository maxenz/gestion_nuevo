using Paramedic.Gestion.Model.Enums;
using Serilog;
using System;

namespace Paramedic.Gestion.Service
{

    public sealed class LoggingService
    {
        private static volatile LoggingService instance;
        private static object syncRoot = new Object();

        private LoggingService()
        {
            SetLogger();
        }

        private void SetLogger()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.RollingFile(AppDomain.CurrentDomain.BaseDirectory + "logs\\log-{Date}.txt")
               .CreateLogger();
        }

        public static LoggingService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LoggingService();
                    }
                }

                return instance;
            }
        }

        public void Write(LoggingTypes type, string data)
        {
            if (Log.Logger == null)
            {
                SetLogger();
            }

            switch (type)
            {
                case LoggingTypes.Information:
                    Log.Information(data);
                    break;
                case LoggingTypes.Error:
                    Log.Error(data);
                    break;
            }
        }

    }
}
