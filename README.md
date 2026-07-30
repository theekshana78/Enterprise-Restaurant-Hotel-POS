# 🏨 ABC Hotel & Restaurant — Enterprise POS & Management System

> **Enterprise-Level Desktop POS Application for Restaurants, Hotel Room Service & Mini Bar Management**
>
> Version: `v1.0.0 Enterprise Edition`  
> Platform: `C# .NET 9 WPF Desktop` | Database: `SQLite / EF Core Embedded`

---

## 📌 System Overview

**EnterprisePOS** is a comprehensive multi-tier desktop software engineered for hospitality operations. It seamlessly unifies **Point-of-Sale (POS) Billing, Hotel Room Reservation & Charge Routing, Kitchen Display Tracking (KDS), Mini Bar Stock Management, Recipe-Based Inventory Auto-Deduction, Customer Loyalty, and AI-Powered Demand Forecasting**.

```
EnterprisePOS Solution Architecture
 ├── EnterprisePOS.Core       --> Domain Entities, Enums & Security Helpers
 ├── EnterprisePOS.Data       --> EF Core DbContext & SQLite Persistence
 ├── EnterprisePOS.Services   --> Business Logic, Audit & AI Forecast Engines
 └── EnterprisePOS.UI         --> WPF Desktop Views, Dialogs & Touch Control Templates
```

---

## ✨ Key Features & Modules

### 1. 🎬 Splash Screen & Secure Login
* **3-Second Animated Loader**: Automatic DB initialization & schema validation.
* **Multi-Branch Selector**: Login into specific branch/terminal locations (e.g., *Main Branch Colombo*, *Kandy Resort*).
* **Password Eye Toggle**: Easily show/hide password text 👁️.
* **Account Locking Security**: Automatically locks user account after **5 consecutive failed login attempts**.
* **Audit Trail**: Logs all user login/logout activity via `AuditService`.

### 2. 💵 Shift Management & Cash Float
* **Shift Opening Window**: Requires cashiers to enter opening float (e.g. LKR 5,000) before taking sales orders.
* **Cash Drawer Tracking**: Computes real-time cash, card, and room tab totals.
* **Z-Report Shift Closing**: Computes physical drawer cash variance (Shortage/Excess) upon shift closing.

### 3. 🪑 Visual Table Management
* **Interactive Color-Coded Grid**:
  * 🟢 **Available** — Ready for seating
  * 🔴 **Occupied** — Active dining order
  * 🟡 **Reserved** — Guest reservation
  * 🔵 **Cleaning** — Table clearing in progress
* **Table Transfers & Merges**: Easily move orders between tables or merge multiple tables together.

### 4. 🛏️ Hotel Room Charge & Checkout Merge
* **Charge to Room Option**: Cashier can route restaurant orders directly to guest room tabs (e.g. `Room 205`).
* **Merged Final Checkout**: Merges **Room Stay (Nights x Rate) + Restaurant POS Tab + Mini Bar Usage** into one single itemized checkout invoice.

### 5. 🍺 Hotel Mini Bar System
* **Room Stock Allocation**: Log room minibar consumption (Beer, RedBull, Snacks, Water).
* **Auto-Billing**: Automatically posts minibar consumption charges to the guest's pending accrued bill.

### 6. 🍳 Kitchen Display System (KDS)
* **Real-Time Touch Monitor**: Interactive screen for kitchen chefs.
* **Status Progression**: Update KOT status dynamically: `Pending` ➔ `Preparing` ➔ `Ready` ➔ `Served` ➔ `Cancelled`.

### 7. 📦 Recipe Management & Stock Deduction
* **Automatic Raw Ingredient Deduction**: Selling a menu item (e.g., *Chicken Fried Rice*) automatically deducts raw ingredient stock levels (Rice, Chicken, Eggs).
* **Consumables & Barcode Support**: Track non-salable inventory (Receipt Paper, Paper Towels) and barcode scanning.

### 8. 👥 Customer Loyalty & Membership
* **Points Earning**: 1 Loyalty point earned per LKR 100 spent.
* **Membership Tiers**: Auto-upgrades customer tier (`Silver` ➔ `Gold` ➔ `Platinum`).
* **Point Redemption**: Redeem points for bill discounts.

### 9. 📊 Reports & Analytics Hub
* **Financial Reports**: Daily Sales, Monthly Summaries, Gross Profit Analysis.
* **Operational Logs**: Best Selling Items, Waiter/Cashier Performance, Room Service Audit Logs.
* **1-Click PDF/Excel Export**: Instant export of business metrics.

### 10. ⚙️ Hardware & Backup Settings
* **Thermal Receipt & KOT Printer Configuration**: 80mm receipt format and Kitchen printer routing.
* **Database Maintenance**: 1-Click Database Backup & Restore (`pos_enterprise.db`).

---

## 🔑 Default System Login Credentials

| Role | Username | Password | Accessible Modules & Permissions |
| :--- | :--- | :--- | :--- |
| 👑 **System Admin** | `admin` | `admin123` | **Full Access** (POS, Hotel, Inventory, AI, Reports, Settings, Users) |
| 👨💼 **General Manager** | `manager` | `manager123` | Dashboard, Inventory Stock, Financial Reports |
| 💵 **Head Cashier** | `cashier` | `cashier123` | POS Billing, Shift Drawer, Room Charge, Receipts |
| 🪑 **Senior Waiter** | `waiter` | `waiter123` | Visual Table Layout, Table Move/Merge, Dine-In Orders |
| 👨🍳 **Head Chef** | `chef` | `chef123` | Kitchen Display Screen (KDS Monitor Only) |

---

## 🚀 How to Run the Application

### Prerequisites
* **.NET 9.0 SDK** (or .NET 8.0 SDK) installed on Windows.

### Method 1: Using Command Line / PowerShell
1. Open PowerShell in the project root directory (`c:\games\all`).
2. Execute the run command:
   ```powershell
   dotnet run --project EnterprisePOS.UI
   ```

### Method 2: Using Visual Studio
1. Open `EnterprisePOS.sln` in Visual Studio.
2. In **Solution Explorer**, right-click **`EnterprisePOS.UI`** and select **Set as Startup Project**.
3. Press **`F5`** (or click the green **Start** button).

---

## 🖨️ Keyboard Shortcuts

* **`F1`**: Cashier POS View
* **`F2`**: Visual Table Layout
* **`F3`**: Hotel Rooms Manager
* **`F4`**: Kitchen Display System (KDS)
* **`F5`**: Stock & Inventory
* **`F6`**: AI Dashboard & Analytics

---

© 2026 ABC Hotel & Restaurant Management Systems. All rights reserved.
