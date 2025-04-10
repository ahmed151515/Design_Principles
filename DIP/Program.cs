// Dependency Inversion Principle (DIP) Example

/*
 * Core Concepts of Dependency Inversion Principle (DIP):
 * 1. High-level modules should not depend directly on low-level modules. Both should depend on abstractions (e.g., interfaces or abstract classes).
 * 2. Abstractions should not depend on details (concrete implementations). Details (concrete implementations) should depend on abstractions.
 *
 * Benefits:
 * - Decoupling: Reduces dependencies between modules.
 * - Flexibility: Easier to swap implementations or add new ones.
 * - Testability: Easier to mock dependencies for unit testing.
 * - Maintainability: Changes in low-level modules are less likely to break high-level modules.
 * Instead of High-Level -> Low-Level dependency, we have:
 *		High-Level -> Abstraction <- Low-Level
 */

namespace DIP
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// --- Code demonstrating the violation of DIP (Before applying the principle) ---
			// This section is commented out to focus on the corrected 'After' version.
			// In the 'Before' version, NotificationService directly creates and depends on concrete EmailService and SMSService.
			// var c = Before.Repository.Customers;
			// foreach (var item in c)
			// {
			// 	// Creates tight coupling: NotificationService knows about specific implementations.
			// 	NotificationService notificationService = new NotificationService(item);
			// 	notificationService.Notify();
			// }

			// --- Code demonstrating adherence to DIP (After applying the principle) ---
			Console.WriteLine("--- Demonstrating DIP with Dependency Injection ---");
			var c = After.Repository.Customers; // Get customer data

			// Loop through each customer to send notifications
			foreach (var item in c)
			{
				Console.WriteLine($"\nProcessing customer: {item.Name}");

				// Create a list of notification services (low-level modules).
				// Crucially, these are stored as the abstraction type 'IMessageService'.
				// The specific types (EmailService, SMSService, MailService) implement this interface.
				List<After.IMessageService> services = new List<After.IMessageService>
				{
					new After.EmailService(item.EmailAddress), // Concrete EmailService, stored as IMessageService
					new After.SMSService(item.MobileNo),       // Concrete SMSService, stored as IMessageService
					new After.MailService(item.Address)        // Concrete MailService, demonstrating extensibility
                };

				// --- Demonstrating different Dependency Injection techniques ---

				// 1. Constructor Injection: Dependencies are provided when the object is created.
				Console.WriteLine("-- Constructor Injection Example --");
				// The NotificationService receives the list of services (abstractions) via its constructor.
				// It doesn't know or care about the concrete types, only that they fulfill the IMessageService contract.
				var notificationService = new After.NotificationService(services);
				notificationService.Notify(); // Execute notification logic using the injected services.

				// 2. Property Injection: Dependencies are set through a public property or setter method after object creation.
				Console.WriteLine("\n-- Property Injection Example --");
				var notificationService1 = new After.NotificationService1();
				// Dependencies are injected via the SetServices method.
				notificationService1.SetServices(services);
				notificationService1.Notify(); // Execute notification logic.

				// 3. Method Injection: Dependencies are passed directly to the specific method that requires them.
				Console.WriteLine("\n-- Method Injection Example --");
				var notificationService2 = new After.NotificationService2();
				// Dependencies are passed as an argument to the Notify method.
				notificationService2.Notify(services); // Execute notification logic.
			}

		}
	}
}

/*
 * High-level modules should not import anything from low-level modules. Both should depend on abstractions (e.g., interfaces).
 * Abstractions should not depend on details. Details (concrete implementations) should depend on abstractions.
 */


// ==================================================================================
// BEFORE: Example violating the Dependency Inversion Principle
// ==================================================================================
namespace DIP.Before
{
	// Represents customer data.
	internal class Customer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		public string MobileNo { get; set; }
		public string Address { get; set; }
	}

	// Low-level module: Concrete implementation for sending SMS.
	internal class SMSService
	{
		public string MobileNo { get; set; }

		// Sends an SMS.
		public void Send()
		{
			Console.WriteLine($"SMS is sent to {MobileNo}");
		}
	}

	// Low-level module: Concrete implementation for sending Email.
	internal class EmailService
	{
		public string EmailAddress { get; set; }

		// Sends an email.
		public void Send()
		{
			Console.WriteLine($"e-mail is sent to {EmailAddress}");
		}
	}

	// High-level module: Responsible for notifying customers.
	// *** VIOLATION OF DIP ***
	// This NotificationService directly depends on the concrete low-level modules: EmailService and SMSService.
	// It creates instances ('new EmailService', 'new SMSService') within its constructor.
	// This leads to tight coupling. Adding a new notification method (e.g., MailService) requires modifying this class.
	internal class NotificationService
	{
		// Direct dependencies on concrete classes.
		private readonly EmailService emailService;
		private readonly SMSService smsService;

		// The constructor takes customer data and *creates* the specific service instances.
		public NotificationService(Customer customer)
		{
			// Instantiating concrete dependencies directly.
			emailService = new EmailService
			{
				EmailAddress = customer.EmailAddress
			};
			smsService = new SMSService
			{
				MobileNo = customer.MobileNo
			};
		}

		// The notification logic is hardcoded to use the specific services created in the constructor.
		public void Notify()
		{
			emailService.Send(); // Directly calls the concrete EmailService.
			smsService.Send();   // Directly calls the concrete SMSService.
								 // Cannot easily add other notification types without changing this code.
		}
	}

	// Static class acting as a simple data source for customers.
	internal static class Repository
	{
		// Provides a list of sample customers.
		public static List<Customer> Customers =>
		 new List<Customer>()
		{
			 new Customer
			 {
				 Id = 1,
				 Name = "John Doe",
				 EmailAddress = "john.doe@example.com",
				 MobileNo = "+1 (606)123-4567",
				 Address = "123 2nd Avenue California, USA"
			 },
			 new Customer
			 {
				 Id = 2,
				 Name = "Sarah Sarah",
				 EmailAddress = "sarah.sarah@example.com",
				 MobileNo = "+1 (606)124-4567",
				 Address = "345 4th Avenue Florida, USA"
			 },
			 new Customer
			 {
				 Id = 3,
				 Name = "Steve Pado",
				 EmailAddress = "steve.pado@example.com",
				 MobileNo = "+1 (606)125-4567",
				 Address = "678 3rd Avenue Chicago, USA"
			 }
		};
	}

}


// ==================================================================================
// AFTER: Example adhering to the Dependency Inversion Principle
// ==================================================================================
namespace DIP.After
{
	// Represents customer data (same as Before).
	internal class Customer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		public string MobileNo { get; set; }
		public string Address { get; set; }
	}

	// --- The Abstraction ---
	// This interface defines the contract for any message sending service.
	// Both high-level modules (NotificationService variations) and low-level modules (EmailService, SMSService, MailService)
	// will depend on this abstraction, not on concrete implementations.
	interface IMessageService
	{
		// Method that all concrete message services must implement.
		public void Send();
	}


	// --- Low-Level Modules (Concrete Implementations) ---
	// These classes provide the specific details for sending messages via different channels.
	// They now depend on the abstraction by implementing the IMessageService interface.

	// Concrete implementation for sending SMS. Implements the abstraction.
	internal class SMSService : IMessageService
	{
		public string MobileNo { get; set; } // Specific data needed for SMS.

		// Constructor takes the necessary data.
		public SMSService(string mobileNo)
		{
			MobileNo = mobileNo;
		}

		// Implementation of the Send method as required by IMessageService.
		public void Send()
		{
			Console.WriteLine($"\t[SMS Service] SMS is sent to {MobileNo}");
		}
	}

	// Concrete implementation for sending Email. Implements the abstraction.
	internal class EmailService : IMessageService
	{
		public string EmailAddress { get; set; } // Specific data needed for Email.

		// Constructor takes the necessary data.
		public EmailService(string emailAddress)
		{
			EmailAddress = emailAddress;
		}

		// Implementation of the Send method as required by IMessageService.
		public void Send()
		{
			Console.WriteLine($"\t[Email Service] e-mail is sent to {EmailAddress}");
		}
	}

	// Concrete implementation for sending physical Mail. Implements the abstraction.
	// Demonstrates how easily a new notification type can be added without changing the high-level NotificationService classes.
	internal class MailService : IMessageService
	{
		public string Address { get; set; } // Specific data needed for Mail.

		// Constructor takes the necessary data.
		public MailService(string address)
		{
			Address = address;
		}

		// Implementation of the Send method as required by IMessageService.
		public void Send()
		{
			Console.WriteLine($"\t[Mail Service] mail is sent to {Address}");
		}
	}


	// --- High-Level Modules ---
	// These classes are responsible for the overall notification logic.
	// They now depend on the IMessageService abstraction, not concrete implementations.
	// Dependencies (the actual service instances) are injected from outside.

	// High-level module demonstrating Property Injection.
	internal class NotificationService1
	{
		// Dependency on the abstraction (list of services).
		// It's declared as the interface type IMessageService.
		public List<IMessageService> Services { get; private set; }

		// The notification logic iterates through the collection of abstract services.
		// It doesn't know the concrete types, only that they implement Send().
		public void Notify()
		{
			// Null check is good practice with property injection
			if (Services == null)
			{
				Console.WriteLine("\t[Property Injection] Services not set.");
				return;
			}
			foreach (var item in Services)
			{
				item.Send(); // Polymorphic call to the appropriate Send() method.
			}
		}

		// Setter method used for Property Injection.
		// Allows injecting the dependencies after the object is created.
		public void SetServices(List<IMessageService> services)
		{
			Services = services;
		}
	}

	// High-level module demonstrating Method Injection.
	internal class NotificationService2
	{
		// This class doesn't hold the services as state.
		// Dependencies are passed directly into the method that needs them.

		// The Notify method receives the dependencies (list of services) as a parameter.
		public void Notify(List<IMessageService> services) // Dependency injected via method parameter.
		{
			// Null check is good practice
			if (services == null)
			{
				Console.WriteLine("\t[Method Injection] No services passed to Notify method.");
				return;
			}
			// Logic iterates through the provided abstract services.
			foreach (var item in services)
			{
				item.Send(); // Polymorphic call.
			}
		}

	}

	// High-level module demonstrating Constructor Injection (often the preferred method).
	internal class NotificationService
	{
		// Dependency on the abstraction (list of services).
		// Marked readonly as it's set once during construction and shouldn't change.
		private readonly List<IMessageService> _services;

		// Constructor Injection: Dependencies (the list of IMessageService) are provided when creating an instance.
		// The NotificationService *receives* its dependencies rather than creating them (Inversion of Control).
		public NotificationService(List<IMessageService> services)
		{
			// Stores the injected dependencies.
			this._services = services;
		}

		// The notification logic iterates through the injected abstract services.
		public void Notify()
		{
			// Null check might be useful depending on how constructor handles null
			if (_services == null)
			{
				Console.WriteLine("\t[Constructor Injection] Services collection is null.");
				return;
			}
			foreach (var item in _services)
			{
				item.Send(); // Polymorphic call.
			}
		}
	}

	// Static class acting as a simple data source for customers (same as Before).
	internal static class Repository
	{
		// Provides a list of sample customers.
		public static List<Customer> Customers =>
		 new List<Customer>()
		{
			 new Customer
			 {
				 Id = 1,
				 Name = "John Doe",
				 EmailAddress = "john.doe@example.com",
				 MobileNo = "+1 (606)123-4567",
				 Address = "123 2nd Avenue California, USA"
			 },
			 new Customer
			 {
				 Id = 2,
				 Name = "Sarah Sarah",
				 EmailAddress = "sarah.sarah@example.com",
				 MobileNo = "+1 (606)124-4567",
				 Address = "345 4th Avenue Florida, USA"
			 },
			 new Customer
			 {
				 Id = 3,
				 Name = "Steve Pado",
				 EmailAddress = "steve.pado@example.com",
				 MobileNo = "+1 (606)125-4567",
				 Address = "678 3rd Avenue Chicago, USA"
			 }
		};
	}
}
