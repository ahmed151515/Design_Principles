


# Dependency Inversion Principle (DIP) - C# Example

## Core Principle

The Dependency Inversion Principle (DIP) is one of the SOLID principles of object-oriented design. It states:

> 1. High-level modules should not depend directly on low-level modules. Both should depend on abstractions (e.g., interfaces or abstract classes).
> 2. Abstractions should not depend on details (concrete implementations). Details (concrete implementations) should depend on abstractions.

**In simpler terms:** Don't let your important, high-level business logic code know about the specific, low-level implementation details (like *how* an email is sent). Instead, make both depend on a common contract (interface). This decouples your code, making it more flexible, testable, and maintainable.

---

## The Problem: Violation of DIP (Before)

Consider a `NotificationService` that needs to send emails and SMS messages.

**Scenario:** The `NotificationService` (high-level module) directly creates and uses `EmailService` and `SMSService` (low-level modules).

```csharp
// Namespace: DIP.Before

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
    public void Send()
    {
        Console.WriteLine($"SMS is sent to {MobileNo}");
    }
}

// Low-level module: Concrete implementation for sending Email.
internal class EmailService
{
    public string EmailAddress { get; set; }
    public void Send()
    {
        Console.WriteLine($"e-mail is sent to {EmailAddress}");
    }
}

// High-level module: Responsible for notifying customers.
// *** VIOLATION OF DIP ***
internal class NotificationService
{
    // Direct dependencies on concrete classes.
    private readonly EmailService emailService;
    private readonly SMSService smsService;

    // The constructor creates the specific service instances.
    public NotificationService(Customer customer)
    {
        // Instantiating concrete dependencies directly. VIOLATION!
        emailService = new EmailService
        {
            EmailAddress = customer.EmailAddress
        };
        smsService = new SMSService
        {
            MobileNo = customer.MobileNo
        };
    }

    // Logic is hardcoded to use the specific services.
    public void Notify()
    {
        emailService.Send(); // Directly calls concrete EmailService.
        smsService.Send();   // Directly calls concrete SMSService.
    }
}

// Data Source
internal static class Repository
{
    public static List<Customer> Customers => new List<Customer>() { /* ... customer data ... */ };
}
```

**Problems with this approach:**

1.  **Tight Coupling:** `NotificationService` is directly tied to `EmailService` and `SMSService`.
2.  **Rigidity:** If you want to add a new notification method (e.g., `MailService`), you *must* modify `NotificationService`.
3.  **Difficult Testing:** You cannot easily test `NotificationService` without also involving the real `EmailService` and `SMSService`. Mocking is hard.

---

## The Solution: Adhering to DIP (After)

We introduce an abstraction (an interface) that both high-level and low-level modules will depend on. We then use Dependency Injection (DI) to provide the concrete implementations to the high-level module.

### 1. Define an Abstraction

Create an interface that defines the contract for sending messages.

```csharp
// Namespace: DIP.After

// --- The Abstraction ---
// Defines the contract for any message sending service.
interface IMessageService
{
    void Send(); // All notification services must implement this.
}
```

### 2. Implement the Abstraction in Low-Level Modules

Make the concrete service classes implement the `IMessageService` interface.

```csharp
// Namespace: DIP.After

// Concrete implementation for sending SMS. Implements the abstraction.
internal class SMSService : IMessageService
{
    public string MobileNo { get; set; }
    public SMSService(string mobileNo) { MobileNo = mobileNo; }
    public void Send() { Console.WriteLine($"\t[SMS Service] SMS is sent to {MobileNo}"); }
}

// Concrete implementation for sending Email. Implements the abstraction.
internal class EmailService : IMessageService
{
    public string EmailAddress { get; set; }
    public EmailService(string emailAddress) { EmailAddress = emailAddress; }
    public void Send() { Console.WriteLine($"\t[Email Service] e-mail is sent to {EmailAddress}"); }
}

// Concrete implementation for sending physical Mail. Implements the abstraction.
// Easily added without changing NotificationService!
internal class MailService : IMessageService
{
    public string Address { get; set; }
    public MailService(string address) { Address = address; }
    public void Send() { Console.WriteLine($"\t[Mail Service] mail is sent to {Address}"); }
}
```

### 3. Depend on Abstraction in High-Level Modules (using Dependency Injection)

Modify the high-level module (`NotificationService`) to depend on the `IMessageService` abstraction, not the concrete classes. The actual service instances are "injected" from outside.

**Technique 1: Constructor Injection (Preferred)**

```csharp
// Namespace: DIP.After

// High-level module using Constructor Injection.
internal class NotificationService
{
    // Dependency on the abstraction (list of services).
    private readonly List<IMessageService> _services;

    // Constructor Injection: Dependencies provided upon creation.
    public NotificationService(List<IMessageService> services)
    {
        // Stores the injected dependencies (IoC).
        this._services = services;
    }

    // Logic iterates through abstract services. Doesn't know concrete types.
    public void Notify()
    {
        if (_services == null) return;
        foreach (var item in _services)
        {
            item.Send(); // Polymorphic call.
        }
    }
}
```

**Technique 2: Property Injection**

```csharp
// Namespace: DIP.After

// High-level module using Property Injection.
internal class NotificationService1
{
    // Dependency on the abstraction, settable via property/method.
    public List<IMessageService> Services { get; private set; }

    public void Notify()
    {
        if (Services == null) return;
        foreach (var item in Services)
        {
            item.Send();
        }
    }

    // Setter method used for Property Injection.
    public void SetServices(List<IMessageService> services)
    {
        Services = services;
    }
}
```

**Technique 3: Method Injection**

```csharp
// Namespace: DIP.After

// High-level module using Method Injection.
internal class NotificationService2
{
    // Dependencies passed directly to the method needing them.
    public void Notify(List<IMessageService> services) // Injected here.
    {
        if (services == null) return;
        foreach (var item in services)
        {
            item.Send();
        }
    }
}
```

### 4. Demonstrating the Usage (`Program.Main`)

The calling code now creates the concrete services and injects them into the high-level module(s).

```csharp
// Namespace: DIP

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("--- Demonstrating DIP with Dependency Injection ---");
        var c = After.Repository.Customers; // Get customer data

        foreach (var item in c)
        {
            Console.WriteLine($"\nProcessing customer: {item.Name}");

            // Create a list of desired notification services (as the abstraction type).
            List<After.IMessageService> services = new List<After.IMessageService>
            {
                new After.EmailService(item.EmailAddress),
                new After.SMSService(item.MobileNo),
                new After.MailService(item.Address) // Easily added!
            };

            // --- Using different Dependency Injection techniques ---

            // 1. Constructor Injection
            Console.WriteLine("-- Constructor Injection Example --");
            var notificationService = new After.NotificationService(services);
            notificationService.Notify();

            // 2. Property Injection
            Console.WriteLine("\n-- Property Injection Example --");
            var notificationService1 = new After.NotificationService1();
            notificationService1.SetServices(services); // Inject via method
            notificationService1.Notify();

            // 3. Method Injection
            Console.WriteLine("\n-- Method Injection Example --");
            var notificationService2 = new After.NotificationService2();
            notificationService2.Notify(services); // Inject via parameter
        }
    }
}

// Customer and Repository classes remain similar in DIP.After
// namespace DIP.After {
//     internal class Customer { /* ... properties ... */ }
//     internal static class Repository { /* ... static Customer list ... */ }
// }

```

---

## Benefits of Applying DIP

*   **Decoupling:** High-level modules don't depend on low-level implementation details.
*   **Flexibility:** Easy to add new low-level implementations (like `MailService`) without changing high-level modules. Easy to swap implementations.
*   **Testability:** High-level modules can be easily tested by injecting mock implementations of the abstractions (`IMessageService`).
*   **Maintainability:** Changes in low-level details are less likely to break high-level logic.

