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
}

