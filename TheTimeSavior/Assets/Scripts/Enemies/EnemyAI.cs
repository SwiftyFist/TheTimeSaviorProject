using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enemies
{
    public class EnemyAI : Enemy
    {
        #region Variabili
        
        public bool StayOnPlatform;
        public float PushBackVelocityModificatorOnPlatform = -1;
        public GameObject PlatformToStay;
        public float JumpForce = 10f;
        public float StartJumpOffSet = 5;
        private Vector3 _rightLimitPosition, _leftLimitPosition;
        private bool _stayOnPlatform;

        #endregion

        protected override void Awake()
        {
            PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
            base.Awake();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void OnLevelFinishedLoading(Scene arg0, LoadSceneMode arg1)
        {
            if (PlatformToStay != null) SetPlatform(PlatformToStay);
        }

        protected override void Update()
        {
            if (StayOnPlatform && PlatformToStay != null && _rightLimitPosition == new Vector3(0,0,0))
                SetPlatform(PlatformToStay);
            SetStatus();
            switch (MyStatus)
            {
                case EStatus.Inactive:
                    InactiveScheme();
                    break;
                case EStatus.Walking:
                    SetTheRightFacing();
                    WalkingScheme();
                    break;
                case EStatus.Triggered:
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
            Move();
        }

        private void PatrolScheme()
        {
            if (MyTransform.position.x > _rightLimitPosition.x)
            {
                BIsFacingLeft = true;
                MyTransform.localScale = new Vector3(
                    -1,
                    MyTransform.localScale.y,
                    MyTransform.localScale.z
                );
            }
            else if (MyTransform.position.x < _leftLimitPosition.x)
            {
                BIsFacingLeft = false;
                MyTransform.localScale = new Vector3(
                    1,
                    MyTransform.localScale.y,
                    MyTransform.localScale.z
                );
            }


            MyCurrentVelocity = WalkVelocity * (BIsFacingLeft ? -1 : 1);
        }

        private bool IsOutOfPosition()
        {
            return (
                MyTransform.position.x > _rightLimitPosition.x ||
                MyTransform.position.x < _leftLimitPosition.x
             );
        }

        private bool OnPlatform(Vector3 position)
        {
            return ( 
                position.y >= _rightLimitPosition.y && 
                position.x <= _rightLimitPosition.x && 
                position.x >= _leftLimitPosition.x
             );
        }

        protected override void Move()
        {
            if (StayOnPlatform && HaveToJumpOnPlatform())
            {
                Jump();
            }
            base.Move();
        }

        private void Jump()
        {
//            var y = Vector3.MoveTowards(
//                MyTransform.position, 
//                PlatformToStay.transform.position, 
//                Mathf.Abs(JumpVelocity) * Time.deltaTime).y;
//        
//            MyTransform.position = new Vector2(MyTransform.position.x, y);
            MyRigidBody2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }

        private bool HaveToJumpOnPlatform()
        {
            if (PlatformToStay == null || 
                !(PlatformToStay.transform.position.y > MyTransform.position.y)) return false;
            
            return (!BIsFacingLeft 
                    && MyTransform.position.x > _leftLimitPosition.x - StartJumpOffSet
                    && MyTransform.position.x < _leftLimitPosition.x)
                || (BIsFacingLeft 
                    && MyTransform.position.x < _rightLimitPosition.x + StartJumpOffSet
                    && MyTransform.position.x > _rightLimitPosition.x);
        }

        protected override IEnumerator RunningVelIncrease()
        {
            Called = true;
            RunningVelIncreaser = LastRunningVelIncreaser;

            yield return new WaitForSeconds(0.2f);

            if (BIsFacingLeft && MyCurrentVelocity > MaxRunningVelocity * -1)
                MyCurrentVelocity -= AccelerationOnRun;
            else if (MyCurrentVelocity < MaxRunningVelocity)
                MyCurrentVelocity += AccelerationOnRun;

            if (IsOutOfPosition() && _stayOnPlatform && !TriggeredByGun)
                MyCurrentVelocity = 0;
            
            MyAnimator.SetFloat(AnimatorVelocity, Mathf.Abs(MyCurrentVelocity));
        
            if (MyStatus == EStatus.Triggered)
            {
                LastRunningVelIncreaser = StartCoroutine(RunningVelIncrease());
                if (RunningVelIncreaser != null)
                    StopCoroutine(RunningVelIncreaser);
            }
            else
            {
                Called = false;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!StayOnPlatform) return;
            if (PlatformToStay == null ||
                collision.gameObject.GetInstanceID() == PlatformToStay.GetInstanceID())
                _stayOnPlatform = false;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (StayOnPlatform)
            {
                if (PlatformToStay == null && collision.gameObject.CompareTag("LevelObject"))
                {
                    SetPlatform(collision.gameObject);
                }
                if (PlatformToStay != null &&
                    collision.gameObject.GetInstanceID() == PlatformToStay.GetInstanceID() &&
                    !_stayOnPlatform)
                {
                    _stayOnPlatform = true;
                }
            }
            base.OnCollisionEnter2D(collision);
        }

        private void SetPlatform(GameObject platform)
        {
            PlatformToStay = platform;

            var lunghezzaPiattaforma = PlatformToStay.GetComponent<Platform_Script>().Lunghezza;

            if (lunghezzaPiattaforma == 0) return;
            
            _rightLimitPosition = new Vector3(
                (PlatformToStay.transform.position.x + lunghezzaPiattaforma / 2) - 2f,
                PlatformToStay.transform.position.y - 0.05f,
                PlatformToStay.transform.position.z
            );

            _leftLimitPosition = new Vector3(
                (PlatformToStay.transform.position.x - lunghezzaPiattaforma / 2) + 2f,
                PlatformToStay.transform.position.y - 0.05f,
                PlatformToStay.transform.position.z
            );
        }

        protected override void SetStatus()
        {
            if (_stayOnPlatform && !TriggeredByGun)
            {
                if (!OnPlatform(PlayerTransform.position))
                {
                    StopAllCoroutines();
                    MyStatus = EStatus.Patrol;
                    MyAnimator.SetBool(AnimatorTriggered, false);
                    MyAnimator.SetBool(AnimatorRun, false);
                }
                else
                {
                    SetTrigger();
                }
            }
            else
            {
                base.SetStatus();
            }
        }        
    }
}
