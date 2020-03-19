﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace GameMaster.Game
{
    public class Player
    {
        private int id;
        private int messageCorellationId;
        private Team team;
        private bool isLeader;
        private AbstractPiece holding;
        private AbstractField position;
        private DateTime lockedTill;
        //private MessageSenderService messageService; //TODO:MessageSenderService

        public Player(DateTime _lockedTill, int _id = -1, int _messageCorellationId = -1, Team _team = Team.Blue, bool _isLeader = false)
        {
            id = _id;
            messageCorellationId = _messageCorellationId;
            team = _team;
            isLeader = _isLeader;
            lockedTill = _lockedTill;
        }
        public Player(DateTime _lockedTill, AbstractField _field, AbstractPiece _piece, int _id = -1, int _messageCorellationId = -1, Team _team = Team.Blue, bool _isLeader = false)
        {
            id = _id;
            messageCorellationId = _messageCorellationId;
            team = _team;
            isLeader = _isLeader;
            lockedTill = _lockedTill;
            holding = _piece;
            position = _field;
        }

        public bool TryLock(DateTime newLockTime)
        {
            if(lockedTill < DateTime.Now)
            {
                lockedTill = newLockTime;
                return true;
            }
            return false;
        }
        public void Move(AbstractField field)
        {
            if(position.MoveOut(this) == false)
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
            holding = null;
            //TODO: MessageSenderService
        }
        public void CheckHolding()
        {
            if (holding != null && holding.IsTrue() == false)
                holding = null;
            //TODO: MessageSenderService
        }
        public void Discover(AbstractField[][] map)
        {
            //TODO: MessageSenderService
        }
        public bool Put()
        {
            if(holding.IsTrue() == false)
            {
                holding = null;
                //TODO: MessageSenderSevrice
                return false;
            }
            return position.Put(holding);
        }
        public void SetHolding()
        {
            position.PickUp(this);
            //TODO:MessageSenderService
        }
        public int X
        {
            get => position.X;
        }
        public int Y
        {
            get => position.Y;
        }
    }
}