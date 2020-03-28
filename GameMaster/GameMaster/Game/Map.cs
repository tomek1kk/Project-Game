using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.GUI;
using GameMaster.Configuration;
using CommunicationLibrary.Response;
using CommunicationLibrary.Model;

namespace GameMaster.Game
{
    public class Map : IGuiDataProvider
    {
        private AbstractField[,] _fieldsArray;
        private Dictionary<int, Player> _players;
        private int _goalAreaHeight;
        private int _heigth;
        private int _width;
        private int _numberOfGoals;
        private int _numberOfPieces;
        private int _numberOfPlayers;
        public Dictionary<int, Player> Players => _players;
        public AbstractField this[int x, int y]
        {
            get => _fieldsArray[x, y];
        }
        public Map(GMConfiguration config)
        {
            _heigth = config.BoardY;
            _width = config.BoardX;
            _goalAreaHeight = config.GoalAreaHight;
            _numberOfGoals = config.NumberOfGoals;
            _numberOfPieces = config.NumberOfPieces;
            _numberOfPlayers = 10; // TODO: should be from config (not included in documentation)
            _players = new Dictionary<int, Player>();
            _fieldsArray = new AbstractField[_width, _heigth];
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _heigth; j++)
                    _fieldsArray[i, j] = new Field(i, j);
            AddGoalFields();
            AddPieces();
            //for demo only:
            //for (int i = 0; i < 5; i++)
            //{
            //    AddPlayer(Team.Red, 2 * i);
            //    AddPlayer(Team.Blue, 2 * i + 1);
            //}
        }
        //TODO: tests
        public int ClosestPieceForField(AbstractField field)
        {
            int distance = int.MaxValue;
            for (int x = 0; x < _width; x++)
                for (int y = _goalAreaHeight; y < _heigth - _goalAreaHeight; y++)
                    if (field.ContainsPieces() && Manhattan(field, x, y) < distance)
                        distance = Manhattan(field, x, y);
            return distance;
        }
        public BoardModel GetCurrentBoardModel()
        {
            //prepare fieldsForGUI:
            FieldType[,] fieldsForGUI = new FieldType[_width, _heigth];
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _heigth; j++)
                    fieldsForGUI[i, j] = _fieldsArray[i, j].GetFieldTypeForGUI();
            return new BoardModel()
            {
                Width = _width,
                Height = _heigth,
                GoalAreaHeight = _goalAreaHeight,
                Fields = fieldsForGUI
            };
        }
        /// <summary>
        /// Add GoalFields to map, symmetric for both teams
        /// </summary>
        private void AddGoalFields()
        {
            List<int> randomList = TakeRandomsFromRange(_numberOfGoals, 0, _goalAreaHeight * _width - 1, new Random());
            for (int i = 0; i < _numberOfGoals; i++)
            {
                int redX = randomList[i] % _width;
                int redY = randomList[i] / _width;
                _fieldsArray[redX, redY] = new GoalField(redX, redY);
                int blueX = (_width - 1) - redX;
                int blueY = (_heigth - 1) - redY;
                _fieldsArray[blueX, blueY] = new GoalField(blueX, blueY);
            }
        }
        private void AddPieces()
        {
            var rand = new Random();
            for (int i = 0; i < _numberOfPieces; i++)
            {
                int idx = rand.Next(_width * _goalAreaHeight - 1, _width * (_heigth - _goalAreaHeight));
                _fieldsArray[idx % _width, idx / _width].PutGeneratedPiece();
            }
        }
        public bool AddPlayer(Team team, int agentId)
        {
            if (_numberOfPlayers == _players.Count)
                return false;
            var rand = new Random();
            Player player = new Player(team, agentId);
            int idx = rand.Next(_width * _goalAreaHeight - 1, _width * (_heigth - _goalAreaHeight));
            _fieldsArray[idx % _width, idx / _width].MoveHere(player);
            _players.Add(agentId, player);
            return true;
        }
        public Player GetPlayerById(int agentId)
        {
            if (_players.ContainsKey(agentId))
                return _players[agentId];
            return null;
        }
        public bool IsInsideMap(int x, int y)
        {
            return (x >= 0 && x < _width && y > 0 && y < _heigth) ? true : false;
        }
        public bool IsInGoalArea(AbstractField field)
        {
            return IsInsideRedGoalArea(field) || IsInsideBlueGoalArea(field);
        }
        public bool IsInsideRedGoalArea(int x, int y)
        {
            return (x >= 0 && x < _width && y >= 0 && y < _goalAreaHeight) ? true : false;
        }
        public bool IsInsideBlueGoalArea(int x, int y)
        {
            return (x >= 0 && x < _width && y >= _heigth - _goalAreaHeight && y < _heigth) ? true : false;
        }
        public bool IsInsideBlueGoalArea(AbstractField field)
        {
            return IsInsideBlueGoalArea(field.X, field.Y);
        }
        public bool IsInsideRedGoalArea(AbstractField field)
        {
            return IsInsideRedGoalArea(field.X, field.Y);
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
        //TODO: tests
        private static int Manhattan(AbstractField field, int x, int y)
        {
            return Math.Abs(field.X - x) + Math.Abs(field.Y - y);
        }
    }
}