namespace FakeNewsMVC.Models
{
    public partial class NewsController
    {
        // Flask API JSON cevabı için sınıf
        public class FlaskResponse
        {
            public string Prediction { get; set; }
            public double Confidence { get; set; }
        }
    }
}
