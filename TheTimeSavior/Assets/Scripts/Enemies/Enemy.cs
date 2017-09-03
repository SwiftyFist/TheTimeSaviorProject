using System.Collections;
using System.Linq;
using UnityEngine;

namespace Enemies
{
    public enum EStatus
    {
        Inactive,
        Walking,
        Triggered,
        Patrol
    }
    public class Enemy : MonoBehaviour
    {
        public const string AnimatorRun = "Run";
        public const string AnimatorTriggered = "Triggered";
        public const string AnimatorVelocity = "Velocity";
        
        #region Variabili
        
        public EStatus MyStatus = EStatus.Inactive;
        public float AccelerationOnRun = 2.5f; //Drone 2
        public float DistanceFromPlayerToDeath = 10000;
        public float LimitPushBackVelocity = 100f;
        public float MaxRunningVelocity = 18f; //Drone 10
        public float PushBackOnHit = 6;
        public float RangeToRun = 12f, RangeToActivate = 20; //Drone 8.5, 14
        public float WalkVelocity = 8f; //Drone 6
        
        protected bool BIsFacingLeft = true;
        protected float MyCurrentVelocity;
        protected Rigidbody2D MyRigidBody2D;
        protected Transform MyTransform;
        protected Transform PlayerTransform;
        protected score_manager_script ScoreManager;
        protected Animator MyAnimator;
        protected Coroutine LastRunningVelIncreaser, RunningVelIncreaser;
        protected bool Called;
        protected GameObject Shield;
        protected bool CanDieForDistanceFromPlayer;
        
        #endregion
        
        protected virtual void Awake()
        {
            ScoreManager = GameObject.Find("Score_Manager").GetComponent<score_manager_script>();
            MyAnimator = GetComponent<Animator>();
            MyRigidBody2D = GetComponent<Rigidbody2D>();
            MyTransform = GetComponent<Transform>();
            PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
            var shield = gameObject.GetComponentsInChildren<Transform>()
                .FirstOrDefault(x => x.gameObject.CompareTag("Shield"));
            if ( shield != null)
                Shield = shield.gameObject;   
            SetStatus();
            SetTheRightFacing();
        }

        protected virtual void Update()
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
                    break;
                default:
                    InactiveScheme();
                    break;
            }
            
            Move();
        }

        protected virtual void FixedUpdate()
        {
            if (CalcDistanceFromPlayer() > DistanceFromPlayerToDeath && CanDieForDistanceFromPlayer)
            {
                GetComponent<EnemyDeath>().DestroyEnemy(0);
                return;
            }
            SetStatus();
        }
        
        //Cambia colore quando il player è in range
        //void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(transform.position, rangeToActivate);
        //    Gizmos.color = Color.black;
        //    Gizmos.DrawSphere(transform.position, rangeToRun);
        //}
        
        public void SetTheRightFacing()
        {
            BIsFacingLeft = MyTransform.position.x > PlayerTransform.position.x;
            MyTransform.localScale = new Vector3( BIsFacingLeft ? -1 : 1, 1, 1 );
        }
        
        protected void FlipFacing()
        {
            MyTransform.localScale = new Vector3(
                MyTransform.localScale.x * -1,
                MyTransform.localScale.y,
                MyTransform.localScale.z
            );
            
            BIsFacingLeft = !BIsFacingLeft;
        }
        
        protected float CalcDistanceFromPlayer()
        {
            return Mathf.Abs(MyTransform.position.x - PlayerTransform.position.x);
        }
        
        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            var collidedGameObject = collision.gameObject;
            if (collidedGameObject.name == "Player")
            {
                var playerScript = collidedGameObject.GetComponent<player_script>();
            
                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(0);
            
                ScoreManager.EnemyDeathCountReset();
            
                if (!playerScript.isInvincible)
                    playerScript.SetInvincible();
            }
            if (collidedGameObject.CompareTag("TriggerGate"))
                GetComponent<EnemyDeath>().DestroyEnemy(0);
        }
        
        public void SetTrigger(bool activate = true)
        {
            MyStatus = activate ? EStatus.Triggered : EStatus.Inactive;
            MyAnimator.SetBool(AnimatorTriggered, activate);
            MyAnimator.SetBool(AnimatorRun, activate);
            CanDieForDistanceFromPlayer = true;
        }
        
        protected virtual IEnumerator RunningVelIncrease()
        {
            Called = true;
            RunningVelIncreaser = LastRunningVelIncreaser;
        
            yield return new WaitForSeconds(0.2f);
        
            if (BIsFacingLeft && MyCurrentVelocity > (MaxRunningVelocity * -1))
                MyCurrentVelocity -= AccelerationOnRun;
            else if (MyCurrentVelocity < MaxRunningVelocity)
                MyCurrentVelocity += AccelerationOnRun;
        
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
        
        protected virtual void SetStatus()
        {
            var distance = CalcDistanceFromPlayer();
            
            if (MyStatus == EStatus.Triggered)
                return;
                
            if (distance >= RangeToActivate)
            {
                MyStatus = EStatus.Inactive;
                MyAnimator.SetBool(AnimatorTriggered, false);
                MyAnimator.SetBool(AnimatorRun, false);
            }
            else if (distance < RangeToActivate && distance >= RangeToRun)
            {
                MyStatus = EStatus.Walking;
                MyAnimator.SetBool(AnimatorTriggered, false);
                MyAnimator.SetBool(AnimatorRun, false);
            }
            else if (distance < RangeToRun)
            {
                SetTrigger();
            }
        }

        protected virtual void Move()
        {
            MyRigidBody2D.velocity = new Vector2(MyCurrentVelocity, MyRigidBody2D.velocity.y);
        }
        
        protected void InactiveScheme()
        {
            MyRigidBody2D.velocity = new Vector2(0, MyRigidBody2D.velocity.y);
        }
        
        protected void WalkingScheme()
        {
            MyCurrentVelocity = WalkVelocity * (BIsFacingLeft ? -1 : 1);
        }
        
        protected void RunningScheme()
        {
            if (!Called)
                LastRunningVelIncreaser = StartCoroutine(RunningVelIncrease());
        }

        public void ActiveShield(Vector3 playerPosition)
        {
            var myPosition = Camera.main.ScreenToWorldPoint(MyTransform.position);
            playerPosition = Camera.main.ScreenToWorldPoint(playerPosition);
            var angularCoefficentBullet =
                (playerPosition.y - myPosition.y) /
                (playerPosition.x - myPosition.x);
            var rotZ = Mathf.Atan(angularCoefficentBullet) * Mathf.Rad2Deg;
            var currentEulerAngle = Shield.transform.localEulerAngles;

            var offSet = (myPosition.y > playerPosition.y) ? -90 : 0;

            Shield.transform.localEulerAngles = new Vector3(
                currentEulerAngle.x,
                currentEulerAngle.y,
                rotZ + offSet
                );

            var mySprite = Shield.GetComponent<SpriteRenderer>();
            mySprite.enabled = true;
            StartCoroutine(ShieldDown(mySprite));
        }

        private static IEnumerator ShieldDown(SpriteRenderer mySprite)
        {
            yield return new WaitForSeconds(0.5f);
            mySprite.enabled = false;
        }
    }
}
