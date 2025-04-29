using System.Collections.Generic;
using System.Web.Mvc;

namespace ClinicManagement.Controllers
{
    public class AnalysisResultController : Controller
    {
        public ActionResult Index()
        {
            // TempData'dan veriyi güvenli þekilde alýn
            var explanation = TempData["Explanation"] as string ?? "Açýklama bulunamadý.";
            var prediction = TempData["Prediction"] as string ?? "Sonuç belirtilmedi.";
            var recommendations = TempData["Recommendations"] as List<string> ?? new List<string> { "Öneri bulunamadý." };

            // ViewBag kullanarak verileri View'e aktarýn
            ViewBag.Explanation = explanation;
            ViewBag.Prediction = prediction;
            ViewBag.Recommendations = recommendations;

            return View();
        }
    }
}
