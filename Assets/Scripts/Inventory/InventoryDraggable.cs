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
        [SerializeField, Tweakable] private UnityEvent onHighlight;
        [SerializeField, Tweakable] private UnityEvent onDehighlight;

        public override void Highlight()
        {
           onHighlight?.Invoke();
        }

        public override void Dehighlight()
        {
            onDehighlight?.Invoke();
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