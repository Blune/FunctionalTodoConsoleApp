using System;
using System.Collections.Generic;

namespace ToDoFunctionalConsoleApp
{
    partial class Program
    {
        #endregion

        public struct Todos
        {
            private IReadOnlyCollection<Todo> Items { get; }

            private Todos(IEnumerable<Todo> todos)
            {
                Items = new List<Todo>(todos);
            }

            public static Todos Create()
            {
                return new Todos(new List<Todo>());
            }

            public Todos With(Todo todo)
                => new Todos(Items.Append(todo));

            public Todos Without(Todo todo)
                => new Todos(Items.Where(item => !item.Equals(todo)));

        }
    }
}
