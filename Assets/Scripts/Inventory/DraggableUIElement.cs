using System;
using Satisfy.Attributes;
using TestTD.Data;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TestTD.UI
{
    public abstract class DraggableUIElement : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        public enum  State  
        {
            Sleep, Drag, Highlighted
        }
       
        [SerializeField, Tweakable] private UnityEvent onStartDrag;
        [SerializeField, Tweakable] private UnityEvent onEndDrag;
        
        public IObservable<State> DragStart => stateChanged.Where(x => x == State.Drag);
        
        public IObservable<State> DragEnd => stateChanged.Pairwise()
            .Where(x => x.Previous == State.Drag && x.Current == State.Sleep)
            .Select(x => x.Current);

        public IObservable<State> HighlightEnd => stateChanged.Pairwise()
            .Where(x => (x.Previous == State.Drag || x.Previous == State.Highlighted) && x.Current == State.Sleep)
            .Select(x => x.Current);

        public IObservable<State> HighlightStart => stateChanged.Pairwise()
            .Where(x => x.Previous == State.Sleep && x.Current == State.Highlighted)
            .Select(x => x.Current);
        
        private IObservable<State> stateChanged => this.ObserveEveryValueChanged(x => x.state);
        
        private State state = State.Sleep;
        
        protected virtual void Start()
        {
            DragStart.Subscribe(_ => { onStartDrag?.Invoke(); }).AddTo(this);
            DragEnd.Subscribe(_ => { onEndDrag?.Invoke(); }).AddTo(this);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            state = State.Drag;
        }
        
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            state = State.Sleep;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            state = State.Highlighted;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            state = State.Sleep;
        }

        public abstract void Highlight();
        public abstract void Dehighlight();
    }
}