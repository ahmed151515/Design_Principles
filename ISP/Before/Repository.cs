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
	// Provides sample data using the 'Before' classes.
	static class Repository
	{
		public static IEnumerable<Employee> LoadEmployees()
		{
			return new List<Employee>
			{
					 new Staff
					 {
						 EmployeeNo = "2017-FI-1343",
						 Name = "Cochran Cole (Staff)",
						 Salary = 1000
					 },
					 new Consultant
					 {
						 EmployeeNo = "2018-FI-1755",
						 Name = "Jaclyn Wolfe (Consultant)",
						 Salary = 1000,
					 },
					 new Daylabourer
					 {
						 EmployeeNo = "2016-IT-1441",
						 Name = "Cochran Cole (Day Labourer)",
						 Salary = 1000,
					 }
			};
		}
	}
}

