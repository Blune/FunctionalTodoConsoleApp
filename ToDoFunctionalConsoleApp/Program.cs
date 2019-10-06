using System;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;

namespace ToDoFunctionalConsoleApp
{
    partial class Program
    {
        static void Main()
        {
            List<Todo> todos = new List<Todo>();

            Console.WriteLine("What do you want to do? Choose between: Add, Print, PrintAll, Done");

            while (true)
            {
                var input = ReadInput();
                var lowerInput = input.ToLower();
                if (lowerInput.StartsWith("add")) AddTodo(todos, input.Substring(3));
                else if (lowerInput.StartsWith("done")) todos = MarkTodoAsDone(todos, input.Substring(4).Trim());
                else if (lowerInput == "print") PrintTodos(todos.Take(3));
                else if (lowerInput == "printall") PrintTodos(todos);
                else PrintMessage($"Unknown command: {input}");
            }
        }

        private static List<Todo> CreateTodosWithout(Position position, List<Todo> todos)
            => todos.Where(todo => todo.Position != position).ToList();

        private static List<Todo> MarkTodoAsDone(List<Todo> todos, string positionString)
        {
            return Position.Create(positionString)
                .Match(
                    errors => PrintErrorAndReturnList(errors, todos),
                    todo => CreateTodosWithout(todo, todos)
                );
        }

        private static List<Todo> PrintErrorAndReturnList(IEnumerable<Error> erros, List<Todo> todos)
        {
            PrintErrors(erros);
            return todos;
        }

        private static void PrintTodos(IEnumerable<Todo> todos)
            => PrintMessage(string.Join(Environment.NewLine, todos.Select(x => $"{x.Position}: {x.Title}")));

        static Validation<Todo> CreateTodo(int position, string title) => F.Valid(Todo.Create)
            .Apply(Position.Create(position))
            .Apply(Title.Create(title));

        private static void AddTodo(List<Todo> todos, string message)
        {
            CreateTodo(todos.Count + 1, message)
                .Match(PrintErrors, todos.Add);
        }

        #region console

        private static string ReadInput()
            => Console.ReadLine();

        private static void PrintMessage(string message)
            => Console.WriteLine(message);

        private static void PrintErrors(IEnumerable<Error> errors)
            => Console.WriteLine(string.Join(", ", errors.Select(e => e.Message)));

        #endregion
    }
}
