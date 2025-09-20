using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_SubEdge : Entity<Data_Cell>
    {
        public static int DirectionHash = Shader.PropertyToID("_Direction");
        public FourDirectionalRotation direction;
        public Entity_EdgeGroup parent;
        public Entity_SubEdge relativeEdge;

        private Transform preview;

        public override void UpdateData()
        {
            var data = DataManager_Cell.Instance.GetData(transform.position);
            SetParentCellData_JustUseThisFunction(data);
        }

        protected override void SetParentCellData_JustUseThisFunction(Data_Cell cell)
        {
            //Old
            if (ParentData != null)
            {
                ParentData.edges.Remove(this);
            }

            base.SetParentCellData_JustUseThisFunction(cell);

            //New
            if (ParentData != null)
            {
                ParentData.edges.Add(this);
            }
        }

        protected override bool TryReplace(BuildableFreeObjectSO temelpte)
        {
            var reslut = base.TryReplace(temelpte);
            if (reslut)
            {
                var relativeRoom = DataManager_Room.Instance.GetData(relativeEdge.ParentData.Data.TileCoord);
                if (relativeRoom == null)
                {
                    relativeEdge.TryReplace(temelpte);
                }
            }
            return reslut;
        }

        public override void ApplyBiomeRule()
        {
            var room = DataManager_Room.Instance.GetData(ParentData.Data.TileCoord);
            if (room != null)
            {
                var wallTemplete = RandomUtility.GetRandomElement(room.biome.walls);
                if (wallTemplete != null)
                {
                    TryReplace(wallTemplete);
                }
            }
        }

        public override void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (currentObject != null)
            {
                currentObject.transform.position = position;
                currentObject.transform.rotation = rotation;
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
