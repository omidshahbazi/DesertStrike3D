//Rambo Team
using System;
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
        public GameObject TutorialPanel;
        private bool isGamePaused;

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
                isGamePaused = false;
            });

            ReturnToMainMenu.onClick.AddListener(() =>
            {
                RamboSceneManager.Instance.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            });

            QuitButton.onClick.AddListener(() => Application.Quit());
            this.gameObject.SetActive(false);
            InputManager.Instance.AddInput(KeyCode.Escape, PauseGame);
            InputManager.Instance.AddInput(KeyCode.F1, ShowTutorial);

        }

        private void ShowTutorial()
        {
            if (TutorialPanel.activeSelf || isGamePaused)
                return;
            this.gameObject.SetActive(true);
            this.TutorialPanel.SetActive(true);
            this.gameObject.transform.SetAsLastSibling();
            Time.timeScale = 0;
            InputManager.Instance.OnKeyRealeased += OnkeyCode;
        }

        private void  OnkeyCode(KeyCode KeyCode)
        {
            if (KeyCode != KeyCode.F1)
                return;
            this.gameObject.SetActive(false);
            this.TutorialPanel.SetActive(false);
            Time.timeScale = 1;
            InputManager.Instance.OnKeyRealeased -= OnkeyCode;
        }


        private void PauseGame()
        {
            isGamePaused = true;
            this.gameObject.transform.SetAsLastSibling();
            this.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            InputManager.Instance.RemoveInput(KeyCode.Escape, PauseGame);
            InputManager.Instance.RemoveInput(KeyCode.F1, ShowTutorial);
        }
    }

}