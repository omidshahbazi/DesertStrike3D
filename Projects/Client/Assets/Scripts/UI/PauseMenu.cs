//Rambo Team
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RamboTeam.Client.UI
{
    public class PauseMenu : MonoBehaviorBase
    {
        public static PauseMenu Instance
        {
            get;
            private set;
        }

        public Button ReturnToGame;
        public Button ReturnToMainMenu;
        public Button QuitButton;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
                Instance = this;

            ReturnToGame.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
                Time.timeScale = 1;
            });

            ReturnToMainMenu.onClick.AddListener(() =>
            {
                RamboSceneManager.Instance.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            });

            QuitButton.onClick.AddListener(() => Application.Quit());
            this.gameObject.SetActive(false);
            InputManager.Instance.AddInput(KeyCode.Escape, PauseGame);
        }

        private void PauseGame()
        {
            this.gameObject.transform.SetAsLastSibling();
            this.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            InputManager.Instance.RemoveInput(KeyCode.Escape, PauseGame);
        }
    }

}