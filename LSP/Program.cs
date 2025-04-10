// Namespace demonstrating the Liskov Substitution Principle (LSP)
namespace LSP
{
	// Liskov Substitution Principle (LSP):
	// This principle states that objects of a superclass should be replaceable 
	// with objects of its subclasses without affecting the correctness of the program.
	// In essence, a subclass should behave in a way that is consistent with the 
	// expectations set by its superclass's contract.

	internal class Program
	{
		static void Main(string[] args)
		{
			// --- Demonstrating the 'Before' scenario (LSP Violation) ---
			// Uncommenting these lines would show the problem.
			// Create a FixedAccount using the base Account reference.
			// Before.Account fixedAccountBefore = new Before.FixedAccount("ahmed", 10_000); 
			// Attempting to call Withdraw violates LSP because FixedAccount's 
			// Withdraw throws an exception, which is not the expected behavior
			// for all 'Account' types based on the base class definition.
			// fixedAccountBefore.Withdraw(1000); // This would throw NotSupportedException

			// --- Demonstrating the 'After' scenario (LSP Compliant) ---
			// Create a FixedAccount using the specific FixedAccount type or the base Account type.
			After.FixedAccount fixedAccountAfter = new After.FixedAccount("ahmed", 10_000);

			// Note: fixedAccountAfter does *not* have a Withdraw method. 
			// The compiler prevents the LSP violation seen in the 'Before' example.
			// We can only call methods defined in the 'After.Account' base class (like Deposit).
			// fixedAccountAfter.Withdraw(1000); // This line would cause a compile-time error.

			// Create a CheckingAccount, assigning it to a RegularAccount reference.
			// RegularAccount is the appropriate base type for accounts that *can* withdraw.
			After.RegularAccount r = new After.CheckingAccount("ahmed", 10_000);

			// We can safely call Withdraw on 'r' because the RegularAccount contract
			// guarantees a Withdraw method, and CheckingAccount implements it correctly.
			// This substitution is valid according to LSP.
			r.Withdraw(500); // Example call (implementation allows this if amount < 1000)

		}
	}
}
/*
 * Summary of LSP:
 * Objects of a superclass should be replaceable with objects of its subclasses 
 * without affecting the correctness of the program. If code uses a base class reference, 
 * substituting it with any derived class object should not break the logic or cause errors.
 * 
 * The 'Before' example violates this because FixedAccount cannot fulfill the Withdraw
 * contract of the base Account class (it throws an exception instead).
 * 
 * The 'After' example fixes this by restructuring the hierarchy. Only accounts that 
 * genuinely support withdrawal inherit from a base class (RegularAccount) that defines 
 * the Withdraw method. FixedAccount inherits from a more basic Account class that 
 * does *not* define Withdraw.
 */


// Namespace containing the code structure *before* applying LSP.
namespace Before
{
	/// <summary>
	/// Base abstract class for all account types.
	/// Defines common properties and requires all derived classes to implement Deposit and Withdraw.
	/// </summary>
	abstract class Account
	{
		// Constructor to initialize account details.
		protected Account(string name, decimal balance)
		{
			Name = name;
			Balance = balance;
		}

		// Public property for the account holder's name.
		public string Name { get; set; }
		// Public property for the account balance.
		public decimal Balance { get; set; } // Note: Often better to have protected set

		// Abstract method for depositing money. Must be implemented by subclasses.
		public abstract void Deposit(decimal amount);
		// Abstract method for withdrawing money. Must be implemented by subclasses.
		// This is the source of the LSP violation for FixedAccount.
		public abstract void Withdraw(decimal amount);
	}

	/// <summary>
	/// Represents a Checking Account. Allows deposits and withdrawals with a limit.
	/// </summary>
	class CheckingAccount : Account
	{
		// Constructor calling the base class constructor.
		public CheckingAccount(string name, decimal balance) : base(name, balance)
		{
		}

		// Implementation of the Deposit method.
		public override void Deposit(decimal amount)
		{
			Balance += amount; // Increases balance
		}

		// Implementation of the Withdraw method with a specific rule.
		public override void Withdraw(decimal amount)
		{
			if (amount < 1000) // Check withdrawal limit
			{
				Balance -= amount; // Decreases balance if within limit
			}
			else
			{
				// Provides feedback if the limit is exceeded.
				Console.WriteLine("you can not withdraw more than 1000");
			}
		}

	}
	/// <summary>
	/// Represents a Fixed Deposit Account. Allows deposits but conceptually shouldn't allow withdrawals.
	/// </summary>
	class FixedAccount : Account
	{
		// Constructor calling the base class constructor.
		public FixedAccount(string name, decimal balance) : base(name, balance)
		{
		}

		// Implementation of the Deposit method.
		public override void Deposit(decimal amount)
		{
			Balance += amount; // Increases balance
		}

		/// <summary>
		/// LSP VIOLATION: This implementation breaks the contract of the base class.
		/// Instead of performing a withdrawal or handling insufficient funds gracefully,
		/// it throws an exception, changing the expected behavior for an 'Account'.
		/// </summary>
		public override void Withdraw(decimal amount)
		{
			// Throwing an exception here violates LSP because a user of the base 'Account'
			// class expects Withdraw to be a valid operation, not one that crashes the program.
			throw new NotSupportedException($"You can not withdraw from Fixed Deposit Account!!!");
		}
	}

	/// <summary>
	/// Represents a Savings Account. Allows deposits and standard withdrawals.
	/// </summary>
	class SavingAccount : Account
	{
		// Constructor calling the base class constructor.
		public SavingAccount(string name, decimal balance)
			: base(name, balance)
		{
		}

		// Implementation of the Deposit method.
		public override void Deposit(decimal amount)
		{
			Balance += amount; // Increases balance
		}

		// Implementation of the Withdraw method.
		public override void Withdraw(decimal amount)
		{
			Balance -= amount; // Decreases balance (assuming sufficient funds)
		}
	}
}

// Namespace containing the code structure *after* applying LSP.
namespace After
{
	/// <summary>
	/// Base abstract class for all account types.
	/// Defines only the truly common properties and operations (like Deposit).
	/// Does NOT define Withdraw, adhering to LSP.
	/// </summary>
	abstract class Account
	{
		// Constructor to initialize account details.
		protected Account(string name, decimal balance)
		{
			Name = name;
			Balance = balance;
		}

		// Public property for the account holder's name.
		public string Name { get; set; }
		// Public property for the account balance.
		public decimal Balance { get; set; } // Note: Often better to have protected set

		// Abstract method for depositing money. Must be implemented by subclasses.
		public abstract void Deposit(decimal amount);

		// Withdraw method is intentionally removed from this base class.
	}

	/// <summary>
	/// Intermediate abstract class for accounts that support withdrawals.
	/// Inherits from Account and adds the Withdraw capability.
	/// Subclasses of this type *must* implement Withdraw.
	/// </summary>
	abstract class RegularAccount : Account
	{
		// Constructor calling the base class constructor.
		protected RegularAccount(string name, decimal balance)
			: base(name, balance)
		{

		}

		// Abstract method for withdrawing money. 
		// Only accounts inheriting from RegularAccount need to implement this.
		public abstract void Withdraw(decimal amount);
	}

	/// <summary>
	/// Represents a Checking Account. Allows deposits and withdrawals with a limit.
	/// Inherits from RegularAccount as it supports both Deposit and Withdraw.
	/// </summary>
	class CheckingAccount : RegularAccount // Inherits from the withdrawable account type
	{
		// Constructor calling the base class constructor.
		public CheckingAccount(string name, decimal balance) : base(name, balance)
		{
		}

		// Implementation of the Deposit method (inherited from Account via RegularAccount).
		public override void Deposit(decimal amount)
		{
			Balance += amount; // Increases balance
		}

		// Implementation of the Withdraw method (required by RegularAccount).
		public override void Withdraw(decimal amount)
		{
			if (amount < 1000) // Check withdrawal limit
			{
				Balance -= amount; // Decreases balance if within limit
			}
			else
			{
				// Provides feedback if the limit is exceeded.
				Console.WriteLine("you can not withdraw more than 1000");
			}
		}

	}

	/// <summary>
	/// Represents a Fixed Deposit Account. Allows deposits only.
	/// Inherits directly from Account, *not* RegularAccount, because it doesn't support Withdraw.
	/// This structure adheres to LSP.
	/// </summary>
	class FixedAccount : Account // Note: Inherits directly from the base Account
	{
		// Constructor calling the base class constructor.
		public FixedAccount(string name, decimal balance) : base(name, balance)
		{
		}

		// Implementation of the Deposit method (required by Account).
		public override void Deposit(decimal amount)
		{
			Balance += amount; // Increases balance
		}

		// No Withdraw method is defined or required, preventing the LSP violation.
	}

	/// <summary>
	/// Represents a Savings Account. Allows deposits and standard withdrawals.
	/// Inherits from RegularAccount as it supports both Deposit and Withdraw.
	/// </summary>
	class SavingAccount : RegularAccount // Inherits from the withdrawable account type
	{
		// Constructor calling the base class constructor.
		public SavingAccount(string name, decimal balance)
			: base(name, balance)
		{
		}

		// Implementation of the Deposit method (inherited from Account via RegularAccount).
		public override void Deposit(decimal amount)
		{
			Balance += amount; // Increases balance
		}

		// Implementation of the Withdraw method (required by RegularAccount).
		public override void Withdraw(decimal amount)
		{
			Balance -= amount; // Decreases balance (assuming sufficient funds)
		}
	}

} // End of After namespace
