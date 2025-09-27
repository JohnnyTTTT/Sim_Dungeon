using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

// Include material common properties names
using static UnityEngine.Rendering.HighDefinition.HDMaterialProperties;

namespace UnityEditor.Rendering.HighDefinition
{
    public enum UnlitSSMappingMode
    {
        UV0 = 0,
        Triplanar = 1,
        TriplanarNoBlending = 2
    }


    /// <summary>
    /// GUI for HDRP unlit shaders (does not include shader graphs)
    /// </summary>
    class UnlitStencilShowGUI : HDShaderGUI
    {
        MaterialUIBlockList uiBlocks = new MaterialUIBlockList
        {
            new SurfaceOptionUIBlock(MaterialUIBlock.ExpandableBit.Base, features: SurfaceOptionUIBlock.Features.Unlit),
            new UnlitSurfaceInputsUIBlock(MaterialUIBlock.ExpandableBit.Input),
            new TransparencyUIBlock(MaterialUIBlock.ExpandableBit.Transparency),
            new EmissionUIBlock(MaterialUIBlock.ExpandableBit.Emissive),
            new AdvancedOptionsUIBlock(MaterialUIBlock.ExpandableBit.Advance, AdvancedOptionsUIBlock.Features.Instancing | AdvancedOptionsUIBlock.Features.AddPrecomputedVelocity),
            new StencilShowOptionsUIBlock(MaterialUIBlock.ExpandableBit.Other)
        };

        protected override void OnMaterialGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            uiBlocks.OnGUI(materialEditor, props);
        }

        public override void ValidateMaterial(Material material) => UnlitAPI.ValidateMaterial(material);

        // All Setup Keyword functions must be static. It allow to create script to automatically update the shaders with a script if code change

    }
} // namespace UnityEditor
