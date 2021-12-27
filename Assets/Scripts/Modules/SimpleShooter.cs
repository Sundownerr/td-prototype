using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using Satisfy.Attributes;
using System.Linq;
using Satisfy.Utility;

namespace TestTD.Entities
{
    [HideMonoScript]
    public class SimpleShooter : Shooter
    {
        [SerializeField, Tweakable] private float travelDistanceModifier;
        [SerializeField, Tweakable] private AnimationCurve xCurve = AnimationCurve.Linear(0, 0, 1, 0);
        [SerializeField, Tweakable] private AnimationCurve yCurve = AnimationCurve.Linear(0, 0, 1, 0);

        public override void Initialize()
        {
            loader.Loaded.Where(_ => targetProvider.Target != null)
                .Subscribe(x =>
                {
                    Shoot(loader.GiveProjectile().transform, targetProvider.Target.transform);
                }).AddTo(this);
        }

        protected override void Shoot(Transform projectile, Transform target)
        {
            // var previousProjectilePosition = projectile.position;
            // var distance = transform.GetDistanceTo(target);
            // var modifiedDistance = distance * travelDistanceModifier;
            // var targetPosition = target.position;
            //
            // Observable.EveryUpdate()
            //     .Take((int)modifiedDistance)
            //     .TakeUntil(targetProvider.TargetLost)
            //     .DoOnCompleted(() =>
            //     {
            //         HandleProjectileReachedEndPoint(projectile.gameObject, targetProvider.Target);
            //     })
            //     .DoOnCancel(() =>
            //     {
            //         attackFailed.OnNext(null);
            //     })
            //     .Subscribe(tick =>
            //     {
            //         if (target != null)
            //         {
            //             targetPosition = target.position;
            //         }
            //
            //         var travelPercent = tick / modifiedDistance;
            //
            //         projectile.rotation = GetProjectileRotation(ref previousProjectilePosition, projectile.position);
            //         projectile.position = Vector3.Lerp(transform.position, targetPosition, travelPercent);
            //     }).AddTo(this);
        }
    }
}