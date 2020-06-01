using Agent.Board;
using CommunicationLibrary.Information;
using CommunicationLibrary.Response;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;

namespace Agent.Strategies.LongBoard
{
    public class CommonBoard
    {
        /// <summary>
        /// y-bounds in which agent will move. Both inclusive.
        /// </summary>
        public (int Min, int Max) MyBounds { get; set; }
        public int MyAreaSize => MyBounds.Max - MyBounds.Min + 1;
        /// <summary>
        /// Goalie is an agent which operates only in goal area and puts pieces there to score points
        /// </summary>
        public AgentType Type { get; }
        public bool MoveError { get; set; } = false;
        public (int? nearGoal, int? nearFront) neighborIds;
        public PosField[] FieldsToPlaceOn { get; set; }
        public PosField[] FieldsToTakeFrom { get; set; }
        public PosField[,] MySubareaFields { get; set; }

        public Team Team { get; }

        public CommonBoard(GameStarted gameInfo, AgentType type)
        {
            Type = type;
            FieldsToPlaceOn = new PosField[gameInfo.BoardSize.X.Value];
            FieldsToTakeFrom = new PosField[gameInfo.BoardSize.X.Value];
            Team = gameInfo.TeamId == "red" ? Team.Red: Team.Blue;
        }

        private bool WithinXBounds(Point position) =>
            0 <= position.X && position.X < MySubareaFields.GetLength(0);

        public bool IsInMyArea(Point position)
        {
            return MyBounds.Min <= position.Y && position.Y <= MyBounds.Max && WithinXBounds(position);
        }

        public bool IsFieldToPlaceOn(Point position)
        {
            return position.Y == (Team == Team.Blue ? MyBounds.Min - 1 : MyBounds.Max + 1)
                && WithinXBounds(position);
        }

        public bool IsFieldToTakeFrom(Point position)
        {
            return position.Y == (Team == Team.Red ? MyBounds.Min : MyBounds.Max)
                && WithinXBounds(position);
        }

        public PosField GetFieldAt(Point position)
        {
            if (IsInMyArea(position))
            {
                return MySubareaFields[position.X, position.Y - MyBounds.Min];
            }
            if (IsFieldToPlaceOn(position))
                return FieldsToPlaceOn[position.X];
            return null;
        }

        public void UpdateDistances(DiscoveryResponse discoveryResponse, Point position)
        {
            bool IsAssignable(int x, int y) =>
                IsInMyArea(new Point(x, y)) || IsFieldToTakeFrom(new Point(x, y));
            if(IsAssignable(position.X, position.Y))
                GetFieldAt(position).DistToPiece = discoveryResponse.DistanceFromCurrent;
            if (IsAssignable(position.X - 1, position.Y + 1))
                GetFieldAt(new Point(position.X - 1, position.Y + 1)).DistToPiece = discoveryResponse.DistanceNW;
            if (IsAssignable(position.X, position.Y + 1))
                GetFieldAt(new Point(position.X, position.Y + 1)).DistToPiece = discoveryResponse.DistanceN;
            if (IsAssignable(position.X + 1, position.Y + 1))
                GetFieldAt(new Point(position.X + 1, position.Y + 1)).DistToPiece = discoveryResponse.DistanceNE;
            if (IsAssignable(position.X - 1, position.Y))
                GetFieldAt(new Point(position.X - 1, position.Y)).DistToPiece = discoveryResponse.DistanceW;
            if (IsAssignable(position.X + 1, position.Y))
                GetFieldAt(new Point(position.X + 1, position.Y)).DistToPiece = discoveryResponse.DistanceE;
            if (IsAssignable(position.X - 1, position.Y - 1))
                GetFieldAt(new Point(position.X - 1, position.Y - 1)).DistToPiece = discoveryResponse.DistanceSW;
            if (IsAssignable(position.X, position.Y - 1))
                GetFieldAt(new Point(position.X, position.Y - 1)).DistToPiece = discoveryResponse.DistanceS;
            if (IsAssignable(position.X + 1, position.Y - 1))
                GetFieldAt(new Point(position.X + 1, position.Y - 1)).DistToPiece = discoveryResponse.DistanceSE;
        }
        public void UpdateDistance(MoveResponse moveResponse, Point position)
        {
            if (GetFieldAt(position) is null) return;
            GetFieldAt(position).DistToPiece = moveResponse.ClosestPiece ?? Int32.MaxValue;
        }
        public void UpdateDistances(ExchangeInformationGMResponse moveResponse)
        {
            int[] distances = moveResponse.Distances.ToArray();
            for(int i = 0; i < FieldsToTakeFrom.Length; i++)
            {
                DateTime neighborsDate = new DateTime(
                    (((long)distances[i + FieldsToTakeFrom.Length]) << 32)
                    + (long)distances[i + 2 * FieldsToTakeFrom.Length]);
                if (neighborsDate > FieldsToTakeFrom[i].LastUpdateDistToPiece)
                    FieldsToTakeFrom[i].DistToPiece = distances[i];
            }
        }

        public enum AgentType
        {
            Standard,
            Goalie,
            Leader
        }
        public class PosField : Field
        {
            public Point Position { get; }
            public PosField(int x, int y) : base()
            {
                Position = new Point(x,y);
            }
        }
    }
}