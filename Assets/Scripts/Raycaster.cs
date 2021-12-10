﻿using System;
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
    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject>
    {
    }

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

        [PropertySpace(10)] [SerializeField, BoxGroup("tweakable", false)]
        private LayerMask layerMask;

        [SerializeField, BoxGroup("tweakable", false)]
        private float maxDistance = 1000;

        [SerializeField, BoxGroup("tweakable", false)]
        private QueryTriggerInteraction triggerInteraction;

        [PropertySpace(10)] [SerializeField, BoxGroup("tweakable", false)]
        private bool retainLastHitObject;

        [SerializeField, BoxGroup("tweakable", false)]
        private bool ignoreGUI = false;

        [PropertySpace(10)] [SerializeField, BoxGroup("tweakable", false)]
        private GameObjectEvent onHit;

        [SerializeField, BoxGroup("tweakable", false)]
        private UnityEvent onLostHitObject;

        [SerializeField, BoxGroup("tweakable", false), HideInEditorMode]
        private GameObject hitObject;

        [SerializeField, BoxGroup("tweakable", false), HideInEditorMode]
        private GameObject nextHitObject;

        private bool isObjectLost;

        private RaycastHit castResult => fromCameraToPointer ? CastRayFromCamera() : CastRayFromStartPoint();

        private void Start()
        {
            var update = Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf);
            var eventSystem = EventSystem.current;

            update.Subscribe(_ =>
            {
                if (ignoreGUI)
                {
                    SetHitObject(castResult.transform);
                    return;
                }

                if (!eventSystem.IsPointerOverGameObject(0))
                {
                    SetHitObject(castResult.transform);
                }
            }).AddTo(this);
        }

        private RaycastHit CastRay(Vector3 origin, Vector3 direction)
        {
            Physics.Raycast(origin,
                direction,
                out var hitInfo,
                maxDistance,
                layerMask,
                triggerInteraction);

            return hitInfo;
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

        private void SetHitObject(Transform obj)
        {
            if (nextHitObject.transform == obj)
                return;

            if (obj == null)
            {
                HandleHitObjectLost();
                return;
            }

            nextHitObject = obj.gameObject;
            hitObject = nextHitObject;

            onHit?.Invoke(hitObject);
        }

        private void HandleHitObjectLost()
        {
            nextHitObject = null;

            if (retainLastHitObject)
            {
                return;
            }

            onLostHitObject?.Invoke();
            hitObject = null;
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

            if (hitObject == null)
            {
                Gizmos.DrawRay(rayStartPoint.position, rayStartPoint.forward * maxDistance);
                return;
            }

            var position = hitObject.transform.position;
            Gizmos.DrawLine(rayStartPoint.position, position);
            Gizmos.DrawSphere(position, 0.5f);
        }
#endif
    }
}