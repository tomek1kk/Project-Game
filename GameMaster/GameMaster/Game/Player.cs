using CommunicationLibrary;
using CommunicationLibrary.Response;
using System;
using GameMaster.GUI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace GameMaster.Game
{
    public class Player
    {
        private int _agentId;
        private int _messageCorellationId;
        private Team _team;
        private bool _isLeader;
        private AbstractPiece _holding;
        private AbstractField _position;
        private DateTime _lockedTill;
        public int X => _position.X;
        public int Y => _position.Y;
        public bool IsHolding => _holding != null;
        public Team Team => _team;
        public int AgentId => _agentId;
        public bool IsLeader => _isLeader;
        public bool IsLocked => _lockedTill > DateTime.Now;
        public AbstractPiece Holding { get => _holding; set => _holding = value; }
        public DateTime LockedTill => _lockedTill;
        public AbstractField Position { get => _position; set => _position = value; }
        public Player(Team team, int agentId)
        {
            _agentId = agentId;
            _team = team;
        }
        public Player(DateTime lockedTill, int agentId = -1, int messageCorellationId = -1, Team team = Team.Blue, bool isLeader = false)
        {
            _agentId = agentId;
            _messageCorellationId = messageCorellationId;
            _team = team;
            _isLeader = isLeader;
            _lockedTill = lockedTill;
        }
        public Player(DateTime lockedTill, AbstractField field, AbstractPiece piece, int agentId = -1, int messageCorellationId = -1, Team team = Team.Blue, bool isLeader = false)
        {
            _agentId = agentId;
            _messageCorellationId = messageCorellationId;
            _team = team;
            _isLeader = isLeader;
            _lockedTill = lockedTill;
            _holding = piece;
            _position = field;
        }

        public bool TryLock(DateTime newLockTime)
        {
            if(_lockedTill < DateTime.Now)
            {
                _lockedTill = newLockTime;
                return true;
            }
            Console.WriteLine("Agent " + AgentId.ToString() + " cannot be locked before he is unlocked. Locked till: " +
                _lockedTill.ToString() + ", new TryLock time: " + newLockTime.ToString());
            return false;
        }
        public FieldType GetGUIFieldFromPlayer()
        {
            if (IsHolding)
            {
                if (Holding.IsSham())
                {
                    return Team == Team.Red ? FieldType.RedPlayerWithSham : FieldType.BluePlayerWithSham;
                }
                else
                {
                    return Team == Team.Red ? FieldType.RedPlayerWithPiece : FieldType.BluePlayerWithPiece;
                }
            }
            else
            {
                return Team == Team.Red ? FieldType.RedPlayer : FieldType.BluePlayer;
            }
        }
    }
}
