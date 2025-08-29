from flask import Flask, request, jsonify
import joblib
from flask_cors import CORS  # <-- Bunu ekliyoruz

# Flask app
app = Flask(__name__)
CORS(app)  # <-- Bu satır tüm origin'lere izin verir

# Eğitilmiş modeli yükle
model = joblib.load(r"C:\xampp\htdocs\Fake_News_Detection_Website\fakenewsdetectionwebsite\models\sgd_model.pkl")

@app.route("/predict", methods=["POST"])
def predict():
    try:
        data = request.get_json()
        text = data.get("text", "")
        if not text.strip():
            return jsonify({"error": "Text is empty"}), 400
        prediction = model.predict([text])[0]
        if hasattr(model, "predict_proba"):
            proba = model.predict_proba([text])[0].max()
            return jsonify({
                "text": text,
                "prediction": str(prediction*100),
                "confidence": float(proba)
            })
        else:
            return jsonify({
                "text": text,
                "prediction": str(prediction)
            })
    except Exception as e:
        return jsonify({"error": str(e)}), 500
if __name__ == "__main__":
    app.run(debug=True)
