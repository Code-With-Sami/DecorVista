# 🏡 DecorVista – Home Interior Design Web Application

DecorVista is a **full-featured ASP.NET MVC** web application designed for the home interior design industry.  
It connects **homeowners**, **professional interior designers**, and **admins** through a single platform for inspiration galleries, product catalogs, consultations, reviews, and more.

---

## 📋 Features

### **Public/Guest**
- Browse inspiration gallery
- View blog posts
- About Us & Contact Us pages
- Register & Login

### **User Dashboard**
- View & save designs
- Browse & search products
- Book consultations with designers
- Manage saved items
- Write reviews for products/designers
- View notifications & order history

### **Designer Dashboard**
- Manage portfolio
- Accept/decline consultations
- Respond to reviews
- View upcoming bookings

### **Admin Dashboard**
- Manage users, products, and categories
- Manage inspiration gallery
- Create/edit blog posts
- Generate and download reports

---

## 🗂 Technology Stack

**Frontend**
- HTML5, CSS3, Bootstrap
- JavaScript, jQuery
- Razor Views (.cshtml)

**Backend**
- C# ASP.NET MVC 5 / ASP.NET Core MVC (optional upgrade)
- Entity Framework (Code First or Database First)

**Database**
- Microsoft SQL Server 2019 or higher

**IDE**
- Visual Studio 2019 or higher

---

## 📊 Database Structure

### **Main Tables**
1. `Users`
2. `UserDetails`
3. `InteriorDesigner`
4. `Products`
5. `Categories`
6. `ProductCategories`
7. `Consultations`
8. `Reviews`
9. `InspirationImages`
10. `BlogPosts`
11. `Notifications`
12. `OrderHistory`
13. `Reports`

**Relationships:**
- One-to-One: `Users` ↔ `UserDetails`
- One-to-Many: `Users` → `Consultations`
- One-to-Many: `InteriorDesigner` → `Consultations`
- Many-to-Many: `Products` ↔ `Categories` (via `ProductCategories`)

---

## 📁 Project Structure (ASP.NET MVC)

```
DecorVista/
│
├── Controllers/
│   ├── HomeController.cs
│   ├── AccountController.cs
│   ├── UserDashboardController.cs
│   ├── InspirationGalleryController.cs
│   ├── ProductController.cs
│   ├── ConsultationController.cs
│   ├── ReviewController.cs
│   ├── OrderHistoryController.cs
│   ├── DesignerDashboardController.cs
│   ├── PortfolioController.cs
│   ├── DesignerConsultationController.cs
│   ├── DesignerReviewController.cs
│   ├── AdminDashboardController.cs
│   ├── AdminUserController.cs
│   ├── AdminProductController.cs
│   ├── AdminGalleryController.cs
│   ├── AdminBlogController.cs
│   ├── AdminReportController.cs
│
├── Models/
│   ├── User.cs
│   ├── UserDetail.cs
│   ├── InteriorDesigner.cs
│   ├── Product.cs
│   ├── Category.cs
│   ├── ProductCategory.cs
│   ├── Consultation.cs
│   ├── Review.cs
│   ├── InspirationImage.cs
│   ├── BlogPost.cs
│   ├── Notification.cs
│   ├── OrderHistory.cs
│   ├── Report.cs
│
├── Views/
│   ├── Shared/ (_Layout.cshtml, _Navbar.cshtml, _Footer.cshtml)
│   ├── Home/
│   ├── Account/
│   ├── UserDashboard/
│   ├── DesignerDashboard/
│   ├── AdminDashboard/
│   ├── Product/
│   ├── Consultation/
│   ├── Review/
│   ├── Blog/
│
├── wwwroot/ (CSS, JS, Images)
│
├── appsettings.json (for DB connection in ASP.NET Core)
│
└── README.md
```

---

## ⚙️ Installation & Setup

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/your-username/DecorVista.git
```

### 2️⃣ Open in Visual Studio
- Open the `.sln` file in **Visual Studio 2019 or higher**.

### 3️⃣ Configure Database
- Update your **SQL Server connection string** in:
  - `Web.config` (ASP.NET MVC 5) OR
  - `appsettings.json` (ASP.NET Core MVC)

Example:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=DecorVistaDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 4️⃣ Run Database Migrations
If using **Entity Framework Code First**:
```powershell
Update-Database
```
If using **Database First**:
- Import `DecorVistaDB.sql` into SQL Server.

### 5️⃣ Run the Application
Press **F5** in Visual Studio.

---

## 🔑 Default User Roles for Testing
| Role | Email | Password |
|------|-------|----------|
| Admin | admin@decorvista.com | Admin123 |
| Designer | designer@decorvista.com | Designer123 |
| User | user@decorvista.com | User123 |

---

## 📹 Project Demonstration
- A video demonstration of all functionalities is **mandatory**.
- Include the hosted URL if deploying online.

---

## 📌 Future Enhancements
- Online payment integration
- AI-based interior design suggestions
- Augmented Reality (AR) product previews
- Mobile app version

---

## 📜 License
This project is for **academic and learning purposes** only.  
You may modify and enhance it for your own use.

---

