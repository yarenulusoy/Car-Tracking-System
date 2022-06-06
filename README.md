# Car-Tracking-System
 .Net Core Mvc ile yazılmış Google Maps  ve İsveç taksi verisi kullanılarak RabbitMQ ve Firebase ile kullanıcıların araçlarının konumlarını ve saatlerini görebildiği web sitesi

-Proje .NET Core Mvc dilinde gerçekleştirilmiştir.

-Message broker olarak RabbitMq kullanılmıştır.

-Nosql veritabanı olarak Google Firestore kullanılmıştır.

-Harita işlemleri için Google Maps Api kullanılmıştır.

Bu projede bir araç takip sistemi yapmamız istenmiştir. Her kullanıcının sahip olduğu araçlar vardır ve bu bilgilere dayanarak kullanıcı girişi olan, sisteme girdiğinde sadece kendi araçlarını görebilen ve araçların geçmiş konumlarını ve zaman aralığını seçip görebilen bir sistem tasarlanmıştır. 


  <img
  src="/images/1.png"
  alt="Alt text"
  title="Optional title"
  style="display: inline-block; margin: 0 auto; width: 500px"> 
  
  -Giriş yapıldıktan sonra, sadece kendi araçlarının konumlarını görebildiği sayfa çıkıyor. Bu sayfada son 30 dk veriler gelir.

  
  <img
  src="/images/2.png"
  alt="Alt text"
  title="Optional title"
  style="display: inline-block; margin: 0 auto; width: 500px"> 
  
  -Eğer kullanıcı araçların geçmiş konumuna bakmak isterse araç seçtikten sonra bir diyalog penceresi çıkar ve zaman aralığı seçimi yapar.
  
  
  <img
  src="/images/3.png"
  alt="Alt text"
  title="Optional title"
  style="display: inline-block; margin: 0 auto; width: 500px"> 
  
  --Kullanıcı seçim yaptıktan sonra, sadece girdiği zaman aralığındaki o aracın geçmiş verileri gelir.
  
  
