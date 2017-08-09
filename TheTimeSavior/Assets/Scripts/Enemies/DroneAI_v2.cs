using System.Collections;
using Enemies;
using UnityEngine;

public class DroneAI_v2 : Enemy
{
    private void Update()
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

        MyRigidBody2D.velocity = new Vector2(MyCurrentVelocity, MyRigidBody2D.velocity.y);
        var y = Vector3.MoveTowards(MyTransform.position, PlayerTransform.position, Mathf.Abs(MyCurrentVelocity) * Time.deltaTime).y;//MoveTowards
        MyTransform.position = new Vector2(MyTransform.position.x, y);

        if (CalcDistanceFromPlayer() > DistanceFromPlayerToDeath)
            GetComponent<EnemyDeath>().DestroyEnemy(0);
    }
    

}
