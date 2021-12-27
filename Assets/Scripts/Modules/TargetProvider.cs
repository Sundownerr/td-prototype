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
        [SerializeField, Editor_R] Range range;

        public GameObject Target => GetTarget();

        public IObservable<GameObject> TargetChanged => this.ObserveEveryValueChanged(x => x.currentTarget);
        public IObservable<GameObject> GotNewTarget => TargetChanged.Where(x => x != null);
        public IObservable<GameObject> TargetLost => TargetChanged.Where(x => x == null);

        public bool HaveTargets => targets.Count > 0;

        private readonly HashSet<GameObject> targets = new HashSet<GameObject>();
        private GameObject currentTarget;


        public override void Initialize()
        {
            range.Entered.Subscribe(AddTarget).AddTo(this);
            range.Exit.Subscribe(RemoveTarget).AddTo(this);
        }

        private void RemoveTarget(Collider targetCollider)
        {
            targets.Remove(targetCollider.gameObject);

            if (currentTarget == targetCollider.gameObject)
            {
                currentTarget = GetTarget();
            }
        }

        private void AddTarget(Collider targetCollider)
        {
            targets.Add(targetCollider.gameObject);

            if (currentTarget == null)
            {
                currentTarget = targetCollider.gameObject;
            }
        }

        private GameObject GetTarget()
        {
            if (currentTarget != null)
            {
                return currentTarget.gameObject;
            }

            targets.RemoveWhere(x => x == null);
            
            return targets.Count == 0 ? null : targets.First();
        }
    }
}