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
namespace ISP
{
	internal class Program
	{
		static void Main(string[] args)
		{

			var e = Before.Repository.LoadEmployees();
			foreach (var item in e)
			{
				Console.WriteLine(item.PrintSalarySlip());

			}
		}
	}

