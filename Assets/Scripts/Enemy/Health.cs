using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using TestTD.Data;
using Satisfy.Utility;
using Satisfy.Attributes;
using UnityEngine.Events;

namespace TestTD.Entities
{
    [Serializable]
    public class Health : InitializableModule
    {
        [SerializeField, LabelWidth(60), InlineProperty] private Memo<float> value;
        [SerializeField] private float max;

        [SerializeField, Tweakable] private UnityEvent onDead;
        [SerializeField, Tweakable] private UnityEvent onDamaged;
        [SerializeField, Tweakable] private UnityEvent onHealed;

        public Subject<float> Healed { get; } = new Subject<float>();
        public Subject<float> Damaged { get; } = new Subject<float>();
        public IObservable<float> ReachedZero => Damaged.Where(_ => Mathf.Approximately(value.Current, 0)).Take(1);
        public IObservable<float> FullHealed =>
            Healed.Where(_ => Mathf.Approximately(value.Current, max)).TakeUntil(ReachedZero);
        public IObservable<float> HalfHealed =>
            Healed.Where(_ => Mathf.Approximately(value.Current, max / 2f)).TakeUntil(ReachedZero);
        public IObservable<float> HalfDead =>
            Damaged.Where(_ => Mathf.Approximately(value.Current, max / 2f)).TakeUntil(ReachedZero);

        public float Value => value.Current;
        public float Max => max;

        public override void Initialize()
        {
            Healed.TakeUntil(ReachedZero).Subscribe(_ =>
            {
                onHealed?.Invoke();
                // Debug.Log("healed");
            }).AddTo(this);
            Damaged.TakeUntil(ReachedZero).Subscribe(_ =>
            {
                onDamaged?.Invoke();
                // Debug.Log("damaged");
            }).AddTo(this);
            ReachedZero.Take(1).Subscribe(_ =>
            {
                onDead?.Invoke();
                // Debug.Log("dead");
            }).AddTo(this);
        }

        public void Initialize(float current)
        {
            this.value = new Memo<float>(current);
            this.max = current;

            Initialize();
        }

        public void Initialize(float current, float max)
        {
            this.value = new Memo<float>(current);
            this.max = max;

            Initialize();
        }

        public void TakeHealing(float healValue)
        {
            // Debug.Log($"taking {healValue} heal ", gameObject);
            if (healValue < 0)
            {
                TakeDamage(healValue);
                return;
            }

            this.value.Current += healValue;
            Healed.OnNext(healValue);
        }

        public void TakeDamage(float damageValue)
        {
            // Debug.Log($"taking {damageValue} damage ", gameObject);
            if (damageValue < 0)
            {
                TakeHealing(damageValue);
                return;
            }

            this.value.Current -= damageValue;
            Damaged.OnNext(damageValue);
        }
    }
}