# 🌿 VegShop - Vegetable Shop

## 📌 Overview
VegShop is a **C# console application** that manages a vegetable shop, allowing users to process **product pricing, purchases, and special offers** based on configurable rules.

The system reads product details and purchases from **CSV files**, applies configured **discounts and offers**, and generates a **receipt** displaying the final price and applied promotions.

---

## ⚙️ Configuration

### **1️⃣ Offers Configuration**
Offers are **configurable** in the `Offers` section of `appsettings.json`. Each offer type follows a structured format.

#### ✅ Example: Configuration for Offers in `appsettings.json`
```json
"Offers": {
  "BuyXGetYFree": [
    {
      "Product": "Tomato",
      "RequiredQuantity": 3,
      "FreeQuantity": 1
    }
  ],
  "GetFreeXFromMultipleY": [
    {
      "Product": "Aubergine",
      "RequiredQuantity": 10,
      "FreeProduct": "Carrot"
    }
  ],
  "DiscountPerPriceAmount": [
    {
      "Product": "Carrot",
      "MinAmount": 5,
      "Discount": 0.5
    }
  ]
}
```

### 📌 Supported Offer Types

| Offer Type               | Description                      |
|--------------------------|----------------------------------|
| **BuyXGetYFree**         | Buy `X` items, get `Y` free     |
| **DiscountPerAmount**    | Spend `X` amount, get `Y` discount |
| **GetOneFreeForEveryX**  | Buy `X` items, get `1` free     |

### 2️⃣ Input File Paths Configuration
The input files for products and purchases can be configured in appsettings.json.

✅ Example: Configuring Input File Paths
These files should be placed on an InputFiles folder on the root folder for the solution.
```json
{
  "InputFiles": {
    "Products": "InputFiles/products.csv",
    "Purchases": "InputFiles/purchases.csv"
  }
}
```

### 📂 Project Structure
```
VegetableShop/
│── InputFiles/
│   ├── products.csv           # List of products and prices
│   ├── purchases.csv          # List of purchased products and quantities
│── Interfaces/
│   ├── IShoppingService.cs    # Handles purchases and processing
│   ├── IOfferService.cs       # Manages offer calculations
│   ├── IErrorHandlerService.cs# Handles error logging
│── Services/
│   ├── ShoppingService.cs     # Implements shopping logic
│   ├── OfferService.cs        # Applies discounts and offers
│── Models/
│   ├── Product.cs             # Represents a product
│   ├── Offer.cs               # Represents an offer
│── Program.cs                 # Entry point for the application
│── appsettings.json           # Configurations for offers and file paths
│── README.md                  # Documentation
```


### 🛠️ Setup & Running the Application

###1️⃣ Configure the System
Modify appsettings.json to customize offers and input file paths.
Place CSV files (products.csv, purchases.csv) inside the InputFiles/ folder.

###2️⃣ Run the Application
To execute the program, use the following command:

dotnet run

###3️⃣ Expected Output
The receipt will be displayed in the console, showing:

✅ Purchased Products
✅ Applied Offers
✅ Final Total Price


### 🧪 Unit Tests
The project includes unit tests for business logic using xUnit and Moq. To run the tests:

dotnet test
