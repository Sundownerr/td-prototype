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
using Cinemachine;

namespace TestTD.Entities
{
    [HideMonoScript]
    public class CinemachinePathFollower : CinemachineDollyCart
    {
        [SerializeField, Tweakable] UnityEvent onMoveToStart;

        public void SetPath(GameObjectVariable pathGO)
        {
            m_Path = pathGO.Value.GetComponent<CinemachinePathBase>();
        }

        public void MoveToStart()
        {
            onMoveToStart?.Invoke();
            m_Position = 0;
        }
    }
}