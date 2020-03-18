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
        private int GoalAreaHeight;
        private int heigth;
        private int width;

        public Map(GMConfiguration config)
        {
            heigth = config.BoardY;
            width = config.BoardX;
            GoalAreaHeight = config.GoalAreaHight;
            //TODO: board creation with goals and pieces, for now only standard empty fields
            fieldsArray = new AbstractField[width, heigth];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    fieldsArray[i, j] = new Field();
            TakeRandomsFromRange(1, 0, 30, new Random());
        }
        public BoardModel GetCurrentBoardModel()
        {
            //prepare fieldsForGUI:
            FieldType[,] fieldsForGUI = new FieldType[width, heigth];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    fieldsForGUI[i, j] = ConvertFieldForGUI(fieldsArray[i, j]);
            return new BoardModel()
            {
                Width = width,
                Height = heigth,
                GoalAreaHeight = this.GoalAreaHeight,
                Fields = fieldsForGUI
            };
        }
        private static FieldType ConvertFieldForGUI(AbstractField field)
        {
            return field.GetFieldTypeForGUI();
        }
        public static List<int> TakeRandomsFromRange(int randomCounts, int rangeFrom, int rangeTo, Random rand, int shuffleCount = 50)
        {
            var range = Enumerable.Range(rangeFrom, rangeTo).ToList();
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