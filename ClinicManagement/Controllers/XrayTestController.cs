using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClinicManagement.Controllers
{
    public class XrayTestController : Controller
    {
        private readonly string flaskApiUrl = "http://localhost:5000/xray_predict"; // Flask API adresini gerektiğinde güncelleyin

        // GET: XrayTest
        public ActionResult Index(string result = null, string explanation = null, string error = null)
        {
            ViewBag.Result = result;
            ViewBag.Explanation = explanation;
            ViewBag.Error = error;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadXray(HttpPostedFileBase xrayFile)
        {
            if (xrayFile == null || xrayFile.ContentLength == 0)
            {
                return RedirectToAction("Index", new { error = "Lütfen bir X-ray görüntüsü seçin." });
            }

            try
            {
                using (var client = new HttpClient())
                using (var content = new MultipartFormDataContent())
                {
                    var streamContent = new StreamContent(xrayFile.InputStream);
                    content.Add(streamContent, "file", xrayFile.FileName);

                    var response = await client.PostAsync(flaskApiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseString);
                        string prediction = json.prediction;
                        string explanation = json.explanation;
                        return RedirectToAction("Index", new { result = prediction, explanation = explanation });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { error = "API Hatası: " + responseString });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { error = "Bir hata oluştu: " + ex.Message });
            }
        }
    }
} 