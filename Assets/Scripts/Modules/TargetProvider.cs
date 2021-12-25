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
            range.Entered.Subscribe(x =>
            {
                targets.Add(x.gameObject);

                if (currentTarget == null)
                {
                    currentTarget = x.gameObject;
                }

            }).AddTo(this);

            range.Exit.Subscribe(x =>
            {
                targets.Remove(x.gameObject);

                if (currentTarget == x.gameObject)
                {
                    currentTarget = GetTarget();
                }
            }).AddTo(this);
        }

        private GameObject GetTarget()
        {
            if (currentTarget != null)
            {
                return currentTarget.gameObject;
            }

            if (targets.Count == 0)
            {
                return null;
            }

            if (currentTarget == null)
            {
                targets.Remove(currentTarget);
                return GetTarget();
            }

            return targets.First();
        }
    }
}