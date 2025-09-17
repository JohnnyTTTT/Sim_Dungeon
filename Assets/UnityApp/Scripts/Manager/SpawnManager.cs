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
        private Dictionary<RoomType, SpawnRulee> spawnRulesDic = new Dictionary<RoomType, SpawnRulee>();

        private void Start()
        {
            foreach (var item in spawnRules)
            {
                spawnRulesDic[item.roomType] = item;
            }
        }
        [Button]
        public void SpawnWorld()
        {
            if (!Application.isPlaying) return;

            RandomUtility.SetSeed((int)DungeonController.Instance.dungeon.Config.Seed);

            EasyGridBuilderProController.Instance.ChangeCurrentGrid(GridType.SizeTwo);

            var rooms = DataManager_Room.Instance.roomList;
            foreach (var room in rooms)
            {
                if (spawnRulesDic.TryGetValue(room.roomType, out var rule))
                {
                    ApplyRule(room, rule);
                }
            }


            //var rooms = DataManager_Room.Instance.roomList;
            //foreach (var item in rooms)
            //{
            //    if (item.roomType == RoomType.OriginaCave)
            //    {
            //        Spawn(item, spawnRules[0]);
            //    }
            //}
        }
        private bool a;
        private void ApplyRule(Room room, SpawnRulee rule)
        {
            var cells = room.containedCells;
            foreach (var cell in cells)
            {
                foreach (var edge in cell.edges)
                {
                    //if (!a)
                    {
                        var wallTemplete = RandomUtility.GetRandomElement(rule.Biome.walls);
                        edge.TryReplace(wallTemplete);
                        a = true;
                    }

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

        public void SpawnObjects(List<Data_Tile> tiles, BiomeSO biome)
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

                    if (RandomUtility.NextFloat() <= obj.probability)
                    {
                        var randomPrefabs = RandomUtility.UpdateBuildableObjectSORandomPrefab(obj.prefab);
                        var spawned = gidBuilderPro.TryInitializeBuildableGridObjectSinglePlacement(tile.worldPosition,
                               obj.prefab, rotation, true, true, verticalGridIndex, true, out _, randomPrefabs);

                    }
                }
            }
        }


        private BiomeSpawnObject PickByProbability(List<BiomeSpawnObject> list)
        {
            var total = list.Sum(o => o.probability);
            if (total <= 0f) return null;

            var roll = RandomUtility.GetRandomFloat(total);
            foreach (var obj in list)
            {
                if (roll < obj.probability)
                    return obj;
                roll -= obj.probability;
            }
            return null;
        }



    }
}
