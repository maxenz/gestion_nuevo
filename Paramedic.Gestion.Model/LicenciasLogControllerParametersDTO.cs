using System;
using System.Globalization;

namespace Paramedic.Gestion.Model
{
    public class LicenciasLogControllerParametersDTO : QueryControllerParametersDTO
    {
        #region Properties

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public bool AndroidLogsVisible { get; set; }

        #endregion

        #region Constructors

        public LicenciasLogControllerParametersDTO(string searchDescription, int pageSize, int page, string dateFrom, string dateTo, bool chkAndroidLogs) : base(searchDescription, pageSize, page)
        {
            this.AndroidLogsVisible = chkAndroidLogs;
            initializeDates(dateFrom, dateTo);
        }

        #endregion

        private void initializeDates(string dateFrom, string dateTo)
        {
            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                dateFrom = dateFrom + " 00:00";
                dateTo = dateTo + " 23:59";

                this.DateFrom = DateTime.ParseExact(dateFrom, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                this.DateTo = DateTime.ParseExact(dateTo, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            else
            {
                this.DateFrom = DateTime.Now.AddDays(-3);
                this.DateTo = DateTime.Now;
            }
        }
    }
}
