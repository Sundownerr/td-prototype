using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace TestTD.Entities
{
    public class MouseInteractionHandler : MonoBehaviour
    {
        [SerializeField, Tweakable] private UnityEvent onMouseEnter;
        [SerializeField, Tweakable] private UnityEvent onMouseExit;
        [SerializeField, Tweakable] private UnityEvent onMouseDown;
        [SerializeField, Tweakable] private UnityEvent onMouseUp;

        private void OnMouseEnter() { onMouseEnter?.Invoke(); }

        private void OnMouseExit() { onMouseExit?.Invoke(); }

        private void OnMouseDown() { onMouseDown?.Invoke(); }

        private void OnMouseUp() { onMouseUp?.Invoke(); }
    }
}
