
# 🔐 Role-Based Access Control (RBAC) Web Application

A secure and modular web app built using **ASP.NET Core MVC**, **Entity Framework Core**, **ASP.NET Identity**, and **MySQL**.

This app supports:
- ✅ User registration & login
- ✅ Email confirmation
- ✅ Role-based dashboards (Admin, Manager, User)
- ✅ Admin tools to manage user roles

---

## 🚀 Features

- 🔐 ASP.NET Identity for secure user authentication
- 🛡️ Role-based authorization with dynamic routing
- 👨‍💼 Admin panel to manage users and assign roles
- 📬 Email confirmation using SMTP (Gmail supported)
- 🧱 Entity Framework Core with MySQL backend

---

## 🧪 Demo Credentials (Default Admin)

> ⚠️ For development/testing purposes only

| Role   | Email            | Password   |
|--------|------------------|------------|
| Admin  | `admin@rbac.com` | `Admin@123` |

✅ Email confirmation is **bypassed or auto-confirmed** for this default Admin.  
You can log in immediately and manage other users.

---

## 📁 Tech Stack

- **Frontend**: Razor Views (.cshtml)
- **Backend**: ASP.NET Core MVC, Identity, EF Core
- **Database**: MySQL 8.0
- **Email**: SMTP (Gmail App Password)

---

## ⚙️ Setup Instructions

### 1. 📦 Clone the Repository

```bash
git clone https://github.com/Akshat-Ajit/RBACWebApp.git
cd RBACWebApp
```

> 🔍 **Before proceeding**, make sure to open and review your `appsettings.json` file — update your MySQL credentials and Gmail SMTP settings.

---

### 2. 🛠 Configure `appsettings.json`

> 🔐 **Do not commit real credentials to version control!**

#### 🔧 🔐 MySQL Connection

Update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=rbac_db;user=YOUR_USERNAME;password=YOUR_PASSWORD;"
}
```

#### 📧 Gmail SMTP Email Settings

Update with your Gmail address and **App Password**:

```json
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "EnableSSL": true,
  "UserName": "your-email@gmail.com",
  "Password": "your-app-password"
}
```

---

### 🔑 How to Create a Gmail App Password

1. Go to your [Google Account Security Settings](https://myaccount.google.com/security)
2. Enable **2-Step Verification**
3. Visit [App Passwords](https://myaccount.google.com/apppasswords)
4. Generate an app password for "Mail" and "Windows Computer"
5. Copy it and paste it in `EmailSettings:Password`

---

### 3. 🧱 Run Migrations & Seed Roles/Admin

In **Package Manager Console** or terminal:

```bash
Add-Migration InitialCreate
Update-Database
```

Or via CLI:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This will:

- Create the database
- Add roles: `Admin`, `Manager`, `User`
- Seed the default admin user (auto-confirmed)

---

### 4. ▶️ Run the App

```bash
dotnet run
```

Visit: [https://localhost:7001](https://localhost:7001)

---

## 📂 Project Structure (Highlights)

```
RBACWebApp/
├── Controllers/
│   ├── AccountController.cs
│   └── AdminController.cs
├── Models/
│   ├── ApplicationUser.cs
│   └── ViewModels/
├── Data/
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── Services/
│   ├── IEmailSender.cs
│   └── EmailSender.cs
├── Views/
│   ├── Admin/
│   ├── Account/
│   └── Shared/
└── appsettings.json  <-- 🔍 VERIFY THIS BEFORE RUNNING
```

---

## 📬 Email Confirmation

✔️ All new users must confirm their email before logging in  
✉️ Emails are sent via the configured SMTP server (e.g. Gmail)

---

## 🧩 Future Enhancements

- 🔄 Forgot Password / Reset Password Flow
- 📊 Admin Dashboard with system stats
- 🔍 User search & filtering
- 🌐 Localization support

---

## 🧑‍💻 Author

**Akshat Ajit**  
📧 [your-email@example.com]  
🔗 [github.com/Akshat-Ajit](https://github.com/Akshat-Ajit)

---
