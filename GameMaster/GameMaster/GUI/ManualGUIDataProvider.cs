using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.GUI
{
    public class ManualGUIDataProvider : IGUIDataProvider
    {
        readonly BoardModel _currentBoardModel;
        public BoardModel GetCurrentBoardModel()
        {
            return _currentBoardModel;
        }
        public ManualGUIDataProvider(int width, int height, int goalAreaHeight)
        {
            _currentBoardModel = new BoardModel
            {
                Width = width,
                Height = height,
                GoalAreaHeight = goalAreaHeight,
                Fields = new FieldType[width, height]
            };
        }
        public void SetField(int x, int y, FieldType type)
        {
            _currentBoardModel.Fields[x, y] = type;
        }
    }
}
