# 🏥 ClaimCare Backend API

## 📌 Overview

ClaimCare Backend is a RESTful API built using ASP.NET Core Web API to manage insurance claim processing, invoice requests, and user management. It serves as the backend service for the ClaimCare application, handling business logic, authentication, and database operations.

---

## ⚙️ Tech Stack

* **Framework:** ASP.NET Core Web API
* **Language:** C#
* **Database:** SQL Server
* **ORM:** Entity Framework Core
* **Authentication:** JWT (JSON Web Token)
* **Tools:** Swagger (API Testing)

---

## 🚀 Features

* 👤 User Authentication & Authorization (JWT-based)
* 🏥 Patient & Provider Management
* 📄 Claim Submission & Tracking
* 🧾 Invoice Request & Processing
* 📧 Email Notifications
* 🔐 Role-Based Access Control (Admin, Patient, Provider)
* 📊 Logging & Error Handling

---

## 📁 Project Structure

Controllers → Handle HTTP requests
Services → Business logic
Repositories → Database operations
Models → Entity definitions
DTOs → Data transfer objects

---

## 🔧 Setup Instructions

### 1️⃣ Clone the repository

```bash
git clone https://github.com/your-username/claimcare-backend.git
cd claimcare-backend
```

---

### 2️⃣ Configure Database

Update connection string in:
`appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Your_SQL_Server_Connection_String"
}
```

---

### 3️⃣ Run Migrations

```bash
dotnet ef database update
```

---

### 4️⃣ Run the application

```bash
dotnet run
```

---

### 5️⃣ Access API

Swagger UI:

```
https://localhost:xxxx/swagger
```

---

## 🔐 Authentication

* JWT-based authentication is implemented
* Include token in header:

```
Authorization: Bearer <your_token>
```

---

## 📌 API Endpoints (Sample)

* POST /api/auth/login
* POST /api/patient/request-invoice
* POST /api/claims/submit
* GET /api/claims/{id}

---

## ⚠️ Important Notes

* Do not expose sensitive data (passwords, secrets)
* Use environment variables for production
* Ensure proper validation and error handling

---

## 👩‍💻 Author

Developed by Janani

---

## 🌟 Future Enhancements

* Microservices architecture
* Docker containerization
* Cloud deployment (Azure)
* Advanced logging & monitoring

---

## 📄 License

This project is for educational and demonstration purposes.
