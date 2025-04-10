namespace KissYagniTDV1
{
	internal class Program
	{
		/*
		 * do the req. but it VIOLATION a OCP and RSP 
		 * method Services.PaymentService.pay mapy become huge method
		 */
		static void Main(string[] args)
		{
			do
			{
				Console.Clear();
				Console.Write("Amount: ");
				decimal amount = decimal.Parse(Console.ReadLine());
				Console.Write("Payment Method: ");
				string paymentMetthod = Console.ReadLine();

				// processing
				var paymentService = new Services.PaymentService();
				var receipt = paymentService.pay(amount, paymentMetthod);
				Console.WriteLine("------------------");
				Console.WriteLine(receipt);

				Console.WriteLine("Another Payment, press any Key");
				Console.ReadKey();
			}
			while (true);
		}
	}

	namespace Services
	{
		class PaymentService
		{
			public Models.Receipt pay(decimal amount, string paymentMethod)
			{
				if (string.Equals(paymentMethod, Commn.PaymentConst.Cash, StringComparison.OrdinalIgnoreCase))
				{
					decimal finaleAmount = amount + (amount * Commn.PaymentConst.CashDiscount);
					return new Models.Receipt(finaleAmount, paymentMethod);
				}
				else if (string.Equals(paymentMethod, Commn.PaymentConst.Debit, StringComparison.OrdinalIgnoreCase))
				{
					decimal finaleAmount = amount - (amount * Commn.PaymentConst.DebitFeez);
					return new Models.Receipt(finaleAmount, paymentMethod);
				}
				else
				{
					throw new ArgumentException(nameof(paymentMethod));
				}
			}
		}
	}
	namespace Models
	{
		record Receipt
		{
			public Receipt(decimal amount, string paymentMethod)
			{
				Id = Guid.NewGuid().ToString().Substring(0, 8);
				Amount = amount;
				PaymentMethod = paymentMethod;
			}

			public string Id { get; }
			public decimal Amount { get; }
			public string PaymentMethod { get; }
		}
	}
	namespace Commn
	{
		static class PaymentConst
		{
			public static readonly string Cash = "cash";
			public static readonly string Debit = "debit";
			public static readonly decimal CashDiscount = 0.05m;
			public static readonly decimal DebitFeez = 0.02m;
		}
	}
}
