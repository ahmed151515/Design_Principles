namespace Encapsulate_What_Varies
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Pizza pizza = Pizza.Order("chicken");

			Console.WriteLine(pizza);
		}
	}

	public class PizzaConst
	{
		public readonly static string CheesePizza = "cheese";
		public readonly static string ChickenPizza = "chicken";

	}

	public class Pizza
	{
		public virtual string Name => $"{nameof(Pizza)}";
		public virtual decimal Price => 10m;


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
			else
			{
				pizza = new Pizza();
			}
			return pizza;
		}

		public static Pizza Order(string type)
		{
			Pizza pizza = Create(type);
			/*
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
				pizza = new Pizza();
			}*/
			Prepare();
			Cook();
			Cut();

			return pizza;
		}

		private static void Prepare()
		{
			Console.Write("preparing...");
			Thread.Sleep(500);
			Console.WriteLine("Complated");
		}
		private static void Cook()
		{
			Console.Write("Cooking...");
			Thread.Sleep(500);
			Console.WriteLine("Complated");
		}
		private static void Cut()
		{
			Console.Write("Cut and Boxing...");
			Thread.Sleep(500);
			Console.WriteLine("Complated");
		}

		public override string ToString()
		{
			return $"name: {Name}\nprice: {Price}";
		}
	}

	public class Cheese : Pizza
	{
		public override string Name => $"{nameof(Cheese)} {base.Name}";
		public override decimal Price => base.Price + 3m;
	}
	public class Chicken : Pizza
	{
		public override string Name => $"{nameof(Chicken)} {base.Name}";
		public override decimal Price => base.Price + 6m;
	}

}
