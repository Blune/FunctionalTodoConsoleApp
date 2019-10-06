using System;

namespace ToDoFunctionalConsoleApp
{
    public struct Todo
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
