using System;
using Satisfy.Attributes;
using Satisfy.Bricks;
using UnityEngine;
using UnityEngine.Events;

namespace TestTD.UI
{
    public class InventoryDraggable : DraggableUIElement
    {
        public enum PlaceState
        {
            Good,
            Bad,
            Placed
        }
        
        public Unit Type => type;
        [SerializeField, Variable_R] private Unit type;
        [SerializeField, Tweakable] private UnityEvent onBadPlace;
        [SerializeField, Tweakable] private UnityEvent onGoodPlace;
        [SerializeField, Tweakable] private UnityEvent onPlaced;

        public override void Highlight()
        {
            Debug.Log($"Highlight {name}");
            // throw new NotImplementedException();
        }

        public override void Dehighlight()
        {
            Debug.Log($"DeHighlight {name}");
            // throw new NotImplementedException();
        }
        
        public void SetPlaceState(PlaceState state)
        {
            switch (state)
            {
                case PlaceState.Good:
                {
                    onGoodPlace?.Invoke();
                }
                    return;

                case PlaceState.Bad:
                {
                    onBadPlace?.Invoke();
                }
                    return;

                case PlaceState.Placed:
                {
                    onPlaced?.Invoke();
                }
                    return;
                default:
                    return;
            }
        }
    }
}