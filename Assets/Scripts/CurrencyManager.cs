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
using Satisfy.Managers;

namespace TestTD.Systems
{
    [CreateAssetMenu(fileName = "Currency Manager", menuName = "Managers/Custom/Currency")]
    [HideMonoScript]	
    public class CurrencyManager : ScriptableObjectSystem
    {
        public override void Initialize()
        {
            // throw new NotImplementedException();
        }
    }
}