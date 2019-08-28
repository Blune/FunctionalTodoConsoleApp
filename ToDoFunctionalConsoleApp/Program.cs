using System;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;

namespace ToDoFunctionalConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Todo> todos = new List<Todo>();

            Console.WriteLine("What do you want to do? Choose between: Add, Print, PrintAll");

            while (true)
            {
                var input = ReadInput();
                var lowerInput = input.ToLower();
                if (lowerInput.StartsWith("add")) AddTodo(todos, input.Substring(3));
                else if (lowerInput == "print") PrintTodos(todos.Take(3));
                else if (lowerInput == "printall") PrintTodos(todos);
                else PrintMessage($"Unknown command: {input}");
            }
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

        private static string ReadInput() 
            => Console.ReadLine();

        private static void PrintMessage(string message) 
            => Console.WriteLine(message);

        private static void PrintErrors(IEnumerable<Error> errors) 
            => Console.WriteLine(string.Join(", ", errors.Select(e => e.Message)));
    }
    
    public class Position
    {
        internal int PositionNumber { get; }

        public static implicit operator int(Position p) => p.PositionNumber;

        public override string ToString() => PositionNumber.ToString();

        private Position(int position)
        {
            PositionNumber = position;
        }

        public static Validation<Position> Create(int position)
        {
            if (position < 0) return F.Invalid("Position is not allowed to be negative");
            if (position == 0) return F.Invalid("Counting starts at 1");
            if (position > 9) return F.Invalid("More than 9 todos are not allowed");
            return F.Valid(new Position(position));
        }
    }

    public class Title
    {
        public string TitleMessage { get; }
        private Title(string message)
        {
            TitleMessage = message;
        }

        public override string ToString() => TitleMessage;

        public static Validation<Title> Create(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return F.Invalid("An empty TODO title is not allowed");
            return F.Valid(new Title(message.Trim()));
        }
    }

    public class Todo
    {
        public Position Position { get; }
        public Title Title { get; }

        private Todo(Position position, Title title)
        {
            Position = position;
            Title = title;
        }

        public static Func<Position, Title, Todo> Create = (position, title) => new Todo(position, title);
    }
}
