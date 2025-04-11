namespace FavorCompositionOverInheritance
{
	/*
		use inheritance if relation is pure is-a
		this app is good for now but if you want  
		pizza  cheese and chicken is defcult

	Inheritance Issue:

- Suppose we start with a `Pizza` class, and for every new topping (e.g., Cheese, Chicken, Veggies), we create a subclass like `CheesePizza`, `ChickenPizza`, `VeggiePizza`, etc.
    
- As we add more toppings, the number of classes increases exponentially, leading to a **combinatorial explosion of subclasses**.
	 */
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
