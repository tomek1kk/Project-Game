using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.GUI;
using GameMaster.Configuration;

namespace GameMaster.Game
{
    public class Map : IGuiDataProvider
    {
        private AbstractField[,] fieldsArray;
        private int goalAreaHeight;
        private int heigth;
        private int width;
        private int numberOfGoals;
        private int numberOfPieces;

        public Map(GMConfiguration config)
        {
            heigth = config.BoardY;
            width = config.BoardX;
            goalAreaHeight = config.GoalAreaHight;
            numberOfGoals = config.NumberOfGoals;
            numberOfPieces = config.NumberOfPieces;
            fieldsArray = new AbstractField[width, heigth];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    fieldsArray[i, j] = new Field(i, j);
            AddGoalFields();
            AddPieces();
            //for demo only:
            for (int i = 0; i < 5; i++)
            {
                AddPlayer(Team.Red, 2 * i);
                AddPlayer(Team.Blue, 2 * i + 1);
            }
        }
        public BoardModel GetCurrentBoardModel()
        {
            //prepare fieldsForGUI:
            FieldType[,] fieldsForGUI = new FieldType[width, heigth];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    fieldsForGUI[i, j] = fieldsArray[i, j].GetFieldTypeForGUI();
            return new BoardModel()
            {
                Width = width,
                Height = heigth,
                GoalAreaHeight = this.goalAreaHeight,
                Fields = fieldsForGUI
            };
        }
        /// <summary>
        /// Add GoalFields to map, symmetric for both teams
        /// </summary>
        private void AddGoalFields()
        {
            List<int> randomList = TakeRandomsFromRange(numberOfGoals, 0, goalAreaHeight * width - 1, new Random());
            for (int i = 0; i < numberOfGoals; i++)
            {
                int redX = randomList[i] % width;
                int redY = randomList[i] / width;
                fieldsArray[redX, redY] = new GoalField(redX, redY);
                int blueX = (width - 1) - redX;
                int blueY = (heigth - 1) - redY;
                fieldsArray[blueX, blueY] = new GoalField(blueX, blueY);
            }
        }
        private void AddPieces()
        {
            var rand = new Random();
            for (int i = 0; i < numberOfPieces; i++)
            {
                int idx = rand.Next(width * goalAreaHeight - 1, width * (heigth - goalAreaHeight));
                fieldsArray[idx % width, idx / width].PutGeneratedPiece();
            }
        }
        public void AddPlayer(Team team, int agentId)
        {
            var rand = new Random();
            Player player = new Player(team, agentId);
            int idx = rand.Next(width * goalAreaHeight - 1, width * (heigth - goalAreaHeight));
            fieldsArray[idx % width, idx / width].MoveHere(player);
        }
        /// <summary>
        /// Returns random list of integers from range [rangeFrom, rangeTo] of length equal randomCounts
        /// </summary>
        /// <param name="randomCounts">length of returned list</param>
        /// <param name="rangeFrom">boundary of given range</param>
        /// <param name="rangeTo">boundary of given range</param>
        /// <param name="rand">Random object use in function</param>
        /// <param name="shuffleCount">optional: amount of "shuffles" to be done</param>
        public static List<int> TakeRandomsFromRange(int randomCounts, int rangeFrom, int rangeTo, Random rand, int shuffleCount = 200)
        {
            var range = Enumerable.Range(rangeFrom, rangeTo - rangeFrom + 1).ToList();
            for (int i = 0; i < shuffleCount; i++)
            {
                for (int j = range.Count() - 1; j > 0; j--)
                {
                    int r = rand.Next(0, j + 1);
                    (range[r], range[j]) = (range[j], range[r]);
                }
            }
            return Enumerable.Take(range, randomCounts).ToList();
        }
    }
}