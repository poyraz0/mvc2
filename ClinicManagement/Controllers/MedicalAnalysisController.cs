using System.Linq;
using System.Web.Mvc;
using ClinicManagement.Core.Models;
using ClinicManagement.Persistence;
using System.Data.Entity; // Include için gerekli
using Newtonsoft.Json.Linq; // JSON işlemek için gerekli
using System.Collections.Generic;

using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;



namespace ClinicManagement.Controllers
{
    public class MedicalAnalysisController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor
        public MedicalAnalysisController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: MedicalAnalysis
        //public ActionResult Index()
        //{
        //    // Retrieve all medical analyses along with related medical reports
        //    //var analyses = _context.MedicalAnalysis.Include("MedicalReport").ToList();

        //    var analyses = from analysis in _context.MedicalAnalysis
        //                   join report in _context.MedicalReports
        //                   on analysis.MedicalReportId equals report.ReportId
        //                   select new
        //                   {
        //                       Analysis = analysis,
        //                       Report = report
        //                   };



        //    return View(analyses);
        //}

        //public ActionResult Index()
        //{
        //    // MedicalAnalysis ile birlikte ilişkili MedicalReport verilerini getir
        //    var analyses = _context.MedicalAnalysis.Include("MedicalReport").ToList();
        //    return View(analyses);
        //}


        [HttpGet]
        //public ActionResult Index()
        //{
        //    if (TempData["PredictionResult"] != null)
        //    {
        //        ViewBag.PredictionResult = TempData["PredictionResult"];
        //    }
        //    return View();
        //}

        //public ActionResult Index()
        //{
        //    if (TempData["PredictionResult"] != null)
        //    {
        //        ViewBag.PredictionResult = TempData["PredictionResult"];    //ÇALIŞAN KOD
        //    }
        //    else
        //    {
        //        ViewBag.PredictionResult = null; // Bu hata döndürmez
        //    }
        //    return View();
        //}



        //public ActionResult Index()
        //    {
        //        var predictionResultJson = "{ \"prediction\": 1 }"; // Bu örnek veri
        //        int prediction = 0;

        //        try
        //        {
        //            // JSON verisini ayrıştır ve prediction değerini al
        //            var parsedJson = JObject.Parse(predictionResultJson);    // Öneri öncesi
        //            prediction = (int)parsedJson["prediction"];
        //        }
        //        catch
        //        {
        //            // Hata durumunda varsayılan değer ata
        //            prediction = -1; // -1: geçersiz durum için
        //        }

        //        ViewBag.PredictionResult = prediction; // ViewBag'e int değer gönder
        //        return View();
        //    }


        //public ActionResult Index()
        //{
        //    // Örnek: API'den tahmin sonucu al
        //    var features = new double[] { 110, 6.0, 150, 45, 180, 220, 250, 50, 12 };
        //    var apiResponse = GetPredictionFromApi(features);

        //    if (apiResponse != null)
        //    {
        //        ViewBag.PredictionResult = apiResponse.Prediction;
        //        ViewBag.Recommendations = apiResponse.Recommendations;
        //    }
        //    else
        //    {
        //        ViewBag.PredictionResult = null;
        //        ViewBag.Recommendations = null;
        //    }
        //DÜMENDEN
        //    return View();
        //}

        //// API çağrısı (örnek)
        //private dynamic GetPredictionFromApi(double[] features)
        //{
        //    // Burada gerçek bir HTTP isteği yapılacak
        //    return new
        //    {
        //        Prediction = 1, // Örnek
        //        Recommendations = new List<string>
        //{
        //    "Kan şekerinizi düzenlemek için şekerli gıdalardan kaçının.",
        //    "Daha fazla su tüketin.",
        //    "Sağlıklı bir diyet planı oluşturun."
        //}
        //    };
        //}


        // Index Action: API'den tahmin sonucu alır ve View'e gönderir
        public async Task<ActionResult> Index()
        {
            // Gönderilecek veriler (örnek features)
            var features = new double[] { 110, 6.0, 150, 45, 180, 220, 250, 50, 12 };

            // Flask API'ye istek gönder
            var apiResponse = await GetPredictionFromApi(features);

            if (apiResponse != null)
            {
                ViewBag.PredictionResult = apiResponse.Prediction;
                ViewBag.Recommendations = apiResponse.Recommendations;
            }
            else
            {
                ViewBag.PredictionResult = "Tahmin yapılamadı. API yanıtı boş veya hatalı.";
                ViewBag.Recommendations = new List<string>();
            }

            return View();
        }

        // Flask API'ye POST isteği yapan yardımcı metod
        private async Task<dynamic> GetPredictionFromApi(double[] features)
        {
            // Flask API'nin URL'si
            var apiUrl = "https://muratpoyraz.pythonanywhere.com/predict";

            try
            {
                using (var client = new HttpClient())
                {
                    // JSON verisini oluştur
                    var payload = new { features = features };
                    var jsonContent = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // POST isteğini gönder
                    var response = await client.PostAsync(apiUrl, content);

                    // Yanıt başarılıysa işle
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine($"API Yanıtı: {result}");
                        return JsonConvert.DeserializeObject<dynamic>(result);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"API Hatası: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP Hatası: {ex.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Genel Hata: {ex.Message}");
            }

            return null; // Hata durumunda null döner
        }











        // GET: MedicalAnalysis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicalAnalysis/Create
        [HttpPost]
        public ActionResult Create(MedicalAnalysis analysis)
        {
            if (ModelState.IsValid)
            {
                _context.MedicalAnalysis.Add(analysis);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(analysis);
        }

     





        // GET: MedicalAnalysis/Details/{id}
        public ActionResult Details(int id)
        {
            var analysis = _context.MedicalAnalysis
                .Include("MedicalReport")
                .SingleOrDefault(a => a.Id == id);

            if (analysis == null)
                return HttpNotFound();

            return View(analysis);
        }

        // GET: MedicalAnalysis/Delete/{id}
        public ActionResult Delete(int id)
        {
            var analysis = _context.MedicalAnalysis.SingleOrDefault(a => a.Id == id);

            if (analysis == null)
                return HttpNotFound();

            _context.MedicalAnalysis.Remove(analysis);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
