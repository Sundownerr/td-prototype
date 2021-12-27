using UnityEngine;
using UniRx;
using Satisfy.Attributes;
using System;
using UnityEngine.Events;

namespace TestTD.Entities
{
    public abstract class Shooter : InitializableModule
    {
        public IObservable<GameObject> ProjectileReachedEndPoint => projectileReachedEndPoint;
        public IObservable<GameObject> StartAttacking => startAttacking;
        public IObservable<GameObject> AttackSuccesful => attackSuccesful;
        public IObservable<GameObject> AttackFailed => attackFailed;

        [SerializeField, Editor_R] protected ProjectileLoader loader;
        [SerializeField, Editor_R] protected TargetProvider targetProvider;
        [SerializeField, Tweakable] protected UnityEvent<GameObject> hitAction;

        protected readonly Subject<GameObject> projectileReachedEndPoint = new Subject<GameObject>();
        protected readonly Subject<GameObject> startAttacking = new Subject<GameObject>();
        protected readonly Subject<GameObject> attackSuccesful = new Subject<GameObject>();
        protected readonly Subject<GameObject> attackFailed = new Subject<GameObject>();
        
        protected abstract void Shoot(Transform projectile, Transform target);

        protected void HandleProjectileReachedEndPoint(GameObject projectile)
        {
            projectileReachedEndPoint.OnNext(projectile);
            hitAction?.Invoke(projectile);
        }

        protected Quaternion GetProjectileRotation(ref Vector3 previousPos, Vector3 currentPos)
        {
            var velocity = (currentPos - previousPos);
            previousPos = currentPos;

            return Quaternion.LookRotation(velocity, Vector3.up);
        }
    }
}