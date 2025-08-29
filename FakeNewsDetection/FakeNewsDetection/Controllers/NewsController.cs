using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FakeNewsMVC.Data;
using FakeNewsMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FakeNewsMVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://127.0.0.1:5000/") // Flask API adresi
            };
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Predict()
        {
            return View(new PredictionResult());
        }

        [HttpPost]
        
public async Task<IActionResult> Predict(string newsText)
        {
            // Tahmin sonucu modeli oluþtur
            var model = new PredictionResult
            {
                Text = newsText,
                Error = null,         // Baþlangýçta hata yok
                CreatedAt = DateTime.Now  // Oluþturulma zamaný
            };

            try
            {
                // Session'dan kullanýcý Id al
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    model.Error = "You must be logged in to make a prediction.";
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(newsText))
                {
                    model.Error = "Please enter some news text.";
                    return View(model);
                }

                // Flask API’ye POST isteði gönder
                var response = await _httpClient.PostAsJsonAsync("predict", new { text = newsText });
                response.EnsureSuccessStatusCode();

                var resultJson = await response.Content.ReadFromJsonAsync<FlaskResponse>();

                if (resultJson == null)
                {
                    model.Error = "Flask API returned null.";
                    return View(model);
                }

                // Prediction deðerini string olarak kaydet
                model.Prediction = resultJson.Prediction == "0" ? "Fake News" :
                                   resultJson.Prediction == "1" ? "Real News" :
                                   "Unknown";

                // Confidence yüzde olarak kaydet (0.63 -> 63.00%)
                model.Confidence = (resultJson.Confidence * 100).ToString("F2") + "%";

                // Session'dan gelen kullanýcý Id
                model.UserId = userId.Value;

                // Veritabanýna kaydet
                _context.Predictions.Add(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Hata oluþursa model.Error alanýna yaz
                model.Error = ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrediction(int id)
        {
            var prediction = await _context.Predictions.FindAsync(id);
            if (prediction == null)
            {
                return NotFound();
            }

            // Sadece oturumdaki kullanýcý kendi tahminini silebilir
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || prediction.UserId != userId.Value)
            {
                return Forbid();
            }

            _context.Predictions.Remove(prediction);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Prediction deleted successfully!";
            return RedirectToAction("PatientResults");
        }


        [HttpGet]
        public IActionResult PatientResults()
        {
            // Session'dan UserId al
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                // Giriþ yoksa boþ liste döndürebilirsin veya login sayfasýna yönlendirebilirsin
                return RedirectToAction("Login", "Account");
            }

            // Sadece session'daki kullanýcýya ait tahminleri al
            var predictions = _context.Predictions
                                      .Include(p => p.User) // User bilgisi gelsin
                                      .Where(p => p.UserId == userId.Value)
                                      .OrderByDescending(p => p.CreatedAt)
                                      .ToList();

            return View(predictions);
        }




        // Flask API’den dönen JSON için sýnýf
        private class FlaskResponse
        {
            public string Prediction { get; set; } = "0";    // "0" veya "1"
            public double Confidence { get; set; } = 0.0;    // numeric deðer
        }
    }
}
