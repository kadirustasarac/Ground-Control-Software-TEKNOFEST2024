import cv2

# Webcam'den video akışını al
cap = cv2.VideoCapture(0)  # 0, birincil kamerayı temsil eder, eğer birden fazla kamera varsa uygun numarayı girin

# RTSP yayını için codec ve ayarlar
codec = cv2.VideoWriter_fourcc(*'H264')  # Video codec, H.264'ü kullanıyoruz
fps = 30.0  # FPS (frame per second), 30 olarak ayarlandı
resolution = (640, 480)  # Çözünürlük, (width, height) şeklinde

# RTSP sunucusu ayarları
rtsp_server = "rtsp://localhost:8554/live"  # RTSP sunucu adresi ve portu, dilediğiniz gibi değiştirebilirsiniz

# RTSP sunucusuna bağlanmak için VideoWriter nesnesi oluştur
rtsp_out = cv2.VideoWriter(rtsp_server, codec, fps, resolution)

# Webcam yayınını al ve RTSP sunucusuna gönder
while True:
    ret, frame = cap.read()
    if not ret:
        print("Webcam yayını alınamadı.")
        break

    # Frame'i RTSP sunucusuna gönder
    rtsp_out.write(frame)

    # Alınan görüntüyü göster
    cv2.imshow('Webcam Yayını', frame)

    # Çıkış için 'q' tuşuna bas
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Bağlantıları serbest bırak
cap.release()
rtsp_out.release()
cv2.destroyAllWindows()