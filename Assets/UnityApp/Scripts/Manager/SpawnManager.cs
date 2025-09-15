using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Johnny.SimDungeon
{
    [System.Serializable]
    public class SpawnRulee
    {
        public RoomType roomType;
        public BiomeSO Biome;
    }
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<SpawnManager>();
                }
                return s_Instance;
            }

        }
        private static SpawnManager s_Instance;

        public SpawnRulee[] spawnRules;

        private System.Random rng;


        [Button]
        public void SpawnWorld()
        {
            var intSeed = unchecked((int)DungeonController.Instance.dungeon.Config.Seed);
            rng = new System.Random(intSeed);

            EasyGridBuilderProController.Instance.ChangeCurrentGrid(GridType.SizeOne);

            var rooms = DataManager_Room.Instance.roomList;
            foreach (var item in rooms)
            {
                if (item.roomType == RoomType.OriginaCave)
                {
                    Spawn(item, spawnRules[0]);
                }
            }
        }

        private void Spawn(Room room, SpawnRulee spawnRule)
        {
            var biome = spawnRule.Biome;
            var tiles = new List<Data_Tile>();
            foreach (var cell in room.containedCells)
            {
                foreach (var tile in cell.tiles)
                {
                    tiles.Add(tile);
                }
            }
            SpawnObjects(tiles, biome);
        }

        public void SpawnObjects(List<Data_Tile> tiles, BiomeSO  biome)
        {
            var spawnObjects = biome.prefabs;
            var gidBuilderPro = GridManager.Instance.GetActiveEasyGridBuilderPro();
            var verticalGridIndex = gidBuilderPro.GetActiveVerticalGridIndex();
            var rotation = RandomUtility.GetRandomFourDirectionalRotation();
            foreach (var tile in tiles)
            {
                foreach (var obj in spawnObjects)
                {
                    // ºÏ≤ÈπÊ‘Ú
                    if (obj.spawnRule == SpawnRule.OnlyEdge && !tile.isEdge)
                        continue;

                    if (NextFloat() <= obj.probability)
                    {
                        var randomPrefabs = UpdateActiveBuildableObjectSORandomPrefab(obj.prefab);
                        var spawned = gidBuilderPro.TryInitializeBuildableGridObjectSinglePlacement(tile.worldPosition,
                               obj.prefab, rotation, false, true, verticalGridIndex, true, out _, randomPrefabs);

                    }
                }
            }
        }



        private float NextFloat()
        {
            return (float)rng.NextDouble();
        }

        private BiomeSpawnObject PickByProbability(List<BiomeSpawnObject> list)
        {
            var total = list.Sum(o => o.probability);
            if (total <= 0f) return null;

            var roll = rng.NextDouble() * total;
            foreach (var obj in list)
            {
                if (roll < obj.probability)
                    return obj;
                roll -= obj.probability;
            }
            return null;
        }


        private BuildableObjectSO.RandomPrefabs UpdateActiveBuildableObjectSORandomPrefab(BuildableObjectSO buildableObjectSO)
        {
            float totalProbability;
            float randomPoint;

            totalProbability = CalculateTotalProbability(buildableObjectSO);
            randomPoint = UnityEngine.Random.Range(0f, totalProbability);

            return SelectPrefabByProbability(buildableObjectSO, randomPoint);
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
