using System;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace ClinicManagement.Controllers
{
    public class HealthAssistantController : Controller
    {
        private readonly string _apiUrl = "http://localhost:1234/v1/chat/completions";

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Health Assistant";
            return View("~/Views/HealthAssistant/Index.cshtml", "_Layout");
        }

        [HttpPost]
        public async Task<JsonResult> SendMessage(string message)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var requestData = new
                    {
                        messages = new[]
                        {
                            new { role = "user", content = message }
                        },
                        temperature = 0.7,
                        max_tokens = 1000
                    };

                    var content = new StringContent(
                        JsonConvert.SerializeObject(requestData),
                        Encoding.UTF8,
                        "application/json");

                    var response = await client.PostAsync(_apiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    return Json(new { success = true, response = responseString });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
} 