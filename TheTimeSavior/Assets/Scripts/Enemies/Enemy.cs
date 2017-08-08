using UnityEngine;

namespace Enemies
{
    public enum EStatus
    {
        Inactive,
        Walking,
        Running,
        Patrol
    }
    
    public class Enemy : MonoBehaviour
    {
        public EStatus MyStatus = EStatus.Inactive;
    }
}
