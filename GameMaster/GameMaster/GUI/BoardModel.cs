using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.GUI
{
    public class BoardModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int GoalAreaHeight { get; set; }
        public FieldType[,] Fields { get; set; }
        public string Message { get; set; }
        public bool StartButtonDisabled { get; set; }
    }
}
