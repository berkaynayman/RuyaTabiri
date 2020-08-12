using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ruyadaherseycom.Models
{
    public class RuyaTabiri
    {
        public string cHarf { get; set; }
        public string cBaslik { get; set; }
        public string cSeo { get; set; }
        public string cIcerik { get; set; }
        public int iKodRuyaTabiri { get; set; }
        public DateTime dTarih { get; set; }
        public string cBaslikOnceki { get; set; }
        public string cSeoOnceki { get; set; }
        public string cBaslikSonraki { get; set; }
        public string cSeoSonraki { get; set; }
        public List<RuyaTabiri> ruyaTabiris { get; set; }
    }
}