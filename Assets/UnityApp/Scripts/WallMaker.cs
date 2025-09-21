using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class WallMaker : MonoBehaviour
    {
        public GameObject origin;
        public GameObject cutter;
        public Material sectionMaterial;
        public CSG.BooleanOp operation;
        public string savePath;



        [Button]
        public void Perform()
        {
            var result = CSG.Perform(operation, origin, cutter);
            var composite = new GameObject();
            //composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
            //var mats = result.materials;
            //mats[mats.Count - 1] = sectionMaterial;
            //composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
            //composite.name = origin.name.ToString() + " Cutted";

            AssetDatabase.CreateAsset(Object.Instantiate(result.mesh), savePath);
            AssetDatabase.SaveAssets();
        }
    }
}
