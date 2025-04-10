namespace OCP
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// berfor
			//Before.Quiz quiz = new Before.Quiz(Befor.QuestionBank.Generate());
			// after
			After.Quiz quiz = new After.Quiz(After.QuestionBank.Generate());
			quiz.Print();
		}
	}
}
/*
 * ============================================================================
 * Open/Closed Principle (OCP) Demonstration
 * ============================================================================
 *
 * Principle Definition:
 * "Software entities (classes, modules, functions, etc.) should be open for
 *  extension, but closed for modification."
 * This means you should be able to add new functionality (extend the behavior)
 * without changing the existing, working source code (modification).
 *
 * Purpose of this Example:
 * This code demonstrates the OCP using a Quiz application scenario.
 * It contrasts a design that violates OCP (in the `Before` namespace) with one
 * that adheres to it (in the `After` namespace) using abstraction and polymorphism.
 *
 * Why OCP is Useful:
 * 1. Reduced Risk of Bugs: Modifying existing, tested code can introduce
 *    new bugs into previously stable functionality. OCP minimizes this risk
 *    by encouraging adding new code rather than changing old code.
 * 2. Improved Maintainability & Readability: Code becomes easier to manage
 *    as changes for new features are often isolated in new classes/modules,
 *    making the system less complex over time.
 * 3. Enhanced Extensibility: Adding new features (like new question types
 *    in this example) becomes straightforward without requiring widespread
 *    changes across the application's core logic. The system design naturally
 *    accommodates growth.
 *
 * How it's Achieved in the "After" Example:
 * - An abstract `Question` base class defines a common contract (`Print` method).
 * - Concrete question types (`WHQuestion`, `TrueFalseQuestion`, `MCQQuestion`, `MatchQuestion`)
 *   inherit from `Question` and provide their specific `Print` implementation.
 * - The `Quiz.Print` method iterates through a list of `Question` objects and calls
 *   the `Print` method on each. Thanks to polymorphism, the correct implementation
 *   is executed for each question type.
 * - To add a new question type (e.g., "FillInTheBlankQuestion"), we only need to:
 *     a) Create a new class `FillInTheBlankQuestion` inheriting from `Question`.
 *     b) Implement its specific `Print` method.
 *     c) Update the `QuestionBank` (or wherever questions are generated).
 *   Crucially, the `After.Quiz` class does *not* need to be modified at all.
 */


namespace Before
{
	public enum QuestionType
	{
		WH,
		TrueFasle,
		MCQ
	}

	public class Question
	{
		public string Title { get; set; }
		public int Mark { get; set; }
		public QuestionType QuestionType { get; set; }
		public List<string> Choices { get; set; } = new List<string>();
	}

	public class Quiz
	{
		public List<Question> Questions { get; }

		public Quiz(List<Question> questions)
		{
			Questions = questions;
		}

		public void Print()
		{
			foreach (var question in Questions)
			{
				Console.WriteLine($"({question.Title}) [{question.Mark}]");
				switch (question.QuestionType)
				{
					case QuestionType.WH:
						Console.WriteLine("---------------------");
						Console.WriteLine("---------------------");
						Console.WriteLine("---------------------");
						break;
					case QuestionType.TrueFasle:
						Console.WriteLine("True");
						Console.WriteLine("False");
						break;
					case QuestionType.MCQ:
						foreach (var c in question.Choices)
						{
							Console.WriteLine(c);
						}
						break;
				}
				Console.WriteLine();
			}
		}
	}

	public static class QuestionBank
	{
		public static List<Question> Generate()
		{
			return new List<Question>
			{
				new Question
				{
					Title = "What are the four pillars of OOP?",
					QuestionType = QuestionType.WH,
					Mark = 8
				},
				new Question
				{
					Title = "Which of the following are value types?",
					QuestionType = QuestionType.MCQ,
					Mark = 6,
					Choices = new List<string>
					{
						"A: Integer",
						"B: Array",
						"C: Single",
						"D: String",
						"E: Long",
					}
				},
			   new Question
				{
					Title = "Earth is Bigger than sun?",
					QuestionType = QuestionType.TrueFasle,
					Mark = 4
				},
				new Question
				{
					Title = "Which of the following is an 8-byte Integer?",
					QuestionType = QuestionType.MCQ,
					Mark = 6,
					Choices = new List<string>
					{
					   "A.  Char",
					   "B.  Long",
					   "C.  Short",
					   "D.  Byte",
					   "E.  Integer"
					}
				}

			};
		}
	}
}
namespace After
{
	public abstract class Question
	{
		public string Title { get; set; }
		public int Mark { get; set; }

		public abstract void Print();

	}
	class WHQuestion : Question
	{
		public override void Print()
		{
			Console.WriteLine($"({Title}) [{Mark}]");
			Console.WriteLine("---------------------");
			Console.WriteLine("---------------------");
			Console.WriteLine("---------------------");

		}
	}
	class TrueFalseQuestion : Question
	{
		public override void Print()
		{
			Console.WriteLine($"({Title}) [{Mark}]");
			Console.WriteLine("True");
			Console.WriteLine("False");


		}
	}
	class MSQQuestion : Question
	{
		public List<string> Choices { get; set; }



		public override void Print()
		{
			Console.WriteLine($"({Title}) [{Mark}]");
			foreach (var c in Choices)
			{
				Console.WriteLine(c);
			}

		}
	}
	class MatchQuestion : Question
	{
		public Dictionary<string, string> Rows { get; set; }



		public override void Print()
		{
			Console.WriteLine($"({Title}) [{Mark}]");
			foreach (var r in Rows)
			{
				Console.WriteLine($"{r.Key.PadRight(40)}{r.Value}");
			}

		}
	}
	public class Quiz
	{
		public List<Question> Questions { get; }

		public Quiz(List<Question> questions)
		{
			Questions = questions;
		}

		public void Print()
		{
			foreach (var question in Questions)
			{
				question.Print();
			}
		}
	}

	public static class QuestionBank
	{
		public static List<Question> Generate()
		{
			return new List<Question>
			{
				new WHQuestion
				{
					Title = "What are the four pillars of OOP?",

				},
				new MSQQuestion
				{
					Title = "Which of the following are value types?",

					Mark = 6,
					Choices = new List<string>
					{
						"A: Integer",
						"B: Array",
						"C: Single",
						"D: String",
						"E: Long",
					}
				},
			   new TrueFalseQuestion
				{
					Title = "Earth is Bigger than sun?",

					Mark = 4
				},
				new MSQQuestion
				{
					Title = "Which of the following is an 8-byte Integer?",

					Mark = 6,
					Choices = new List<string>
					{
					   "A.  Char",
					   "B.  Long",
					   "C.  Short",
					   "D.  Byte",
					   "E.  Integer"
					}
				},
				new MatchQuestion
				{
					Title="print of language",
					Mark = 5,
					Rows=new Dictionary<string, string>{
						{ "print","C#" },
						{ "Console.WriteLine","java" },
						{ "cout","C++" }
					}
				}

			};
		}
	}
}
