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

namespace TestTD.Systems
{
    [HideMonoScript]
    public class PerformanceManager : MonoBehaviour
    {
        void Start()
        {
            Application.targetFrameRate = 60;

        }
    }
}