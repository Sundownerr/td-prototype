using UnityEngine;
using UniRx;
using Satisfy.Attributes;
using System;
using UnityEngine.Events;

namespace TestTD.Entities
{
    public abstract class Shooter : InitializableModule
    {
        public IObservable<GameObject> ProjectileReachedTarget => projectileReachedTarget;
        [SerializeField, Editor_R] protected ProjectileLoader loader;
        [SerializeField, Editor_R] protected TargetProvider targetProvider;
        [SerializeField, Tweakable] protected UnityEvent<GameObject> hitAction;

        protected readonly Subject<GameObject> projectileReachedTarget = new Subject<GameObject>();
        protected abstract void Shoot(Transform projectile, Transform target);

        protected void HandleProjectileHitTarget(GameObject projectile)
        {
            projectileReachedTarget.OnNext(projectile);
            loader.DisposeProjectile(projectile);

            hitAction?.Invoke(projectile);
        }
    }
}