using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Satisfy.Attributes;
using Satisfy.Bricks;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UniRx.Triggers;
using Event = Satisfy.Bricks.Event;
using Unit = Satisfy.Bricks.Unit;

namespace TestTD.UI
{
    public class InventoryUI : UIElement
    {
        [SerializeField, Variable_R] private GameObjectEvent itemRemoved;
        [SerializeField, Variable_R] private List<Unit> itemTypes;
        [SerializeField, Variable_R] private GameObjectVariable activeInventory;
        [SerializeField, Editor_R] private RectTransform inventoryRect;
        [SerializeField, Editor_R] private Transform itemParent;
        [SerializeField, Editor_R] private InventoryCellHandler cellHandler;
        [SerializeField, Editor_R] private UiDragHandler dragHandler;

        private readonly List<InventoryDraggable> items = new List<InventoryDraggable>();

        private void OnEnable()
        {
            var canPlaceItem = false;
            InventoryCell freeCell = null;
            
            
        // todo: fix used cells bug: sometimes replacing from one to another inventory leaves used cell 
            dragHandler.StartedDrag.TakeWhile(_ => enabled && gameObject.activeSelf)
                .Subscribe(item =>
                {
                    RemoveItem(item);
                    SetTopVisible(item.transform);

                    Observable.EveryUpdate().TakeWhile(_ => enabled && gameObject.activeSelf)
                        .Do(_ =>
                        {
                            var insideRect =
                                RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, Input.mousePosition);
                            
                            if(insideRect)
                            {
                                activeInventory.SetValue(gameObject);
                                return;
                            }

                            canPlaceItem = false;
                            cellHandler.DeselectCells();
                        })
                        .TakeUntil(dragHandler.EndedDrag)
                        .Where(_ => activeInventory.Value == gameObject)
                        .Subscribe(_ =>
                        {
                            var haveFreeCell = cellHandler.TryGetFreeCell(out freeCell);
                            
                            canPlaceItem = haveFreeCell;
                            
                            if (!canPlaceItem)
                            {
                                cellHandler.DeselectCells();
                            }

                            item.SetPlaceState(canPlaceItem
                                ? InventoryDraggable.PlaceState.Good
                                : InventoryDraggable.PlaceState.Bad);
                        }).AddTo(this);
                }).AddTo(this);

            dragHandler.EndedDrag
                .Where(_ => canPlaceItem)
                .Subscribe(item => { AddItem(item, freeCell); }).AddTo(this);
        }

        [Button]
        public void AddItem(InventoryDraggable item)
        {
            if (!cellHandler.TryGetFreeCell(out var cell))
                return;
            
            AddItem(item,cell);
        }

        private void AddItem(InventoryDraggable item, InventoryCell cell)
        {
            if (!itemTypes.Contains(item.Type))
                return;
            
            Debug.Log($"add {item.name} to {name}");

            item.SetPlaceState(InventoryDraggable.PlaceState.Placed);
            
            dragHandler.Place(item, cell.transform.position, 0.15f);
            dragHandler.Observe(item);

            cellHandler.UseCell(cell, item.transform);
            
            items.Add(item);
            
            item.transform.SetParent(itemParent);
        }

        public bool RemoveItem(InventoryDraggable item)
        {
            if (!items.Remove(item))
            {
                // Debug.LogError($"{name} dont have item {item.name}");
                return false;
            }

            cellHandler.ReleaseCell(item.transform);
            dragHandler.StopObserve(item);

            itemRemoved.Raise(item.gameObject);
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