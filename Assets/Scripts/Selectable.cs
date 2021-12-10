using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Attributes;

namespace TestTD
{
    [HideMonoScript]
    public class Selectable : MonoBehaviour
    {
        [SerializeField, Tweakable] private GameObject reference;
        [SerializeField, Tweakable, HideInEditorMode]
        private bool isSelected;
        [SerializeField, Tweakable] private UnityEvent onSelected;
        [SerializeField, Tweakable] private UnityEvent onHighlighted;
        [SerializeField, Tweakable] private UnityEvent onDeselected;
        [SerializeField, Tweakable] private UnityEvent onDehighlighted;

        public GameObject Reference => reference != null ? reference : gameObject;
        public bool IsSelected => isSelected;
        public IObservable<bool> Selected => this.ObserveEveryValueChanged(x => x.isSelected).Where(x => x == true);

        public void Select()
        {
            onSelected?.Invoke();
            isSelected = true;
        }

        public void Highlight()
        {
            onHighlighted?.Invoke();
        }

        public void Dehighlight()
        {
            onDehighlighted?.Invoke();
        }

        public void Deselect()
        {
            onDeselected?.Invoke();
            isSelected = false;
        }
    }
}