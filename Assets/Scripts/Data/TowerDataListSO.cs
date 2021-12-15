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

namespace TestTD.Data
{
    [CreateAssetMenu(fileName = "TowerDataListSO", menuName = "Variables/Custom/Tower Data List")]
    [HideMonoScript]	
    public class TowerDataListSO : ListSO<TowerData>
    {
      
    }
}