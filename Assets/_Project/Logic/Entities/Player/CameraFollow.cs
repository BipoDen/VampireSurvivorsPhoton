using System;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Entities.Player
{
    public class CameraFollow : SimulationBehaviour
    {
        [SerializeField] private Vector3 _offset = new Vector3(0f, 0f, -10f);
        [SerializeField] private float _smoothSpeed = 0.125f;

        private Transform _target;
        private Vector3 _smoothedVelocity;

        public void SetTarget(Transform target) => _target = target;

        private void LateUpdate()
        {
            if (_target == null) return;

            Vector3 desiredPos = _target.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref _smoothedVelocity, _smoothSpeed);
        }
    }
}