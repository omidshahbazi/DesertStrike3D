﻿//Rambo Team
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RamboTeam.Client
{
    public class RamboSceneManager : MonoBehaviorBase
    {

        public static RamboSceneManager Instance
        {
            get;
            private set;
        }

        public string CurrentSceneName
        {
            get;
            private set;
        }
        public Action FinalAction { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
            if (Instance == null)
                Instance = this;
            Init();
        }

        public void Init()
        {
            Scene scene = SceneManager.GetActiveScene();
            CurrentSceneName = scene.name;
        }

        public void LoadScene(string Name, LoadSceneMode SceneMode, Action FinalAction = null)
        {
            SceneManager.LoadScene(Name, SceneMode);
            SceneManager.LoadSceneAsync(Name).completed += RamboSceneManager_completed;
            CurrentSceneName = Name;
            this.FinalAction = FinalAction ;

        }

        private void RamboSceneManager_completed(AsyncOperation obj)
        {
            FinalAction?.Invoke();
            obj.completed -= RamboSceneManager_completed;
        }
    }
}