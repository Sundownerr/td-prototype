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
        private List<InventoryCell> cells;
        private List<InventoryCell> freeCells => cells.FindAll(x => x.IsFree);

        private readonly Dictionary<Transform, InventoryCell>
            usedCells = new Dictionary<Transform, InventoryCell>();

        private void Start()
        {
            cells = cellsParent.Children().OfComponent<InventoryCell>().ToList();
            var update = Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf);
        }

        public bool TryGetFreeClosestCell(Transform item, out InventoryCell closestCell)
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
            
            closestCell.CurrentState = InventoryCell.State.Selected;
            
            return true;
        }

        public bool TryGetFreeCell(out InventoryCell cell)
        {
            DeselectCells();
            cell = null;
            
            if (freeCells.Count == 0)
                return false;

            cell = freeCells.First();
            cell.CurrentState = InventoryCell.State.Selected;
            
            return cell != null;
        }

        public bool TryGetItemCell(Transform item, out InventoryCell cell)
        {
            return usedCells.TryGetValue(item, out cell);
        }

        public void DeselectCells()
        {
            var selectedCells = cells.Where(x => x.CurrentState == InventoryCell.State.Selected);

            foreach (var cell in selectedCells)
            {
                cell.CurrentState = InventoryCell.State.Free;
            }
        }

        public void ReleaseCell(Transform item)
        {
            usedCells[item].CurrentState = InventoryCell.State.Free;
            usedCells.Remove(item);
        }

        public void UseCell(InventoryCell cell, Transform item)
        {
            cell.CurrentState = InventoryCell.State.Used;
            usedCells.Add(item, cell);
        }
    }
}