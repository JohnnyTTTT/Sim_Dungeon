using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_SubEdge : Entity
    {
        public static int DirectionHash = Shader.PropertyToID("_Direction");
        public Entity_Edge parent;
        public Entity_SubEdge relativeEdge;

        private Transform preview;

        public override void UpdateData()
        {
            Direction = DirectionUtility.GetDirection(transform.position, parent.transform.position);
            var element = ElementManager_Cell.Instance.GetElement(transform.position);
            SetParentCellElement_JustUseThisFunction(element);
        }

        public override void CreateOrUpdateModel()
        {
            var room = ElementManager_Cell.Instance.GetElement(ParentElement.Data.TileCoord).room;
            BuildableFreeObjectSO wallTemplete = null;
            if (room == null)
            {
                wallTemplete = SpawnManager.Instance.defaultWall;
            }
            //wallTemplete = RandomUtility.GetRandomElement(ParentElement.Data.TileCoord, room.biome.walls);
            if (wallTemplete != null)
            {
                TryAddOrUpdateModel(wallTemplete);
            }
        }


        protected override void SetParentCellElement_JustUseThisFunction(Element_Cell element)
        {
            //Old
            var horizontal = Direction == Direction.Up || Direction == Direction.Down;

            if (ParentElement != null)
            {
                if (horizontal)
                {
                    ParentElement.horizontalSubEdge = null;
                }
                else
                {
                    ParentElement.verticalSubEdge = null;
                }
            }


            base.SetParentCellElement_JustUseThisFunction(element);

            //New
            if (horizontal)
            {
                ParentElement.horizontalSubEdge = this;
            }
            else
            {
                ParentElement.verticalSubEdge = this;
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
