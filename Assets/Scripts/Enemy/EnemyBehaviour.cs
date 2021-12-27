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

namespace TestTD.Entities
{
    [HideMonoScript]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField, Editor_R] private CinemachinePathFollower follower;
        [SerializeField, HideInEditorMode] private EnemyData data;
        [SerializeField, Editor_R] private Health health;
        [SerializeField, Tweakable] private UnityEvent onReachedPlayer;

        public IObservable<float> ReachedPlayer => follower.ReachedEnd;
        public Health Health => health;

        public void SetData(EnemyData value)
        {
            data = Instantiate(value);

            HandleFollower();
            
            health.Initialize(data.Parameters.Health.Value);
        }

        private void HandleFollower()
        {
            follower.m_Speed = data.Parameters.Speed.Value;

            ReachedPlayer.Subscribe(_ =>
            {
                onReachedPlayer?.Invoke();
                Destroy(gameObject, 0.3f);
            }).AddTo(this);

            data.Parameters.ObserveEveryValueChanged(x => x.Speed.Value)
                .Subscribe(x =>
                {
                    follower.m_Speed = x;
                }).AddTo(this);
        }
    }
}