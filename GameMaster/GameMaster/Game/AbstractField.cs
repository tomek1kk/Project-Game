using System;
using CommunicationLibrary.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.GUI;

namespace GameMaster.Game
{
    public abstract class AbstractField
    {
        protected readonly int _x;
        protected readonly int _y;
        protected Player _whos_here;
        protected List<AbstractPiece> _pieces;
        protected bool _discovered = false;
        public bool IsOccupied { get => _whos_here != null; }

        public AbstractField(int x, int y)
        {
            _x = x;
            _y = y;
            _pieces = new List<AbstractPiece>();
        }

        abstract public AbstractPiece PickUp();
        abstract public void Put(AbstractPiece piece);
        public bool MoveHere(Player player)
        {
            _whos_here = player;
            player.Position = this;
            return true;
        }
        public bool MoveOut(Player player)
        {
            if (_whos_here == player)
            {
                _whos_here = null;
                return true;
            }
            return false;
        }
        public bool ContainsPieces()
        {
            return _pieces.Count > 0 ? true : false;
        }
        public int X => _x;
        public int Y => _y;
        public FieldType GetFieldTypeForGUI()
        {
            if (_whos_here == null)
            {
                return GetGUIFieldFromField();
            }
            else
            {
                return _whos_here.GetGUIFieldFromPlayer();
            }
        }
        abstract protected FieldType GetGUIFieldFromField();
        abstract public bool IsGoalField {get;}
        public void PutGeneratedPiece(AbstractPiece piece)
        {
            _pieces.Add(piece);
        }
        public void Discover()
        {
            _discovered = true;
        }
        public static explicit operator Position(AbstractField field) 
            => new Position()
            {
                X = field.X,
                Y = field.Y
            };
    }
}
