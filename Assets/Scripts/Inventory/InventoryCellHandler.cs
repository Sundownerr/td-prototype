using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.Linq;
using System.Linq;
using Satisfy.Attributes;
using Satisfy.Utility;
using Sirenix.Utilities;

namespace TestTD.UI
{
    public class InventoryCellHandler : MonoBehaviour
    {
        [SerializeField, Tweakable] private float distanceToCell;
        [SerializeField, Editor_R] private GameObject cellsParent;
        private List<InventoryCellUI> cells;
        private List<InventoryCellUI> freeCells => cells.FindAll(x => x.IsFree);

        private readonly Dictionary<Transform, InventoryCellUI>
            usedCells = new Dictionary<Transform, InventoryCellUI>();

        private void Start()
        {
            cells = cellsParent.Children().OfComponent<InventoryCellUI>().ToList();
            var update = Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf);
        }

        public bool TryGetFreeClosestCell(Transform item, out InventoryCellUI closestCell)
        {
            DeselectCells();

            closestCell = null;
          
            if (freeCells.Count == 0)
                return false;

            closestCell = freeCells.Where(x => x.transform.GetDistanceTo(item) < distanceToCell)
                .OrderBy(x => x.transform.GetDistanceTo(item))
                .FirstOrDefault();
         
            if (closestCell == null) 
                return false;
            
            closestCell.CurrentState = InventoryCellUI.State.Selected;
            
            return true;
        }

        public bool TryGetFreeCell(out InventoryCellUI cell)
        {
            cell = null;
            
            if (freeCells.Count == 0)
                return false;

            cell = freeCells.First();
            
            return cell != null;
        }

        public bool TryGetItemCell(Transform item, out InventoryCellUI cell)
        {
            return usedCells.TryGetValue(item, out cell);
        }

        private void DeselectCells()
        {
            var selectedCells = cells.Where(x => x.CurrentState == InventoryCellUI.State.Selected);

            foreach (var cell in selectedCells)
            {
                cell.CurrentState = InventoryCellUI.State.Free;
            }
        }

        public void ReleaseCell(Transform item)
        {
            usedCells[item].CurrentState = InventoryCellUI.State.Free;
            usedCells.Remove(item);
        }

        public void UseCell(InventoryCellUI cell, Transform item)
        {
            cell.CurrentState = InventoryCellUI.State.Used;
            usedCells.Add(item, cell);
        }
    }
}