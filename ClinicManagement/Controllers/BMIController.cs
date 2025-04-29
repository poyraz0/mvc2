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
                ViewBag.Error = "L�tfen ge�erli de�erler giriniz.";
                return View("Index");
            }

            // BK� hesaplama
            float heightInMeters = height / 100; // cm'den metreye �evir
            float bmi = weight / (heightInMeters * heightInMeters);

            // BK� kategorisi
            string category;
            if (bmi < 18.5)
                category = "Zay�f";    
            else if (bmi < 25)
                category = "Sa�l�kl�";
            else if (bmi < 30) 
                category = "�i�man";
            else if (bmi < 35)
                category = "Obez";
            else
                category = "A��r� Obez";

            // Sonu�lar� ViewBag ile View'e g�nder
            ViewBag.BMI = bmi.ToString("0.00");
            ViewBag.Category = category;
            ViewBag.Gender = gender;
            ViewBag.Age = age;

            return View("Index");
        }
    }
}
