

namespace KissYagniTDV3
{
	internal class Program
	{
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
				var receipt = paymentService.Pay(amount, paymentMetthod);
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
		interface IPaymaentStrategy
		{
			public Models.Receipt Pay(decimal amount);
		}
		class CashPaymantStrategy : IPaymaentStrategy
		{
			public Models.Receipt Pay(decimal amount)
			{
				decimal finaleAmount = amount + (amount * Commn.PaymentConst.CashDiscount);
				return new Models.Receipt(finaleAmount, Commn.PaymentConst.Cash);
			}
		}
		class DebitPaymantStrategy : IPaymaentStrategy
		{
			public Models.Receipt Pay(decimal amount)
			{
				decimal finaleAmount = amount - (amount * Commn.PaymentConst.DebitFeez);
				return new Models.Receipt(finaleAmount, Commn.PaymentConst.Debit);
			}
		}

		class PaymentService
		{
			private readonly IDictionary<string, IPaymaentStrategy> _paymaentStrategies;
			public PaymentService()
			{
				_paymaentStrategies = new Dictionary<string, IPaymaentStrategy>
				{
					{Commn.PaymentConst.Cash, new CashPaymantStrategy() },
					{Commn.PaymentConst.Debit, new DebitPaymantStrategy() }
				};
			}
			public Models.Receipt Pay(decimal amount, string paymentMethod)
			{
				paymentMethod = paymentMethod.ToLower();
				if (_paymaentStrategies.ContainsKey(paymentMethod))
				{
					return _paymaentStrategies[paymentMethod].Pay(amount);
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
			// bad you dont know if you this staregy
			public static readonly string Creidt = "creidt";
			public static readonly decimal CashDiscount = 0.05m;
			public static readonly decimal DebitFeez = 0.02m;
		}
	}
}
