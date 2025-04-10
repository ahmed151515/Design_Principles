namespace myImprove
{

	internal class Program
	{
		public static Dictionary<int, ITopping> ToppingsMenu
		{ get; }
		= new Dictionary<int, ITopping>
		{
			{1,new Chicken()},
			{2,new Cheese ()},
			{3,new Beef   ()},
			{4,new Ranch  ()}
		};
		static void Main(string[] args)
		{
			var pizza = new Pizza();
			var choice = 0;
			do
			{
				Console.Clear();
				choice = ReadChoice(choice);
				if (choice >= 1 && choice <= ToppingsMenu.Count)
				{
					var topping = CreateTopping(choice);
					pizza.addTopping(topping);
					Console.WriteLine("Press any key to continue (0 to exit)");
				}
			} while (choice != 0);

			Console.WriteLine(pizza);
		}

		private static int ReadChoice(int choice)
		{
			Console.WriteLine("Toppings");
			Console.WriteLine("------------");
			foreach (var topping in ToppingsMenu)
			{
				Console.WriteLine($"{topping.Key}: {topping.Value.Name}");
			}
			Console.WriteLine("what is your Topping: ");
			if (int.TryParse(Console.ReadLine(), out int ch))
			{
				choice = ch;
			}

			return choice;
		}

		private static ITopping CreateTopping(int choice)
		{
			ITopping topping = ToppingsMenu[choice];

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
