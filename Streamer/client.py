import cv2

# RTSP stream URL
rtsp_url = 'rtsp://10.53.201.206:8554/mystream'

# RTSP bağlantısını başlat
cap = cv2.VideoCapture(rtsp_url)

# Bağlantı başarılı mı kontrol et
if not cap.isOpened():
    print("RTSP yayını açılamadı.")
    exit()

# Yayını al ve göster
while True:
    ret, frame = cap.read()
    if not ret:
        print("Yayın sona erdi.")
        break

    # Alınan görüntüyü göster
    cv2.imshow('RTSP Yayını', frame)

    # Çıkış için 'q' tuşuna bas
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Bağlantıyı kapat
cap.release()
cv2.destroyAllWindows()