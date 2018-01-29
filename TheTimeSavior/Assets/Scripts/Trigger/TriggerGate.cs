using Destroyer;
using UnityEngine;

namespace Trigger
{
    public class TriggerGate : MonoBehaviour
    {
        public float OffSetActivation = 3f;
        public bool Activating;
        public LevelMaking.LevelMaking.LevelTypes NextLevelType = LevelMaking.LevelMaking.LevelTypes.Middle;
        private bool _activated;
        private Transform _playerTransform;
        private LevelMaking.LevelMaking _levelMaking;

        public void Awake()
        {
            _levelMaking = GameObject.Find("LevelMaker").GetComponent<LevelMaking.LevelMaking>();
            _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        }

        public void Update()
        {
            if (_playerTransform == null) return;
            if ((_playerTransform.position.x >= (transform.position.x + OffSetActivation) && !_activated))
            {
                _activated = true;
                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().SetActive(Activating);
                transform.GetChild(0).gameObject.SetActive(true);

                if (!Activating)
                    _levelMaking.InstantiateNextLevel(NextLevelType);
            }            
        }
    }
}
