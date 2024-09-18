
# Coding Tracker

This is a console based CRUD app to track coding time, using SQLite.

## Requirements

- To show the data on the console, you should use the "Spectre.Console" library.
- You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- You'll need to create a configuration file that you'll contain your database path and connection strings.
- You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
- The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- The user should be able to input the start and end times manually.
- You need to use Dapper ORM for the data access instead of ADO.NET. (This requirement was included in Feb/2024)
- When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.

# Additional requirements
- Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
- Create reports where the users can see their total and average coding session per period.
- Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.




## Features

- SQLite database connection. The program uses a SQLite db connection to store and read information. If no database exists, one will be created, along with a table.
- A console based UI where users can navigate by key presses, customized with the Spectre.Console library
- Input validation and exception handling
- Customizable report creation

# Challenges

- Trying to utilize classes functionalities properly
- Separating code to be as legible as possible


## Lessons Learned

- Basic LINQ
- Collection expressions
- Lambda expressions
- Utilizing external libraries (Spectre.Console)

# Areas to improve

- LINQ
- Lambda expressions
- Clean code
- OOP

