# ErsaCommerce

## 📦 E-Commerce API

.NET 8 tabanlı bu RESTful API, bir e-ticaret platformunun müşteri, ürün ve sipariş yönetimi ihtiyaçlarını karşılamak için geliştirilmiştir.

---

### 🛠 Kullanılan Teknolojiler

* ✅ **.NET 8**
* ✅ **Entity Framework Core** (Code/Migration-first)
* ✅ **MSSQL**
* ✅ **MediatR** (CQRS Pattern)
* ✅ **JWT Authentication**
* ✅ **Soft Delete** (`DeletedAt` field)
* ✅ **Custom Response Wrapper** (`Response<T>` modeli)

---

### 📁 Katmanlı Mimari

```
📁 API              → Controller'lar ve giriş noktası
📁 Application      → CQRS yapısı, MediatR Handler'lar, DTO'lar
📁 Domain           → Entity modelleri
📁 Data             → EF Core DbContext, veri erişimi
📁 Infrastructure   → JWT, DBInitializer
📁 Shared           → Response model

```

---

### 🧹 Veri Modelleri

* **Customer**
* **Product**
* **Order**
* **OrderItem**

---

### 🔐 Kimlik Doğrulama

JWT tabanlı authentication uygulanmıştır.
Kullanıcı girişinde token alınır, sonraki işlemler `Authorization: Bearer <token>` ile yapılır.

```json
POST /api/auth/login
{
  "username": "admin",
  "password": "123456"
}
```

---

### 🚀 API Uç Noktaları (Özet)

| Metot | URL                                          | Açıklama                           |
| ----- | --------------------------                   | ---------------------------------- |
| POST  | /api/auth/register                           | Kullanıcı kaydı                    |
| POST  | /api/auth/login                              | Giriş ve token alma                |
| GET   | /api/customer/list-customer                  | Müşteri listeleme                  |
| POST  | /api/customer/create-customer                | Müşteri ekleme                     |
| POST  | /api/order/create-order                      | Sipariş oluşturma                  |
| GET   | /api/order/get-order-detail                  | Sipariş detaylarını getirme        |
| GET   | /api/customer/list-order-by-customers        | Müşterinin siparişlerini listeleme |
| PUT   | /api/order/update-order-status               | Sipariş durumunu güncelleme        |
| GET   | /api/customer/sp-list-order-by-customers     | Stored Procedure ile müşterinin siparişlerini listeleme |


---

### 📌 Sipariş Kuralları

* Sipariş oluşturulurken `CustomerId` geçerli olmalıdır.
* Sipariş sadece `Pending` durumundayken güncellenebilir.
* `TotalAmount` uygulama içinden ürün fiyatlarına göre hesaplanır.

---

### ⚙️ Projeyi Çalıştırma

1. **Veritabanı ayarlarını yap** → `appsettings.json`
2. **Migration ve DB oluştur**
3. **Projeyi başlat**

---
