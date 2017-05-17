using System;
using System.ServiceProcess;
using System.Configuration;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model.Enums;
using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.TicketsService
{
    public partial class TicketsService : ServiceBase
    {
        #region Properties

        /// <summary>
        /// Timer para el chequeo de mensajes de los distintos servicios
        /// </summary>
        private System.Timers.Timer t = new System.Timers.Timer();

        #endregion

        #region Constructors
        //public TicketsService(ISocialServicesService SocialServicesService)
        //{
        //    InitializeComponent();
        //    _SocialServicesService = SocialServicesService;
        //}

        public TicketsService()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrided Service Methods

        protected override void OnStart(string[] args)
        {
            LoggingService.Instance.Write(LoggingTypes.Information, "Servicio inicializado.");

            t.Elapsed += delegate { ElapsedHandler(); };
            t.Interval = 5000;
            t.Start();
        }

        protected override void OnPause()
        {
            LoggingService.Instance.Write(LoggingTypes.Information, "Servicio pausado.");
            t.Stop();
        }

        protected override void OnContinue()
        {
            LoggingService.Instance.Write(LoggingTypes.Information, "Servicio ejecutó OnContinue.");
            t.Start();
        }

        protected override void OnStop()
        {
            LoggingService.Instance.Write(LoggingTypes.Information, "Servicio detenido.");
            t.Stop();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Método core que revisa por nuevos mensajes en los distintos canales de comunicación
        /// </summary>
        private void ElapsedHandler()
        {

        }

        #endregion

    }
}
