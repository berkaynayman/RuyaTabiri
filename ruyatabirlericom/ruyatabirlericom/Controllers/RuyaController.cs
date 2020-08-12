using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace ruyadaherseycom.Controllers
{
    public class RuyaController : Controller
    {
        public ActionResult Listele(int? sayfa, string arama, string harf)
        {
            if (!string.IsNullOrEmpty(arama))
            {
                arama = arama.Trim().Replace(" ", string.Empty).ToLower().Replace("ruyada", "").Replace("rüyada", "").Replace("görmek", "").Replace("gormek", "").Replace("yorumu", "").Replace("gordum", "").Replace("gördüm", "").Replace("anlamı", "").Replace("anlami", "");
            }
            ViewBag.cArama = arama;
            ViewBag.cHarf = harf;

            using (Data.DataClassesDataContext dc = new Data.DataClassesDataContext())
            {
                List<Models.RuyaTabiri> result =
                        (from table in dc.RuyaTabiris
                         where
                            table.iAktifMi == 1
                         select new Models.RuyaTabiri
                         {
                             cHarf = table.cHarf,
                             cBaslik = table.cBaslik,
                             cSeo = table.cSeo,
                             cIcerik = table.cIcerik.Length > 250 ? table.cIcerik.Substring(0, 250) + "..." : table.cIcerik,
                             iKodRuyaTabiri = table.iKodRuyaTabiri
                         }).OrderBy(x => x.cBaslik).ToList();

                if (!string.IsNullOrEmpty(arama))
                {
                    result = result.Where(x => x.cBaslik.ToLower().Contains(arama.ToLower())).OrderBy(x => x.cBaslik).ToList();
                }
                if (!string.IsNullOrEmpty(harf))
                {
                    result = result.Where(x => x.cHarf.ToLower() == harf.ToLower()).OrderBy(x => x.cBaslik).ToList();
                }
                int iSayfaNo = sayfa ?? 1;
                return View(result.ToPagedList(iSayfaNo, 25));
            }
        }
        public ActionResult Yorum(string id, string id2)
        {
            using (Data.DataClassesDataContext dc = new Data.DataClassesDataContext())
            {
                var result =
                    (from table in dc.RuyaTabiris
                     where
                        table.cSeo == id &&
                        table.iAktifMi == 1
                     select new Models.RuyaTabiri
                     {
                         iKodRuyaTabiri = table.iKodRuyaTabiri,
                         cHarf = table.cHarf,
                         cBaslik = table.cBaslik,
                         cSeo = table.cSeo,
                         cIcerik = table.cIcerik                         
                     }).FirstOrDefault();
                if (result != null)
                {
                    result.ruyaTabiris =
                        (from table in dc.RuyaTabiris
                         where
                            table.cBaslik.ToLower().Contains(result.cBaslik.ToLower()) &&
                            table.iAktifMi == 1
                         select new Models.RuyaTabiri
                         {
                             cHarf = table.cHarf,
                             cBaslik = table.cBaslik,
                             cSeo = table.cSeo,
                             cIcerik = table.cIcerik.Length > 250 ? table.cIcerik.Substring(0, 250) + "..." : table.cIcerik,
                             iKodRuyaTabiri = table.iKodRuyaTabiri
                         }).OrderBy(x => x.cBaslik).Skip(0).Take(5).ToList();
                    var resultOnceki =
                        (from table in dc.RuyaTabiris
                         where
                            table.iKodRuyaTabiri < result.iKodRuyaTabiri &&
                            table.iAktifMi == 1
                         select table).OrderByDescending(x => x.iKodRuyaTabiri).FirstOrDefault();
                    if (resultOnceki  != null)
                    {
                        result.cBaslikOnceki = resultOnceki.cBaslik;
                        result.cSeoOnceki = resultOnceki.cSeo;
                    }
                    var resultSonraki =
                        (from table in dc.RuyaTabiris
                         where
                            table.iKodRuyaTabiri > result.iKodRuyaTabiri &&
                            table.iAktifMi == 1
                         select table).OrderBy(x => x.iKodRuyaTabiri).FirstOrDefault();
                    if (resultSonraki != null)
                    {
                        result.cBaslikSonraki = resultSonraki.cBaslik;
                        result.cSeoSonraki = resultSonraki.cSeo;
                    }
                    if (!string.IsNullOrEmpty(id2))
                    {
                        if (id2 == "1")
                        {
                            ViewBag.cBaslik = "Rüyada " + result.cBaslik + " Görmek";
                        }
                        else if (id2 == "2")
                        {
                            ViewBag.cBaslik = "Rüyada " + result.cBaslik + " Ne Anlamına Geliyor ?";
                        }
                        else if (id2 == "3")
                        {
                            ViewBag.cBaslik = "Rüyada " + result.cBaslik + " Kötüyemi Yorumlanır ?";
                        }
                        else if (id2 == "4")
                        {
                            ViewBag.cBaslik = "Rüyada " + result.cBaslik + " Psikolojik Yorumu";
                        }
                        else if (id2 == "5")
                        {
                            ViewBag.cBaslik = "Rüyada " + result.cBaslik + " İslami Yorumu";
                        }
                    }
                }
                return View(result);
            }
        }
    }
}