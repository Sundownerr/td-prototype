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

namespace TestTD.Entities
{
    [HideMonoScript]
    public class TowerBehaviour : MonoBehaviour
    {
        void Start()
        {
            var update = Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf);

        }
    }
}