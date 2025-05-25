using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClinicManagement.Core.Models;
using ClinicManagement.Persistence;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;

namespace ClinicManagement.Controllers
{
    [Authorize]
    public class UserMedicalTestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserMedicalTestsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: UserMedicalTests
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var userMedicalTests = _context.UserMedicalTests
                .Where(t => t.ApplicationUserId == currentUserId)
                .OrderByDescending(t => t.UploadDate)
                .ToList();

            return View(userMedicalTests);
        }

        // GET: UserMedicalTests/Upload
        public ActionResult Upload()
        {
            return View();
        }

        // POST: UserMedicalTests/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(HttpPostedFileBase medicalTestFile)
        {
            if (medicalTestFile != null && medicalTestFile.ContentLength > 0)
            {
                try
                {
                    var currentUserId = User.Identity.GetUserId();
                    var user = _context.Users.Find(currentUserId);

                    if (user == null)
                    {
                        ViewBag.Error = "Kullanıcı bulunamadı.";
                        return View();
                    }

                    // Dosya adını ve uzantısını al
                    var fileName = Path.GetFileName(medicalTestFile.FileName);
                    // Kullanıcıya özel dosya yükleme klasörü yolu
                    var userUploadFolder = Server.MapPath($"~/Uploads/MedicalTests/{currentUserId}/");

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(userUploadFolder))
                    {
                        Directory.CreateDirectory(userUploadFolder);
                    }

                    // Dosyayı sunucuya kaydet
                    var filePath = Path.Combine(userUploadFolder, fileName);
                    medicalTestFile.SaveAs(filePath);

                    // Veritabanına tahlil bilgisini kaydet
                    var userMedicalTest = new UserMedicalTest
                    {
                        ApplicationUserId = currentUserId,
                        FileName = fileName,
                        FilePath = $"/Uploads/MedicalTests/{currentUserId}/{fileName}", // Göreceli yolu kaydet
                        UploadDate = DateTime.Now
                    };

                    _context.UserMedicalTests.Add(userMedicalTest);
                    await _context.SaveChangesAsync();

                    ViewBag.Message = "Medical test uploaded and saved successfully.";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Dosya yüklenirken bir hata oluştu: " + ex.Message;
                    // Hata loglama ekleyebilirsiniz
                }
            }
            else
            {
                ViewBag.Error = "Lütfen yüklenecek bir dosya seçin.";
            }

            // Yükleme sonrası listeleme sayfasına yönlendirme veya aynı sayfada kalma kararı size kalmış.
            // Şimdilik aynı sayfada kalıp mesaj göstertiyorum.
            var currentUserIdForList = User.Identity.GetUserId();
            var userMedicalTests = _context.UserMedicalTests
                .Where(t => t.ApplicationUserId == currentUserIdForList)
                .OrderByDescending(t => t.UploadDate)
                .ToList();
            return View("Index", userMedicalTests);
        }

        // POST: UserMedicalTests/SendEmail
        [HttpPost]
        public async Task<JsonResult> SendEmail(int id, string recipientEmail)
        {
            // Basic email format validation in backend as well
            if (string.IsNullOrEmpty(recipientEmail) || !new System.Net.Mail.MailAddress(recipientEmail).Address.Contains("@"))
            {
                return Json(new { status = "error", message = "Invalid recipient email address." }, JsonRequestBehavior.AllowGet);
            }

            var currentUserId = User.Identity.GetUserId();
            var userMedicalTest = await _context.UserMedicalTests.SingleOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == currentUserId);

            if (userMedicalTest == null)
            {
                return Json(new { status = "error", message = "Medical test not found or access denied." }, JsonRequestBehavior.AllowGet);
            }

            // Dosyanın fiziksel yolunu bul
            var serverFilePath = Server.MapPath(userMedicalTest.FilePath);

            if (!System.IO.File.Exists(serverFilePath))
            {
                return Json(new { status = "error", message = "File not found on the server." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                // E-posta gönderme mantığı burada olacak
                // Örnek olarak System.Net.Mail kullanalım
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new System.Net.Mail.MailAddress("test.proje@dogrubulut.com"); // Gönderen e-posta adresi
                    mail.To.Add(recipientEmail); // Alıcı e-posta adresi
                    mail.Subject = "Tıbbi Tahlil Sonucu";
                    mail.Body = $"Dear User,\n\nPlease find your medical test result from {userMedicalTest.UploadDate:dd.MM.yyyy} attached.\n\nBest regards."; // İngilizce mesaj

                    // Dosyayı e-postaya ekle
                    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(serverFilePath);
                    mail.Attachments.Add(attachment);

                    // SMTP ayarları
                    using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("mail.dogrumail.com")) // SMTP Sunucu adresi
                    {
                        smtp.Port = 587; // SMTP Port
                        smtp.Credentials = new System.Net.NetworkCredential("test.proje@dogrubulut.com", "Test123!"); // SMTP Kullanıcı adı ve Şifre
                        smtp.EnableSsl = true; // SSL kullanımı
                        smtp.Send(mail);
                    }
                }

                ViewBag.Message = "Tahlil e-posta ile başarıyla gönderildi.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "E-posta gönderilirken bir hata oluştu: " + ex.Message;
                // Hata loglama ekleyebilirsiniz
            }

            // İşlem sonrası tekrar Index view'ine yönlendir
            // Return a success or failure JSON result
            if (ViewBag.Message != null) // Assuming ViewBag.Message is set on success
            {
                return Json(new { status = "success", message = ViewBag.Message }, JsonRequestBehavior.AllowGet);
            }
            else if (ViewBag.Error != null) // Assuming ViewBag.Error is set on error
            {
                return Json(new { status = "error", message = ViewBag.Error }, JsonRequestBehavior.AllowGet);
            }
            else // Default fallback if neither message nor error is set
            {
                return Json(new { status = "error", message = "An unexpected error occurred." }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: UserMedicalTests/Delete
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();
                var userMedicalTest = await _context.UserMedicalTests
                    .SingleOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == currentUserId);

                if (userMedicalTest == null)
                {
                    return Json(new { success = false, message = "Medical test not found or access denied." });
                }

                // Get the physical file path
                var serverFilePath = Server.MapPath(userMedicalTest.FilePath);

                // Remove from database first
                _context.UserMedicalTests.Remove(userMedicalTest);
                await _context.SaveChangesAsync();

                // Attempt to delete the physical file
                if (System.IO.File.Exists(serverFilePath))
                {
                    System.IO.File.Delete(serverFilePath);
                }
                // Note: If file deletion fails but database record is removed,
                // the record is gone but file remains. This might be acceptable,
                // or you might want more robust error handling/logging.
                // For now, proceed as success if DB deletion worked.

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception ex
                // Consider returning a more specific error message in production
                return Json(new { success = false, message = "An error occurred while trying to delete the medical test: " + ex.Message });
            }
        }

        // Dosya indirme veya görüntüleme metodları daha sonra eklenebilir.

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}