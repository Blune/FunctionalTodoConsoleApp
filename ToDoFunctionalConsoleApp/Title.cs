using LaYumba.Functional;

namespace ToDoFunctionalConsoleApp
{
    public struct Title
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
}
