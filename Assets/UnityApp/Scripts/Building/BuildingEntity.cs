using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum Direction
    {
        Left,
        Up,
        Right,
        Down
    }
    public class BuildingEntity : MonoBehaviour
    {

    }
    public abstract class BuildingEntity<T> : BuildingEntity
    {
        public int GUID;
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

        public virtual void Init(T data)
        {
            GUID = GetHashCode();
            Data = data;
        }

    }
}
