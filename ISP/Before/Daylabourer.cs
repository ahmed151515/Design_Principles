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
}
