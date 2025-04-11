


# Favor Composition Over Inheritance (Pizza Example)

**Principle:** Favor composition over inheritance. While inheritance models an "is-a" relationship, it can lead to rigid hierarchies and combinatorial explosion when dealing with features or variations that can be combined. Composition models a "has-a" relationship and often provides greater flexibility and maintainability.

**Scenario:** We want to model a pizza ordering system where pizzas can have various toppings, each potentially affecting the price.

---

## The Problem: Inheritance Approach

Initially, one might think of using inheritance. A base `Pizza` class with subclasses for each specific type (`CheesePizza`, `ChickenPizza`, `MexicanPizza`).

```csharp
/*
    use inheritance if relation is pure is-a
    this app is good for now but if you want
    pizza cheese and chicken is defcult

Inheritance Issue:

- Suppose we start with a `Pizza` class, and for every new topping (e.g., Cheese, Chicken, Veggies), we create a subclass like `CheesePizza`, `ChickenPizza`, `VeggiePizza`, etc.

- As we add more toppings, the number of classes increases exponentially, leading to a **combinatorial explosion of subclasses**.
    (e.g., CheesePizza, ChickenPizza, CheeseAndChickenPizza, CheeseAndMexicanPizza, etc.)
*/
namespace FavorCompositionOverInheritance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var choice = 0;
            do
            {
                Console.Clear();
                choice = ReadChoice(choice);
                if (choice >= 1 && choice <= 3)
                {
                    var pizza = CreatePizza(choice);
                    Console.WriteLine(pizza);
                    Console.WriteLine("Press any key to continue");
                }
                Console.ReadKey();
            } while (choice != 0);
        }

        private static int ReadChoice(int choice)
        {
            Console.WriteLine("Today's Menu");
            Console.WriteLine("------------");
            Console.WriteLine("1. Chicken");
            Console.WriteLine("2. Cheese");
            Console.WriteLine("3. Mexican");
            Console.WriteLine("what is your order: ");
            if (int.TryParse(Console.ReadLine(), out int ch))
            {
                choice = ch;
            }
            return choice;
        }

        private static Pizza CreatePizza(int choice)
        {
            Pizza pizza = null;
            switch (choice)
            {
                case 1:
                    pizza = new Chicken();
                    break;
                case 2:
                    pizza = new Cheese();
                    break;
                case 3:
                    pizza = new Mexican();
                    break;
                default:
                    break;
            }
            return pizza;
        }
    }

    public class Pizza
    {
        public virtual string Name => $"{nameof(Pizza)}";
        public virtual decimal Price => 10m;

        public override string ToString()
        {
            return $"name: {Name}\nprice: {Price}";
        }
    }

    // Example subclasses - imagine needing CheeseAndChickenPizza, etc.
    public class Mexican : Pizza
    {
        public override string Name => $"{GetType().Name} {base.Name}";
        public override decimal Price => base.Price + 3m;
    }
    public class Cheese : Pizza
    {
        public override string Name => $"{GetType().Name} {base.Name}";
        public override decimal Price => base.Price + 3m;
    }
    public class Chicken : Pizza
    {
        public override string Name => $"{GetType().Name} {base.Name}";
        public override decimal Price => base.Price + 6m;
    }
}
```

**Problem Summary:** This approach works for single-topping pizzas but breaks down quickly. If we want a pizza with both Cheese and Chicken, we'd need a `CheeseAndChickenPizza` class. Adding more toppings leads to an unmanageable number of classes.

---

## The Solution: Composition Approach

Instead of inheriting, we model the relationship as "a Pizza *has* Toppings". We create a base `Pizza` class and separate `Topping` components (using an interface or abstract class). The `Pizza` object holds a collection of these toppings.

```csharp
/*
 *  Composition Solution:

Instead of creating subclasses for every possible pizza type, we use composition. We can model each topping as a separate component (using interfaces or base classes) and add them to a `Pizza` class dynamically at runtime. This way, we can create any pizza by combining different toppings, without changing the structure of the `Pizza` class itself.
*/
namespace FavorCompositionOverInheritanceAfter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pizza = new Pizza(); // Create one base pizza
            var choice = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Current Pizza:"); // Show current state
                Console.WriteLine(pizza);
                Console.WriteLine("----------\n");

                choice = ReadChoice(choice);
                if (choice >= 1 && choice <= 4) // Updated range for new toppings
                {
                    var topping = CreateTopping(choice);
                    if (topping != null)
                    {
                        pizza.addTopping(topping);
                        Console.WriteLine($"Added {topping.Name}. Press any key to add more (0 to finish)...");
                    }
                    else {
                         Console.WriteLine("Invalid topping choice. Press any key...");
                    }
                }
                else if (choice != 0)
                {
                    Console.WriteLine("Invalid choice. Press any key...");
                }

                if(choice != 0) Console.ReadKey(); // Pause unless exiting

            } while (choice != 0);

            Console.Clear();
            Console.WriteLine("\n--- Your Final Pizza Order ---");
            Console.WriteLine(pizza);
            Console.WriteLine("--- Enjoy! ---");
            Console.ReadKey(); // Final pause
        }

        private static int ReadChoice(int choice)
        {
            Console.WriteLine("Add Topping (0 to Finish)");
            Console.WriteLine("-------------------------");
            Console.WriteLine("1. Chicken");
            Console.WriteLine("2. Cheese");
            Console.WriteLine("3. Beef");
            Console.WriteLine("4. Ranch"); // Corrected numbering
            Console.Write("Enter topping number: "); // More direct prompt
            if (int.TryParse(Console.ReadLine(), out int ch))
            {
                choice = ch;
            } else {
                choice = -1; // Indicate invalid input
            }

            return choice;
        }

        private static ITopping CreateTopping(int choice)
        {
            ITopping topping = null;
            switch (choice)
            {
                case 1:
                    topping = new Chicken();
                    break;
                case 2:
                    topping = new Cheese();
                    break;
                case 3:
                    topping = new Beef();
                    break;
                case 4:
                    topping = new Ranch();
                    break;
                // No default needed, returns null for invalid choices
            }
            return topping;
        }
    }

    // Base Pizza class - no longer needs virtual properties for toppings
    public class Pizza
    {
        public string Name => $"{nameof(Pizza)}";
        public decimal BasePrice => 10m; // Renamed for clarity
        public List<ITopping> Toppings { get; private set; } = new List<ITopping>();

        public void addTopping(ITopping topping) => Toppings.Add(topping);

        public decimal getTotalPrice()
        {
            decimal totalPrice = BasePrice; // Start with base price
            foreach (var topping in Toppings)
            {
                totalPrice += topping.Price; // Add price of each topping
            }
            return totalPrice;
        }

        public override string ToString()
        {
            var toppingDescriptions = Toppings.Select(t => $"{t.Name} (+{t.Price:C})").ToList();
            string output = $"Base {Name} ({BasePrice:C})\n";
            if (toppingDescriptions.Any())
            {
                output += $"---- Toppings ----\n";
                output += string.Join("\n", toppingDescriptions);
                output += "\n------------------\n";
            } else {
                 output += "(No toppings added)\n";
            }
            output += $"Total Price: {getTotalPrice():C}\n"; // Use currency format

            return output;
        }
    }

    // Interface for all toppings
    public interface ITopping
    {
        public string Name { get; }
        public decimal Price { get; }
    }

    // Concrete topping classes - easy to add more!
    class Cheese : ITopping
    {
        public string Name => GetType().Name;
        public decimal Price => 3m;
    }
    class Chicken : ITopping
    {
        public string Name => GetType().Name;
        public decimal Price => 4m;
    }
    class Beef : ITopping
    {
        public string Name => GetType().Name;
        public decimal Price => 6m;
    }
    class Ranch : ITopping
    {
        public string Name => GetType().Name;
        public decimal Price => 5m; // Example price
    }
}

```

---

## Benefits of Composition (in this scenario)

1.  **Flexibility & Extensibility:** New toppings (`ITopping` implementations) can be added easily without modifying the `Pizza` class or creating new combined subclasses. We just create a new class implementing `ITopping`.
2.  **Customization:** Users can dynamically combine any number and type of toppings at runtime onto a single `Pizza` object.
3.  **Maintainability:** Avoids the combinatorial explosion of classes. Changes to a topping's price or name only require modification in one place (the specific topping class).
4.  **Simpler Codebase:** The logic is more straightforward. The `Pizza` class manages a list of toppings, and each topping encapsulates its own data (name, price). No complex inheritance tree to manage for combinations.
5.  **Clearer Relationships:** It accurately reflects the real-world relationship: a pizza *has* toppings, rather than a cheese-and-chicken pizza *is-a* specialized type of pizza distinct from a cheese pizza or a chicken pizza.

