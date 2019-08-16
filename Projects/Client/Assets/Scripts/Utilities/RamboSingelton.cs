﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RamboTeam.Client.Utilities
{
    public class RamboSingelton<T> : MonoBehaviorBase where T : MonoBehaviorBase
    {
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = (T)FindObjectOfType(typeof(T));
                    if (_Instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).ToString());
                        _Instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);
                    }

                }

                return _Instance;
            }
        }

        private static T _Instance = null;

        private void OnApplicationQuit()
        {
            if (_Instance.gameObject != null)
                GameObject.Destroy(_Instance.gameObject);
        }

        private void OnDestroy()
        {
            if (_Instance.gameObject != null)
                GameObject.Destroy(_Instance.gameObject);
        }
    }

}