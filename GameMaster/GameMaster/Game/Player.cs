using CommunicationLibrary;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace GameMaster.Game
{
    public class Player
    {
        protected int _agentId;
        protected int _messageCorellationId;
        protected Team _team;
        protected bool _isLeader;
        protected AbstractPiece _holding;
        protected AbstractField _position;
        protected DateTime _lockedTill;
        //private MessageSenderService messageService; //TODO:MessageSenderService

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
            return false;
        }
        public void Move(AbstractField field)
        {
            if(_position.MoveOut(this) == false)
            {
                throw new InvalidOperationException("Player is not occupying this field!");
            }
            if(field.MoveHere(this) == false)
            {
                throw new InvalidOperationException("Player can not move to that field");
            }
            //TODO: MessageSenderService
        }
        public void DestroyHolding()
        {
            _holding = null;
            //TODO: MessageSenderService
        }
        public MessagePayload CheckHolding()
        {
            bool sham;
            if (_holding != null && _holding.IsTrue() == false)
                sham = false;
            else
                sham = true;
            return new CheckHoldedPieceResponse()
            {
                Sham = sham
            };
        }
        public void Discover(AbstractField[][] map)
        {
            //TODO: MessageSenderService
        }
        public bool Put()
        {
            if(_holding.IsTrue() == false)
            {
                _holding = null;
                //TODO: MessageSenderSevrice
                return false;
            }
            return _position.Put(_holding);
        }
        public void SetHolding()
        {
            _position.PickUp(this);
            //TODO:MessageSenderService
        }
        public int X => _position.X;
        public int Y => _position.Y;
        public bool IsHolding => _holding != null;
        public Team Team => _team;
        public int AgentId => _agentId;
    }
}
