#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float _destroyAfterSeconds;

        private Rigidbody _rb;
        private float _timePassed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void AddForce(Vector3 force)
        {
            _rb.AddForce(force);
            _timePassed = 0f;
        }

        private void Update()
        {
            if (_timePassed > _destroyAfterSeconds)
                Destroy(gameObject);

            _timePassed += Time.deltaTime;
        }
    }
}