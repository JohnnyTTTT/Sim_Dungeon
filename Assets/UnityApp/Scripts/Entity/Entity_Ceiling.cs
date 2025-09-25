using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Ceiling : Entity
    {
        public override void UpdateData()
        {
            var data = ElementManager_LargeCell.Instance.GetElement(transform.position);
            SetParentCellElement_JustUseThisFunction(data);
        }

        protected override void SetParentCellElement_JustUseThisFunction(Element_LargeCell element)
        {
            base.SetParentCellElement_JustUseThisFunction(element);
            element.ceiling = this;
            name = $"Ceiling - {element.Data.TileCoord.x},{element.Data.TileCoord.y}";
        }


        public override void CreateOrUpdateModel()
        {
            //var ceilingTemplete = SpawnManager.Instance.defaultCeiling;
            //if (ceilingTemplete != null)
            //{
            //    //TryAddOrUpdateModel(ceilingTemplete);
            //}
        }
    }
}
