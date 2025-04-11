namespace FavorCompositionOverInheritanceAfter
{
	/*
	 *  Composition Solution:

Instead of creating subclasses for every possible pizza type, we use composition. We can model each topping as a separate component (using interfaces or base classes) and add them to a `Pizza` class dynamically at runtime. This way, we can create any pizza by combining different toppings, without changing the structure of the `Pizza` class itself.

Benefits:

1. Extensibility: New toppings can be added without modifying the core pizza class.
    
2. Customization: Users can freely combine different toppings to create their pizzas.
    
3. Maintainability: No need to update the code for all pizza types whenever a new topping is added.
    
4. Simpler Codebase: The logic for handling different pizza types doesn't require maintaining a large inheritance tree.
	 */
	internal class Program
	{
		static void Main(string[] args)
		{
			var pizza = new Pizza();
			var choice = 0;
			do
			{
				Console.Clear();
				choice = ReadChoice(choice);
				if (choice >= 1 && choice <= 3)
				{
					var topping = CreateTopping(choice);
					pizza.addTopping(topping);
					Console.WriteLine("Press any key to continue (0 to exit)");
				}
				Console.ReadKey();
			} while (choice != 0);

			Console.WriteLine(pizza);
		}

		private static int ReadChoice(int choice)
		{
			Console.WriteLine("Toppings");
			Console.WriteLine("------------");
			Console.WriteLine("1. Chicken");
			Console.WriteLine("2. Cheese");
			Console.WriteLine("3. Beef");
			Console.WriteLine("3. Ranch");
			Console.WriteLine("what is your Topping: ");
			if (int.TryParse(Console.ReadLine(), out int ch))
			{
				choice = ch;
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
				default:
					break;
			}
			return topping;
		}
	}

	public class Pizza
	{
		public string Name => $"{nameof(Pizza)}";
		public decimal Price => 10m;
		public List<ITopping> Toppings { get; private set; } = new List<ITopping>();
		public void addTopping(ITopping topping) => Toppings.Add(topping);

		public decimal getTotalPrice()
		{
			decimal basePrice = Price;
			foreach (var topping in Toppings)
			{
				basePrice += topping.Price;
			}
			return basePrice;
		}

		public override string ToString()
		{
			string output = $"{Name}\n";
			output += $"----Toppings------\n";
			foreach (var topping in Toppings)
			{
				output += $"{topping.Name}: {topping.Price}\n";
			}
			output += "----------\n";
			output += $"Total Price: {getTotalPrice()}\n";

			return output;

		}
	}

	public interface ITopping
	{
		public string Name { get; }
		public decimal Price { get; }
	}

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

		public decimal Price => 5m;
	}

}
