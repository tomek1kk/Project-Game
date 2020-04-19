using System;
using System.Collections.Generic;
using System.Linq;
using GameMaster.GUI;
using GameMaster.Configuration;

namespace GameMaster.Game
{
    public class Map : IGuiDataProvider
    {
        private AbstractField[,] _fieldsArray;
        private Dictionary<int, Player> _players;
        private List <(int, int)> _exchangeInformationList;
        private int _goalAreaHeight;
        private int _heigth;
        private int _width;
        private int _numberOfGoals;
        private int _numberOfPieces;
        private int _numberOfPlayers;
        private int _shamPieceProbability;
        private int _redPoints;
        private int _bluePoints;
        public Dictionary<int, Player> Players => _players;
        public AbstractField this[int x, int y]
        {
            get { return (x >= 0 && x < _width && y >= 0 && y < _heigth) ? _fieldsArray[x, _heigth - 1 - y] : null; }
            private set { if (x >= 0 && x < _width && y >= 0 && y < _heigth) _fieldsArray[x, _heigth - 1 - y] = value; }
        }
        public bool GameEnded { get => _redPoints == _numberOfGoals || _bluePoints == _numberOfGoals; }
        public Team Winner { get => _redPoints == _numberOfGoals ? Team.Red : Team.Blue; }

        public Map
            (List<(int x, int y)> goalFields = null,
            List<(int x, int y)> realPieces = null,
            List<(int x, int y)> shamPieces = null,
            List<(int x, int y, int id, Team team)> players = null,
            int heigth = 10,
            int width = 10,
            int goalAreaHeight = 3,
            int numberOfPlayers = 10)
        {
            _heigth = heigth;
            _width = width;
            _goalAreaHeight = goalAreaHeight;
            _numberOfGoals = goalFields == null ? 0 : goalFields.Count;
            _numberOfPieces = realPieces == null ? 0 : realPieces.Count;
            _numberOfPieces += shamPieces == null ? 0 : shamPieces.Count;
            _numberOfPlayers = numberOfPlayers;
            _players = new Dictionary<int, Player>();
            _fieldsArray = new AbstractField[_width, _heigth];
            for (int i = 0; i < _width; i++)
                for (int j = _heigth - 1; j >= 0; j--)
                    this[i, j] = new Field(i, j);
            for (int i = 0; i < _numberOfGoals; i++)
                this[goalFields[i].x, goalFields[i].y] = new GoalField(goalFields[i].Item1, goalFields[i].Item2);
            for (int i = 0; realPieces != null && i < realPieces.Count; i++)
                this[realPieces[i].x, realPieces[i].y].PutGeneratedPiece(new Piece());
            for (int i = 0; shamPieces != null && i < shamPieces.Count; i++)
                this[shamPieces[i].x, shamPieces[i].y].PutGeneratedPiece(new ShamPiece());
            for (int i = 0; players != null && i < players.Count; i++)
            {
                Player player = new Player(players[i].team, players[i].id, false);
                this[players[i].x, players[i].y].MoveHere(player);
                _players.Add(player.AgentId, player);
            }
            _exchangeInformationList = new List<(int, int)>();
        }
        public Map(GMConfiguration config)
        {
            _heigth = config.BoardY;
            _width = config.BoardX;
            _goalAreaHeight = config.GoalAreaHight;
            _numberOfGoals = config.NumberOfGoals;
            _numberOfPieces = config.NumberOfPieces;
            _numberOfPlayers = config.NumberOfPlayers; // TODO: should be from config (not included in documentation)
            _shamPieceProbability = config.ShamPieceProbability;
            _players = new Dictionary<int, Player>();
            _fieldsArray = new AbstractField[_width, _heigth];
            for (int i = 0; i < _width; i++)
                for (int j = _heigth - 1; j >= 0; j--)
                    this[i, j] = new Field(i, j);
            AddGoalFields();
            AddPieces();
            _exchangeInformationList = new List<(int, int)>();
        }

        public int ClosestPieceForField(AbstractField field)
        {
            int distance = int.MaxValue;
            for (int x = 0; x < _width; x++)
                for (int y = _goalAreaHeight; y < _heigth - _goalAreaHeight; y++)
                    if (this[x, y].ContainsPieces() && Manhattan(field, x, y) < distance)
                        distance = Manhattan(field, x, y);
            return distance;
        }
        public BoardModel GetCurrentBoardModel()
        {
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
                int blueX = randomList[i] % _width;
                int blueY = randomList[i] / _width;
                this[blueX, blueY] = new GoalField(blueX, blueY);
                int redX = (_width - 1) - blueX;
                int redY = (_heigth - 1) - blueY;
                this[redX, redY] = new GoalField(redX, redY);
            }
        }
        private void AddPieces()
        {
            var rand = new Random();
            for (int i = 0; i < _numberOfPieces; i++)
            {
                AddPiece(rand);
            }
        }
        public void AddPiece()
        {
            var rand = new Random();
            AbstractPiece piece;
            if (rand.Next(101) < _shamPieceProbability)
                piece = new ShamPiece();
            else
                piece = new Piece();
            int idx = rand.Next(_width * _goalAreaHeight, _width * (_heigth - _goalAreaHeight));
            this[idx % _width, idx / _width].PutGeneratedPiece(piece);
        }
        public void AddPiece(Random rand)
        {
            AbstractPiece piece;
            if (rand.Next(101) < _shamPieceProbability)
                piece = new ShamPiece();
            else
                piece = new Piece();
            int idx = rand.Next(_width * _goalAreaHeight, _width * (_heigth - _goalAreaHeight));
            this[idx % _width, idx / _width].PutGeneratedPiece(piece);
        }
        public bool AddPlayer(Team team, int agentId)
        {
            var freeFields = Enumerable.Range(_goalAreaHeight * _width, _width * (_heigth - 2 * _goalAreaHeight)).ToList();
            foreach (Player p in _players.Values)
                freeFields.Remove(p.X + p.Y * _width);
            if (freeFields.Count == 0 || _players.Count == _numberOfPlayers)
                return false;
            var rand = new Random();
            int idx = freeFields[rand.Next(freeFields.Count)];

            Player player = new Player(team, agentId, TeamHasNoPlayers(team));
            this[idx % _width, idx / _width].MoveHere(player);
            _players.Add(agentId, player);
            return true;
        }
        public bool TeamHasNoPlayers(Team team)
        {
            foreach(var tuple in _players)
            {
                if (tuple.Value.Team == team)
                    return false;
            }
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
            return (x >= 0 && x < _width && y >= 0 && y < _heigth) ? true : false;
        }
        public bool IsInGoalArea(AbstractField field)
        {
            return IsInsideRedGoalArea(field) || IsInsideBlueGoalArea(field);
        }
        public bool IsInsideRedGoalArea(int x, int y)
        {
            return (x >= 0 && x < _width && y >= _heigth - _goalAreaHeight && y < _heigth) ? true : false;
        }
        public bool IsInsideBlueGoalArea(int x, int y)
        {
            return (x >= 0 && x < _width && y >= 0 && y < _goalAreaHeight) ? true : false;
        }
        public bool IsInsideBlueGoalArea(AbstractField field)
        {
            return IsInsideBlueGoalArea(field.X, field.Y);
        }
        public bool IsInsideRedGoalArea(AbstractField field)
        {
            return IsInsideRedGoalArea(field.X, field.Y);
        }
        public void ScorePoint(AbstractField field, int agentId)
        {
            if (field.ContainsPieces())
                return;
            _redPoints += Players[agentId].Team == Team.Red ? 1 : 0;
            _bluePoints += Players[agentId].Team == Team.Blue ? 1 : 0;
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
        private static int Manhattan(AbstractField field, int x, int y)
        {
            return Math.Abs(field.X - x) + Math.Abs(field.Y - y);
        }
        public void SaveInformationExchange(int requester, int respondent)
        {
            _exchangeInformationList.Add((requester, respondent));
        }
        public bool ValidateAndRemoveInformationExchange(int requester, int respondent)
        {
            for (int i = 0; i < _exchangeInformationList.Count; i++)
            {
                if(_exchangeInformationList[i] == (requester, respondent))
                {
                    _exchangeInformationList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}