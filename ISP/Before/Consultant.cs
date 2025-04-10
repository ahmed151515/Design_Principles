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
}


