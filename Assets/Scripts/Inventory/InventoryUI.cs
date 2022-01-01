using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Satisfy.Attributes;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Event = Satisfy.Bricks.Event;
using Unit = Satisfy.Bricks.Unit;

namespace TestTD.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField, Variable_R] private List<Unit> itemTypes;
        [SerializeField, Editor_R] private InventoryCellHandler cellHandler;
        [SerializeField, Editor_R] private UiDragHandler dragHandler;

        private readonly List<InventoryDraggable> items = new List<InventoryDraggable>();

        private void OnEnable()
        {
            var canPlaceItem = false;
            var isItemFromThisInventory = false;    
            InventoryCellUI closestCell = null;
            InventoryCellUI usedCell = null;

            dragHandler.StartedDrag.TakeWhile(_ => enabled && gameObject.activeSelf)
                .Do(item =>
                {
                    isItemFromThisInventory = cellHandler.TryGetItemCell(item.transform, out usedCell);
                    
                     if (!isItemFromThisInventory)
                        return;
                     
                     RemoveItem(item);
                })
                .Subscribe(item =>
                {
                    SetTopVisible(item.transform);

                    Observable.EveryUpdate().TakeWhile(_ => enabled && gameObject.activeSelf)
                        .TakeUntil(dragHandler.EndedDrag)
                        .Subscribe(_ =>
                        {
                            canPlaceItem = cellHandler.TryGetFreeClosestCell(item.transform, out closestCell);

                            item.SetPlaceState(canPlaceItem
                                ? InventoryDraggable.PlaceState.Good
                                : InventoryDraggable.PlaceState.Bad);
                        }).AddTo(this);
                }).AddTo(this);

            dragHandler.EndedDrag
                .Where(_ => canPlaceItem)
                .Subscribe(item => { AddItem(item, closestCell); }).AddTo(this);

            dragHandler.EndedDrag
                .Where(_ => !canPlaceItem)
                .Where(_ => isItemFromThisInventory)
                .Subscribe(item => { AddItem(item, usedCell); }).AddTo(this);
        }

        [Button]
        public void AddItem(InventoryDraggable item)
        {
            if (!cellHandler.TryGetFreeCell(out var cell))
                return;
            
            AddItem(item,cell);
        }

        private void AddItem(InventoryDraggable item, InventoryCellUI cell)
        {
            if (!itemTypes.Contains(item.Type))
                return;

            item.SetPlaceState(InventoryDraggable.PlaceState.Placed);
            
            dragHandler.Place(item, cell.transform.position);
            dragHandler.Observe(item);

            cellHandler.UseCell(cell, item.transform);
            
            items.Add(item);
        }

        public bool RemoveItem(InventoryDraggable item)
        {
            if (!items.Remove(item))
            {
                Debug.LogError($"{name} dont have item {item.name}");
                return false;
            }

            cellHandler.ReleaseCell(item.transform);
            dragHandler.StopObserve(item);
            return true;
        }

        private void SetTopVisible(Transform item)
        {
            if (items.Count == 0)
                return;

            var topItemHierarchyIndex = items.Select(x => x.transform.GetSiblingIndex())
                                             .OrderByDescending(x => x)
                                             .First();

            if (topItemHierarchyIndex > item.transform.GetSiblingIndex())
                item.transform.SetSiblingIndex(topItemHierarchyIndex);
        }
    }
}