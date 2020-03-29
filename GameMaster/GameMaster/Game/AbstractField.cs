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
        protected List<Player> _whos_here;
        protected List<AbstractPiece> _pieces;
        protected bool _discovered = false;//change this field only for fields in goal area

        public AbstractField(int x, int y)
        {
            _x = x;
            _y = y;
            _pieces = new List<AbstractPiece>();
            _whos_here = new List<Player>();
        }

        abstract public AbstractPiece PickUp();
        abstract public void Put(AbstractPiece piece);
        public bool MoveHere(Player player)
        {
            _whos_here.Add(player);
            player.Position = this;
            return true;
        }
        public bool MoveOut(Player player)
        {
            if (_whos_here.Contains(player))
            {
                _whos_here.Remove(player);
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
        abstract public FieldType GetFieldTypeForGUI();
        abstract public bool IsGoalField {get;}
        public void PutGeneratedPiece(AbstractPiece piece)
        {
            _pieces.Add(piece);
        }
        public static explicit operator Position(AbstractField field) 
            => new Position()
            {
                X = field.X,
                Y = field.Y
            };
    }
}
