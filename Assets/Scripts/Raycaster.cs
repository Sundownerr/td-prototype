using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Attributes;
using UnityEngine.EventSystems;

namespace TestTD
{
    [HideMonoScript]
    public class Raycaster : MonoBehaviour
    {
        [SerializeField, BoxGroup("tweakable", false), HorizontalGroup("tweakable/1")]
        private bool fromCameraToPointer;

        [SerializeField, BoxGroup("tweakable", false), HideIf("@fromCameraToPointer == true")]
        private Transform rayStartPoint;

        [SerializeField, BoxGroup("tweakable", false), HideIf("@fromCameraToPointer == false"),
         HorizontalGroup("tweakable/1"), HideLabel]
        private Camera rayCamera;

        [PropertySpace(10)]
        [SerializeField, BoxGroup("tweakable", false)]
        private LayerMask layerMask;

        [SerializeField, BoxGroup("tweakable", false)]
        private float maxDistance = 1000;

        [SerializeField, BoxGroup("tweakable", false)]
        private QueryTriggerInteraction triggerInteraction;

        [PropertySpace(10)]
        [SerializeField, BoxGroup("tweakable", false)]
        private bool retainLastHitObject;

        [SerializeField, BoxGroup("tweakable", false)]
        private bool ignoreGUI = false;

        [PropertySpace(10)]
        [SerializeField, BoxGroup("tweakable", false)]
        private UnityEvent<GameObject> onHit;

        [SerializeField, BoxGroup("tweakable", false)]
        private UnityEvent onLostHitObject;

        [SerializeField, BoxGroup("tweakable", false), HideInEditorMode]
        private Transform hitTransform;

        private bool isObjectLost;

        private RaycastHit castResult => fromCameraToPointer ? CastRayFromCamera() : CastRayFromStartPoint();
        private RaycastHit[] results = new RaycastHit[1];

        public GameObject HitObject => hitTransform == null ? null : hitTransform.gameObject;
        public Subject<GameObject> Hit { get; } = new Subject<GameObject>();
        public Subject<int> LostHitObject { get; } = new Subject<int>();

        private void Start()
        {
            var update = Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf);
            var eventSystem = EventSystem.current;

            update.Subscribe(_ =>
            {
                if (ignoreGUI)
                {
                    HandleHitObject(castResult.transform);
                    return;
                }

                if (!eventSystem.IsPointerOverGameObject(0))
                {
                    HandleHitObject(castResult.transform);
                }
            }).AddTo(this);
        }

        private RaycastHit CastRay(Vector3 origin, Vector3 direction)
        {
            var hits = Physics.RaycastNonAlloc(origin,
                direction,
                results,
                maxDistance,
                layerMask,
                triggerInteraction);

            return hits > 0 ? results[0] : default;
        }

        private RaycastHit CastRayFromCamera()
        {
            var ray = rayCamera.ScreenPointToRay(Input.mousePosition);
            return CastRay(ray.origin, ray.direction);
        }

        private RaycastHit CastRayFromStartPoint()
        {
            return CastRay(rayStartPoint.position, rayStartPoint.forward);
        }

        private void HandleHitObject(Transform hitObject)
        {
            if (hitObject == null)
            {
                HandleHitObjectLost();
                return;
            }

            if (hitObject == hitTransform)
                return;


            hitTransform = hitObject;

            onHit?.Invoke(hitObject.gameObject);
            Hit.OnNext(hitObject.gameObject);
        }

        private void HandleHitObjectLost()
        {
            if (retainLastHitObject)
                return;

            onLostHitObject?.Invoke();
            LostHitObject.OnNext(1);
            hitTransform = null;
        }

        public GameObject TryGetHitObject()
        {
            return castResult.transform == null ? null : castResult.transform.gameObject;
        }

        public void SetRetainLastHitObject(bool value)
        {
            retainLastHitObject = value;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (rayStartPoint == null)
                return;

            Gizmos.color = Color.cyan;

            if (hitTransform == null)
            {
                Gizmos.DrawRay(rayStartPoint.position, rayStartPoint.forward * maxDistance);
                return;
            }

            var position = hitTransform.transform.position;
            Gizmos.DrawLine(rayStartPoint.position, position);
            Gizmos.DrawSphere(position, 0.5f);
        }
#endif
    }
}