using UnityEngine;

namespace UnityEditor.Rendering.HighDefinition
{
    /// <summary>
    /// The UI block that represents surface inputs for unlit materials.
    /// </summary>
    public class CrossSectionUIBlock : MaterialUIBlock
    {
        internal class Styles
        {
            public static GUIContent header { get; } = EditorGUIUtility.TrTextContent("CrossSection");
            public static GUIContent useSectionColorText = new GUIContent("UseSectionColor", " Albedo (RGB) and Transparency (A).");
            public static GUIContent sectionColorText = new GUIContent("SectionColor", " Albedo (RGB) and Transparency (A).");
            public static GUIContent inverseText = new GUIContent("Inverse");
            public static GUIContent retractBackfacesText = new GUIContent("RetractBackfaces");
            public static GUIContent backfaceShadowsText = new GUIContent("backfaceShadows");
            //_retractBackfaces
        }
        MaterialProperty useSectionColor = null;
        const string kuseSectionColor = "_useSectionColor";
        MaterialProperty sectionColor = null;
        const string kSectionColor = "_SectionColor";
        MaterialProperty inverse = null;
        const string kInverse = "_inverse";
        MaterialProperty retractBackfaces = null;
        const string kRetractBackfaces = "_retractBackfaces";
        MaterialProperty backfaceShadows = null;
        const string kBackfaceShadows = "_backfaceShadows";

        /// <summary>
        /// Constructs an UnlitSurfaceInputsUIBlock based on the parameters.
        /// </summary>
        /// <param name="expandableBit">Bit index used to store the foldout state.</param>
        public CrossSectionUIBlock(ExpandableBit expandableBit)
            : base(expandableBit, Styles.header)
        {
        }

        /// <summary>
        /// Loads the material properties for the block.
        /// </summary>
        public override void LoadMaterialProperties()
        {
            useSectionColor = FindProperty(kuseSectionColor);
            sectionColor = FindProperty(kSectionColor);
            inverse = FindProperty(kInverse, false);
            retractBackfaces = FindProperty(kRetractBackfaces, false);
            backfaceShadows = FindProperty(kBackfaceShadows, false);
        }

        /// <summary>
        /// Renders the properties in the block.
        /// </summary>
        protected override void OnGUIOpen()
        {
            bool showSectionColor = true;
            if (useSectionColor != null)
            {
                materialEditor.ShaderProperty(useSectionColor, "_useSectionColor");
                showSectionColor = (useSectionColor.floatValue == 1.0f);
            }
            if(showSectionColor) materialEditor.ColorProperty(sectionColor, Styles.sectionColorText.text);
            if (inverse!=null) materialEditor.ShaderProperty(inverse, Styles.inverseText.text);
            if (retractBackfaces != null) materialEditor.ShaderProperty(retractBackfaces, Styles.retractBackfacesText.text);
            Material material = materialEditor.target as Material;
            int bfShadows = material.GetInt("_CullMode");
            if (backfaceShadows != null && bfShadows == 2)
            {
                materialEditor.ShaderProperty(backfaceShadows, Styles.backfaceShadowsText.text);
                if (material.GetFloat(kBackfaceShadows) == 1) bfShadows = 0;
                material.SetInt("_CullModeShadows", bfShadows);
            }
        }
    }
}
