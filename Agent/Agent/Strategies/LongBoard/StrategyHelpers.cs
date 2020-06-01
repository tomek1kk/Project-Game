using CommunicationLibrary;
using CommunicationLibrary.Request;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Agent.Strategies.LongBoard
{
    public static class StrategyHelpers
    {
        private static readonly Random _rng = new Random();
        public static String GetDirectionToField(Point currentPos, Point target)
        {
            if (target.Y > currentPos.Y) return "N";
            if (target.Y < currentPos.Y) return "S";
            if (target.X > currentPos.X) return "E";
            if (target.X < currentPos.X) return "W";
            throw new Exception("agent is already at target");
        }

        public static int Dist(Point f1, Point f2)
            => Math.Abs(f1.X - f2.X) + Math.Abs(f1.Y - f2.Y);

        public static Message GetRandomMove()
        {
            return new Message<MoveRequest>(new MoveRequest
            {
                Direction = new string[] { "N", "S", "W", "E" }[_rng.Next(4)]
            });
        }

        public static Message GetRandomMove(params string[] validDirections)
        {
            return new Message<MoveRequest>(new MoveRequest
            {
                Direction = validDirections[_rng.Next(validDirections.Length)]
            });
        }

        public static Message GetMoveTo(Point currentPos, Point target)
        {
            return new Message<MoveRequest>(new MoveRequest()
            {
                Direction = GetDirectionToField(
                currentPos,
                target
                )
            });
        }
    }
}
