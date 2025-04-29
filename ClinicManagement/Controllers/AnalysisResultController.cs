using System.Collections.Generic;
using System.Web.Mvc;

namespace ClinicManagement.Controllers
{
    public class AnalysisResultController : Controller
    {
        public ActionResult Index()
        {
            // TempData'dan veriyi g�venli �ekilde al�n
            var explanation = TempData["Explanation"] as string ?? "A��klama bulunamad�.";
            var prediction = TempData["Prediction"] as string ?? "Sonu� belirtilmedi.";
            var recommendations = TempData["Recommendations"] as List<string> ?? new List<string> { "�neri bulunamad�." };

            // ViewBag kullanarak verileri View'e aktar�n
            ViewBag.Explanation = explanation;
            ViewBag.Prediction = prediction;
            ViewBag.Recommendations = recommendations;

            return View();
        }
    }
}
