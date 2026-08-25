
---

## 🌟 2. Comprehensive System Modules & Features

### 🛒 Module 1: Cashier POS Billing Terminal (`EnterprisePOS.CashierApp`)
* **Fast Touch Interface**: Designed for rapid billing in busy restaurants, cafes, and hotel bars.
* **Strict Inventory Stock Limits**: Real-time stock validation queries SQLite before adding to cart or increasing quantity.
  * **Out of Stock**: Displays `🚨 OUT OF STOCK!` warning if stock is 0 and blocks billing.
  * **Stock Limit Exceeded**: Restricts item quantity to available inventory if cashier attempts to sell more than in stock.
  * **Payment Blocking**: Secondary validation blocks transaction if any cart item quantity exceeds available stock.
* **Cart Operations**: Instant item quantity increment (`+`), decrement (`-`), item modifier notes, and row deletion (`🗑️`).
* **Multi-Order Queue (Slot Bar)**: Hold and switch between up to 10 active customer orders simultaneously.
* **Barcode Scanner Integration**: Fast lookup for packaged items & beverages.

---

### 🛏️ Module 2: Hotel Room Management & Accrued Bill Merging
* **Room Service & Tab Routing**: Cashiers can route food & beverage orders directly to occupied hotel rooms (e.g. `Room 101`).
* **Accrued Pending Tabs**: Tracks pending accrued restaurant & bar charges for each checked-in guest.
* **Merged Final Checkout Invoice**: Combines **Room Stay (Nights x Daily Rate) + POS Restaurant Tab + Mini Bar Usage** into a single itemized checkout bill.

---

### 🍺 Module 3: Mini Bar Stock & Consumption Management
* **Room Stock Allocation**: Log room minibar consumption (Beer, RedBull, Snacks, Mineral Water).
* **Auto-Billing**: Automatically posts minibar consumption charges to the guest's pending accrued bill upon checkout.

---

### 🪑 Module 4: Visual Table Management & Dining Grid
* **Color-Coded Interactive Grid**:
  * 🟢 **Available** — Table ready for seating
  * 🔴 **Occupied** — Active dining order in progress
  * 🟡 **Reserved** — Table reserved for guest
  * 🔵 **Cleaning** — Sanitation / clearing in progress
* **Table Transfers & Merges**: Move orders between tables or merge multiple tables together for large groups.

---

### 🍳 Module 5: Kitchen Display System (KDS) & KOT Routing
* **Interactive Chef Touch Monitor**: Real-time order queue screen for kitchen chefs.
* **Order Status Tracking**: Update orders dynamically: `Pending` ➔ `Preparing` ➔ `Ready` ➔ `Served`.
* **Thermal KOT Printing**: Auto-generates KOT slips for kitchen and bar counters.

---

### 📦 Module 6: Inventory, Menu Catalog & Recipe Auto-Deduction
* **Menu Master Catalog**: Add, edit, and update product names, categories, cost prices, selling prices, and stock levels.
* **Live Stock Refresh**: `🔄 Refresh Live Stock` button reloads fresh inventory numbers directly from SQLite disk storage.
* **Recipe-Based Ingredient Auto-Deduction**: Selling a menu item (e.g., *Chicken Fried Rice*) automatically deducts raw ingredient stock levels (Rice, Chicken, Eggs).
* **Consumables & Barcode Support**: Track non-salable inventory (Receipt Paper, Paper Towels).

---

### 👑 Module 7: Executive Admin & Manager Console (`EnterprisePOS.ManagerApp`)
* **Real-time Live Sales Dashboard**: Total revenue, total invoices, occupied rooms, and low-stock counters.
* **Staff User Management**: Create, edit, and delete staff user accounts (**Cashier, Manager, Admin, Waiter, KitchenStaff**). Passwords encrypted via SHA-256.
* **🤖 AI Demand Forecasting Engine**: Calculates sales velocity to recommend smart stock reorder quantities and flags low stock risks.
* **🛡️ Cashier Security Audit Log**: Audits manual cashier discounts, zero sales, cash removals, and after-hours transactions.

---

### 📊 Module 8: Reports & Analytics Hub
* **Financial Reports**: Daily Sales Report, Monthly Summaries, Gross Profit Analysis.
* **Operational Logs**: Best Selling Items, Staff Sales Performance, Room Service Audit Logs.
* **1-Click Export**: Export business performance reports to PDF and CSV formats.

---

### 🏬 Module 9: Store Profile & Receipt Branding Customization
* **Custom Receipts**: Edit Store Name, Address, Telephone, and Footer Messages for 80mm thermal receipts.
* **Database Maintenance**: 1-Click Database Backup & Restore (`pos_enterprise.db`).
* **Clean System Reset**: 1-Click test data reset tool (`🧹 Reset System — Clean Start for Client`) to clear test sales before client delivery.

---

### 🔑 Module 10: License Key Generator Tool (`EnterprisePOS.LicenseKeygen`)
* Vendor utility app to generate HMAC-SHA256 encrypted license keys for clients.
* Supports **Trial**, **1-Year Commercial**, and **Lifetime Enterprise** license keys bound to Client Business Name and Machine Fingerprint.

---

## 🔑 Default System Login Credentials

| Role | Username | Password | Accessible Modules & Permissions |
| :--- | :--- | :--- | :--- |
| 👑 **System Admin** | `admin` | `admin123` | **Full Access** (POS, Hotel, Inventory, AI, Reports, Settings, Users) |
| 👨‍💼 **General Manager** | `manager` | `manager123` | Executive Dashboard, Inventory Stock, Financial Reports |
| 💵 **Head Cashier** | `cashier` | `cashier123` | Cashier POS Terminal, Shift Drawer, Room Charges, Receipts |
| 🪑 **Senior Waiter** | `waiter` | `waiter123` | Visual Table Layout, Table Move/Merge, Dine-In Orders |
| 👨‍🍳 **Head Chef** | `chef` | `chef123` | Kitchen Display Screen (KDS Monitor Only) |

---

## 📦 Client Installation & Deployment

Package given to client:
📁 **`OutputInstaller\OasisPeakPOS_Setup.exe`**

1. Run `OasisPeakPOS_Setup.exe` once on client PC.
2. Creates 2 Desktop Shortcuts:
   * 🛒 **Cashier POS Terminal** (`EnterprisePOS.CashierApp.exe`)
   * 👑 **Admin Manager Console** (`EnterprisePOS.ManagerApp.exe`)
3. **No .NET required on client PC** — 100% self-contained win-x64 deployment operating 100% offline.

---

## 🖨️ Hotkeys & Shortcuts

* **`F1`**: Executive Dashboard / Cashier POS
* **`F2`**: Visual Table Layout / Inventory & Menu Catalog
* **`F3`**: Hotel Rooms Manager / Analytics & Reports Hub
* **`F4`**: Kitchen Display System / System Settings

---

© 2026 **OASIS PEAK ELLA**. All rights reserved.  
`ELLA ROAD, ELLA 90090, SRI LANKA` | Hotline: `070 539 0566`
