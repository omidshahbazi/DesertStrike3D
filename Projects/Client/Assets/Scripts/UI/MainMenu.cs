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
        private AudioSource MainMenuMusic;

        protected override void Awake()
        {
            base.Awake();

            MainMenuMusic = GetComponent<AudioSource>();
            MainMenuMusic.Play();
            SingleButton.onClick.AddListener(() =>
            {
                MainMenuMusic.Stop();
                RamboSceneManager.Instance.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            );
            QuitButton.onClick.AddListener(() => Application.Quit());
        }

    }

}