using Paramedic.Gestion.Model.Enums;
using System;

namespace Paramedic.Gestion.Model
{
	public class DateQueryControllerParametersDTO : QueryControllerParametersDTO
	{
		#region Properties
		public DateTime DateFrom { get; set; }

		public DateTime DateTo { get; set; }

		#endregion

		#region Constructors

		public DateQueryControllerParametersDTO(
			string searchDescription,
			int pageSize,
			int page,
			DateTime dateFrom,
			DateTime dateTo

			) : base(searchDescription, pageSize, page)
		{
			this.DateFrom = dateFrom;
			this.DateTo = dateTo;
		}

		#endregion

	}
}