using DungeonArchitect;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class RandomFloorTransformRule : TransformationRule
    {
        private static readonly float[] Angles = { 0f, 90f, 180f, 270f };
        public override void GetTransform(PropSocket socket, DungeonModel model, Matrix4x4 propTransform, System.Random random, out Vector3 outPosition, out Quaternion outRotation, out Vector3 outScale)
        {
            base.GetTransform(socket, model, propTransform, random, out outPosition, out outRotation, out outScale);

            // Random rotation
            var angle = random.value() * 360;
            var rotation = Quaternion.Euler(0, GetRandomRotation(), 0);
            outRotation = rotation;
        }

        private float GetRandomRotation()
        {
            var index = Random.Range(0, Angles.Length);
            return Angles[index];
        }
    }
}
