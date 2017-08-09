using Enemies;
using UnityEngine;

public class DroneAI_v2 : Enemy
{
    protected override void Move()
    {
        base.Move();
        var y = Vector3.MoveTowards(
            MyTransform.position, 
            PlayerTransform.position, 
            Mathf.Abs(MyCurrentVelocity) * Time.deltaTime).y;
        
        MyTransform.position = new Vector2(MyTransform.position.x, y);
    }
}
