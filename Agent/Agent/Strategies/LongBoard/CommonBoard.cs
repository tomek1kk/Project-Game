namespace Agent.Strategies.LongBoard
{
    internal class CommonBoard
    {
        /// <summary>
        /// y-bounds in which agent will move. Both inclusive.
        /// </summary>
        public (int Min, int Max) MyBounds { get; set; }
        /// <summary>
        /// Goalie is an agent which operates only in goal area and puts pieces there to score points
        /// </summary>
        public bool AmGoalie { get; set; } = false;
        public bool MoveError { get; set; } = false;
    }
}