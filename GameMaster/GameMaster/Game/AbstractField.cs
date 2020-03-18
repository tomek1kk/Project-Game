using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.GUI;

namespace GameMaster.Game
{
    public abstract class AbstractField
    {
        protected readonly int x;
        protected readonly int y;
        protected List<Player> whos_here;
        protected List<AbstractPiece> pieces;
        protected bool discovered = false;//change this field only for fields in goal area

        public AbstractField(int _x, int _y)
        {
            pieces = new List<AbstractPiece>();
            whos_here = new List<Player>();
        }

        public void LeavePlayer(Player player)
        {
            whos_here.Remove(player);
        }
        abstract public void PickUp(Player player);
        abstract public bool Put(AbstractPiece piece);
        public bool MoveHere(Player player)
        {
            whos_here.Add(player);
            return true;
        }
        public bool MoveOut(Player player)
        {
            if(whos_here.Contains(player))
            {
                whos_here.Remove(player);
                return true;
            }
            return false;
        }
        public bool ContainsPieces()
        {
            return pieces.Count > 0 ? true : false;
        }
        public int X
        {
            get => x;
        }
        public int Y
        {
            get => y;
        }
        abstract public FieldType GetFieldTypeForGUI();
        public void PutGeneratedPiece()
        {
            pieces.Add(new Piece());
        }
    }
}
