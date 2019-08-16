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
     
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
                Instance = this;     

            ReturnToMainMenu.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
                RamboSceneManager.Instance.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            });

            this.gameObject.SetActive(false); 
        }
   
    }

}