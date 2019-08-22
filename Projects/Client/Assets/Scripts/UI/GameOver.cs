//Rambo Team
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RamboTeam.Client.UI
{
    public class GameOver : MonoBehaviorBase
    {
        public static GameOver Instance
        {
            get;
            private set;
        }

        public Button ReturnToMainMenu;

        private bool isOpen = false;
     
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
                Instance = this;     

            ReturnToMainMenu.onClick.AddListener(() =>
            {
                Close();
                RamboSceneManager.Instance.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            });

            this.gameObject.SetActive(false);
            isOpen = false;
        }

        public void Open()
        {
            gameObject.SetActive(true);
            isOpen = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            isOpen = false;
        }
   
    }

}