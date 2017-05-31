using Autofac;
using Paramedic.Gestion.Model.Enums;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.SocialMedia.Services;
using SocialMedia.Services;
using System.ServiceProcess;

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
            var builder = new ContainerBuilder();
            builder.RegisterType<SocialServicesService>().As<ISocialServicesService>();
            builder.RegisterType<MailSocialService>().As<ISocialMediaService>();

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IService>();
            }
        }

        #endregion

    }
}
