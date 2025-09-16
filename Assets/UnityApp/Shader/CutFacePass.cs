using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

class CutFacePass : CustomPass
{
    public Material cutFaceMat;
    public float cutHeight = 0.5f;

    protected override void Execute(CustomPassContext ctx)
    {
        // 设置高度
        cutFaceMat.SetFloat("_CutHeight", cutHeight);

        // 绘制切面（一个 Quad 或 Procedural Draw）
        // 这里用 DrawProcedural 画平面
        ctx.cmd.DrawProcedural(Matrix4x4.identity, cutFaceMat, 0,
            MeshTopology.Triangles, 6, 1);
    }
}
