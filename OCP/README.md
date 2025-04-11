

> "Software entities (classes, modules, functions, etc.) should be **open for extension**, but **closed for modification**."

This means you should be able to add new functionality (extend the behavior) without changing the existing, working source code (modification).

## Purpose of This Example

This note demonstrates the OCP using a Quiz application scenario. It contrasts a design that violates OCP (`Before` example) with one that adheres to it (`After` example) using abstraction and polymorphism.

## Why OCP is Useful

1.  **Reduced Risk of Bugs**: Modifying existing, tested code can introduce new bugs into previously stable functionality. OCP minimizes this risk by encouraging adding new code rather than changing old code.
2.  **Improved Maintainability & Readability**: Code becomes easier to manage as changes for new features are often isolated in new classes/modules, making the system less complex over time.
3.  **Enhanced Extensibility**: Adding new features (like new question types) becomes straightforward without requiring widespread changes across the application's core logic. The system design naturally accommodates growth.

---

## Example 1: Before OCP (Violation)

This section shows a design that **violates** the Open/Closed Principle.

### The Violation

The primary violation occurs in the `Before.Quiz.Print()` method. It uses a `switch` statement based on the `QuestionType` enum.

*   **Problem:** Every time a new type of question needs to be added (e.g., a "Matching" question):
    1.  The `QuestionType` enum must be modified.
    2.  The `Quiz.Print()` method **must be modified** by adding a new `case` to the `switch` statement.

*   **Consequence:** The `Quiz` class is **not closed for modification**. Changes required for extending functionality (adding question types) force modifications to existing, potentially stable code, increasing the risk of introducing bugs.

### Code (`Before` Namespace)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

// ============================================================================
// BEFORE: Violation of Open/Closed Principle
// ============================================================================
namespace Before
{
    // --- OCP Violation Example ---
    // This namespace demonstrates a design that violates the Open/Closed Principle.
    // Specifically, the `Quiz.Print` method needs to be modified (adding a new
    // case to the switch statement) every time a new `QuestionType` is introduced.
    // This makes the `Quiz` class *not closed* for modification when extending
    // the system with new question types.

    // Enum determines question type. Adding a new type requires modifying the switch below.
	public enum QuestionType
	{
		WH,          // "WH" questions (What, Why, How, etc.)
		TrueFalse,   // True/False questions (Corrected typo)
		MCQ          // Multiple Choice Question
        // If we add Matching here, Quiz.Print() *must* change.
	}

    // Single class holding all data, type determined by enum.
	public class Question
	{
		public string Title { get; set; }
		public int Mark { get; set; }
		public QuestionType QuestionType { get; set; }
		public List<string> Choices { get; set; } = new List<string>(); // Only used by MCQ
	}

	public class Quiz
	{
		public List<Question> Questions { get; }

		public Quiz(List<Question> questions)
		{
			Questions = questions;
		}

		// =============================================
		// OCP VIOLATION POINT: This switch statement
		// must be modified to add new question types.
		// The Quiz class is NOT closed for modification.
		// =============================================
		public void Print()
		{
			foreach (var question in Questions)
			{
				Console.WriteLine($"({question.Title}) [{question.Mark}]");
				switch (question.QuestionType)
				{
					case QuestionType.WH:
						Console.WriteLine("Answer: ____________________");
						Console.WriteLine("        ____________________");
						Console.WriteLine("        ____________________");
						break;
					case QuestionType.TrueFalse:
						Console.WriteLine(" () True");
						Console.WriteLine(" () False");
						break;
					case QuestionType.MCQ:
						foreach (var c in question.Choices)
						{
							Console.WriteLine($" () {c}");
						}
						break;
                    // Adding a new QuestionType requires adding a new 'case' HERE.
					default:
                        Console.WriteLine("Error: Unknown question type.");
                        break;
				}
				Console.WriteLine(); // Blank line between questions
			}
		}
	}

	// Generates questions using the single Question class and enum.
	public static class QuestionBank
	{
		public static List<Question> Generate()
		{
			return new List<Question>
			{
				new Question { Title = "What are the four pillars of OOP?", QuestionType = QuestionType.WH, Mark = 8 },
				new Question { Title = "Which of the following are value types?", QuestionType = QuestionType.MCQ, Mark = 6, Choices = new List<string> { "A: Integer", "B: Array", "C: Single", "D: String", "E: Long" } },
			    new Question { Title = "Earth is Bigger than sun?", QuestionType = QuestionType.TrueFalse, Mark = 4 },
				new Question { Title = "Which of the following is an 8-byte Integer?", QuestionType = QuestionType.MCQ, Mark = 6, Choices = new List<string> { "A.  Char", "B.  Long", "C.  Short", "D.  Byte", "E.  Integer" } }
			};
		}
	}
}
```

---

## Example 2: After OCP (Adherence)

This section demonstrates a design that **adheres** to the Open/Closed Principle using abstraction and polymorphism.

### How it Adheres to OCP

1.  **Abstraction:** An `abstract class Question` defines a common interface (`Title`, `Mark`, and an `abstract void Print()`).
2.  **Extension:** New question types are added by creating new classes (`WHQuestion`, `TrueFalseQuestion`, `MCQQuestion`, `MatchQuestion`, etc.) that inherit from `Question` and provide their own specific implementation of the `Print()` method.
3.  **Polymorphism:** The `After.Quiz.Print()` method iterates over a `List<Question>`. For each `question` object in the list, `question.Print()` is called. The runtime determines *which specific* `Print()` implementation to execute based on the actual object's type (e.g., `WHQuestion.Print()`, `MCQQuestion.Print()`).
4.  **Closed for Modification:** The crucial point is that the `Quiz.Print()` method **never needs to be changed** when a new `Question` subclass is added. The `Quiz` class is *closed* for modification regarding how questions are printed. It's *open* for extension because you can add new `Question` subclasses freely.

### How OCP is Achieved in the "After" Example (Summary)

*   An **abstract `Question` base class** defines a common contract (`Print` method).
*   **Concrete question types** inherit from `Question` and provide their specific `Print` implementation.
*   The `Quiz.Print` method iterates and calls `Print()` on each object. **Polymorphism** ensures the correct implementation runs.
*   **Adding new question types** only requires creating a new subclass and updating the generator (`QuestionBank`). The `Quiz` class itself remains untouched.

### Code (`After` Namespace)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

// ============================================================================
// AFTER: Adherence to Open/Closed Principle
// ============================================================================
namespace After
{
    // --- OCP Adherence Example ---
    // This namespace demonstrates a design that adheres to the Open/Closed Principle.
    // It uses abstraction (abstract Question class) and polymorphism.
    // New question types can be added by creating new classes that inherit
    // from `Question` and implement the abstract `Print` method.
    // The `Quiz.Print` method does *not* need to be modified, thus adhering to OCP.
    // The `Quiz` class is *open for extension* (by adding new Question subclasses)
    // but *closed for modification*.

	/// <summary>
    /// Abstract base class for all question types.
    /// Defines the common properties and requires derived classes
    /// to implement their own way of printing.
    /// </summary>
	public abstract class Question
	{
		public string Title { get; set; }
		public int Mark { get; set; }

        /// <summary>
        /// Abstract method to print the question in its specific format.
        /// Each derived class must provide its own implementation.
        /// </summary>
		public abstract void Print();
	}

	/// <summary>
    /// Represents a "WH" type question (What, Why, How...).
    /// </summary>
	class WHQuestion : Question
	{
		public override void Print()
		{
			Console.WriteLine($"({Title}) [{Mark}]");
			Console.WriteLine("Answer: ____________________");
			Console.WriteLine("        ____________________");
			Console.WriteLine("        ____________________");
		}
	}

	/// <summary>
    /// Represents a True/False question.
    /// </summary>
	class TrueFalseQuestion : Question // Corrected typo
	{
		public override void Print()
		{
			Console.WriteLine($"({Title}) [{Mark}]");
			Console.WriteLine(" () True");
			Console.WriteLine(" () False");
		}
	}

	/// <summary>
    /// Represents a Multiple Choice Question (MCQ).
    /// </summary>
	class MCQQuestion : Question // Renamed from MSQQuestion for consistency
	{
		public List<string> Choices { get; set; } = new List<string>();

		public override void Print()
		{
			Console.WriteLine($"({Title}) [{Mark}]");
			foreach (var c in Choices)
			{
				Console.WriteLine($" () {c}");
			}
		}
	}

	/// <summary>
    /// Represents a Matching question type (NEW type added).
    /// Demonstrates extensibility without modifying the Quiz class.
    /// </summary>
	class MatchQuestion : Question
	{
		public Dictionary<string, string> Rows { get; set; } = new Dictionary<string, string>();

		public override void Print()
        {
            Console.WriteLine($"({Title}) [{Mark}] - Match the items:");
            // Find the longest key for formatting
            int padWidth = Rows.Keys.Any() ? Rows.Keys.Max(k => k.Length) + 4 : 40;

            // Create a list of values to shuffle for the right column
            var rightColumnItems = Rows.Values.ToList();
            var random = new Random();
            var shuffledValues = rightColumnItems.OrderBy(x => random.Next()).ToList();

            Console.WriteLine($"{"Column A".PadRight(padWidth)} Column B");
            Console.WriteLine($"{new string('-', padWidth-1)}  {new string('-', Math.Max(10, Rows.Values.Max(v=>v.Length)))}"); // Adjusted padding

            for (int i = 0; i < Rows.Count; i++)
            {
                var key = Rows.Keys.ElementAt(i);
                // Ensure shuffledValues has enough items, prevent index out of bounds
                var shuffledValue = (i < shuffledValues.Count) ? shuffledValues[i] : "???";
                Console.WriteLine($"{i + 1}. {key.PadRight(padWidth - 3)} {((char)('A' + i))}. {shuffledValue}");
            }
        }
	}

	/// <summary>
    /// Represents the Quiz, containing a list of various Question objects.
    /// </summary>
	public class Quiz
	{
		public List<Question> Questions { get; }

		public Quiz(List<Question> questions)
		{
			Questions = questions;
		}

		/// <summary>
        /// Prints all questions in the quiz.
        /// OCP Compliant: This method does NOT need modification when new
        /// Question subclasses (like MatchQuestion) are added. It relies on
        /// polymorphism to call the correct Print() method for each object.
        /// The Quiz class IS closed for modification here.
        /// </summary>
		public void Print()
		{
            int questionNumber = 1;
			foreach (var question in Questions)
			{
                Console.WriteLine($"Question {questionNumber++}:");
				question.Print(); // Polymorphism determines which Print() is called
                Console.WriteLine(); // Blank line between questions
			}
		}
	}

	/// <summary>
    /// Generates a sample list of diverse Question objects.
    /// </summary>
	public static class QuestionBank
	{
		public static List<Question> Generate()
		{
			return new List<Question>
            {
                new WHQuestion { Title = "What are the four pillars of OOP?", Mark = 8 },
                new MCQQuestion { Title = "Which of the following are value types?", Mark = 6, Choices = new List<string> { "A: Integer", "B: Array", "C: Single", "D: String", "E: Long" } },
                new TrueFalseQuestion { Title = "Earth is Bigger than sun?", Mark = 4 },
                new MCQQuestion { Title = "Which of the following is an 8-byte Integer?", Mark = 6, Choices = new List<string> { "A.  Char", "B.  Long", "C.  Short", "D.  Byte", "E.  Integer" } },
                // New Question Type Added Easily:
                new MatchQuestion { Title="Match the programming language construct to the language", Mark = 5, Rows=new Dictionary<string, string>{ { "Console.WriteLine()","C#" }, { "System.out.println()","Java" }, { "cout <<","C++" }, { "print()", "Python"} } }
            };
		}
	}
}
```

---

## Conceptual Usage

```csharp
// In Program.Main or similar entry point:

// --- BEFORE ---
// If we add a new QuestionType enum value,
// Before.Quiz.Print() needs modification.
// Before.Quiz quizBefore = new Before.Quiz(Before.QuestionBank.Generate());
// quizBefore.Print();

// --- AFTER ---
// Adding a new Question subclass (like MatchQuestion)
// requires NO changes to After.Quiz.Print().
After.Quiz quizAfter = new After.Quiz(After.QuestionBank.Generate());
quizAfter.Print(); // This works without changing After.Quiz
```
