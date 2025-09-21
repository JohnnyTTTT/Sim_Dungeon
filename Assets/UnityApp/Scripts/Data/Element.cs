using DungeonArchitect;
using UnityEngine;

namespace Johnny.SimDungeon
{

    public abstract class Element
    {
        public int GUID;
    }
    public abstract class ElementData<T> : Element
    {
        public T Data;

        //[SerializeField] protected Direction Direction;
        //[SerializeField] protected Vector2 DirectionVector;
        //public virtual void SetDirection(Direction direction)
        //{
        //    Direction = direction;

        //    DirectionVector = Direction switch
        //    {
        //        Direction.Left => new Vector2(-1f, 0f),
        //        Direction.Up => new Vector2(0f, 1f),
        //        Direction.Right => new Vector2(1f, 0f),
        //        Direction.Down => new Vector2(0f, -1f),
        //    };
        //}
        public ElementData(T data)
        {
            GUID = GetHashCode();
            Data = data;
        }
    }
}
