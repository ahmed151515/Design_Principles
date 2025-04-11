# Design Principles Learning Notes

## Introduction

Welcome! This repository contains my personal notes and code examples as I learn about fundamental Software Design Principles. The goal is to build a better understanding of how to write maintainable, flexible, and scalable software.

These notes are primarily based on the following YouTube course:
[Design Principles | مبادئ التصميم](https://www.youtube.com/playlist?list=PL4n1Qos4Tb6ThSyydEJTm7xJ3qEwE8Oyu)


## Covered Principles

Here's a breakdown of the principles covered in this repository, with links to the specific notes/examples:

*   **[Encapsulate What Varies](https://github.com/ahmed151515/Design_Principles/tree/main/Encapsulate_What_Varies)**
    *   *Preview:* Focuses on identifying aspects of your application that are likely to change and separating them from the parts that remain stable, reducing the impact of future modifications.

*   **[Composition vs. Inheritance](https://github.com/ahmed151515/Design_Principles/tree/main/_2_CompositionAndInheritance)**
    *   *Preview:* Explores the trade-offs between establishing 'is-a' relationships (Inheritance) and 'has-a' relationships (Composition), often advocating for composition to achieve greater flexibility and avoid rigid class hierarchies.

*   **[Loose Coupling vs. Tight Coupling](https://github.com/ahmed151515/Design_Principles/tree/main/_3_TightlyVsLooselyCoupled)**
    *   *Preview:* Discusses the degree of dependency between software components. Loose coupling is preferred as it means changes in one component have minimal impact on others, enhancing modularity and maintainability.

*   **[Single Responsibility Principle (SRP)](https://github.com/ahmed151515/Design_Principles/tree/main/SPR)**
    *   *Preview:* States that a class or module should have only one reason to change, meaning it should have a single, well-defined responsibility.

*   **[Open/Closed Principle (OCP)](https://github.com/ahmed151515/Design_Principles/tree/main/OCP)**
    *   *Preview:* Suggests that software entities (classes, modules, functions, etc.) should be open for extension but closed for modification. This allows adding new functionality without altering existing, tested code.

*   **[Liskov Substitution Principle (LSP)](https://github.com/ahmed151515/Design_Principles/tree/main/LSP)**
    *   *Preview:* Asserts that objects of a superclass should be replaceable with objects of its subclasses without affecting the correctness of the program. Ensures that inheritance hierarchies are sound.

*   **[Interface Segregation Principle (ISP)](https://github.com/ahmed151515/Design_Principles/tree/main/ISP)**
    *   *Preview:* Advises that clients should not be forced to depend on interfaces they do not use. It promotes creating smaller, more specific interfaces rather than large, monolithic ones.

*   **[Dependency Inversion Principle (DIP)](https://github.com/ahmed151515/Design_Principles/tree/main/DIP)**
    *   *Preview:* Recommends that high-level modules should not depend on low-level modules; both should depend on abstractions (e.g., interfaces). It also states that abstractions should not depend on details; details should depend on abstractions. This facilitates loose coupling.

*   **[KISS & YAGNI](https://github.com/ahmed151515/Design_Principles/tree/main/KISS_And_YAGNI)**
    *   *Preview:* Encompasses two related ideas: 'Keep It Simple, Stupid' (KISS), which advocates for simplicity in design, and 'You Ain't Gonna Need It' (YAGNI), which advises against adding functionality until it is actually required.
