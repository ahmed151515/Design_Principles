// Interface Segregation Principle (ISP) Demonstration
// ==================================================
// ISP states: "No code should be forced to depend on methods it does not use."
// It focuses on designing interfaces that are specific to client needs, avoiding "fat" interfaces.
//
// Relationship to Liskov Substitution Principle (LSP):
// While distinct, ISP and LSP are related.
// - ISP focuses on INTERFACE DESIGN: Breaking large interfaces into smaller, specific ones.
// - LSP focuses on INHERITANCE BEHAVIOR: Ensuring subtypes are substitutable for base types without breaking functionality.
//

//






// =========================================================================
// Example BEFORE applying the Interface Segregation Principle
// =========================================================================
namespace ISP.Before
{
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
}

