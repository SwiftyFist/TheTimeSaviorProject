using System.Collections;
using System.Linq;
using UnityEngine;

namespace Enemies
{
    public class EnemyAI : Enemy
    {
        #region Variabili
        
        public bool StayOnPlatform;
        public float PushBackVelocityModificatorOnPlatform = -1;
        public GameObject PlatformToStay;
        private Vector3 _rightLimitPosition, _leftLimitPosition;
        private bool _stayOnPlatform;

        #endregion

        protected override void Awake()
        {
            _stayOnPlatform = StayOnPlatform;
            PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
            base.Awake();
        }

        protected override void Update()
        {
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

            var lunghezzaPiattaforma = PlatformToStay.GetComponent<Collider2D>().bounds.size.x;

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
