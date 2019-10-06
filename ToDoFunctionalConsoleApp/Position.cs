using LaYumba.Functional;

namespace ToDoFunctionalConsoleApp
{
    public struct Position
    {
        internal int PositionNumber { get; }

        public static implicit operator int(Position p) => p.PositionNumber;

        public override string ToString() => PositionNumber.ToString();

        private Position(int position)
        {
            PositionNumber = position;
        }

        public static Validation<Position> Create(string positionString)
        {
            return int.TryParse(positionString, out int position) 
                ? Create(position) 
                : F.Invalid($"{positionString} is not a number");
        }

        public static Validation<Position> Create(int position)
        {
            if (position < 0) return F.Invalid("Position is not allowed to be negative");
            if (position == 0) return F.Invalid("Counting starts at 1");
            if (position > 9) return F.Invalid("More than 9 todos are not allowed");
            return F.Valid(new Position(position));
        }
    }
}
