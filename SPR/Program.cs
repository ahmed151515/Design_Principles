/*
 * ============================================================================
 * Single Responsibility Principle (SRP) Demonstration
 * ============================================================================
 *
 * Principle Definition:
 * "A class should have only one reason to change."
 * This means a class should encapsulate a single, cohesive responsibility. If a
 * class has multiple responsibilities, changes related to one responsibility
 * might inadvertently affect or require changes related to another.
 *
 * Purpose of this Example:
 * This code demonstrates SRP using a simple Bank Account scenario.
 * It contrasts a design that violates SRP (in the `Before` namespace), where the
 * Account class handles both balance management and notification logic, with a
 * design that adheres to SRP (in the `After` namespace), separating these
 * concerns into distinct classes (`Account`, `AccountService`, `EmailClient`).
 *
 * Context: Anemic vs. Rich Domain Models
 *
 * Anemic Domain Model:
 *   - Domain objects primarily contain data (properties/state) with little to no
 *     business logic (behavior).
 *   - Business logic resides in separate "Service" or "Manager" classes that
 *     operate *on* the domain objects.
 *   - Often simpler to start with, especially for basic CRUD operations.
 *   - Can lead to scattered logic, poor encapsulation (objects can't enforce
 *     their own rules), and procedural-style code within services as complexity grows.
 *   - The 'After' example leans towards this by moving logic to AccountService.
 *
 * Rich Domain Model:
 *   - Domain objects encapsulate both state (data) *and* behavior (business logic,
 *     validations) related to that state.
 *   - Promotes strong encapsulation, cohesion (logic lives with the data it affects),
 *     and better modeling of the actual business domain.
 *   - Objects are responsible for maintaining their own consistency.
 *   - Can have a steeper learning curve and requires careful design.
 *   - Adhering strictly to SRP often encourages moving towards richer domain objects,
 *     although helper services for cross-cutting concerns (like notifications) are still common.
 *     (Note: A richer model might have Put `Withdraw` and `Deposit` methods *on* the
 *     `Account` class itself, enforcing balance rules internally, while still using
 *     a separate `EmailClient` for notifications).
 *
 * Why SRP is Useful:
 * 1. Reduced Coupling: Classes are less dependent on each other. Changes in one
 *    responsibility are less likely to break others.
 * 2. Increased Cohesion: Classes have a clear, focused purpose.
 * 3. Improved Testability: It's easier to test a class with a single responsibility
 *    in isolation.
 * 4. Enhanced Maintainability & Readability: Code is easier to understand, locate
 *    relevant logic, and modify safely.
 */

namespace SPR
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("--- Running BEFORE SRP ---");
			Before.Account accountBefore = new Before.Account("Ahmed Before", "ahmed.before@example.com", 400);
			// This single method call does balance logic AND email formatting/sending.
			accountBefore.MakeTransaction(-100);
			accountBefore.MakeTransaction(50);
			accountBefore.MakeTransaction(-500); // Overdraft attempt

			Console.WriteLine("\n\n--- Running AFTER SRP ---");
			// Account holds data.
			After.Account accountAfter = new After.Account("Fatima After", "fatima.after@example.com", 400);
			// Service handles transaction logic.
			After.AccountService accountService = new After.AccountService();
			// Service orchestrates logic and delegates notification.
			accountService.MakeTransaction(accountAfter, -100);
			accountService.MakeTransaction(accountAfter, 50);
			accountService.MakeTransaction(accountAfter, -500); // Overdraft attempt

		}
	}
}


// ============================================================================
// BEFORE: Violation of Single Responsibility Principle
// ============================================================================
namespace Before
{
	// --- SRP Violation Example ---
	// This Account class violates SRP because it has MULTIPLE responsibilities:
	// 1. Managing account state (Name, Email, Balance).
	// 2. Handling transaction logic (deposits, withdrawals, overdraft checks).
	// 3. Formatting and sending notification emails.
	// This means the class has multiple reasons to change.

	public class Account
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public decimal Balance { get; set; }

		public Account(string name, string email, decimal balance)
		{
			Name = name;
			Email = email;
			Balance = balance;
			Console.WriteLine($"Account '{Name}' created with balance {Balance:C2}.");
		}

		// VIOLATION: This method handles BOTH transaction logic AND notification.
		// Reason to change #1: Transaction rules change (e.g., new fees, different overdraft logic).
		// Reason to change #2: Email format or delivery mechanism changes.
		public void MakeTransaction(decimal amount)
		{
			var transactionMessage = "";
			bool isOverdraft = false;

			// Responsibility: Transaction Logic (Withdrawal)
			if (amount < 0)
			{
				if (Balance < Math.Abs(amount))
				{
					isOverdraft = true;
					transactionMessage =
					$"OVERDRAFT when trying to withdraw " +
					$"{Math.Abs(amount):C2}," +
					$" current balance {Balance:C2}";
				}
				else
				{
					// Apply the change
					this.Balance += amount;
					transactionMessage =
					   $"OK Withdraw {Math.Abs(amount):C2}" +
					   $", current balance {Balance:C2}";
				}
			}
			// Responsibility: Transaction Logic (Deposit)
			else if (amount > 0) // Explicitly check for deposit amount > 0
			{
				// Apply the change
				this.Balance += amount;
				transactionMessage =
					$"OK Deposit {amount:C2}" +
					$", current balance {Balance:C2}";
			}
			else // amount is zero
			{
				transactionMessage = $"No transaction performed for zero amount. Current balance {Balance:C2}";
			}

			// Responsibility: Notification Logic (Email Formatting & Sending Simulation)
			// This part does not belong here according to SRP.
			// If email requirements change (e.g., use SMS, change format), this class needs modification,
			// even if the transaction logic hasn't changed.
			Console.WriteLine(
			 $"\n\n\t\t --- Email Notification ---" + // Added clarity
			 $"\n\t\t To: {Email}" +
			 $"\n\t\t Subject: Fake Bank Account Activity" +
			 $"\n\n\t\t Dear {Name}," +
			 $"\n\n\t\t\t A recent activity on your account occurred at {DateTime.Now:yyyy-MM-dd HH:mm}" + // Use HH for 24-hour
			 $"\n\t\t\t\t ===> {transactionMessage}" + // Directly use the formatted message
			 $"\n\n\t\t Thank You,\n\t\t Fake Bank." +
			 $"\n\t\t --------------------------- ");
		}
	}
}

// ============================================================================
// AFTER: Adherence to Single Responsibility Principle
// ============================================================================
namespace After
{
	// --- SRP Adherence Example ---
	// Responsibilities are now separated into distinct classes:
	// - Account: Holds account data/state. (Closer to Anemic model here)
	// - AccountService: Handles transaction logic.
	// - EmailClient: Handles notification formatting and sending.

	/// <summary>
	/// Represents the state of a bank account.
	/// Its single responsibility is to hold account data.
	/// (In this structure, it leans towards an Anemic Domain Object).
	/// </summary>
	public class Account
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public decimal Balance { get; set; } // Still mutable from outside in this simple example

		public Account(string name, string email, decimal balance)
		{
			Name = name;
			Email = email;
			Balance = balance;
			Console.WriteLine($"Account '{Name}' created with balance {Balance:C2}.");
		}
	}

	/// <summary>
	/// Handles the business logic related to account transactions.
	/// Its single responsibility is orchestrating deposits and withdrawals.
	/// </summary>
	public class AccountService
	{
		// Dependency on the EmailClient (could be injected for better testability)
		private readonly EmailClient _emailClient = new EmailClient();

		// Responsibility: Perform withdrawal logic
		private string Withdraw(Account account, decimal amount) // amount expected to be negative
		{
			if (account.Balance < Math.Abs(amount))
			{
				// Overdraft: Do not change balance, just report
				return $"OVERDRAFT when trying to withdraw " +
					   $"{Math.Abs(amount):C2}," +
					   $" current balance {account.Balance:C2}";
			}
			else
			{
				// Perform withdrawal
				account.Balance += amount; // amount is negative
				return $"OK Withdraw {Math.Abs(amount):C2}" +
					   $", current balance {account.Balance:C2}";
			}
		}

		// Responsibility: Perform deposit logic
		private string Deposit(Account account, decimal amount) // amount expected to be positive
		{
			if (amount <= 0) // Guard clause for invalid deposit
			{
				return $"Deposit amount must be positive. Current balance {account.Balance:C2}";
			}
			// Perform deposit
			account.Balance += amount;
			return $"OK Deposit {amount:C2}" +
				   $", current balance {account.Balance:C2}";
		}

		/// <summary>
		/// Orchestrates a transaction (withdraw or deposit) and triggers notification.
		/// </summary>
		/// <param name="account">The account to transact on.</param>
		/// <param name="amount">Positive for deposit, negative for withdrawal.</param>
		public void MakeTransaction(Account account, decimal amount)
		{
			string transactionMessage;

			if (amount < 0)
			{
				transactionMessage = Withdraw(account, amount);
			}
			else if (amount > 0)
			{
				transactionMessage = Deposit(account, amount);
			}
			else // amount is zero
			{
				transactionMessage = $"No transaction performed for zero amount. Current balance {account.Balance:C2}";
			}

			// Delegate notification to the specialized EmailClient
			// AccountService's responsibility ends with performing the transaction logic.
			_emailClient.SendTransactionNotification(account, transactionMessage, DateTime.Now);
		}
	}

	/// <summary>
	/// Handles the responsibility of sending notifications, specifically emails in this case.
	/// Its single responsibility is communication/notification formatting and delivery.
	/// </summary>
	public class EmailClient
	{
		/// <summary>
		/// Sends a notification email about a transaction.
		/// </summary>
		/// <param name="account">The account associated with the notification.</param>
		/// <param name="transactionMessage">The message describing the transaction outcome.</param>
		/// <param name="transactionDate">The date and time of the transaction.</param>
		public void SendTransactionNotification(Account account, string transactionMessage, DateTime transactionDate)
		{
			// Responsibility: Formatting and sending the email (simulation)
			Console.WriteLine(
			 $"\n\n\t\t --- Email Notification ---" + // Added clarity
			 $"\n\t\t To: {account.Email}" +
			 $"\n\t\t Subject: Fake Bank Account Activity" +
			 $"\n\n\t\t Dear {account.Name}," +
			 $"\n\n\t\t\t A recent activity on your account occurred at {transactionDate:yyyy-MM-dd HH:mm}" + // Use HH for 24-hour
			 $"\n\t\t\t\t ===> {transactionMessage}" + // Use the message passed in
			 $"\n\n\t\t Thank You,\n\t\t Fake Bank." +
			 $"\n\n\t\t --------------------------- ");
		}
	}
}