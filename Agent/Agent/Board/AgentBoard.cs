﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Agent.Board
{
    public class AgentBoard
    {
        public Field[,] Board { get; private set; }
        public string GoalDirection { get; private set; }
        public (int start, int end) GoalArea { get; private set; }
        public int GoalAreaSize { get; private set; }
        public AgentBoard(int width, int height, string teamId, int goalAreaSize)
        {
            Board = new Field[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    Board[i, j] = new Field();

            GoalDirection = teamId == "Red" ? "N" : "S";
            GoalAreaSize = goalAreaSize;
            GoalArea = teamId == "Blue" ? (0, GoalAreaSize - 1) : (Board.GetLength(1) - GoalAreaSize, Board.GetLength(1) - 1);
        }

        public bool InGoalArea(Point Position)
        {
            return Position.Y >= GoalArea.start && Position.Y <= GoalArea.end;
        }
        public IEnumerable<string> GetGoalInfo()
        {

                if (GoalDirection == "N")
                {
                    for (int i = GoalArea.start; i <= GoalArea.end; ++i)
                        for (int j = 0; j < Board.GetLength(0); j++)
                            if (Board[j, i].goalInfo == GoalInfo.DiscoveredGoal || Board[j, i].goalInfo == GoalInfo.DiscoveredNotGoal)
                                yield return Board[j, i].goalInfo == GoalInfo.DiscoveredGoal ? "G" : "NG";
                            else
                                yield return "IDK";
            }
                else
                {
                    for (int i = GoalArea.end ; i >= 0; --i)
                        for (int j = 0; j < Board.GetLength(0); j++)
                            if (Board[j, i].goalInfo == GoalInfo.DiscoveredGoal || Board[j, i].goalInfo == GoalInfo.DiscoveredNotGoal)
                                yield return Board[j, i].goalInfo == GoalInfo.DiscoveredGoal ? "G" : "NG";
                            else
                                yield return "IDK";
            }

        }
        public void UpdateGoalInfo(IEnumerable<string> info)
        {
            IEnumerator<string> iterator = info.GetEnumerator();
            if (GoalDirection == "N")
            {
                for (int i = GoalArea.start; i <= GoalArea.end; ++i)
                    for (int j = 0; j < Board.GetLength(0) && iterator.MoveNext(); j++)
                        if (iterator.Current == "G" || iterator.Current == "NG")
                            Board[j, i].goalInfo = iterator.Current == "G" ? GoalInfo.DiscoveredGoal : GoalInfo.DiscoveredNotGoal;

            }
            else
            {
                for (int i = GoalArea.end; i >= 0; --i)
                    for (int j = 0; j < Board.GetLength(0) && iterator.MoveNext(); j++)
                        if (iterator.Current == "G" || iterator.Current == "NG")
                            Board[j, i].goalInfo = iterator.Current == "G" ? GoalInfo.DiscoveredGoal : GoalInfo.DiscoveredNotGoal;
            }           
        }
        public IEnumerable<int> GetDistances()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    yield return Board[i, j].DistToPiece;
                    yield return Board[i, j].LastUpdateDistToPiece.Minute * 60 + Board[i, j].LastUpdateDistToPiece.Second;
                }
        }
        public void UpdateDistances(IEnumerable<int> distances, Func<int,int, int, int, int> update)
        {
            IEnumerator<int> iterator = distances.GetEnumerator();
            for (int i = 0; i < Board.GetLength(0); i++)
                for (int j = 0; j < Board.GetLength(1) && iterator.MoveNext(); j++)
                {
                    int dist = iterator.Current;
                    iterator.MoveNext();
                    int last_update = Board[i, j].LastUpdateDistToPiece.Minute * 60 + Board[i, j].LastUpdateDistToPiece.Second;

                    if (last_update > iterator.Current)
                        Board[i, j].DistToPiece = update(dist, iterator.Current, Board[i, j].DistToPiece, last_update) ;
                }
        }
        public (int, int) FindUndiscoveredGoalCoordinates()
        {
            int lastRowOfGoals;
            if (GoalDirection == "N")
            {
                lastRowOfGoals = GoalArea.start;
                for (int i = lastRowOfGoals; i <= GoalArea.end; ++i)
                    for (int j = 0; j < Board.GetLength(0); j++)
                        if(Board[j, i].goalInfo == GoalInfo.IDK)
                            return (j, i);
            }
            else
            {
                lastRowOfGoals = GoalArea.end;
                for (int i = lastRowOfGoals; i >= 0; --i)
                    for (int j = 0; j < Board.GetLength(0); j++)
                        if (Board[j, i].goalInfo == GoalInfo.IDK)
                            return (j, i);
            }
            throw new Exception("All goals should be realized.");
        }
    }
}
