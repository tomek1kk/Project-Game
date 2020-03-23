﻿using CommunicationLibrary;
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
        private int _agentId;
        private int _messageCorellationId;
        private Team _team;
        private bool _isLeader;
        private AbstractPiece _holding;
        private AbstractField _position;
        private DateTime _lockedTill;
        //private MessageSenderService messageService; //TODO:MessageSenderService
        public int X => _position.X;
        public int Y => _position.Y;
        public bool IsHolding => _holding != null;
        public Team Team => _team;
        public int AgentId => _agentId;
        public bool IsUnlocked => _lockedTill < DateTime.Now;
        public AbstractPiece Holding => _holding;
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
        public DestroyPieceResponse DestroyHolding()
        {
            _holding = null;
            return new DestroyPieceResponse();
        }
        //public void Discover(AbstractField[][] map)
        //{
        //    //TODO: MessageSenderService
        //}
        public void SetHolding()
        {
            _position.PickUp(this);
            //TODO:MessageSenderService
        }
    }
}
