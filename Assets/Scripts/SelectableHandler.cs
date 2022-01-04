using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Variables;
using Satisfy.Attributes;
using UnityEngine.EventSystems;
using TestTD.Variables;
using Satisfy.Bricks;

namespace TestTD
{
    [HideMonoScript]
    public class SelectableHandler : MonoBehaviour
    {
        [Required]
        [SerializeField, Editor_R] private Raycaster raycaster;
        [SerializeField, Variable_R] private Satisfy.Bricks.Event pointerDown;
        [SerializeField, Tweakable] private UnityEvent<Selectable> onSelected;
        [SerializeField, Tweakable] private UnityEvent<Selectable> onDeselected;
        private Selectable currentSelected;

        private void Start()
        {
            Selectable highlighted = null;

            raycaster.Hit.Select(x => x.GetComponent<Selectable>())
                .Where(x => !x.IsSelected)
                .Subscribe(x =>
                {
                    if (highlighted != null && !highlighted.IsSelected)
                    {
                        highlighted.Dehighlight();
                    }

                    x.Highlight();
                    highlighted = x;
                }).AddTo(this);

            raycaster.LostHitObject.Subscribe(_ =>
            {
                if (highlighted == null)
                    return;
                
                highlighted.Dehighlight();
                highlighted = null;
            }).AddTo(this);
            
            var eventSystem = EventSystem.current;

            var click = pointerDown.Raised.Where(_ => enabled)
                                          .Where(_ => !eventSystem.IsPointerOverGameObject());

            click.Subscribe(x =>
            {
                DeselectCurrent();

                if (highlighted == null)
                    return;
                
                Select(highlighted);
            }).AddTo(this);

            this.ObserveEveryValueChanged(x => x.enabled)
                .Where(x => x == false)
                .Where(_ => highlighted != null)
                .Subscribe(x =>
                {
                    highlighted.Dehighlight();
                }).AddTo(this);
        }

        public void Select(Selectable selectable)
        {
            if (currentSelected == selectable)
                return;
            
            currentSelected = selectable;
            currentSelected.Select();

            onSelected?.Invoke(selectable);
        }

        public void Select(SelectableVariable selectableVariable)
        {
            Select(selectableVariable.Value);
        }

        public void DeselectCurrent()
        {
            if (currentSelected == null)
                return;
            
            currentSelected.Deselect();
            onDeselected?.Invoke(currentSelected);

            currentSelected = null;
        }
    }
}