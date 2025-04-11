## Core Principle: Interface Segregation Principle (ISP)

> ISP states: "No code should be forced to depend on methods it does not use."

It focuses on designing interfaces that are specific to client needs, avoiding "fat" interfaces that bundle unrelated methods.

## Relationship to Liskov Substitution Principle (LSP)

While distinct principles, ISP and LSP are often related in practice:

* **ISP** focuses on **Interface Design**: Encourages breaking large interfaces into smaller, more cohesive ones (role interfaces).
* **LSP** focuses on **Inheritance Behavior**: Requires that subtypes must be substitutable for their base types without altering the correctness or expected behavior of the program.



---

## Before Applying ISP (Violation Example)

This section demonstrates code that violates ISP.

### The Problem: Fat Interface (`IEntitlement`)

The `IEntitlement` interface bundles many different types of calculations. Not all employee types are eligible for all entitlements, forcing them to implement methods they don't need.

```csharp
// ISP.Before Namespace

using System;
using System.Collections.Generic;

// ** ISP VIOLATION **
// This is a "fat interface". It declares multiple methods related to different entitlements.
// Not all employee types have all these entitlements.
// Implementing classes are FORCED to provide implementations for methods they don't use/support.
interface IEntitlement
{
    decimal CalculatePension();              // Not applicable to Daylabourer, Consultant
    decimal CalculateHealthInsurance();      // Applicable to all
    decimal CalculateRentalSubsidy();        // Not applicable to Daylabourer, Consultant
    decimal CalculateBonuses();              // Not applicable to Daylabourer
    decimal CalculateTransportationReimbursement(); // Not applicable to Staff
}

// Base class for all employee types.
abstract class Employee
{
    public string EmployeeNo { get; set; }
    public string Name { get; set; }
    public decimal Salary { get; set; }
    // Calculates net salary based on specific employee type's entitlements.
    protected abstract decimal CalculateNetSalary();
    // Prints a formatted salary slip.
    public abstract string PrintSalarySlip();
}
````

### The Consequence: Forced Implementations & LSP Violations

Classes implementing the fat `IEntitlement` interface must provide implementations for all methods, even unsupported ones. This often leads to throwing `NotSupportedException`, which violates LSP because the object is no longer safely substitutable where an `IEntitlement` is expected without careful error handling by the client.



```csharp
// ISP.Before Namespace (Continued)

// Day labourers only get Health Insurance and Transportation.
// They are forced to implement methods for Pension, Rental Subsidy, and Bonuses.
class Daylabourer : Employee, IEntitlement
{
    // ** LSP VIOLATION ** - Forced implementation due to fat interface (ISP Violation).
    // Throws exception because Day labourers don't get bonuses. Breaks substitutability if called via IEntitlement.
    public decimal CalculateBonuses() => throw new NotSupportedException("Day labourer Bonuses not supported");

    public decimal CalculateHealthInsurance() => 300m; // Supported

    // ** LSP VIOLATION ** - Forced implementation due to fat interface (ISP Violation).
    public decimal CalculatePension() =>
        throw new NotSupportedException("Day labourer Pension not supported");

    // ** LSP VIOLATION ** - Forced implementation due to fat interface (ISP Violation).
    public decimal CalculateRentalSubsidy() =>
        throw new NotSupportedException("Day labourer Rental Subsidy not supported");

    public decimal CalculateTransportationReimbursement() => 150; // Supported

    protected override decimal CalculateNetSalary()
    {
        // Only uses the supported entitlements internally.
        return Salary
               + CalculateHealthInsurance()
               + CalculateTransportationReimbursement();
    }

    public override string PrintSalarySlip()
    {
        return $"\n --- {nameof(Daylabourer)} ---" +
               $"\n  No.: {EmployeeNo}" +
               $"\n  Name: {Name}" +
               $"\n  Basic Salary: {Salary:C2}" +
               $"\n  Health Insurance: {CalculateHealthInsurance():C2}" +
               $"\n  Transportation Reimbursement: {CalculateTransportationReimbursement():C2}" +
               $"\n  ----------------------------------------------" +
               $"\n  NetSalary: {CalculateNetSalary():C2}";
    }
}

// Consultants get Bonuses, Health Insurance, and Transportation.
// They are forced to implement methods for Pension and Rental Subsidy.
class Consultant : Employee, IEntitlement
{
    public decimal CalculateBonuses() => Salary * 0.05m; // Supported
    public decimal CalculateHealthInsurance() => 300m; // Supported

    // ** LSP VIOLATION ** - Forced implementation due to fat interface (ISP Violation).
    public decimal CalculatePension() =>
        throw new NotSupportedException("Consultant Pension not supported");

    // ** LSP VIOLATION ** - Forced implementation due to fat interface (ISP Violation).
    public decimal CalculateRentalSubsidy() =>
        throw new NotSupportedException("Consultant Rental Subsidy not supported");

    public decimal CalculateTransportationReimbursement() => 150; // Supported

    protected override decimal CalculateNetSalary()
    {
        // Only uses the supported entitlements internally.
        return Salary
               + CalculateBonuses()
               + CalculateHealthInsurance()
               + CalculateTransportationReimbursement();
    }

    public override string PrintSalarySlip()
    {
       return $"\n --- {nameof(Consultant)} ---" +
               $"\n  No.: {EmployeeNo}" +
               $"\n  Name: {Name}" +
               $"\n  Basic Salary: {Salary:C2}" +
               $"\n  Bonuses: {CalculateBonuses():C2}" +
               $"\n  Health Insurance: {CalculateHealthInsurance():C2}" +
               $"\n  Transportation Reimbursement: {CalculateTransportationReimbursement():C2}" +
               $"\n  ----------------------------------------------" +
               $"\n  NetSalary: {CalculateNetSalary():C2}";
    }
}

// Staff get Bonuses, Health Insurance, Pension, and Rental Subsidy.
// They are forced to implement Transportation Reimbursement.
class Staff : Employee, IEntitlement
{
    public decimal CalculateBonuses() => Salary * 0.05m; // Supported
    public decimal CalculateHealthInsurance() => 300m; // Supported
    public decimal CalculatePension() => .025m * Salary; // Supported
    public decimal CalculateRentalSubsidy() => 150; // Supported

    // ** LSP VIOLATION ** - Forced implementation due to fat interface (ISP Violation).
    public decimal CalculateTransportationReimbursement() =>
       throw new NotSupportedException("Staff TransportationReimbursement not supported");

   protected override decimal CalculateNetSalary()
    {
        // Only uses the supported entitlements internally.
        return Salary
               + CalculateBonuses()
               + CalculateHealthInsurance()
               - CalculatePension() // Pension is a deduction here
               + CalculateRentalSubsidy();
    }

    public override string PrintSalarySlip()
    {
        return $"\n --- {nameof(Staff)} ---" +
               $"\n  No.: {EmployeeNo}" +
               $"\n  Name: {Name}" +
               $"\n  Basic Salary: {Salary:C2}" +
               $"\n  Bonuses: {CalculateBonuses():C2}" +
               $"\n  Pension Deduction: {CalculatePension():C2}" +
               $"\n  Health Insurance: {CalculateHealthInsurance():C2}" +
               $"\n  Rental Subsidy: {CalculateRentalSubsidy():C2}" +
               $"\n  ----------------------------------------------" +
               $"\n  NetSalary: {CalculateNetSalary():C2}";
    }
}

// Provides sample data using the 'Before' classes.
static class Repository
{
    public static IEnumerable<Employee> LoadEmployees()
    {
        return new List<Employee>
        {
                 new Staff
                 {
                     EmployeeNo = "2017-FI-1343", Name = "Cochran Cole (Staff)", Salary = 1000
                 },
                 new Consultant
                 {
                     EmployeeNo = "2018-FI-1755", Name = "Jaclyn Wolfe (Consultant)", Salary = 1000,
                 },
                 new Daylabourer
                 {
                     EmployeeNo = "2016-IT-1441", Name = "Cochran Cole (Day Labourer)", Salary = 1000,
                 }
        };
    }
}

```

---

## After Applying ISP (Adherence Example)

This section shows the code refactored to follow ISP.

### The Solution: Segregated Interfaces

The fat `IEntitlement` interface is broken down into smaller, highly specific "role interfaces". Each interface represents a single entitlement.



```c#
// ISP.Aftre Namespace (Note: Original typo "Aftre" kept in namespace name)
namespace ISP.Aftre
{
    using System;
    using System.Collections.Generic;

    // Base class remains the same.
    abstract class Employee
    {
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        protected abstract decimal CalculateNetSalary();
        public abstract string PrintSalarySlip();
    }

    // ** ISP SOLUTION **
    // The fat IEntitlement interface is segregated into smaller, specific "role interfaces".
    // Each interface declares only one responsibility (one type of entitlement).

    // Interface for Bonus entitlement
    interface IBonusesEntitlement { decimal CalculateBonuses(); }
    // Interface for Health Insurance entitlement
    interface IHealthInsuranceEntitlement { decimal CalculateHealthInsurance(); }
    // Interface for Pension entitlement
    interface IPensionEntitlement { decimal CalculatePension(); }
    // Interface for Rental Subsidy entitlement
    interface IRentalSubsidyEntitlement { decimal CalculateRentalSubsidy(); }
    // Interface for Transportation Reimbursement entitlement
    interface ITransportationReimbursementEntitlement { decimal CalculateTransportationReimbursement(); }

```

### The Benefit: Targeted Implementation & LSP Conformance

Classes now implement _only_ the specific interfaces relevant to the entitlements they actually support. There's no need to implement unused methods or throw exceptions. This adheres to ISP and avoids the LSP violations seen previously. Clients can depend on the specific interfaces they need.



```c#
// ISP.Aftre Namespace (Continued)

    // Staff implements ONLY the entitlement interfaces relevant to them.
    // No need to implement unsupported methods or throw exceptions. Adheres to ISP and avoids LSP violations seen before.
    class Staff : Employee, IBonusesEntitlement, IHealthInsuranceEntitlement, IPensionEntitlement, IRentalSubsidyEntitlement
    {
        // Implements only relevant interfaces
        public decimal CalculateBonuses() => Salary * 0.05m;
        public decimal CalculateHealthInsurance() => 300m;
        public decimal CalculatePension() => .025m * Salary;
        public decimal CalculateRentalSubsidy() => 150;

        protected override decimal CalculateNetSalary()
        {
            return Salary
                   + CalculateBonuses()
                   + CalculateHealthInsurance()
                   - CalculatePension()
                   + CalculateRentalSubsidy();
        }

        public override string PrintSalarySlip()
        {
            // Displaying only relevant entitlements for Staff.
             return $"\n --- {nameof(Staff)} ---" +
                   $"\n  No.: {EmployeeNo}" +
                   $"\n  Name: {Name}" +
                   $"\n  Basic Salary: {Salary:C2}" +
                   $"\n  Bonuses: {CalculateBonuses():C2}" +
                   $"\n  Pension Deduction: {CalculatePension():C2}" +
                   $"\n  Health Insurance: {CalculateHealthInsurance():C2}" +
                   $"\n  Rental Subsidy: {CalculateRentalSubsidy():C2}" +
                   $"\n  ----------------------------------------------" +
                   $"\n  NetSalary: {CalculateNetSalary():C2}";
        }
    }

    // Daylabourer implements ONLY the interfaces for Health Insurance and Transportation.
    // Adheres to ISP, avoids LSP violations.
    class Daylabourer : Employee, IHealthInsuranceEntitlement, ITransportationReimbursementEntitlement
    {
        // Implements only relevant interfaces
        public decimal CalculateHealthInsurance() => 300m;
        public decimal CalculateTransportationReimbursement() => 150;

        protected override decimal CalculateNetSalary()
        {
            return Salary
                   + CalculateHealthInsurance()
                   + CalculateTransportationReimbursement();
        }

         public override string PrintSalarySlip()
        {
            // Displaying only relevant entitlements for Daylabourer.
            return $"\n --- {nameof(Daylabourer)} ---" +
                   $"\n  No.: {EmployeeNo}" +
                   $"\n  Name: {Name}" +
                   $"\n  Basic Salary: {Salary:C2}" +
                   $"\n  Health Insurance: {CalculateHealthInsurance():C2}" +
                   $"\n  Transportation Reimbursement: {CalculateTransportationReimbursement():C2}" +
                   $"\n  ----------------------------------------------" +
                   $"\n  NetSalary: {CalculateNetSalary():C2}";
        }
    }

    // Consultant implements ONLY the interfaces for Bonuses, Health Insurance, and Transportation.
    // Adheres to ISP, avoids LSP violations.
    class Consultant : Employee, IBonusesEntitlement, IHealthInsuranceEntitlement, ITransportationReimbursementEntitlement
    {
        // Implements only relevant interfaces
        public decimal CalculateBonuses() => Salary * 0.05m;
        public decimal CalculateHealthInsurance() => 300m;
        public decimal CalculateTransportationReimbursement() => 150;

        protected override decimal CalculateNetSalary()
        {
            return Salary
                   + CalculateBonuses()
                   + CalculateHealthInsurance()
                   + CalculateTransportationReimbursement();
        }

        public override string PrintSalarySlip()
        {
            // Displaying only relevant entitlements for Consultant.
             return $"\n --- {nameof(Consultant)} ---" +
                   $"\n  No.: {EmployeeNo}" +
                   $"\n  Name: {Name}" +
                   $"\n  Basic Salary: {Salary:C2}" +
                   $"\n  Bonuses: {CalculateBonuses():C2}" +
                   $"\n  Health Insurance: {CalculateHealthInsurance():C2}" +
                   $"\n  Transportation Reimbursement: {CalculateTransportationReimbursement():C2}" +
                   $"\n  ----------------------------------------------" +
                   $"\n  NetSalary: {CalculateNetSalary():C2}";
        }
    }

    // Static class assumed to exist inside ISP.Aftre for the 'After' example usage in Main
    // This provides sample data for the 'After' classes.
    static class Repository
    {
        public static IEnumerable<Employee> LoadEmployees()
        {
            return new List<Employee>
            {
                 new Staff
                 {
                     EmployeeNo = "2017-FI-1343-A", Name = "Cochran Cole (Staff)", Salary = 1000
                 },
                 new Consultant
                 {
                     EmployeeNo = "2018-FI-1755-A", Name = "Jaclyn Wolfe (Consultant)", Salary = 1000
                 },
                 new Daylabourer
                 {
                      EmployeeNo = "2016-IT-1441-A", Name = "Cochran Cole (Day Labourer)", Salary = 1000
                 }
            };
        }
    }
} // End namespace ISP.Aftre
```

---

## Usage Example (`Program.Main`)

This shows how the code might be used and highlights the potential pitfalls of the "Before" (ISP-violating) approach.



```c#
// ISP Namespace (Main Program)
namespace ISP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Demonstrating ISP Adherence (After) ---");
            // Using the 'After' implementation which follows ISP
            var employeesAfter = Aftre.Repository.LoadEmployees(); // Note: Uses the 'Aftre' namespace Repository
            foreach (var item in employeesAfter)
            {
                Console.WriteLine(item.PrintSalarySlip());
                // We could safely check for specific interfaces if needed:
                // if (item is Aftre.IPensionEntitlement p) { /* Use p.CalculatePension() */ }
            }

            Console.WriteLine("\n\n--- Demonstrating ISP Violation (Before) ---");
            var employeesBefore = Before.Repository.LoadEmployees(); // Note: Uses the 'Before' namespace Repository
            foreach (var emp in employeesBefore)
            {
                // This works okay because PrintSalarySlip only calls supported methods internally for each type.
                Console.WriteLine(emp.PrintSalarySlip());

                // However, trying to use the IEntitlement interface directly is unsafe
                // because it leads to LSP violations (NotSupportedException).
                if (emp is Before.IEntitlement entitlement)
                {
                    Console.WriteLine(<span class="math-inline">"Checking entitlements for \{emp\.Name\} \(via Before\.IEntitlement\)\.\.\."\);
try
\{
// Attempting to call methods via the fat interface reference\.
// These calls WILL FAIL for certain employee types\.
// This would throw for Daylabourer and Consultant
// decimal pension \= entitlement\.CalculatePension\(\);
// Console\.WriteLine\(</span>"  - Pension: {pension:C2}"); // <-- Would crash

                        // This would throw for Staff
                        // decimal transport = entitlement.CalculateTransportationReimbursement();
                        // Console.WriteLine(<span class="math-inline">"  \- Transport\: \{transport\:C2\}"\); // <\-\- Would crash
// This works for Staff and Consultant, but throws for Daylabourer
// decimal bonus \= entitlement\.CalculateBonuses\(\);
// Console\.WriteLine\(</span>"  - Bonus: {bonus:C2}"); // <-- Would crash for Daylabourer

                        Console.WriteLine("  (Example calls that would throw exceptions are commented out)");
                    }
                    catch (NotSupportedException ex)
                    {
                        // This catch block demonstrates the runtime failure caused by the LSP violation.
                        Console.WriteLine($"  ERROR: Could not calculate an entitlement - {ex.Message}");
                    }
                }
            }
        }
    }
}
```

---

**Conclusion:** Adhering to the Interface Segregation Principle leads to more flexible, maintainable, and robust code. By creating smaller, focused interfaces, we avoid forcing classes to implement methods they don't need, which in turn prevents common Liskov Substitution Principle violations and makes client code safer and easier to write.