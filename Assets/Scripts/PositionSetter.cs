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

namespace Gongulus.Game
{
    [HideMonoScript]
    public class PositionSetter : MonoBehaviour
    {
        [SerializeField, Editor_R] GameObject source;

        public void SetPositionOf(GameObject target)
        {
            if (target == null)
                return;

            source.transform.position = target.transform.position;
        }
    }
}