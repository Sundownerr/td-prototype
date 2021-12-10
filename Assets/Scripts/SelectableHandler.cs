﻿using System;
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

namespace TestTD
{
    [HideMonoScript]
    public class SelectableHandler : MonoBehaviour
    {
        [SerializeField, Variable_R] private SelectableVariable highlighted;
        [SerializeField, Variable_R] private SelectableVariable selected;
        [SerializeField, Variable_R] private Variable pointerDown;
        private Selectable currentSelected;

        private void Start()
        {
            highlighted.Changed.Select(x => x.Previous)
                .Where(x => x != null)
                .Where(x => !x.IsSelected)
                .Subscribe(x =>
                {
                    x.Dehighlight();
                }).AddTo(this);

            highlighted.Changed.Select(x => x.Current)
                .Where(x => x != null)
                 .Where(x => !x.IsSelected)
                .Subscribe(x =>
                {
                    x.Highlight();
                }).AddTo(this);


            var eventSystem = EventSystem.current;

            var click = pointerDown.Published.Where(_ => enabled)
                                             .Where(_ => !eventSystem.IsPointerOverGameObject());

            click.Where(_ => highlighted.Value != null)
                .Subscribe(x =>
                {
                    DeselectCurrent();
                    Select(highlighted.Value);
                }).AddTo(this);

            click.Where(_ => highlighted.Value == null)
                .Subscribe(_ =>
                {
                    DeselectCurrent();
                }).AddTo(this);
        }

        public void Select(Selectable selectable)
        {
            selected.SetValue(selectable.gameObject);
            currentSelected = selectable;
            currentSelected.Select();
        }

        public void Select(SelectableVariable selectableVariable)
        {
            Select(selectableVariable.Value);
        }

        public void DeselectCurrent()
        {
            if (currentSelected != null)
            {
                currentSelected.Deselect();
                currentSelected = null;
            }

            if (selected.Value != null)
            {
                selected.Value.Deselect();
                selected.SetNullValue();
            }
        }
    }
}