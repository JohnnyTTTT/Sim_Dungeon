using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using SoulGames.Utilities;
using System;
using System.Linq;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class BoxPlacementGhost : MonoBehaviour
    {
        [SerializeField] private BoxPlacementGhostIndicator[] m_Indicator;

        private EasyGridBuilderProXZ m_GridBuilderProSize2;
        private GridManager gridManager;
        private BuildableObjectSO activeBuildableObjectSO;
        private BuildableCornerObjectGhost m_BuildableCornerObjectGhost;
        private bool m_IsBoxPlacement;
        private Vector3[] m_GhostPostions = new Vector3[4];
        private void Start()
        {
            m_GridBuilderProSize2 = EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize2;
            gridManager = GridManager.Instance;

            gridManager.OnActiveBuildableSOChanged += OnActiveBuildableSOChanged;
            gridManager.OnEdgeObjectBoxPlacementStarted += OnEdgeObjectBoxPlacementStarted;
            gridManager.OnEdgeObjectBoxPlacementUpdated += OnEdgeObjectBoxPlacementUpdated;
            gridManager.OnEdgeObjectBoxPlacementFinalized += OnEdgeObjectBoxPlacementFinalized;
            gridManager.OnEdgeObjectBoxPlacementCancelled += OnEdgeObjectBoxPlacementCancelled;
            if (gridManager.TryGetBuildableCornerObjectGhost(out var buildableCornerObjectGhost))
            {

                m_BuildableCornerObjectGhost = buildableCornerObjectGhost;
            }
        }
        private void LateUpdate()
        {
            if (m_IsBoxPlacement)
            {
                if (m_BuildableCornerObjectGhost.TryGetBoxPlacementHolderObject(out var boxPlacementHolderObject))
                {
                    var count = boxPlacementHolderObject.childCount;
                    if (count == 1)
                    {
                        m_Indicator[0].Set(true, false, boxPlacementHolderObject.GetChild(0).GetChild(0).position, Vector3.zero);
                        m_Indicator[1].Set(true, false, Vector3.zero, Vector3.zero);
                        m_Indicator[2].Set(false, false, Vector3.zero, Vector3.zero);
                        m_Indicator[3].Set(false, false, Vector3.zero, Vector3.zero);
                    }
                    else if (count == 2)
                    {
                        for (int i = 0; i < boxPlacementHolderObject.childCount; i++)
                        {
                            m_GhostPostions[i] = boxPlacementHolderObject.GetChild(i).GetChild(0).position;
                        }
                        m_Indicator[0].Set(true, true, m_GhostPostions[0], m_GhostPostions[1]);
                        m_Indicator[1].Set(true, false, m_GhostPostions[1], Vector3.zero);
                        m_Indicator[2].Set(false, false, Vector3.zero, Vector3.zero);
                        m_Indicator[3].Set(false, false, Vector3.zero, Vector3.zero);
                    }
                    else if(count == 4)
                    {
                        for (int i = 0; i < boxPlacementHolderObject.childCount; i++)
                        {
                            m_GhostPostions[i] = boxPlacementHolderObject.GetChild(i).GetChild(0).position;
                        }
                        var orderPositions = m_GhostPostions.OrderBy(p => p.x).ThenBy(p => p.z).ToArray();
                        m_Indicator[0].Set(true, true, orderPositions[0], orderPositions[1]);
                        m_Indicator[1].Set(true, true, orderPositions[1], orderPositions[3]);
                        m_Indicator[2].Set(true, true, orderPositions[2], orderPositions[0]);
                        m_Indicator[3].Set(true, true, orderPositions[3], orderPositions[2]);
                    }
                }
            }
            else if ( activeBuildableObjectSO != null && !MouseInteractionUtilities.IsMousePointerOverUI())
            {
                m_Indicator[0].Set(true, false, m_BuildableCornerObjectGhost.transform.position, Vector3.zero);
            }

        }

        private void OnActiveBuildableSOChanged(EasyGridBuilderPro easyGridBuilderPro, BuildableObjectSO buildableObjectSO)
        {
            if (m_GridBuilderProSize2.GetActiveBuildableObjectSO() is BuildableEdgeObjectSO buildableEdgeObjectSO)
            {
                activeBuildableObjectSO = buildableEdgeObjectSO;
            }
            else
            {
                activeBuildableObjectSO = null;
            }
        }

        private Vector3 Snap(Vector3 vector)
        {
            var startCell = m_GridBuilderProSize2.GetActiveGridCellPosition(vector);
            return m_GridBuilderProSize2.GetActiveGridCellWorldPosition(startCell);
        }

        private void OnEdgeObjectBoxPlacementStarted(EasyGridBuilderPro easyGridBuilderPro, Vector3 boxPlacementStartPosition, EdgeObjectPlacementType placementType)
        {
            m_IsBoxPlacement = true;
        }

        private void OnEdgeObjectBoxPlacementUpdated(EasyGridBuilderPro easyGridBuilderPro, Vector3 boxPlacementEndPosition)
        {
        }

        private void OnEdgeObjectBoxPlacementFinalized(EasyGridBuilderPro easyGridBuilderPro)
        {
            m_IsBoxPlacement = false;
            Clear();
        }

        private void OnEdgeObjectBoxPlacementCancelled(EasyGridBuilderPro easyGridBuilderPro)
        {
            m_IsBoxPlacement = false;
            Clear();
        }

        private void Clear()
        {
            for (int i = 0; i < 4; i++)
            {
                m_Indicator[i].Set(false, false, Vector3.zero, Vector3.zero);
            }
        }

        public static void GetOtherTwoPoints(Vector3 startPoint, Vector3 endPoint, out Vector3 otherPoints1, out Vector3 otherPoints2)
        {
            // 第一个点使用起点的 x 坐标和终点的 y 坐标
            otherPoints1 = new Vector3(startPoint.x, 0f, endPoint.z);

            // 第二个点使用终点的 x 坐标和起点的 y 坐标
            otherPoints2 = new Vector3(endPoint.x, 0f, startPoint.z);
        }
    }
}
