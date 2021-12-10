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
    public class CellObject : Selectable
    {
        public Cell Cell => cell;
        [SerializeField, Tweakable, HideInEditorMode]
        private Cell cell;
        [SerializeField, Tweakable] private UnityEvent onPlaced;

        public void UseCell(Cell cell)
        {
            this.cell = cell;
            onPlaced?.Invoke();
        }
    }
}