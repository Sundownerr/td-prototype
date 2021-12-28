using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Variables;
using Satisfy.Attributes;
using TestTD.Data;

namespace TestTD.Systems
{
    [HideMonoScript]	
    public class CameraManager : MonoBehaviour
    {
        [SerializeField, Editor_R] private Transform cameraTarget;

        [SerializeField, Editor_R] private Transform cameraMoveLimitMin;
        [SerializeField, Editor_R] private Transform cameraMoveLimitMax;
        [SerializeField, Tweakable] private float moveSensivity ;
        [SerializeField, Tweakable] private Vector2 zoomLimit;

        [SerializeField, Variable_R] private Vector2Variable pointerPositionOnScreen;

        private void Start()
        {
            var update = Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf);

            var screenHeightLimit = new Vector2(Screen.height * 0.05f, Screen.height * 0.95f);
            var screenWidthLimit = new Vector2(Screen.width * 0.05f, Screen.width * 0.95f);
            var pointerPosChanged = pointerPositionOnScreen.Changed.Select(x => x.Current)
                .Where(pos => (pos.x <= Screen.width && pos.x >= 0) && (pos.y <= Screen.height && pos.y >= 0));

            var movingDown = pointerPosChanged
                .Where(pos => pos.y <= screenHeightLimit.x)
                .Select(x => Vector3.right * moveSensivity);

            var movingUp = pointerPosChanged
                .Where(pos => pos.y >= screenHeightLimit.y)
                .Select(x => Vector3.left * moveSensivity);

            var movingLeft = pointerPosChanged
                .Where(pos => pos.x <= screenWidthLimit.x)
                .Select(x => Vector3.back * moveSensivity);

            var movingRight = pointerPosChanged
                .Where(pos => pos.x >= screenWidthLimit.y)
                .Select(x => Vector3.forward * moveSensivity);

            var stopMoving = pointerPosChanged.Where(pos =>
                (pos.x <= screenWidthLimit.y && pos.x >= screenWidthLimit.x) &&
                (pos.y <= screenHeightLimit.y && pos.y >= screenHeightLimit.x));

            var isMoving = false;

            stopMoving.Subscribe(_ =>
            {
                isMoving = false;
            }).AddTo(this);

            Observable.Merge(movingDown, movingLeft, movingRight, movingUp)
                .Where(_ => !isMoving)
                .Subscribe(x =>
                {
                    isMoving = true;
                    
                    Observable.EveryUpdate()
                        .TakeUntil(stopMoving)
                        .Subscribe(_ => { MoveCameraTarget(x); }).AddTo(this);
                }).AddTo(this);

            var smoothScrollDelta = 0f;
            
            Observable.EveryUpdate()
                .Select(x => Input.mouseScrollDelta.y)
                .Subscribe(x =>
                {
                    smoothScrollDelta = Mathf.Lerp(smoothScrollDelta, x, Time.deltaTime * 10f);

                    ChangeCameraHeight(-smoothScrollDelta);
                }).AddTo(this);
        }

        private void MoveCameraTarget(Vector3 offset)
        {
            var targetPos = cameraTarget.position + offset;
            var minPos = cameraMoveLimitMin.position;
            var maxPos = cameraMoveLimitMax.position;

            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.z = Mathf.Clamp(targetPos.z, minPos.z, maxPos.z);

            cameraTarget.position = Vector3.Lerp(cameraTarget.position, targetPos, Time.deltaTime * 2f);
        }

        private void ChangeCameraHeight(float offset)
        {
            var targetPos = cameraTarget.position;

            targetPos.y = Mathf.Clamp(targetPos.y + offset, zoomLimit.x, zoomLimit.y);

            cameraTarget.position = targetPos;
        }

        private void OnDrawGizmosSelected()
        {
            if(cameraMoveLimitMax == null || cameraMoveLimitMin == null)
                return;

            Gizmos.color = Color.blue;

            var limit1 = cameraMoveLimitMax.position;
            var limit2 = cameraMoveLimitMin.position;

            var limit3 = limit1;
            limit3.x = limit2.x;

            var limit4 = limit1;
            limit4.z = limit2.z;
            
         
            Gizmos.DrawLine(limit1,limit3);
            Gizmos.DrawLine(limit1, limit4);
            Gizmos.DrawLine(limit2, limit3);
            Gizmos.DrawLine(limit2, limit4);
            
            var cameraTargetPosition = cameraTarget.position;
            Gizmos.DrawLine(cameraTargetPosition, limit1);
            Gizmos.DrawLine(cameraTargetPosition, limit2);
            Gizmos.DrawLine(cameraTargetPosition, limit3);
            Gizmos.DrawLine(cameraTargetPosition, limit4);
        }
    }
}