using System;
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

namespace TestTD
{
    [Serializable]
    public class CellEvent : UnityEvent<Cell> {}
    
    public class CellManager : MonoBehaviour
    {
        [SerializeField, Tweakable] private CellEvent onSelectedTowerCell;
        
        public void HandleTowerSelected(CellObjectVariable tower)
        {
           onSelectedTowerCell?.Invoke(tower.CellObject.Cell);
        }
    }
}