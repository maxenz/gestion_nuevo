using System;
using System.Data.SqlTypes;

namespace Paramedic.Gestion.Model.Helpers
{
	public static class SqlSmallDateTime
	{
		public static readonly SqlDateTime MinValue =
			new SqlDateTime(new DateTime(1900, 01, 01));

		public static readonly SqlDateTime MaxValue =
			new SqlDateTime(new DateTime(2099, 12, 31));
	}
}