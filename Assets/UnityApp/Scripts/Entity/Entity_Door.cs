using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Door : Entity_Edge
    {
        public GameObject cutter;
        public GameObject virtualModel;
        public override void UpdateData()
        {
            base.UpdateData();
            edgeElement.door = this;
        }

        public void CutWall()
        {
            var wall = edgeElement.wall;

            wall.originWalls[0].full = DoCut(wall.originWalls[0].full);
            wall.originWalls[0].shorten = DoCut(wall.originWalls[0].shorten);

            wall.originWalls[1].full = DoCut(wall.originWalls[1].full);
            wall.originWalls[1].shorten = DoCut(wall.originWalls[1].shorten);

            virtualModel.SetActive(false);
        }

        private GameObject DoCut(GameObject origin)
        {
            var result = CSG.Perform(CSG.BooleanOp.Subtraction, origin, cutter);
            var composite = new GameObject();
            composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
            var mats = result.materials;
            mats[mats.Count - 1] = SpawnManager.Instance.defaultSectionMaterial; ;
            composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
            composite.name = origin.name.ToString() + " - Door Cuted";
            composite.transform.parent = origin.transform.parent;
            composite.transform.localPosition = Vector3.zero;
            composite.transform.rotation = Quaternion.identity;
            composite.transform.localScale = Vector3.one;
            Destroy(origin);
            return composite;
        }
    }
}
