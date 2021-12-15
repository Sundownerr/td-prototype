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
using TestTD.Data;

namespace TestTD.Data
{
    [CreateAssetMenu(fileName = "Tower element list", menuName = "Variables/Custom/Tower element list")]
    [HideMonoScript]	
    public class TowerElementListSO : ListSO<TowerElement>
    {
    
    }
}