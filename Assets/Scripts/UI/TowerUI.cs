﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Attributes;
using Satisfy.Variables;
using TestTD.Variables;

namespace TestTD.UI
{
    public class UIElement : MonoBehaviour
    {
        [SerializeField, Editor_R] protected Transform content;
        [SerializeField, Tweakable] private UnityEvent onShow;
        [SerializeField, Tweakable] private UnityEvent onHide;

        public virtual void Show()
        {
            onShow?.Invoke();
        }

        public virtual void Hide()
        {
            onHide?.Invoke();
        }
    }

    public class TowerUI : UIElement
    {
        [SerializeField, Variable_R] private SelectableVariable selectedTower;
        [SerializeField, Variable_R] private GameObjectVariable mainCamera;
        private IObservable<long> update;
        private Camera cam;

        private void Start()
        {
            update = Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf);
            cam = mainCamera.Value.GetComponent<Camera>();

            Hide();

            update.Where(_ => selectedTower.Value != null)
                .Subscribe(x =>
                {
                    content.position = cam.WorldToScreenPoint(selectedTower.Value.transform.position);
                }).AddTo(this);

            selectedTower.Changed.Select(x => x.Current)
                .Where(x => x != null)
                .Subscribe(_ =>
                {
                    Show();
                }).AddTo(this);

            selectedTower.Changed.Select(x => x.Current)
                .Where(x => x == null)
                .Subscribe(_ =>
                {
                    Hide();
                }).AddTo(this);
        }
    }
}