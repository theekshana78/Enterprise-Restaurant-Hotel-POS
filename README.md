
---

## ✨ Key Features & Modules

### 1. 🛒 Cashier POS Billing Terminal
* **Fast Touch Interface**: Designed for rapid billing in busy restaurants & hotel bars.
* **Strict Inventory Control**: Real-time stock validation prevents billing out-of-stock items or exceeding available inventory.
* **Cart Controls**: Instant item quantity increment (`+`), decrement (`-`), notes editing, and item deletion (`🗑️`).
* **Multi-Order Slot Queuing**: Hold and switch between up to 10 active customer orders simultaneously.
* **Barcode Scanner Support**: Quick barcode lookup for packaged items & beverages.

### 2. 🛏️ Hotel Room Charge & Accrued Bill Merging
* **Charge to Room Routing**: Cashiers can route restaurant & bar orders directly to occupied guest rooms (e.g. `Room 101`).
* **Merged Final Checkout Invoice**: Combines **Room Stay (Nights x Rate) + Restaurant POS Tab + Mini Bar Usage** into one itemized checkout bill.

### 3. 🪑 Visual Table Management
* **Color-Coded Dining Grid**:
  * 🟢 **Available** — Table ready for guests
  * 🔴 **Occupied** — Active dining order in progress
  * 🟡 **Reserved** — Guest reservation held
  * 🔵 **Cleaning** — Table clearing / sanitation in progress
* **Table Move & Merge**: Seamlessly transfer orders between tables or merge multiple tables for large groups.

### 4. 🍳 Kitchen Display System (KDS) & KOT Printing
* **Interactive Chef Monitor**: Kitchen display tracking active orders.
* **Status Progression**: Update orders from `Pending` ➔ `Preparing` ➔ `Ready` ➔ `Served`.
* **Thermal Printing**: Auto-generates KOT printouts for kitchen & bar staff.

### 5. 👑 Executive Admin & Manager Console
* **Real-time Live Sales Dashboard**: Total revenue, total invoices, occupied rooms, and low-stock counters.
* **Menu Master Catalog**: Add, edit, and update product names, categories, cost prices, selling prices, and stock levels.
* **Staff User Management**: Create, edit, and delete staff user accounts (**Cashier, Manager, Admin, Waiter, KitchenStaff**). Passwords encrypted via SHA-256.
* **🤖 AI Demand Forecast & Reorder Engine**: Calculates sales velocity to recommend smart stock reorder quantities and flags low stock risks.
* **🛡️ Cashier Security Audit Log**: Audits manual cashier discounts, zero sales, cash removals, and after-hours transactions.

### 6. 📊 Reports & Analytics Hub
* **Comprehensive Reports**: Daily Sales Report, Monthly Summaries, Best Selling Items, Staff Sales Performance, Room Service Log, and Gross Profit Analysis.
* **1-Click Export**: Export business performance reports to PDF and CSV formats.

### 7. 🏬 Store Profile & Receipt Customization
* **Custom Receipts**: Edit Store Name, Address, Telephone, and Footer Messages for 80mm thermal receipts.
* **Database Maintenance**: 1-Click Database Backup & Restore (`pos_enterprise.db`).
* **Clean System Reset**: 1-Click test data reset tool to prepare clean databases for client deployment.

---

## 🔑 Default Login Credentials

| Role | Username | Password | Accessible Modules & Permissions |
| :--- | :--- | :--- | :--- |
| 👑 **System Admin** | `admin` | `admin123` | **Full Access** (POS, Hotel, Inventory, AI, Reports, Settings, Users) |
| 👨‍💼 **General Manager** | `manager` | `manager123` | Executive Dashboard, Inventory Stock, Financial Reports |
| 💵 **Head Cashier** | `cashier` | `cashier123` | Cashier POS Terminal, Shift Drawer, Room Charges, Receipts |
| 🪑 **Senior Waiter** | `waiter` | `waiter123` | Visual Table Layout, Table Move/Merge, Dine-In Orders |
| 👨‍🍳 **Head Chef** | `chef` | `chef123` | Kitchen Display Screen (KDS Monitor Only) |

---

## 📦 Client Deployment & Installation

Give your client the single installer file:
📁 **`OutputInstaller\OasisPeakPOS_Setup.exe`**

* **No .NET Required**: 100% self-contained win-x64 deployment.
* **Automatic Desktop Shortcuts Created**:
  1. 🛒 **Cashier POS Terminal** (`EnterprisePOS.CashierApp.exe`)
  2. 👑 **Admin Manager Console** (`EnterprisePOS.ManagerApp.exe`)

---

## 🚀 Running from Source Code (Developer Mode)

### Prerequisites
* Windows 10/11
* **.NET 9.0 SDK** (or .NET 8.0 SDK)

### Option A: Run via PowerShell
```powershell
# Run Cashier Terminal
dotnet run --project EnterprisePOS.CashierApp

# Run Admin Manager Console
dotnet run --project EnterprisePOS.ManagerApp
