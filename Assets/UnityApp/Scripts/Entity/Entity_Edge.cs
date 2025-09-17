using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Edge : Entity
    {
        public static int DirectionHash = Shader.PropertyToID("_Direction");
        public FourDirectionalRotation direction;
        public Entity_EdgeGroup parent;
        public Entity_Edge relativeEdge;
        private Data_Cell parentCellData;

        private BuildableFreeObject currentObject;
        private Transform preview;

        public override bool TryReplace(BuildableObjectSO temelpte, BuildableObjectSO.RandomPrefabs prefabs = null)
        {
            if (temelpte is BuildableFreeObjectSO buildableFreeObject)
            {
                if (currentObject == null || currentObject.GetBuildableObjectSO() != buildableFreeObject)
                {
                    if (prefabs == null)
                    {
                        prefabs = RandomUtility.UpdateBuildableObjectSORandomPrefab(temelpte);
                    }
                    if (EasyGridBuilderProController.Instance.ReplaceEdge(this, buildableFreeObject, prefabs, currentObject, out var buildable))
                    {
                        currentObject = buildable;
                        DestroyTelempte();
                        if (parent.isRim)
                        {
                            relativeEdge.TryReplace(temelpte, prefabs);
                        }
                        if (parent.parentData.corners != null)
                        {
                            foreach (var item in parent.parentData.corners)
                            {
                                if (buildable.TryGetComponent<Building_Edge>(out var edge))
                                {
                                    var coenerOS = edge.corner;
                                    item.TryReplace(coenerOS);
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void SetParentCellData(Data_Cell cell)
        {
            if (cell != null)
            {
                parentCellData = cell;
                cell.edges.Add(this);
            }
        }

        public void Preview(BuildableObjectSO buildableObjectSO)
        {
            //有可能每帧都call
            if (preview != null) return;
            //baseModel.gameObject.SetActive(false);
            UpdateActiveBuildableObjectSORandomPrefab(buildableObjectSO);
        }

        public void CancelPreview()
        {
            Destroy(preview.gameObject);
            preview = null;
            //baseModel.gameObject.SetActive(true);
        }

        public void EnsurePreview()
        {
            //Destroy(baseModel.gameObject);
            //baseModel = preview.gameObject;
            //baseModel.GetComponent<Collider>().enabled = true;
            //preview = null;
        }

        private void UpdateActiveBuildableObjectSORandomPrefab(BuildableObjectSO buildableObjectSO)
        {
            float totalProbability;
            float randomPoint;

            totalProbability = CalculateTotalProbability(buildableObjectSO);
            randomPoint = UnityEngine.Random.Range(0f, totalProbability);

            var activeBuildableObjectSORandomPrefab = SelectPrefabByProbability(buildableObjectSO, randomPoint);

            var preview = Instantiate(activeBuildableObjectSORandomPrefab.objectPrefab, transform);
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
