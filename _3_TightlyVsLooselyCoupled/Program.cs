namespace _3_TightlyVsLooselyCoupled
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// Example of Tight Coupling
			NotificationService notification = new NotificationService(
				new EmailService(),
				new SmsService()
			);
			notification.Notify();

			Console.WriteLine("--------------------");

			// Example of Loose Coupling
			var serviceMode = NotificationModeFactory.Create(NotificationMode.WEIRD);
			NotificationServiceAfterDecoupling notificationServiceAfterDecoupling =
				new NotificationServiceAfterDecoupling(serviceMode);
			notificationServiceAfterDecoupling.Notify();
		}
	}

	// Concrete implementations of INotificationMode
	class EmailService : INotificationMode
	{
		public void Send()
		{
			Console.WriteLine("Email Sent");
		}
	}
	class SmsService : INotificationMode
	{
		public void Send()
		{
			Console.WriteLine("SMS Sent");
		}
	}
	class WeirdService : INotificationMode
	{
		public void Send()
		{
			Console.WriteLine("Weird Notification Sent");
		}
	}

	// Interface defining the contract for notification modes
	public interface INotificationMode
	{
		void Send();
	}

	/// <summary>
	/// This is an example of Tight Coupling.
	/// - The `NotificationService` class has direct dependencies on concrete services (`EmailService`, `SmsService`).
	/// - It knows all the details of these instances, making changes, maintenance, and scaling harder.
	/// 
	/// **Pros:**
	/// ✅ Faster initial development and launch.  
	/// ✅ Simpler design with fewer abstractions.  
	/// 
	/// **Cons:**
	/// ❌ Difficult to maintain, test, or extend.  
	/// ❌ Changes in one part of the system can significantly impact other parts.  
	/// </summary>
	class NotificationService
	{
		private readonly EmailService _emailService;
		private readonly SmsService _smsService;

		public NotificationService(EmailService emailService, SmsService smsService)
		{
			_emailService = emailService;
			_smsService = smsService;
		}

		public void Notify()
		{
			_emailService.Send();
			_smsService.Send();
		}
	}

	/// <summary>
	/// This is an example of Loose Coupling.
	/// - The `NotificationServiceAfterDecoupling` class depends on an abstraction (`INotificationMode`) instead of concrete implementations.
	/// - This makes the system **easier to maintain and scale**.
	/// - The class only knows **what it needs to know** (i.e., `Send()`), adhering to the **Single Responsibility Principle (SRP)**.
	/// 
	/// **Pros:**
	/// ✅ Easier to maintain and scale.  
	/// ✅ Supports Dependency Injection, making unit testing easier.  
	/// 
	/// **Cons:**
	/// ❌ Requires extra design effort (e.g., interfaces, dependency injection).  
	/// ❌ Can introduce unnecessary complexity in small applications.  
	/// </summary>
	class NotificationServiceAfterDecoupling
	{
		private readonly INotificationMode _notificationMode;

		public NotificationServiceAfterDecoupling(INotificationMode notificationMode)
		{
			_notificationMode = notificationMode;
		}

		public void Notify()
		{
			_notificationMode.Send();
		}
	}

	// Enum representing different notification modes
	public enum NotificationMode
	{
		EMAIL,
		SMS,
		WEIRD
	}

	/// <summary>
	/// Factory Pattern: Creates instances of notification services based on the selected mode.
	/// - Helps decouple object creation logic from the main business logic.
	/// - Allows easy extension by adding new notification types.
	/// </summary>
	public class NotificationModeFactory
	{
		public static INotificationMode Create(NotificationMode notificationMode)
		{
			switch (notificationMode)
			{
				case NotificationMode.EMAIL:
					return new EmailService();
				case NotificationMode.SMS:
					return new SmsService();
				case NotificationMode.WEIRD:
					return new WeirdService();
				default:
					return new EmailService();
			}
		}
	}
}
