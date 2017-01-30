using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Web.HtmlHelpers
{
    public static class TicketHtmlHelper
    {
        public static string getTicketState(TicketEstadoType type)
        {
            string color = "";

            switch (type)
            {
                case TicketEstadoType.NotAnswered:
                    color = "rojo";
                    break;
                case TicketEstadoType.Answered:
                    color = "naranja";
                    break;
                case TicketEstadoType.Resolved:
                    color = "verde";
                    break;

            }

            string htmlItem = string.Format("<td class=\"span2 centrado {0} negrita\">{1}</td>", color, type.GetAttribute<DisplayAttribute>().Name);

            return htmlItem;                                  
            
        }
    }
}