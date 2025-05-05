using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ClinicManagement.Controllers
{
    public class AnalysisResultController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                // TempData'dan veriyi güvenli şekilde alın
                var explanation = TempData["Explanation"] as string ?? "Açıklama bulunamadı.";
                var prediction = TempData["Prediction"] as string ?? "Sonuç belirtilmedi.";
                var recommendations = TempData["Recommendations"] as List<string> ?? new List<string> { "Öneri bulunamadı." };

                // TempData'dan tahlil sonuçlarını al
                var testResults = new Dictionary<string, double>();
                var normalRanges = new Dictionary<string, Tuple<double, double>>();

                var testResultsJson = TempData["TestResults"] as string;
                var normalRangesJson = TempData["NormalRanges"] as string;

                if (!string.IsNullOrEmpty(testResultsJson))
                {
                    testResults = JsonConvert.DeserializeObject<Dictionary<string, double>>(testResultsJson);
                }

                if (!string.IsNullOrEmpty(normalRangesJson))
                {
                    normalRanges = JsonConvert.DeserializeObject<Dictionary<string, Tuple<double, double>>>(normalRangesJson);
                }

                // Eğer tahlil sonuçları yoksa örnek veriler kullan
                if (!testResults.Any())
                {
                    testResults = new Dictionary<string, double>
                    {
                        { "Hemoglobin", 14.5 },
                        { "Glucose", 95 },
                        { "Cholesterol", 180 },
                        { "Triglycerides", 150 },
                        { "HDL", 45 },
                        { "LDL", 100 },
                        { "Creatinine", 1.1 },
                        { "Urea", 35 }
                    };

                    normalRanges = new Dictionary<string, Tuple<double, double>>
                    {
                        { "Hemoglobin", new Tuple<double, double>(12, 16) },
                        { "Glucose", new Tuple<double, double>(70, 100) },
                        { "Cholesterol", new Tuple<double, double>(125, 200) },
                        { "Triglycerides", new Tuple<double, double>(50, 150) },
                        { "HDL", new Tuple<double, double>(40, 60) },
                        { "LDL", new Tuple<double, double>(0, 100) },
                        { "Creatinine", new Tuple<double, double>(0.7, 1.3) },
                        { "Urea", new Tuple<double, double>(10, 50) }
                    };
                }

                // ViewBag'e verileri aktar
                ViewBag.Explanation = explanation;
                ViewBag.Prediction = prediction;
                ViewBag.Recommendations = recommendations;
                ViewBag.TestResults = testResults;
                ViewBag.NormalRanges = normalRanges;

                return View();
            }
            catch (Exception ex)
            {
                // Hata durumunda örnek verilerle devam et
                ViewBag.Explanation = "Veri işleme hatası oluştu.";
                ViewBag.Prediction = "0";
                ViewBag.Recommendations = new List<string> { "Lütfen tekrar deneyin." };
                ViewBag.TestResults = new Dictionary<string, double>
                {
                    { "Hemoglobin", 14.5 },
                    { "Glucose", 95 },
                    { "Cholesterol", 180 },
                    { "Triglycerides", 150 },
                    { "HDL", 45 },
                    { "LDL", 100 },
                    { "Creatinine", 1.1 },
                    { "Urea", 35 }
                };
                ViewBag.NormalRanges = new Dictionary<string, Tuple<double, double>>
                {
                    { "Hemoglobin", new Tuple<double, double>(12, 16) },
                    { "Glucose", new Tuple<double, double>(70, 100) },
                    { "Cholesterol", new Tuple<double, double>(125, 200) },
                    { "Triglycerides", new Tuple<double, double>(50, 150) },
                    { "HDL", new Tuple<double, double>(40, 60) },
                    { "LDL", new Tuple<double, double>(0, 100) },
                    { "Creatinine", new Tuple<double, double>(0.7, 1.3) },
                    { "Urea", new Tuple<double, double>(10, 50) }
                };

                return View();
            }
        }
    }
}
