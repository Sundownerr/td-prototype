using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using TestTD.Data;
using Satisfy.Utility;

namespace TestTD.Entities
{
    [Serializable]
    public class Health
    {
        [SerializeField, InlineProperty] private Memo<float> value;
        [SerializeField] private float max;

        public Subject<float> Healed { get; } = new Subject<float>();
        public Subject<float> Damaged { get; } = new Subject<float>();
        public IObservable<float> Dead => Damaged.Where(x => Mathf.Approximately(value.Current, 0));
        public IObservable<float> FullHealed => Healed.Where(x => Mathf.Approximately(value.Current, max));
        public IObservable<float> HalfHealed => Healed.Where(x => Mathf.Approximately(value.Current, max / 2f));
        public IObservable<float> HalfDead => Damaged.Where(x => Mathf.Approximately(value.Current, max / 2f));

        IObservable<float> currentHealthChanged => this.ObserveEveryValueChanged(x => x.value.Current)
                                                       .Skip(1)
                                                       .TakeUntil(Dead);

        public float Value => value.Current;
        public float Max => max;

        public Health(float current)
        {
            this.value = new Memo<float>(current);
            this.max = current;
        }

        public Health(float current, float max)
        {
            this.value = new Memo<float>(current);
            this.max = max;
        }

        public void Heal(float value)
        {
            if (value <= 0)
                return;

            this.value.Current += value;
            Healed.OnNext(value);
        }

        public void Damage(float value)
        {
            if (value >= 0)
                return;

            this.value.Current -= value;
            Damaged.OnNext(value);
        }
    }
}