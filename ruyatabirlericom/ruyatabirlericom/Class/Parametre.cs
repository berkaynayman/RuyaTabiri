using System;
using System.Web.Routing;

namespace ruyadaherseycom.Class
{
    public class Parametre
    {
        public static RouteValueDictionary Send(int sayfa, RouteValueDictionary parameters)
        {
            if (parameters["sayfa"] != null)
            {
                parameters.Remove("sayfa");
            }

            var newparameters = new RouteValueDictionary(parameters);
            newparameters.Add("sayfa", sayfa);

            return newparameters;
        }
    }
}