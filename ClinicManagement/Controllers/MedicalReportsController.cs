using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity;
using ClinicManagement.Persistence;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ClinicManagement.Core.Models;
using System;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO;





namespace ClinicManagement.Controllers
{
    public class MedicalReportsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public MedicalReportsController()
        {
            _context = new ApplicationDbContext(); // DbContext örneği oluşturma
        }


        //[HttpPost]
        //public async Task<ActionResult> Upload (HttpPostedFileBase pdfFile)
        //{
        //    if (pdfFile != null && pdfFile.ContentLength > 0)
        //    {
        //        var filePath = Server.MapPath("~/App_Data2/") + pdfFile.FileName;
        //        pdfFile.SaveAs(filePath);

        //        // Flask API ile PDF'den veri çıkar
        //        var extractedData = await ExtractDataFromPdf(filePath);

        //        if (extractedData != null)
        //        {
        //            // Flask API'ye analiz için veri gönder
        //            var predictionResult = await GetPredictionFromFlaskApi(extractedData);

        //            // Tahmini veritabanına kaydet
        //            TempData["PredictionResult"] = predictionResult;
        //            return RedirectToAction("Index", "MedicalAnalysis");
        //        }


        //        ViewBag.Error = "PDF'den veri çıkarılamadı.";
        //    }


        //    return View();
        //}


        //[HttpPost]
        //public async Task<ActionResult> Upload(HttpPostedFileBase pdfFile)
        //{
        //    if (pdfFile != null && pdfFile.ContentLength > 0)
        //    {
        //        // PDF'i kaydet
        //        var filePath = Server.MapPath("~/App_Data2/") + pdfFile.FileName;
        //        pdfFile.SaveAs(filePath);

        //        // Flask API ile PDF'ten veri çıkar
        //        var extractedData = await ExtractDataFromPdf(filePath);

        //        System.Diagnostics.Debug.WriteLine($"Çıkarılan Veri: {JsonConvert.SerializeObject(extractedData)}");


        //        if (extractedData != null)
        //        {
        //            // Flask API'ye analiz için veri gönder
        //            var predictionResult = await GetPredictionFromFlaskApi(extractedData);

        //            if (predictionResult != null)
        //            {
        //                TempData["PredictionResult"] = predictionResult; // Tahmin sonucunu sakla
        //                return RedirectToAction("Index", "MedicalAnalysis"); // MedicalAnalysis sayfasına yönlendir
        //            }
        //            else
        //            {
        //                TempData["PredictionResult"] = "Tahmin yapılamadı."; // Hata mesajı
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Error = "PDF'den veri çıkarılamadı.";
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Error = "Lütfen bir PDF dosyası seçin.";
        //    }

        //    return View(); // Hata durumunda aynı sayfada kalır
        //}



        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase pdfFile)
        {
            if (pdfFile != null && pdfFile.ContentLength > 0)
            {
                try
                {
                    var folderPath = Server.MapPath("~/App_Data2/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, pdfFile.FileName);
                    pdfFile.SaveAs(filePath);

                    var extractedData = await ExtractDataFromPdf(filePath);
                    if (extractedData != null)
                    {
                        var predictionResult = await GetPredictionFromFlaskApi(extractedData);
                        if (predictionResult != null)
                        {
                            var predictResponse = JsonConvert.DeserializeObject<PredictResponse>(predictionResult);

                            TempData["Explanation"] = predictResponse.Explanation;
                            TempData["Prediction"] = predictResponse.Prediction.ToString(); // Prediction'ı string'e çevirin
                            TempData["Recommendations"] = predictResponse.Recommendations;


                            return RedirectToAction("Index", "AnalysisResult");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Error = "Lütfen geçerli bir PDF dosyası yükleyin.";
            }

            return View();
        }




        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }




        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase pdfFile)
        //{
        //    if (pdfFile != null && pdfFile.ContentLength > 0)
        //    {
        //        var filePath = Server.MapPath("~/App_Data2/") + pdfFile.FileName;
        //        pdfFile.SaveAs(filePath);

        //        // Burada dosyayı veritabanına kaydetme veya analiz etme işlemlerini gerçekleştirebilirsiniz.

        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View();
        //}

        //private async Task<object> ExtractDataFromPdf(string filePath)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var url = "http://127.0.0.1:5000/extract"; // Flask API PDF extraction endpoint
        //        var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filePath));
        //        var formData = new MultipartFormDataContent();
        //        formData.Add(fileContent, "pdf", "tahlil.pdf");

        //        var response = await client.PostAsync(url, formData);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = await response.Content.ReadAsStringAsync();
        //            dynamic jsonResponse = JsonConvert.DeserializeObject(result);
        //            return jsonResponse.features; // Çıkarılan veriyi döndür
        //        }

        //        return null;
        //    }
        //}


        private async Task<object> ExtractDataFromPdf(string filePath)
        {
            System.Diagnostics.Debug.WriteLine($"Gönderilen Dosya Yolu: {filePath}");
            using (var client = new HttpClient())
            {
                //var url = "http://127.0.0.1:5000/extract"; // Flask API endpoint

                var url = "https://muratpoyraz1.pythonanywhere.com/extract";

                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filePath));
                var formData = new MultipartFormDataContent();
                formData.Add(fileContent, "pdf", "tahlil.pdf");

                var response = await client.PostAsync(url, formData);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Flask API Yanıtı: {result}");
                    dynamic jsonResponse = JsonConvert.DeserializeObject(result);
                    return jsonResponse.features; // API yanıtını işleme
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"API Hatası: {response.StatusCode}");
                }

                return null;
            }
        }




        private async Task<string> GetPredictionFromFlaskApi(object extractedData)
        {
            using (var client = new HttpClient())
            {
                //var url = "http://127.0.0.1:5000/predict"; // Flask API tahmin endpoint

                var url = "https://muratpoyraz1.pythonanywhere.com/predict";


                var json = JsonConvert.SerializeObject(new { features = extractedData });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result; // Tahmin sonucu
                }

                return null; // Hata durumunda null döner
            }
        }



        [HttpPost]
        public ActionResult SaveAnalysisResult(string predictionResult)
        {
            var analysis = new MedicalAnalysis
            {
                Result = predictionResult,
                Date = DateTime.Now
            };

            _context.MedicalAnalysis.Add(analysis);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }






    }
}
