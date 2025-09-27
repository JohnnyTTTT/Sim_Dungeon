using System;
using UnityEngine;

namespace UnityEditor.Rendering.HighDefinition
{
    /// <summary>
    /// The UI block that represents stencil and mapping mode inputs for CrossSection cap materials.
    /// based on UnlitSurfaceInputsUIBlock
    /// </summary>
    public class StencilShowOptionsUIBlock : MaterialUIBlock
    {

        internal class Styles
        {
            public static GUIContent header { get; } = EditorGUIUtility.TrTextContent("Stencil Show Options");
            public static GUIContent mappingModeText = new GUIContent("Mapping Mode");
            public static GUIContent texWorldScaleText = new GUIContent("World scale", "Sets the tiling factor HDRP applies to Planar/Trilinear mapping.");

        }

        static readonly string[] MappingModeNames = Enum.GetNames(typeof(UnlitSSMappingMode));

        MaterialProperty stencilMask = null;
        const string kStencilMask = "_StencilMask";
        MaterialProperty stencilOp = null;
        const string kStencilOp = "_StencilOp";
        MaterialProperty stencilReadMask = null;
        const string kStencilReadMask = "_StencilReadMask";
        static string m_MappingModeText = "_MappingMode";
        MaterialProperty m_MappingMode = null;

        MaterialProperty[] UVMode = new MaterialProperty[3];
        const string kUVMode = "_UVMode";
        MaterialProperty TexWorldScale = null;
        const string kTexWorldScale = "_TexWorldScale";
        MaterialProperty InvTilingScale = null;
        const string kInvTilingScale = "_InvTilingScale";

        ExpandableBit  m_ExpandableBit;
        int m_LayerCount;
        int m_LayerIndex;

        /// <summary>
        /// Constructs an StencilShowOptionsUIBlock based on the parameters.
        /// </summary>
        /// <param name="expandableBit">Bit index used to store the foldout state.</param>
        public StencilShowOptionsUIBlock(ExpandableBit expandableBit)
            : base(expandableBit, Styles.header)
        {
        }

        public override void LoadMaterialProperties()
        {
            stencilMask = FindProperty(kStencilMask);
            stencilReadMask = FindProperty(kStencilReadMask);
            //UVMode = FindPropertyLayered(kUVMode, m_LayerCount, true);
            m_MappingMode = FindProperty(m_MappingModeText);
            TexWorldScale = FindProperty(kTexWorldScale);
            InvTilingScale = FindProperty(kInvTilingScale);

            stencilOp = FindProperty(kStencilOp);
        }

        /// <summary>
        /// Renders the properties in the block.
        /// </summary>
        protected override void OnGUIOpen()
        {
            materialEditor.ShaderProperty(stencilMask, "_StencilMask");
            materialEditor.ShaderProperty(stencilOp, "_StencilOp");
            materialEditor.ShaderProperty(stencilReadMask, "_StencilReadMask");


            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            float val = EditorGUILayout.Popup(Styles.mappingModeText, (int)m_MappingMode.floatValue, MappingModeNames);
            if (EditorGUI.EndChangeCheck())
            {
                Material material = materialEditor.target as Material;
                Undo.RecordObject(material, "Change Mapping Mode");
                m_MappingMode.floatValue = val;
            }

            UnlitSSMappingMode mappingMode = (UnlitSSMappingMode)m_MappingMode.floatValue;
            //m_MappingMask.vectorValue = AxFGUI.AxFMappingModeToMask(mappingMode);

            if (mappingMode >= UnlitSSMappingMode.Triplanar)
            {
                ++EditorGUI.indentLevel;
                materialEditor.ShaderProperty(TexWorldScale, Styles.texWorldScaleText);
                --EditorGUI.indentLevel;
            }

            //materialEditor.TextureScaleOffsetProperty(baseColorMap[m_LayerIndex]);
            if (EditorGUI.EndChangeCheck())
            {
                // Precompute.
                //InvTilingScale[m_LayerIndex].floatValue = 2.0f / (Mathf.Abs(baseColorMap[m_LayerIndex].textureScaleAndOffset.x) + Mathf.Abs(baseColorMap[m_LayerIndex].textureScaleAndOffset.y));
                if (mappingMode >= UnlitSSMappingMode.Triplanar)
                {
                    InvTilingScale.floatValue = InvTilingScale.floatValue / TexWorldScale.floatValue;
                }
            }
        }
    }
}
