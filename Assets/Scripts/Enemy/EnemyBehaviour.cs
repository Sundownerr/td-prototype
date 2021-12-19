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
        [SerializeField, HideInEditorMode] private Health health;

        [SerializeField, Tweakable] UnityEvent onReachedPlayer;
        [SerializeField, Tweakable] UnityEvent onDead;
        [SerializeField, Tweakable] UnityEvent onDamaged;
        [SerializeField, Tweakable] UnityEvent onHealed;

        public IObservable<float> ReachedPlayer => follower.ReachedEnd.Take(1);
        public Health Health => health;

        public void SetData(EnemyData value)
        {
            data = Instantiate(value);

            HandleFollower();
            HandleHealth();
        }

        private void HandleHealth()
        {
            health = new Health(data.Parameters.Health.Value);

            health.Damaged.Subscribe(_ => { onDead?.Invoke(); }).AddTo(this);
            health.Healed.Subscribe(_ => { onHealed?.Invoke(); }).AddTo(this);
            health.Dead.Subscribe(_ => { onDead?.Invoke(); }).AddTo(this);
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