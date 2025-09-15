//using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif
//namespace Johnny.SimDungeon
//{
//    public class Building_Edge : BuildingEntity
//    {
//        public static int DirectionHash = Shader.PropertyToID("_Direction");

//        //public override void SetDirection(Direction dir)
//        //{
//        //    base.SetDirection(dir);
//        //    //if (Application.isPlaying)
//        //    //{
//        //    //    Debug.Log(Direction);
//        //    //    foreach (var edge in edges)
//        //    //    {
//        //    //        var renderers = edge.model.GetComponentsInChildren<Renderer>();
//        //    //        foreach (var renderer in renderers)
//        //    //        {
//        //    //            var mat = renderer.material;
//        //    //            mat.SetVector(DirectionHash, DirectionVector);

//        //    //        }
//        //    //    }
//        //    //}
//        //    //m_Camera = DungeonController.Instance.m_Camera.transform;
//        //}
//        //private void Update()
//        //{
//        //    var camPos = m_Camera.position;
//        //    var dir = camPos - transform.position;
//        //    float dot = Vector2.Dot(DirectionVector, dir);
//        //    bool facing = dot > 0f;
//        //    foreach (var item in edges)
//        //    {
//        //        item.upper.SetActive(!facing);
//        //    }
//        //}
//    }
//}
