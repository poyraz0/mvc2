from flask import Flask, request, jsonify
import pdfplumber

app = Flask(__name__)

@app.route('/extract', methods=['POST'])
def extract_pdf_data():
    data = request.files['pdf']  # Gelen PDF dosyası
    with pdfplumber.open(data) as pdf:
        text = ""
        for page in pdf.pages:
            text += page.extract_text()

    # Örneğin, sadece gerekli verileri parse et
    extracted_data = parse_text_to_features(text)
    return jsonify({"features": extracted_data})

def parse_text_to_features(text):
    # PDF içeriğini analiz için uygun formata dönüştür
    return [1.2, 3.4, 5.6, 2.3, 4.5, 6.7]  # Örnek dönüşüm

if __name__ == '__main__':
    app.run(debug=True)
