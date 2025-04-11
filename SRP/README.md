
# Single Responsibility Principle (SRP) - C# Example

**Principle:** A class should have only one reason to change. This means a class should have only a single responsibility or job.

This example demonstrates refactoring a class that violates SRP into classes that adhere to it.

## The Problem ("Before" Version)

The `Befor.Account` class violates SRP because it has multiple responsibilities:

1.  **Data Holding:** Manages account holder's `Name`, `Email`, and `Balance`.
2.  **Transaction Logic:** Implements the rules for deposits and withdrawals, including overdraft checks.
3.  **Notification:** Formats and sends (simulated) email notifications about transactions.

Changes to transaction rules *or* email formatting/sending mechanisms would require changing this single class.

### `Program.cs` (Usage - Before)

```csharp
// Befor.Account account = new Befor.Account("ahmed", "gmail", 400);
// account.MakeTransaction(-100); // Single call handles logic and notification
```

### `Befor.Account` Code

```csharp
namespace Befor
{
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
		}

		// This method does TOO MUCH (violates SRP)
		public void MakeTransaction(decimal amount)
		{
			var transactionMessage = "";

			// Responsibility 1: Transaction Logic (Withdraw / Deposit)
			// handle withdraw
			if (amount < 0)
			{
				if (Balance < Math.Abs(amount))
				{
					transactionMessage =
					$"OVERDRAFT when trying to withdraw " +
					$"{Math.Abs(amount).ToString("C2")}," +
					$" current balance {Balance.ToString("C2")}";
				}
				else
				{
					this.Balance += amount; // Modifies state
					transactionMessage =
					   $"OK Withdraw {Math.Abs(amount).ToString("C2")}" +
					   $", current balance {Balance.ToString("C2")}";
				}
			}
			else // handle deposit
			{
				if (amount > 0)
				{
					this.Balance += amount; // Modifies state
					transactionMessage =
						$"OK Deposit {amount.ToString("C2")}" +
						$", current balance {Balance.ToString("C2")}";
				}
			}

			// Responsibility 2: Notification Formatting & Sending
			// This class should not be responsible for sending emails.
			Console.WriteLine( // Simulates sending email
			 $"\n\n\t\t To: {Email}" +
			 $"\n\t\t Subject: Fake Bank Account Activity" +
			 $"\n\n\t\t Dear {Name}," +
			 $"\n\n\t\t\t A recent activity on your account occures at {DateTime.Now.ToString("yyyy-MM-dd hh:mm")}" +
			 "\n\t\t\t\t ===> {0}" +
			 $"\n\n\t\t Thank You,\n\t\t Fake Bank." +
			 $"\n\n\t\t--------------------------- ", transactionMessage);
		}
	}
}
```

---

## The Solution ("After" Version)

Responsibilities are separated into distinct classes:

1.  `After.Account`: **Data Holding**. Only holds account data. (This class itself looks like part of an **Anemic Domain Model**, but it works within a structure where responsibilities are separated).
2.  `After.AccountService`: **Transaction Logic**. Handles deposits, withdrawals, and orchestrates the transaction process.
3.  `After.EmailClient`: **Notification**. Handles the formatting and sending of emails.

Now, changes to transaction logic only affect `AccountService`, and changes to email sending only affect `EmailClient`. The `Account` class changes only if the data it holds needs to change.

### `Program.cs` (Usage - After)

```csharp
using After; // Assuming 'using After;' is present

// Create the separate components
Account account = new Account("ahmed", "gmail", 400);
AccountService accountService = new AccountService();

// Service orchestrates the transaction and notification
accountService.MakeTransaction(account, -410);
```

### `After` Namespace Code

**`After.Account` (Data)**

```csharp
namespace After
{
	// Responsibility: Hold Account Data
	public class Account
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public decimal Balance { get; set; } // State can be modified by services

		public Account(string name, string email, decimal balance)
		{
			Name = name;
			Email = email;
			Balance = balance;
		}
	}
}
```

**`After.AccountService` (Transaction Logic)**

```csharp
namespace After
{
	// Responsibility: Handle Account Business Logic (Transactions)
	public class AccountService
	{
		public string Withdraw(Account account, decimal amount)
		{
			// Negative amount expected for withdrawal logic consistency in MakeTransaction
			var transactionMessage = "";
			if (account.Balance < Math.Abs(amount))
			{
				transactionMessage =
				$"OVERDRAFT when trying to withdraw " +
				$"{Math.Abs(amount).ToString("C2")}," +
				$" current balance {account.Balance.ToString("C2")}";
			}
			else
			{
				account.Balance += amount; // Modify account state
				transactionMessage =
				   $"OK Withdraw {Math.Abs(amount).ToString("C2")}" +
				   $", current balance {account.Balance.ToString("C2")}";
			}
			return transactionMessage;
		}

		public string Deposit(Account account, decimal amount)
		{
			// Positive amount expected for deposit
			var transactionMessage = "";
			if (amount > 0)
			{
				account.Balance += amount; // Modify account state
				transactionMessage =
					$"OK Deposit {amount.ToString("C2")}" +
					$", current balance {account.Balance.ToString("C2")}";
			}
			// Consider adding handling for amount <= 0 if needed
			return transactionMessage;
		}

		// Orchestrates the transaction process
		public void MakeTransaction(Account account, decimal amount)
		{
			var transactionMessage = "";

			// Delegate logic based on amount
			if (amount < 0)
			{
				transactionMessage = Withdraw(account, amount);
			}
			else
			{
				transactionMessage = Deposit(account, amount);
			}

			// Delegate notification to the responsible class
			EmailClient emailClient = new EmailClient(); // Could be injected via DI
			if (!string.IsNullOrEmpty(transactionMessage)) // Only send email if something happened
			{
				emailClient.Send(account, transactionMessage, DateTime.Now);
			}
		}
	}
}
```

**`After.EmailClient` (Notification)**

```csharp
namespace After
{
	// Responsibility: Handle Email Notifications
	public class EmailClient
	{
		public void Send(Account account, string transactionMessage, DateTime transactionDate)
		{
			// Logic specifically for formatting and sending the email
			Console.WriteLine( // Simulates sending email
			 $"\n\n\t\t To: {account.Email}" +
			 $"\n\t\t Subject: Fake Bank Account Activity" +
			 $"\n\n\t\t Dear {account.Name}," +
			 $"\n\n\t\t\t A recent activity on your account occures at {transactionDate.ToString("yyyy-MM-dd hh:mm")}" +
			 "\n\t\t\t\t ===> {0}" +
			 $"\n\n\t\t Thank You,\n\t\t Fake Bank." +
			 $"\n\n\t\t--------------------------- ", transactionMessage);
		}
	}
}
```

## Benefits of Applying SRP

*   **Maintainability:** Easier to understand and modify code as changes are localized.
*   **Testability:** Individual components (`AccountService`, `EmailClient`) can be tested in isolation.
*   **Reusability:** Components like `EmailClient` might be reusable elsewhere.
*   **Reduced Coupling:** Changes in one responsibility (e.g., email sending) are less likely to break other parts (e.g., transaction logic).

## Related Concepts

*   [[Anemic Domain Model]]
*   [[Rich Domain Model]]
# Anemic Domain Model

## Definition

An Anemic Domain Model is a design approach where **business logic is separated from the domain objects**. The domain model itself contains **only data (state)**, typically with public getters and setters, while all **logic resides in a separate service layer**.

## Characteristics

*   Domain objects are primarily data containers (often resembling simple data structures or DTOs).
*   Business logic, rules, and validations are implemented in external service classes that operate *on* these data objects.
*   Tends to be on the technical side

## Pros

*   **Simple Initial Understanding:** Can be easier to grasp initially, especially for developers familiar with procedural programming or basic CRUD applications. Looks like database tables mapped to objects.
*   **Works for Simple Cases:** Suitable for very simple applications with minimal business logic (e.g., basic CRUD operations).

## Cons

*   **Weak Encapsulation:** Domain objects cannot protect their own state or guarantee their consistency, as external services modify them directly.
*   **Scattered Logic:** Business logic related to a single domain concept can be spread across multiple service classes, making it difficult to find, understand, maintain, and avoid duplication. 

## Relation to Examples

*   The `After.Account` class in the Single Responsibility Principle (SRP) - C# Example note, when viewed *in isolation*, resembles an anemic object because it only holds data. However, the `AccountService` holds related *domain* logic, differentiating it slightly from a purely anemic approach where services might contain less cohesive or more infrastructure-focused logic.

# Rich Domain Model

## Definition

A Rich Domain Model is a design approach, often associated with Domain-Driven Design (DDD), where domain objects **encapsulate both the state (data/properties) and the behavior (business logic, rules, validations)** that operates on that state.

## Characteristics

*   Domain objects have methods that implement business logic relevant to the object itself.
*   Logic is cohesive and located *with* the data it manipulates.
*   Emphasizes strong encapsulation and information hiding to maintain object consistency and integrity.
*   Objects actively manage their own state transitions according to business rules.
*   Tends to focus more on modeling the actual business domain concepts and their interactions.

## Pros

*   **Cohesive Logic:** Business logic is located with the data it operates on, making it easier to find, understand, modify, and maintain.
*   **Strong Encapsulation:** Protects object state and invariants, ensuring consistency and reducing bugs caused by invalid states.
*   **Improved Maintainability:** Changes related to a specific domain concept are often localized within its corresponding object(s).


## Cons

*   **Steeper Learning Curve:** Requires a solid understanding of Object-Oriented Programming (OOP) principles and potentially DDD concepts.
*   **Upfront Design Effort:** Requires more careful thought about design, responsibilities, and object interactions early in the development process.
*   **Potential for Large Objects:** If not managed carefully (e.g., using aggregates), domain objects can sometimes grow large, although this often indicates a need for further decomposition.

## Relation to Examples

*   The `Befor.Account` class in the [[Single Responsibility Principle (SRP) - C# Example]] *attempted* to be rich by including `MakeTransaction` logic, but it violated [[Single Responsibility Principle (SRP) - C# Example|SRP]] by also including unrelated responsibilities (email sending).
*   A pure Rich Domain Model approach for the banking example *might* have kept `Withdraw` and `Deposit` methods within the `Account` class (as they directly relate to account state and rules), while still moving the `EmailClient` responsibility out, adhering better to SRP than the original "Before" version. The "After" version in the example chose to separate *all* logic into `AccountService` to clearly illustrate the separation aspect of SRP, even if it made the `Account` object itself appear more [[Anemic Domain Model|anemic]].
