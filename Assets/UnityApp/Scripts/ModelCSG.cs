using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class ModelCSG : MonoBehaviour
    {
        public GameObject[] origins;
        public GameObject cutter;
        public Material sectionMaterial;
        public CSG.BooleanOp operation;

        [Button]
        public void Perform()
        {
            foreach (var origin in origins)
            {
                var result = CSG.Perform(operation, origin, cutter);
                var composite = new GameObject();
                composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
                var mats = result.materials;
                mats[mats.Count-1] =sectionMaterial;
                composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
                composite.name = origin.name.ToString() + " Cutted";
                composite.transform.parent = origin.transform.parent;
                origin.gameObject.SetActive(false);
            }

        }
    }
}
