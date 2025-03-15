# SovosDocApi Projesi

Bu Proje Sovos Case Study için oluşturulmuş bir API projesidir.

## İçindekiler
- [Proje Açıklaması](#proje-açıklaması)
- [Kurulum](#kurulum)
- [API İşlemleri](#api-işlemleri)
  - [Fatura İşlemleri](#fatura-i̇şlemleri)
  - [Job İşlemi](#job-i̇şlemi)
- [Proje Yapısı](#proje-yapısı)

## Proje Açıklaması
Proje .Net Core 8 ile geliştirilmiş bir API uygulamasıdır. API'nin temel amacı fatura işlemleri yapar (fatura ekleme, fatura listeleme ve fatura satırlarını alma ) ve zamanlanmış bir job işlemi gerçekleştirir.

- **Veritabanı**: Veritabanı olarak Sqlite kullanılmıştır.
- **Job İşleme**: Cron Expression olarak Quartz kullanılmıştır.
- **Swagger**: API methodları için Swagger arayüzü kullanılmıştır. API'yi çalıştırdığınızda, Swagger arayüzüne `http://localhost:5218/swagger/index.html` adresinden erişebilirsiniz.

## Kurulum

1. **Proje Dosyalarını İndirin**
   Git kullanarak projeyi klonlayabilirsiniz:
   ```bash
   git clone https://github.com/huricaki/Sovos.git
   ```

2. **Bağımlılıkları Yükleyin**
   Proje dizinine gidin ve gerekli NuGet paketlerini yüklemek için aşağıdaki komutu çalıştırın:
   ```bash
   cd SovosDocApi 
   cd SovosDocApi
   dotnet restore
   ```
3. **Projeyi Çalıştırın**
   Projeyi çalıştırmak için aşağıdaki komutu kullanabilirsiniz:
   ```bash
   dotnet run
   ```

   API, `http://localhost:5218` adresinde çalışmaya başlayacaktır.

## API İşlemleri

### Fatura İşlemleri
API, Fatura işlemleri için aşağıda yer alan işlemleri sunar;

1. **CreateInvoice (POST)**
 - URL: `/api/Invoice`
   - Açıklama: Gelen json objesini kaydeder.
   - Request Body:
     ```json
        {
            "InvoiceHeader": {
            "InvoiceId": "SVS202300000013",
            "SenderTitle": "Gönderici Firma7",
            "ReceiverTitle": "Alıcı Firma7",
            "Date": "2023-01-11",
            "EMail": "test7@example.com"
            },
            "InvoiceLine": [
                {
                    "Id": 7,
                    "Name": "test",
                    "Quantity": 4,
                    "UnitCode": "Metre",
                    "UnitPrice": 40
                }
            ]
    }
     ```
   - Response:
     ```json
    {
        "message": "Fatura başarıyla kaydedildi.",
        "invoiceId": "SVS202300000013"
    }
     ```

2. **GetInvoices (GET)**
   - URL: `/api/Invoice`
   - Açıklama: Tüm Faturaların Başlıklarını döner
   - Response:
     ```json
        [
            {
                "InvoiceId": "SVS202300000001",
                "SenderTitle": "Gönderici Firma",
                "ReceiverTitle": "Alıcı Firma",
                "Date": "2023-01-05",
                "EMail": "hurcak04@gmail.com"
            },
            {
                "InvoiceId": "SVS202300000002",
                "SenderTitle": "Gönderici Firma2",
                "ReceiverTitle": "Alıcı Firma2",
                "Date": "2023-01-05",
                "EMail": "hurcak04@gmail.com"
            }
        ]
     ```
3. **GetInvoiceLines (Get("{id}"))**
   - URL: `/api/Invoice/{id}`
   - Açıklama: İlgili Fatura idsine göre faturanın başlık ve satırlarını döner
   - Parametre:
     ```string
     SVS202300000004
     ```
   - Response:
     ```json
        {
            "InvoiceHeader": {
                "InvoiceId": "SVS202300000004",
                "SenderTitle": "Gönderici Firma aş",
                "ReceiverTitle": "Alıcı Firma aş",
                "Date": "2023-01-05",
                "EMail": "hurcak04@gmail.com"
            },
            "InvoiceLine": [
                {
                "Id": 1,
                "Name": "gluten",
                "Quantity": 5,
                "UnitCode": "Adet",
                "UnitPrice": 10
                },
                {
                "Id": 2,
                "Name": "mısır nişasta",
                "Quantity": 2,
                "UnitCode": "Litre",
                "UnitPrice": 3
                },
                {
                "Id": 3,
                "Name": "bugday",
                "Quantity": 25,
                "UnitCode": "Kilogram",
                "UnitPrice": 2
                }
            ]
        }
     ```

### Job İşlemi
Bu proje zamanlanmış bir job işlemi içerir. Bu job işlenmemiş faturaların belirli bir zamanda mail göndermesi için çalışır.

1. **Job Çalıştırma**

Job, belirli bir zamanlamaya göre çalışır, Job'ı tetiklenmesi için aşağıdaki uç noktaların kontrolleri sağlanmalıdır;

- appsetting.json belgesi içinde yer alan **EMailJob** tagı aşağıdaki gibi **Calistir** kısmı **true** olmalı ve **ZamanTrigger** içeriği ise hangi saat ve dakika çalışmasını istiyorsanız soldan sağa; saniye-dakika-saat-gün-ay olacak şekilde ayarlama yapılmalıdır.

  ```json
     "EMailJob": {
    "Calistir": true, 
    "ZamanTrigger": "0 43 22 * * ?" 
    }
     ```

## Proje Yapısı
- **Controllers**: Fatura işlemlerini yöneten denetleyiciler.
- **Models**: Veritabanı modelleri ve Dto.
- **Repositories**: Veritabanı işlemlerinin yapıldığı generic servisler.
- **Quartz**: Arka plan görevlerinin yer aldığı sınıflar.
- **DataAccess**: Veritabanı bağlamı ve migrationlar.
