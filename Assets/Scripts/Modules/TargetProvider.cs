using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using Satisfy.Attributes;
using System.Linq;

namespace TestTD.Entities
{
    [HideMonoScript]
    public class TargetProvider : InitializableModule
    {
        [SerializeField, Editor_R] private Range range;

        public GameObject Target { get; private set; }

        public IObservable<GameObject> TargetChanged => this.ObserveEveryValueChanged(x => x.Target);
        public IObservable<GameObject> GotNewTarget => TargetChanged.Where(x => x != null);
        public IObservable<GameObject> TargetLost => TargetChanged.Where(x => x == null);

        public bool HaveTargets => targets.Count > 0;

        private readonly HashSet<GameObject> targets = new HashSet<GameObject>();

        public override void Initialize()
        {
            range.Entered.Subscribe(AddTarget).AddTo(this);
            range.Exit.Subscribe(RemoveTarget).AddTo(this);

            Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf)
                .Where(_ => targets.Count > 0)
                .Where(_ => Target == null).Subscribe(_ =>
                {
                    Target = GetTarget();
                }).AddTo(this);
        }

        private GameObject GetTarget()
        {
            targets.RemoveWhere(x => x == null);

            return targets.Count <= 0 ? null : targets.First();
        }

        private void AddTarget(Collider targetCollider)
        {
            targets.Add(targetCollider.gameObject);

            if (Target == null)
            {
                Target = targetCollider.gameObject;
            }
            
            Debug.Log($"add target: {targets.Count}");
        }

        private void RemoveTarget(Collider targetCollider)
        {
            if (targetCollider == null)
            {
                targets.RemoveWhere(x => x == null);
                return;
            }
            targets.Remove(targetCollider.gameObject);

           if (Target == targetCollider.gameObject)
           {
               Target = GetTarget();
           }
           
           Debug.Log($"remove target: {targets.Count}");
        }
    }
}