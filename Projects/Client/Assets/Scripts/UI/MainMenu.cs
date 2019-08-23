//Rambo Team
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RamboTeam.Client.UI
{
    public class MainMenu : MonoBehaviorBase
    {
        public Button SingleButton;
        public Button CoOpButton;
        public Button QuitButton;
        public GameObject Story;
        public GameObject TutorialPanel;
        private AudioSource MainMenuMusic;
        private bool isKeyGet;

        protected override void Awake()
        {
            base.Awake();

            MainMenuMusic = GetComponent<AudioSource>();
            MainMenuMusic.Play();
            Story.gameObject.SetActive(false);
            TutorialPanel.gameObject.SetActive(false);
            SingleButton.onClick.AddListener(() =>
            {
                ShowStory();
                InputManager.Instance.OnAnyKeyPressd+=ShowTutorial;

            });

            QuitButton.onClick.AddListener(() => Application.Quit());
        }



        private void ShowStory()
        {
            Story.gameObject.SetActive(true);
           
        }

        private void ShowTutorial()
        {
            TutorialPanel.gameObject.SetActive(true);
            InputManager.Instance.OnAnyKeyPressd -= ShowTutorial;
            InputManager.Instance.OnAnyKeyPressd += loadGame;
          
        }

        private void loadGame()
        {
            InputManager.Instance.OnAnyKeyPressd -= loadGame;
            MainMenuMusic.Stop();
            RamboSceneManager.Instance.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

    }

}