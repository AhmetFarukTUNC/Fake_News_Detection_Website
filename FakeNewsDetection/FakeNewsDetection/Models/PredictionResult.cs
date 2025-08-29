using System;

namespace FakeNewsMVC.Models
{
    public class PredictionResult
    {
        public int Id { get; set; }
        public string Text { get; set; }      // Haber metni
        public string Prediction { get; set; } // Fake / Real
        public string Confidence { get; set; } // Güven yüzdesi
        public string? Error { get; set; }      // Tahmin sırasında oluşan hata varsa
        public DateTime CreatedAt { get; set; }

        // Foreign Key
        public int UserId { get; set; }
        public User User { get; set; } // Navigation property
    }
}
