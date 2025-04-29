using System.Web.Mvc;

namespace ClinicManagement.Controllers
{
    public class BMIController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Calculate(float weight, float height, int age, string gender)
        {
            if (weight <= 0 || height <= 0 || age <= 0 || string.IsNullOrEmpty(gender))
            {
                ViewBag.Error = "Lütfen geçerli deðerler giriniz.";
                return View("Index");
            }

            // BKÝ hesaplama
            float heightInMeters = height / 100; // cm'den metreye çevir
            float bmi = weight / (heightInMeters * heightInMeters);

            // BKÝ kategorisi
            string category;
            if (bmi < 18.5)
                category = "Zayýf";    
            else if (bmi < 25)
                category = "Saðlýklý";
            else if (bmi < 30) 
                category = "Þiþman";
            else if (bmi < 35)
                category = "Obez";
            else
                category = "Aþýrý Obez";

            // Sonuçlarý ViewBag ile View'e gönder
            ViewBag.BMI = bmi.ToString("0.00");
            ViewBag.Category = category;
            ViewBag.Gender = gender;
            ViewBag.Age = age;

            return View("Index");
        }
    }
}
