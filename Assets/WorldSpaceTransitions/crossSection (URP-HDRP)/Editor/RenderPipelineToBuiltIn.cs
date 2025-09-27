using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace WorldSpaceTransitions
{
#if UNITY_EDITOR
    public class RenderPipelineToBuiltIn : MonoBehaviour
    {
        [MenuItem("Edit/Render Pipeline/To Built in")]
        static void DoSomething()
        {
            GraphicsSettings.defaultRenderPipeline = null;
            QualitySettings.renderPipeline = null;
        }
    }
#endif
}