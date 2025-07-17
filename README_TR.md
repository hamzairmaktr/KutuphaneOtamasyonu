# IKitaplik - Kütüphane Otomasyon Sistemi

[🇹🇷 Türkçe](README_TR.md) \| [🇬🇧 English](README.md)

## Genel Bakış

**IKitaplik**, modern .NET teknolojileriyle geliştirilmiş, modüler ve tam kapsamlı bir Kütüphane Otomasyon Sistemidir. Güçlü bir arka uç REST API’si, etkileşimli ve modern bir Blazor UI arayüzü ve sürdürülebilir, ölçeklenebilir katmanlı mimarisiyle öne çıkar. Sistem; kitap, öğrenci, yazar, kategori, ödünç alma (emanet), bağış ve kullanıcı yönetimi ile kimlik doğrulama ve yetkilendirme özellikleri sunar.

---

## İçindekiler

- [Mimari](#mimari)
- [Teknolojiler](#teknolojiler)
- [Özellikler](#özellikler)
- [Proje Yapısı](#proje-yapısı)
- [Kurulum](#kurulum)
- [API Uç Noktaları](#api-uç-noktaları)
- [Arayüz Özeti](#arayüz-özeti)
- [Varlıklar (Entities)](#varlıklar-entities)
- [Katkı Sağlama](#katkı-sağlama)
- [Lisans](#lisans)

---

## Mimari

Çözüm, her biri belirli bir sorumluluğa sahip birkaç projeden oluşur:

- **Core**: Ortak temel varlıklar, arayüzler ve yardımcı sınıflar.
- **IKitaplik.Entities**: Alan modeline ait entity ve DTO tanımları.
- **IKitaplik.DataAccess**: Entity Framework Core ile veri erişim katmanı, repository ve unit of work desenleri.
- **IKitaplik.Business**: İş mantığı, servis arayüzleri ve uygulamaları, doğrulama katmanı.
- **IKitaplik.Api**: RESTful uç noktalar sunan ASP.NET Core Web API.
- **IKitaplik.BlazorUI**: Son kullanıcılar için kimlik doğrulamalı ve CRUD işlemleri içeren Blazor Server arayüzü.

---

## Teknolojiler

- **.NET 7+ / .NET 8+**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **Blazor Server**
- **AutoMapper**
- **FluentValidation**
- **JWT Authentication**
- **MudBlazor** (UI bileşenleri)
- **Blazored.LocalStorage** (token saklama için)
- **Swagger/OpenAPI** (API dokümantasyonu)

---

## Özellikler

- **Kitap Yönetimi**: Kitap ekleme, güncelleme, silme, listeleme ve filtreleme.
- **Öğrenci Yönetimi**: Öğrenci kaydı, güncelleme, silme ve listeleme.
- **Yazar & Kategori Yönetimi**: Yazar ve kategoriler için CRUD işlemleri.
- **Emanet (Ödünç) Sistemi**: Hangi öğrencinin hangi kitabı aldığını, teslim tarihlerini ve iadeleri takip etme.
- **Bağış Takibi**: Öğrencilerden gelen kitap bağışlarını yönetme.
- **Kullanıcı Kimlik Doğrulama**: Kayıt, giriş, JWT tabanlı kimlik doğrulama ve rol bazlı yetkilendirme.
- **Hareketler Logu**: İşlemlerin (emanet, iade, bağış vb.) kaydını tutma.
- **Duyarlı Arayüz**: Modern, etkileşimli Blazor Server arayüzü ve MudBlazor bileşenleri.
- **Doğrulama**: FluentValidation ile sunucu tarafı doğrulama.
- **API Dokümantasyonu**: Swagger UI ile entegre API keşfi.

---

## Proje Yapısı

```
IKitaplik.sln
│
├── Core/                  # Temel varlıklar, arayüzler, yardımcılar
├── IKitaplik.Entities/    # Domain entity ve DTO'lar
├── IKitaplik.DataAccess/  # EF Core repository, migration, unit of work
├── IKitaplik.Business/    # İş mantığı, servisler, doğrulama
├── IKitaplik.Api/         # ASP.NET Core Web API
└── IKitaplik.BlazorUI/    # Blazor Server arayüzü
```

---

## Kurulum

### Gereksinimler

- [.NET SDK 7+](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/tr-tr/sql-server/sql-server-downloads) veya uyumlu bir veritabanı

### Adımlar

1. **Depoyu klonlayın:**
   ```bash
   git clone https://github.com/hamzairmaktr/KutuphaneOtamasyonu
   cd KutuphaneOtamasyonu
   ```

2. **Veritabanı bağlantısını yapılandırın:**
   - `IKitaplik.Api/appsettings.json` ve `IKitaplik.BlazorUI/appsettings.json` dosyalarındaki bağlantı bilgisini güncelleyin.

3. **Migration'ları uygulayın:**
   ```bash
   dotnet ef database update --project IKitaplik.DataAccess
   ```

4. **API'yi başlatın:**
   ```bash
   dotnet run --project IKitaplik.Api
   ```

5. **Blazor UI'yi başlatın:**
   ```bash
   dotnet run --project IKitaplik.BlazorUI
   ```

6. **Uygulamaya erişin:**
   - API: `https://localhost:<port>/swagger`
   - UI: `https://localhost:<port>/`

---

## API Uç Noktaları

API, tüm ana varlıklar için uç noktalar sunar. Örnekler:

- `POST /api/auth/login` - Kullanıcı girişi
- `POST /api/auth/register` - Kullanıcı kaydı
- `GET /api/book/getall` - Tüm kitapları listele
- `POST /api/book/add` - Yeni kitap ekle
- `POST /api/book/update` - Kitap güncelle
- `POST /api/book/delete` - Kitap sil
- `GET /api/student/getall` - Tüm öğrencileri listele
- ...ve kategoriler, yazarlar, emanetler, bağışlar ve hareketler için diğer uç noktalar.

Tüm uç noktalar JWT kimlik doğrulama ve rol bazlı yetkilendirme ile korunmaktadır.

---

## Arayüz Özeti

- **Giriş/Kayıt:** Güvenli kullanıcı kimlik doğrulama.
- **Navigasyon:** Anasayfa, Kitap Listesi, Kitap Ekle vb. için yan menü.
- **Kitap Listesi:** Sayfalı, aramalı ve sıralanabilir kitap tablosu.
- **Kitap Ekle:** Yeni kitap ekleme formu.
- **Öğrenci Yönetimi:** Öğrenci listeleme, ekleme, güncelleme ve silme.
- **Duyarlı Tasarım:** Modern MudBlazor bileşenleriyle şık ve kullanıcı dostu arayüz.

---

## Varlıklar (Entities)

### Temel Varlıklar

- **Book (Kitap):** Barkod, İsim, Kategori, Yazar, Raf, Adet, Durum, Sayfa Sayısı vb.
- **Student (Öğrenci):** Öğrenci No, İsim, Sınıf, Telefon, E-posta, Okunan Kitap Sayısı, Puan vb.
- **Writer (Yazar):** İsim, Doğum Tarihi, Ölüm Tarihi, Biyografi.
- **Category (Kategori):** İsim.
- **Deposit (Emanet):** KitapId, ÖğrenciId, Veriliş Tarihi, Teslim Tarihi, Teslim Edildi mi vb.
- **Donation (Bağış):** Tarih, ÖğrenciId, KitapId, Hasarlı mı.
- **User (Kullanıcı):** Kullanıcı Adı, İsim Soyisim, E-posta, ŞifreHash, Rol, RefreshToken vb.
- **Movement (Hareket):** Hareket Tarihi, Başlık, Not, Tip, ilgili entity ID’leri.

### DTO'lar

DTO’lar, API iletişimi ve doğrulama için kullanılır; iç modeller ile dış sözleşmelerin ayrılmasını sağlar.

---

## Katkı Sağlama

Katkılarınızı bekliyoruz! Hata bildirmek, iyileştirme veya yeni özellik eklemek için lütfen issue açın veya pull request gönderin.

---

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakınız.

---

**Not:** Detaylı API sözleşmeleri için API çalışırken `/swagger` adresindeki Swagger UI'yi inceleyebilirsiniz. 