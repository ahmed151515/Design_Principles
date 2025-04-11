

# Definition: Encapsulate What Varies Principle

**Core Idea:** **Separate what stays the same from what changes frequently.**

**Explanation:** This principle guides developers to identify the aspects of a system that are likely to evolve or vary over time (e.g., algorithms, business rules, data formats, UI elements) and isolate them from the parts of the system that are expected to remain relatively stable. **The idea is to keep stable parts in one place while isolating parts that may change, making the code more flexible, maintainable, and reusable.**

Okay, let's refine the explanation to be clearer, more structured, and directly tie the changes to the "Encapsulate What Varies" principle.


# Applying "Encapsulate What Varies" to Pizza Ordering

This document demonstrates how to improve code structure and maintainability by applying the **"Encapsulate What Varies"** design principle using a C# Pizza ordering example.

## 1. Initial Code Structure

First, let's define the initial classes involved in ordering a pizza.

### Base `Pizza` Class (Initial Version)

```csharp
public class Pizza
{
    // Virtual properties allow subclasses to provide specific details
    public virtual string Name => $"{nameof(Pizza)}";
    public virtual decimal Price => 10m;

    // Static method to order a pizza based on type
    public static Pizza Order(string type)
    {
        Pizza pizza;

        // --- PROBLEM AREA START ---
        // This part handles CREATION logic based on type.
        // It uses hardcoded strings and mixes creation with the process.
        if (type.Equals("cheese"))
        {
            pizza = new Cheese();
        }
        else if (type.Equals("chicken"))
        {
            pizza = new Chicken();
        }
        else
        {
            pizza = new Pizza(); // Default case
        }
        // --- PROBLEM AREA END ---


        // --- FIXED PROCESS START ---
        // These steps are always performed, regardless of pizza type.
        Prepare();
        Cook();
        Cut();
        // --- FIXED PROCESS END ---

        return pizza;
    }

    // Helper methods representing fixed steps in the process
    private static void Prepare()
    {
        Console.Write("preparing...");
        Thread.Sleep(500);
        Console.WriteLine("Completed"); // Corrected typo
    }
    private static void Cook()
    {
        Console.Write("Cooking...");
        Thread.Sleep(500);
        Console.WriteLine("Completed"); // Corrected typo
    }
    private static void Cut()
    {
        Console.Write("Cut and Boxing...");
        Thread.Sleep(500);
        Console.WriteLine("Completed"); // Corrected typo
    }

    public override string ToString()
    {
        return $"name: {Name}\nprice: {Price}";
    }
}
```

### Concrete Pizza Subclasses (`Cheese` and `Chicken`)

These classes represent specific pizza types, overriding base properties.

```csharp
// Represents a Cheese Pizza with its specific name and price adjustment
public class Cheese : Pizza
{
    public override string Name => $"{nameof(Cheese)} {base.Name}";
    public override decimal Price => base.Price + 3m;
}

// Represents a Chicken Pizza with its specific name and price adjustment
public class Chicken : Pizza
{
    public override string Name => $"{nameof(Chicken)} {base.Name}";
    public override decimal Price => base.Price + 6m;
}
```

## 2. Identifying the Problem in `Pizza.Order` 🔴

The initial `Pizza.Order` method has several issues related to maintainability and design principles:

1.  **Mixing Concerns:** It combines the logic for *creating* specific pizza types (`if/else if/else`) with the *fixed process* of preparing, cooking, and cutting. This violates the **Single Responsibility Principle (SRP)**.
2.  **Brittleness:** The creation logic (`if/else if/else`) is the part most likely to **change**. Adding a new pizza type (e.g., "Vegetarian") requires modifying this central `Order` method, increasing the risk of introducing bugs.
3.  **Magic Strings:** It uses hardcoded strings like `"cheese"` and `"chicken"`. Typos can lead to errors, and changing these identifiers requires finding and updating them everywhere they are used.
4.  **Poor Reusability:** The creation logic isn't easily reusable elsewhere if needed.

**The core issue is that the part of the code that *varies* (pizza creation based on type) is tightly coupled with the part that *stays the same* (the sequence of Prepare, Cook, Cut).**

## 3. The Solution: Encapsulate What Varies 🟢

We apply the "Encapsulate What Varies" principle by:

1.  **Identifying the variation:** The logic for selecting and creating the specific `Pizza` subclass based on the `type` string.
2.  **Encapsulating it:** Moving this varying logic into its own dedicated method (`Create`).
3.  **Stabilizing the original method:** Keeping the stable parts (the `Prepare`, `Cook`, `Cut` sequence) in the `Order` method and having it delegate the varying creation part to the new `Create` method.
4.  **Improving Type Safety:** Replacing magic strings with constants.

### Step 1: Define Constants (`PizzaConst`)

Create a class to hold constants for pizza types, avoiding magic strings.

```csharp
public class PizzaConst
{
    // Using constants improves type safety and maintainability
    public readonly static string CheesePizza = "cheese";
    public readonly static string ChickenPizza = "chicken";
    // Add new constants here, e.g., public readonly static string VeggiePizza = "veggie";
}
```

### Step 2: Refactor the `Pizza` Class

Modify the `Pizza` class to separate the creation logic.

```csharp
public class Pizza
{
    public virtual string Name => $"{nameof(Pizza)}";
    public virtual decimal Price => 10m;

    // --- ENCAPSULATED VARIATION ---
    // This private static method now solely handles the varying creation logic.
    // It uses constants for comparison. Often called a Factory Method.
    private static Pizza Create(string type)
    {
        Pizza pizza;
        if (type.Equals(PizzaConst.CheesePizza))
        {
            pizza = new Cheese();
        }
        else if (type.Equals(PizzaConst.ChickenPizza))
        {
            pizza = new Chicken();
        }
        // else if (type.Equals(PizzaConst.VeggiePizza)) // Easy to add new types here
        // {
        //     pizza = new Veggie();
        // }
        else
        {
            // Consider throwing an exception for unknown types or return a default
            Console.WriteLine($"Warning: Unknown pizza type '{type}'. Creating default Pizza.");
            pizza = new Pizza();
        }
        return pizza;
    }

    // --- STABLE PUBLIC INTERFACE & PROCESS ---
    // The Order method is now cleaner and focuses on the fixed process.
    // It delegates the varying creation task to the 'Create' method.
    public static Pizza Order(string type)
    {
        // 1. Create the specific pizza (delegates the variation)
        Pizza pizza = Create(type);

        // 2. Execute the fixed preparation steps
        Prepare();
        Cook();
        Cut();

        // 3. Return the prepared pizza
        return pizza;
    }

    // Helper methods remain unchanged
    private static void Prepare()
    {
        Console.Write("preparing...");
        Thread.Sleep(500);
        Console.WriteLine("Completed");
    }
    private static void Cook()
    {
        Console.Write("Cooking...");
        Thread.Sleep(500);
        Console.WriteLine("Completed");
    }
    private static void Cut()
    {
        Console.Write("Cut and Boxing...");
        Thread.Sleep(500);
        Console.WriteLine("Completed");
    }

    public override string ToString()
    {
        return $"name: {Name}\nprice: {Price}";
    }
}

// Note: Cheese and Chicken classes remain unchanged.
```

## 4. Benefits of the Refactoring ✅

Applying "Encapsulate What Varies" yields significant improvements:

-   **Improved Maintainability:** Adding a new pizza type now mainly involves:
    1.  Creating a new subclass (e.g., `Veggie : Pizza`).
    2.  Adding a constant to `PizzaConst`.
    3.  Adding one `else if` branch to the `Create` method.
    The `Order` method itself *does not need to change*.
-   **Increased Readability:** The `Order` method clearly shows the high-level process (Create -> Prepare -> Cook -> Cut), while the complex creation details are hidden in `Create`.
-   **Adherence to SRP:** The `Create` method is responsible *only* for object creation based on type. The `Order` method is responsible *only* for orchestrating the overall process.
-   **Reduced Risk:** Changes related to adding/modifying pizza types are localized to the `Create` method and the new class, minimizing the risk of breaking the core ordering process.
-   **Flexibility:** The implementation of `Create` could be further improved (e.g., using a dictionary or reflection) without affecting the `Order` method or the client code.

🚀 **Result**: The code is now **better structured, easier to extend (more flexible), more maintainable, and less prone to errors** by clearly separating the stable parts from the parts that are likely to change.
```