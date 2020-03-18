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

        public Map(GMConfiguration config)
        {
            heigth = config.BoardY;
            width = config.BoardX;
            goalAreaHeight = config.GoalAreaHight;
            numberOfGoals = config.NumberOfGoals;
            //TODO: board creation with goals and pieces, for now only standard empty fields
            fieldsArray = new AbstractField[width, heigth];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    fieldsArray[i, j] = new Field();
            AddGoalFields();
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

        private void AddGoalFields()
        {
            List<int> randomList = TakeRandomsFromRange(numberOfGoals, 0, goalAreaHeight * width - 1, new Random());
            for (int i = 0; i < numberOfGoals; i++)
            {
                int redX = randomList[i] % width;
                int redY = randomList[i] / width;
                fieldsArray[redX, redY] = new GoalField();
                int blueX = (width - 1) - redX;
                int blueY = (heigth - 1) - redY;
                fieldsArray[blueX, blueY] = new GoalField();
            }
        }
        /// <summary>
        /// Returns random list of integers from range [rangeFrom, rangeTo] of length equal randomCounts
        /// </summary>
        /// <param name="randomCounts"></param>
        /// <param name="rangeFrom"></param>
        /// <param name="rangeTo"></param>
        /// <param name="rand"></param>
        /// <param name="shuffleCount"></param>
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