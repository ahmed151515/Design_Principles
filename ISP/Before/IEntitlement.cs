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
}


