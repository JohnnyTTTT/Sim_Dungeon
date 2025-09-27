using UnityEngine;
#if RENDERING_URP
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using WorldSpaceTransitions.CrossSection.URP;
using System.Linq;
#endif

namespace WorldSpaceTransitions
{
#if RENDERING_URP && UNITY_2023_2_OR_NEWER
    public static class CameraExtensions
    {
        public static ScriptableRendererData GetCurrentRendererData(this Camera camera)
        {
            var selectedRenderer = camera.GetUniversalAdditionalCameraData().scriptableRenderer;

            var currentRenderPipeline = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
            for (var i = 0; i < currentRenderPipeline.renderers.Length; ++i)
            {
                if (currentRenderPipeline.renderers[i] == selectedRenderer)
                {
                    return currentRenderPipeline.rendererDataList[i];
                }
            }

            return null;
        }
    }
#endif


    //[ExecuteInEditMode]
    public class OutlinesToggle : MonoBehaviour
    {
        public Material edgeMaterial;
        private Color c = Color.white;
        private bool kwdOn = true;
        private float outlineThickness = 1;
#if RENDERING_URP
        IntegratedEdgeFeature edgeFeature;
        [SerializeField] ScriptableRendererData rendererData;
#endif

        void Start()
        {
            kwdOn = edgeMaterial.IsKeywordEnabled("ALL_EDGES");
            outlineThickness = edgeMaterial.GetFloat("_outlineThickness");
            //renderPipelineAsset = GraphicsSettings.renderPipelineAsset;
#if RENDERING_URP
#if UNITY_2023_2_OR_NEWER
            rendererData = CameraExtensions.GetCurrentRendererData(Camera.main); //
#endif
            if(rendererData != null) edgeFeature = rendererData.rendererFeatures.OfType<IntegratedEdgeFeature>().FirstOrDefault();
            if (edgeFeature == null) return;
            //edgeFeature.allEdges = true;
#endif
        }
        void OnEnable()
        {
            kwdOn = edgeMaterial.IsKeywordEnabled("ALL_EDGES");
            outlineThickness = edgeMaterial.GetFloat("_outlineThickness");
            //renderPipelineAsset = GraphicsSettings.renderPipelineAsset;
        }

        void OnDisable()
        {
            if (kwdOn) edgeMaterial.EnableKeyword("ALL_EDGES");
            else edgeMaterial.DisableKeyword("ALL_EDGES");
            edgeMaterial.SetFloat("_all_edges", kwdOn ? 1 : 0);
            edgeMaterial.SetFloat("_outlineThickness", outlineThickness);
        }

        public void ShowEdges(bool val)
        {
            //GraphicsSettings.renderPipelineAsset = val? renderPipelineAsset: plainPipelineAsset;
            //QualitySettings.renderPipeline = val ? renderPipelineAsset : plainPipelineAsset;
            edgeMaterial.SetFloat("_outlineThickness", val? outlineThickness: 0);
#if RENDERING_URP
            if (edgeFeature != null) 
            { 
                edgeFeature.SetActive(val);
                return;
            }

#endif
            edgeMaterial.SetFloat("_outlineThickness", val ? outlineThickness : 0);

        }

        public void BackfaceEdgesOnly(bool val)
        {
#if RENDERING_URP
            if (edgeFeature != null)
            {
                edgeFeature.allEdges = !val;
                edgeFeature.Create();
                return;
            }
#endif
            edgeMaterial.SetFloat("_all_edges", val? 0:1);
            if (val)
            {
                edgeMaterial.DisableKeyword("ALL_EDGES");
            }
            else
            {
                edgeMaterial.EnableKeyword("ALL_EDGES");
            }

        }
    }
}