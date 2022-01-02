using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using Satisfy.Attributes;

namespace TestTD.UI
{
    public class InventoryCellUI : MonoBehaviour
    {
        public enum State { Used, Selected, Free }

        public State CurrentState { get; set; } = State.Free;
        public bool IsFree => CurrentState != State.Used;
        
        [SerializeField, Tweakable] private UnityEvent onSelected;
        [SerializeField, Tweakable] private UnityEvent onUsed;
        [SerializeField, Tweakable] private UnityEvent onFree;

        private void Start()
        {
            var stateChanged = this.ObserveEveryValueChanged(x => x.CurrentState);

            stateChanged.Where(x => x == State.Selected)
                        .Subscribe(_ => { onSelected?.Invoke(); })
                        .AddTo(this);

            stateChanged.Where(x => x == State.Used)
                        .Subscribe(_ => { onUsed?.Invoke(); })
                        .AddTo(this);

            stateChanged.Where(x => x == State.Free)
                        .Subscribe(_ => { onFree?.Invoke(); })
                        .AddTo(this);
        }
    }
}