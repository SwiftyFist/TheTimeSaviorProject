using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using System;

namespace Sound
{
    //public struct Emitters
    //{
    //     _footEmitter = "_footEmitter";
    //}

    public class AudioManager : MonoBehaviour
    {
        
        #region Public Variables
        [EventRef]
        public const string playerMove = "event:/FOLEY/PLAYER/Movement/Footstep";
        [EventRef]
        public const string minigunBank = "event:/SFX/PLAYER/Weapon/Minigun";

        public const string FootEmitter = "_footEmitter";
        public const string ShootEmitter = "_shootEmitter";
        #endregion

        #region Private Variables
        public StudioEventEmitter _footEmitter { get; set; }
        public StudioEventEmitter _shootEmitter { get; set; }
        private FMOD.Studio.EventInstance _playerInstance;
        private FMOD.Studio.EventInstance _gunInstance;
        #endregion

        private void Awake()
        {
            this.DontDestroyOnLoad();
        }

        private void Start()
        {
            _playerInstance = RuntimeManager.CreateInstance(playerMove);
            _gunInstance = RuntimeManager.CreateInstance(minigunBank);
        }

        public bool SetEmitter(StudioEventEmitter emitter, string typeEmitter )
        {
            
            try
            {
                var type = typeof(Sound.AudioManager);
                var propertyInfo = type.GetProperty(typeEmitter);
                propertyInfo.SetValue(this, emitter, null);
                emitter.Event = GetEvent(typeEmitter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool FaiCose(string funzione, string typeEmitter, object[] parameters = null)
        {
            try
            {
                var emitter = (StudioEventEmitter)typeof(Sound.AudioManager).GetProperty(typeEmitter).GetValue(this, null);
                var method = typeof(StudioEventEmitter).GetMethod(funzione);
                method.Invoke(emitter, parameters);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }

        }

        public string GetEvent(string nomeEmitter)
        {
            switch(nomeEmitter)
            {
                case FootEmitter:
                    return playerMove;
                    break;
                case ShootEmitter:
                    return minigunBank;
                    break;
            }
            return null;
        }
    }
}
