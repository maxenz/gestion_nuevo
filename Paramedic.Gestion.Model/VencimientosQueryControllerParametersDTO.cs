using Paramedic.Gestion.Model.Enums;
using System;

namespace Paramedic.Gestion.Model
{
	public class VencimientosQueryControllerParametersDTO : QueryControllerParametersDTO
	{
		#region Properties
		public DateTime DateFrom { get; set; }

		public DateTime DateTo { get; set; }

		#endregion

		#region Constructors

		public VencimientosQueryControllerParametersDTO(
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