//Rambo Team
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RamboTeam.Client
{
    public class RamboSceneManager : RamboTeam.Client.Utilities.RamboSingelton<RamboSceneManager>
    {
        public string CurrentSceneName
        {
            get;
            private set;
        }

        public Action FinalAction { get; private set; }

		public static bool IsMultiplayer
		{
			get;
			set;
		}

		protected override void Awake()
        {
            base.Awake();

            Init();
        }

        public void Init()
        {
            Scene scene = SceneManager.GetActiveScene();
            CurrentSceneName = scene.name;
        }

        public AsyncOperation LoadScene(string Name, LoadSceneMode SceneMode, Action FinalAction = null)
        {
            SceneManager.LoadSceneAsync(Name).completed += RamboSceneManager_completed;
            CurrentSceneName = Name;
            this.FinalAction = FinalAction;
            AsyncOperation operation = SceneManager.LoadSceneAsync(Name,SceneMode);
            return operation;
        }

        public void SetLoadSceneParameters(string Name, bool IsMultiplayer)
        {
            CurrentSceneName = Name;
			RamboSceneManager.IsMultiplayer = IsMultiplayer;
		}

        private void RamboSceneManager_completed(AsyncOperation obj)
        {
            FinalAction?.Invoke();
            obj.completed -= RamboSceneManager_completed;
        }
    }
}