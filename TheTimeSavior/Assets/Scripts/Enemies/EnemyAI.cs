using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyAI : Enemy
    {

        #region Variabili

        public float AccelerationOnRun = 2.5f;
        public float DistanceFromPlayerToDeath = 10000;
        public Transform EnemyGround;
        public LayerMask GroundLayer;
        public float LimitPushBackVelocity = 100f;
        public float MaxRunningVelocity = 18f;
        public float PushBackOnHit = 6;
        public float RangeToRun = 12f, RangeToActivate = 20;
        public Transform RightLimit, LeftLimit;
        public Vector3 RightLimitPosition, LeftLimitPosition;
        public bool StayOnPlatform;
        public float WalkVelocity = 8f;
        [Range(0, -1)]
        public float PushBackVelocityModificatorOnPlatform = -1;
        public GameObject PlatformToStay;
        
        private bool _bIsFacingLeft = true;
        private bool _called;
        private Coroutine _lastRunningVelIncreaser, _runningVelIncreaser;
        private Animator _myAnimator;
        private float _myCurrentVelocity;
        private Rigidbody2D _myRigidBody2D;
        private Transform _myTransform;
        private Transform _playerTransform;
        private score_manager_script _scoreManager;

        #endregion

        private void Awake()
        {
            _scoreManager = GameObject.Find("Score_Manager").GetComponent<score_manager_script>();
            _myAnimator = GetComponent<Animator>();
            _myRigidBody2D = GetComponent<Rigidbody2D>();
            _myTransform = GetComponent<Transform>();
            _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            RightLimitPosition = RightLimit.position;
            LeftLimitPosition = LeftLimit.position;
            Destroy(RightLimit.gameObject);
            Destroy(LeftLimit.gameObject);
            SetStatus();
            SetTheRightFacing();
        }

        private void FixedUpdate()
        {
            SetStatus();
        }

        private void Update()
        {
            Debug.Log(MyStatus);
            switch (MyStatus)
            {
                case EStatus.Inactive:
                    InactiveScheme();
                    break;
                case EStatus.Walking:
                    SetTheRightFacing();
                    WalkingScheme();
                    break;
                case EStatus.Running:
                    SetTheRightFacing();
                    RunningScheme();
                    break;
                case EStatus.Patrol:
                    PatrolScheme();
                    break;
                default:
                    InactiveScheme();
                    break;
            }

            if (IsOutOfPosition() && StayOnPlatform && MyStatus == EStatus.Running)
                _myCurrentVelocity = _myCurrentVelocity * PushBackVelocityModificatorOnPlatform;

            _myRigidBody2D.velocity = new Vector2(_myCurrentVelocity, _myRigidBody2D.velocity.y);

            if (CalcDistanceFromPlayer() > DistanceFromPlayerToDeath)
                GetComponent<EnemyDeath>().DestroyEnemy(0);
        }

        //Cambia colore quando il player è in range
        //void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(transform.position, rangeToActivate);
        //    Gizmos.color = Color.black;
        //    Gizmos.DrawSphere(transform.position, rangeToRun);
        //}

        public void OnCollisionEnter2D(Collision2D collision)
        {
            var collidedGameObject = collision.gameObject;
            if (collidedGameObject.GetInstanceID() == PlatformToStay.GetInstanceID() && !StayOnPlatform)
            {
                StayOnPlatform = true;
            }
            else if (collidedGameObject.name == "Player")
            {
                var playerScript = collidedGameObject.GetComponent<player_script>();

                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(0);

                _scoreManager.EnemyDeathCountReset();

                if (!playerScript.isInvincible)
                    playerScript.SetInvincible();
            }

            if (collidedGameObject.CompareTag("TriggerGate"))
                GetComponent<EnemyDeath>().DestroyEnemy(0);
        }

        public void SetTheRightFacing()
        {
            _bIsFacingLeft = _myTransform.position.x > _playerTransform.position.x;
            _myTransform.localScale = new Vector3( _bIsFacingLeft ? -1 : 1, 1, 1 );
        }

        public void SetTrigger(bool activate = true)
        {
            MyStatus = activate ? EStatus.Running : EStatus.Inactive;
            _myAnimator.SetBool("Triggered", activate);
            _myAnimator.SetBool("Rotate", activate);
        }

        private void InactiveScheme()
        {
            _myRigidBody2D.velocity = new Vector2(0, _myRigidBody2D.velocity.y);
        }

        private void WalkingScheme()
        {
            _myCurrentVelocity = WalkVelocity * (_bIsFacingLeft ? -1 : 1);
        }

        private void RunningScheme()
        {
            if (!_called)
                _lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
        }

        private void PatrolScheme()
        {
            if (IsOutOfPosition())
                FlipFacing();
            
            _myCurrentVelocity = WalkVelocity * (_bIsFacingLeft ? -1 : 1);
        }

        private bool IsOutOfPosition()
        {
            return (
                _myTransform.position.x >= RightLimitPosition.x ||
                _myTransform.position.x <= LeftLimitPosition.x
             );
        }

        private void FlipFacing()
        {
            _myTransform.localScale = new Vector3(
                _myTransform.localScale.x * -1,
                _myTransform.localScale.y,
                _myTransform.localScale.z
             );
            
            _bIsFacingLeft = !_bIsFacingLeft;
        }

        private void SetStatus()
        {
            if (StayOnPlatform)
            {
                if (!PlayerOnPlatform())
                {
                    StopAllCoroutines();
                    MyStatus = EStatus.Patrol;
                    _myAnimator.SetBool("Triggered", false);
                    _myAnimator.SetBool("Rotate", false);
                }
                else
                {
                    MyStatus = EStatus.Running;
                    _myAnimator.SetBool("Triggered", true);
                    _myAnimator.SetBool("Rotate", true);
                }
            }
            else
            {
                var distance = CalcDistanceFromPlayer();
                
                if (MyStatus == EStatus.Running)
                    return;
                
                if (distance >= RangeToActivate)
                {
                    MyStatus = EStatus.Inactive;
                    _myAnimator.SetBool("Triggered", false);
                    _myAnimator.SetBool("Rotate", false);
                }
                else if (distance < RangeToActivate && distance >= RangeToRun)
                {
                    MyStatus = EStatus.Walking;
                    _myAnimator.SetBool("Triggered", false);
                    _myAnimator.SetBool("Rotate", false);
                }
                else if (distance < RangeToRun)
                {
                    MyStatus = EStatus.Running;
                    _myAnimator.SetBool("Triggered", true);
                    _myAnimator.SetBool("Rotate", true);
                }
            }
            
        }

        private bool PlayerOnPlatform()
        {
            return ( 
                _playerTransform.position.y >= RightLimitPosition.y && 
                _playerTransform.position.x <= RightLimitPosition.x && 
                _playerTransform.position.x >= LeftLimitPosition.x
             );
        }

        private float CalcDistanceFromPlayer()
        {
            return Mathf.Abs(_myTransform.position.x - _playerTransform.position.x);
        }

        private IEnumerator RunningVelIncreaser()
        {
            _called = true;
            _runningVelIncreaser = _lastRunningVelIncreaser;

            yield return new WaitForSeconds(0.2f);

            if (_bIsFacingLeft && _myCurrentVelocity > MaxRunningVelocity * -1)
                _myCurrentVelocity -= AccelerationOnRun;
            else if (_myCurrentVelocity < MaxRunningVelocity)
                _myCurrentVelocity += AccelerationOnRun;

            if (IsOutOfPosition() && StayOnPlatform)
                _myCurrentVelocity = 0;

            _myAnimator.SetFloat("Velocity", Mathf.Abs(_myCurrentVelocity));

            if (MyStatus == EStatus.Running)
            {
                _lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
                if (_runningVelIncreaser != null)
                    StopCoroutine(_runningVelIncreaser);
            }
            else
            {
                _called = false;
            }
        }
    }
}
