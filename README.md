# 🛡 DineFearless — Food & Allergy Transparency Hub

> An interactive web application built with **ASP.NET Core Blazor (Interactive Server)** and **Tailwind CSS v4**, designed to remove anxiety from dining out with severe food allergies and dietary restrictions.

---

## 🌟 Key Features

* **Real-Time Safety Evaluation:** Evaluates every menu item dynamically against the diner's active allergen profile (Safe, Modifiable, or Unsafe).
* **Granular Ingredient Inspector:** Click any dish to inspect raw ingredient supplier verification and cross-contact vectors.
* **Kitchen Modification Engine:** Interactively omit ingredients (e.g., *"Omit butter and parmesan"*) and watch safety recalculate in real-time.
* **Multilingual Kitchen Server Handover Card:** Generates physical/digital cards in English, Spanish, and Tagalog to give directly to waitstaff.
* **Dim Restaurant High-Contrast Mode:** High-visibility yellow/black flashcard mode for dark ambient dining rooms.
* **Allergen Compatibility Scoring:** The discovery directory ranks restaurants by personalized safety percentage (e.g., *"94% Match for your profile"*).
* **Interactive Safe Plate Tray:** Order staging tray with real-time quantity steppers, subtotal updates, and tip calculation.

---

## 🏗 Object-Oriented Architecture & Design Patterns

This project adheres to strict OOP principles:

### 1. Strategy Pattern (`ISafetyEvaluationStrategy`)
Safety assessment logic is decoupled from menu items using interchangeable evaluation strategies:
* `StrictMedicalStrategy`: Enforces zero tolerance for shared fryers and cross-contamination prep tables (for Celiac & anaphylactic diners).
* `MildSensitivityStrategy`: Permits trace airborne contact and shared fryers, evaluating only direct ingredients.

### 2. Observer Pattern (Reactive State Management)
Components subscribe directly to state services and re-render only when relevant data changes:
* `UserDietaryProfile`: Emits `OnProfileChanged` when active allergens or sensitivity modes are toggled.
* `SafePlateService`: Emits `OnPlateChanged` when items are added, updated, or removed.
* `ModalService`: Mediates modal visibility (`DishDetail`, `DishModification`, `KitchenProtocols`, `ServerCardPrint`) across independent components.
* `FavoritesService`: Emits `OnFavoritesChanged` to synchronize bookmarked restaurants.

### 3. Encapsulation & Domain Modeling
* `MenuItem` (Abstract Base) & `FoodDish` (Derived Class): Encapsulates preparation station data, dedicated fryer flags, and allergen risk checks.
* `Ingredient`: Strongly-typed entity tracking supplier verification, origin notes, and allergen containment.

---

## 🛠 Technology Stack

* **Framework:** ASP.NET Core Blazor (Interactive Server Render Mode)
* **Language:** C# 12 / .NET
* **Styling:** Tailwind CSS v4 (CSS-first engine with `@source` scanning)
* **Version Control:** Git with Conventional Commits 1.0.0

---

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/YOUR_USERNAME/DineFearless.git
cd DineFearless