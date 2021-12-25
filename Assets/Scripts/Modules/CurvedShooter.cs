using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using Satisfy.Attributes;
using System.Linq;
using Satisfy.Utility;

namespace TestTD.Entities
{
    [HideMonoScript]
    public class CurvedShooter : Shooter
    {
        [SerializeField, Tweakable] private float travelDistanceModifier;
        [SerializeField, Tweakable] private AnimationCurve xCurve = AnimationCurve.Linear(0, 0, 1, 0);
        [SerializeField, Tweakable] private AnimationCurve yCurve = AnimationCurve.Linear(0, 0, 1, 0);

        public override void Initialize()
        {
            loader.Loaded.Where(_ => targetProvider.Target != null)
                .Subscribe(x =>
                {
                    Debug.Log("shooter shooting");
                    Shoot(x.transform, targetProvider.Target.transform);
                }).AddTo(this);
        }

        protected override void Shoot(Transform projectile, Transform target)
        {
            var previousProjectilePosition = projectile.position;
            var distance = transform.GetDistanceTo(target);
            var modifiedDistance = distance * travelDistanceModifier;
            var targetPosition = target.position;

            Observable.EveryUpdate()
                .Take((int)modifiedDistance)
                .Subscribe(tick =>
                {
                    if (target != null)
                    {
                        targetPosition = target.position;
                    }

                    var travelPercent = tick / modifiedDistance;

                    projectile.rotation = GetProjectileRotation(ref previousProjectilePosition, projectile.position);
                    projectile.position = Vector3.Lerp(transform.position, targetPosition, travelPercent);
                    projectile.localPosition = GetCurveOffsettedPosition(projectile.localPosition, distance, travelPercent);
                }, () =>
                {
                    HandleProjectileHitTarget(projectile.gameObject);
                }).AddTo(this);
        }

        private Vector3 GetCurveOffsettedPosition(Vector3 position, float distance, float travelPercent)
        {
            position.x += xCurve.Evaluate(travelPercent) * distance;
            position.y += yCurve.Evaluate(travelPercent) * distance;

            return position;
        }

        private Quaternion GetProjectileRotation(ref Vector3 previousPos, Vector3 currentPos)
        {
            var velocity = (currentPos - previousPos);
            previousPos = currentPos;

            return Quaternion.LookRotation(velocity, Vector3.up);
        }
    }

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
                    Shoot(x.transform, targetProvider.Target.transform);
                }).AddTo(this);
        }

        protected override void Shoot(Transform projectile, Transform target)
        {
            var previousProjectilePosition = projectile.position;
            var distance = transform.GetDistanceTo(target);
            var modifiedDistance = distance * travelDistanceModifier;
            var targetPosition = target.position;

            Observable.EveryUpdate()
                .Take((int)modifiedDistance)
                .Subscribe(tick =>
                {
                    if (target != null)
                    {
                        targetPosition = target.position;
                    }

                    var travelPercent = tick / modifiedDistance;

                    projectile.rotation = GetProjectileRotation(ref previousProjectilePosition, projectile.position);
                    projectile.position = Vector3.Lerp(transform.position, targetPosition, travelPercent);
                }, () =>
                {
                    HandleProjectileHitTarget(projectile.gameObject);
                }).AddTo(this);
        }

        private Quaternion GetProjectileRotation(ref Vector3 previousPos, Vector3 currentPos)
        {
            var velocity = (currentPos - previousPos);
            previousPos = currentPos;

            return Quaternion.LookRotation(velocity, Vector3.up);
        }
    }
}