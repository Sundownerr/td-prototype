using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Satisfy.Attributes;
using Satisfy.Entities;

namespace TestTD.Entities
{
    [HideMonoScript]
    public class Range : InitializableModule
    {
        [SerializeField, Editor_R] GameObject triggerPrefab;
        private CapsuleCollider col;

        public IObservable<Collider> Entered => trigger.Entered;
        public IObservable<Collider> Exit => trigger.Exit;

        private TriggerObject trigger;

        public override void Initialize()
        {
            trigger = Instantiate(triggerPrefab, transform.position, Quaternion.identity, transform)
                .GetComponent<TriggerObject>();

            col = trigger.Collider as CapsuleCollider;
        }

        public void SetRange(float value)
        {
            if (value < 0)
                return;

            col.radius = value;
        }
    }
}