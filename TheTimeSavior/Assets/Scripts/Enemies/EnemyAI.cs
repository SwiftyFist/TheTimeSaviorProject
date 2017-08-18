using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyAI : Enemy
    {
        #region Variabili
        
        public Transform RightLimit, LeftLimit;
        public bool StayOnPlatform;
        public float PushBackVelocityModificatorOnPlatform = -1;
        public GameObject PlatformToStay;
        private Vector3 _rightLimitPosition, _leftLimitPosition;

        #endregion

        protected override void Awake()
        {
            PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
            _rightLimitPosition = RightLimit.position;
            _leftLimitPosition = LeftLimit.position;
            Destroy(RightLimit.gameObject);
            Destroy(LeftLimit.gameObject);
            base.Awake();
        }

        protected override void Update()
        {
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
            if (IsOutOfPosition() && StayOnPlatform && MyStatus == EStatus.Triggered)
                MyCurrentVelocity = MyCurrentVelocity * PushBackVelocityModificatorOnPlatform;
            Move();
        }

        private void PatrolScheme()
        {
            if (IsOutOfPosition())
                FlipFacing();
            
            MyCurrentVelocity = WalkVelocity * (BIsFacingLeft ? -1 : 1);
        }

        private bool IsOutOfPosition()
        {
            return (
                MyTransform.position.x >= _rightLimitPosition.x ||
                MyTransform.position.x <= _leftLimitPosition.x
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

            if (IsOutOfPosition() && StayOnPlatform)
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
            if (PlatformToStay == null || collision.gameObject.GetInstanceID() == PlatformToStay.GetInstanceID())
                StayOnPlatform = false;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (PlatformToStay != null && collision.gameObject.GetInstanceID() == PlatformToStay.GetInstanceID() && !StayOnPlatform)
                StayOnPlatform = true;
            
            base.OnCollisionEnter2D(collision);
        }
        
        protected override void SetStatus()
        {
            if (StayOnPlatform)
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
                    MyStatus = EStatus.Triggered;
                    MyAnimator.SetBool(AnimatorTriggered, true);
                    MyAnimator.SetBool(AnimatorRun, true);
                }
            }
            else
            {
                base.SetStatus();
            }
            
        }        
    }
}
