using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Edge : Entity
    {
        public Transform modelRoot;
        public Transform model;
        public Transform preview;

        public BuildableObjectSO replaceableObjectSO;
        public Data_Cell parentCellData;
        public Room parentRoom;


        public void Preview(BuildableObjectSO buildableObjectSO)
        {       
            //有可能每帧都call
            if (preview != null) return;
            model.gameObject.SetActive(false);
            UpdateActiveBuildableObjectSORandomPrefab(buildableObjectSO);
        }

        public void CancelPreview()
        {
            Destroy(preview.gameObject);
            preview = null;
            model.gameObject.SetActive(true);
        }

        public void EnsurePreview()
        {
            Destroy(model.gameObject);
            model = preview;
            model.GetComponent<Collider>().enabled = true;
            preview = null;
        }

        private void UpdateActiveBuildableObjectSORandomPrefab(BuildableObjectSO buildableObjectSO)
        {
            float totalProbability;
            float randomPoint;

            totalProbability = CalculateTotalProbability(buildableObjectSO);
            randomPoint = UnityEngine.Random.Range(0f, totalProbability);

            var activeBuildableObjectSORandomPrefab = SelectPrefabByProbability(buildableObjectSO, randomPoint);

            var preview = Instantiate(activeBuildableObjectSORandomPrefab.objectPrefab, modelRoot);
            preview.transform.localPosition = Vector3.zero;
            preview.transform.localRotation = Quaternion.identity;
            preview.GetComponent<Collider>().enabled = false;
            this.preview = preview;
        }

        private float CalculateTotalProbability(BuildableObjectSO buildableObjectSO)
        {
            var totalProbability = 0f;
            foreach (var randomPrefab in buildableObjectSO.randomPrefabs)
            {
                totalProbability += randomPrefab.probability;
            }
            return totalProbability;
        }

        private BuildableObjectSO.RandomPrefabs SelectPrefabByProbability(BuildableObjectSO buildableObjectSO, float randomPoint)
        {
            var currentProbability = 0f;
            foreach (var randomPrefab in buildableObjectSO.randomPrefabs)
            {
                currentProbability += randomPrefab.probability;
                if (randomPoint <= currentProbability) return randomPrefab;
            }
            return null;
        }
    }
}
