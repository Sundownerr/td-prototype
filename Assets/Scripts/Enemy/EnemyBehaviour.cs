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

        private EnemyData data;
        private Health health;

        public void SetData(EnemyData data)
        {
            this.data = data;

            follower.m_Speed = data.Parameters.Speed.Value;
            health = new Health(data.Parameters.Health.Value);
        }

        private void Start()
        {

        }
    }

    public class Health
    {
        private float current;
        private float previous;
        private float max;

        public IObservable<float> Healed => currentHealthChanged.Where(x => x > previous);
        public IObservable<float> Damaged => currentHealthChanged.Where(x => x < previous);
        public IObservable<float> Dead => Damaged.Where(x => Mathf.Approximately(x, 0));
        public IObservable<float> FullHealed => Healed.Where(x => Mathf.Approximately(x, max));
        public IObservable<float> HalfHealed => Healed.Where(x => Mathf.Approximately(x, max / 2f));
        public IObservable<float> HalfDead => Damaged.Where(x => Mathf.Approximately(x, max / 2f));

        IObservable<float> currentHealthChanged => this.ObserveEveryValueChanged(x => x.current);

        public Health(float current)
        {
            this.current = current;
            this.max = current;
            previous = current;
        }

        public Health(float current, float max)
        {
            this.current = current;
            this.max = max;
            previous = current;
        }

        public void Heal(float value)
        {
            if (value <= 0)
                return;

            previous = current;
            current += value;
        }

        public void Damage(float value)
        {
            if (value >= 0)
                return;

            previous = current;
            current -= value;
        }
    }
}