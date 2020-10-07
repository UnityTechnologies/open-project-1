#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    public class Gun : MonoBehaviour
    {
        [SerializeField]
        private Bullet _bulletPrefab;
        [SerializeField]
        private float _fireRate;
        [SerializeField]
        private float _bulletForce;
        [SerializeField]
        private Transform _bulletSpawn;

        private float _lastShotTime;

        public void Fire()
        {
            if (Time.time - _lastShotTime > _fireRate)
            {
                _lastShotTime = Time.time;
                Bullet bullet = Instantiate(_bulletPrefab, _bulletSpawn.position, Quaternion.identity);
                bullet.AddForce(_bulletSpawn.forward * _bulletForce);
            }
        }
    }
}
