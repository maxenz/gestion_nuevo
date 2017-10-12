namespace Paramedic.Gestion.Model.Enums
{
	public enum VencimientoType
	{
		Licencia = 1,

		Soporte = 2
	}


	public static class VencimientoTypes
	{
		public static string GetVencimientoTypeName(VencimientoType type)
		{
			switch (type)
			{
				case VencimientoType.Licencia: return "Vencimiento de licencia";
				case VencimientoType.Soporte: return "Vencimiento de soporte";
				default: return ((short) type).ToString();
			}
		}
	}


}
