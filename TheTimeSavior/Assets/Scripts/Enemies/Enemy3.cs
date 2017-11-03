using System.Collections;
using UnityEngine;


namespace Enemies
{
    public class Enemy3 : Enemy
    {
        public Transform FirePoint;
        private Transform _gunTransform;
        public Transform Bullet;
        public Transform FlashPrefab;
        public float FireRate;
        private Vector3 _difference;
        private Transform _playerTransform;
        private bool _direction;
        private int _rotationOffset;
        private Coroutine _doShootCoroutine;
        private Coroutine _shootCoroutine;
        

        protected override void Awake()
        {
            _direction = true;
            _gunTransform = transform.Find("Gun");
            _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            base.Awake();
        }


        protected override void Update()
        {
            base.Update();
            RotateGun();
        }

        private void RotateGun ()
        {
            var gunPosition = Camera.main.ScreenToWorldPoint(_gunTransform.position);
            var playerPosition = Camera.main.ScreenToWorldPoint(_playerTransform.position);
            var angularCoefficent =
                (playerPosition.y - gunPosition.y) /
                (playerPosition.x - gunPosition.x);
            var rotZ = Mathf.Atan(angularCoefficent) * Mathf.Rad2Deg;
            var currentEulerAngle = _gunTransform.localEulerAngles;

            _gunTransform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        public override void SetTrigger(bool activate = true, bool byGun = false) {
            base.SetTrigger(activate, byGun);
            _shootCoroutine = activate ? StartCoroutine(Shoot()) : null;
        }

        IEnumerator Shoot()
        {
            yield return new WaitForSeconds(1f);
            _doShootCoroutine = StartCoroutine(
                DoRandomShoot(
                    Mathf.RoundToInt(
                        Random.Range(1, 5)
                    )
                )
            );
            _shootCoroutine = null;
        }

        IEnumerator DoRandomShoot (int bulletNumber)
        {
            createBullet();
            yield return new WaitForSeconds(FireRate);
            _doShootCoroutine = bulletNumber > 0 ? StartCoroutine(DoRandomShoot(bulletNumber - 1)) : null;
            if (_doShootCoroutine == null && MyStatus == EStatus.Triggered)
                _shootCoroutine = StartCoroutine(Shoot());
        }

        //(BIsFacingLeft ? -1 : 1)

        private void createBullet()
        {
            var bullet = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
            
            //Aggiusto direzione e orientamento della sprite
            bullet.GetComponent<EnemyBullet>().MoveSpeed *= (BIsFacingLeft ? -1 : 1);
            bullet.GetComponent<SpriteRenderer>().flipX = !BIsFacingLeft;

            Transform clone = Instantiate(FlashPrefab, FirePoint.position, FirePoint.rotation) as Transform;
            clone.parent = FirePoint;
            float size = Random.Range(0.6f, 0.9f);
            clone.localScale = new Vector3(size, size, size);
            Destroy(clone.gameObject, 0.04f);
        }

    }
}
