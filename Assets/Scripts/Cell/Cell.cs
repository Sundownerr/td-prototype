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
    public class Cell : Selectable
    {
        [SerializeField, Tweakable] private UnityEvent onUsed;
        [SerializeField, Tweakable] private UnityEvent onFree;
        [SerializeField, Tweakable, HideInEditorMode]
        private bool isUsed;

        public bool IsUsed => isUsed;

        public void SetUsed()
        {
            isUsed = true;
            onUsed?.Invoke();
        }

        public void SetFree()
        {
            isUsed = false;
            onFree?.Invoke();
        }
    }
}