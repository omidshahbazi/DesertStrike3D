//Rambo Team
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public GameObject loadingScreen;
        public Text Text;
        public Image Image;
        private AudioSource MainMenuMusic;
        private bool isKeyGet;
        private float progress;

        protected override void Awake()
        {
            base.Awake();

            NetworkLayer networkLayer = NetworkLayer.Instance;

            MainMenuMusic = GetComponent<AudioSource>();
            MainMenuMusic.Play();
            Story.gameObject.SetActive(false);
            TutorialPanel.gameObject.SetActive(false);
            SingleButton.onClick.AddListener(() =>
            {
                NetworkLayer.Instance.IsPilot = true;
                ShowStory();
                InputManager.Instance.OnAnyKeyPressd += ShowTutorial;
            });

            QuitButton.onClick.AddListener(() => Application.Quit());

            NetworkCommands.OnConnected += NetworkCommands_OnConnected;
            NetworkCommands.OnConnectionLost += NetworkCommands_OnDisconnected;

            NetworkCommands.OnJoinedToRoom += OnJoinedToRoom;

            CoOpButton.interactable = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            CoOpButton.interactable = NetworkLayer.Instance.IsConnected;
        }

        private void NetworkCommands_OnConnected()
        {
            CoOpButton.interactable = true;

            CoOpButton.onClick.AddListener(() =>
            {
                NetworkCommands.JoinToRoom();
            });
        }

        private void NetworkCommands_OnDisconnected()
        {
            CoOpButton.interactable = false;
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

            loadingScreen.SetActive(true);
            StartCoroutine(LoadAsync());
        }

   

        IEnumerator LoadAsync()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync("LevelDesignScene", LoadSceneMode.Single);
            RamboSceneManager.Instance.SetLoadSceneParameters("LevelDesignScene");


            while (!operation.isDone)
            {
                progress = Mathf.Clamp01(operation.progress / 0.9f);

                Image.fillAmount = progress;

                Text.text = "Loading... " + (int)(progress * 100f) + "%";

                yield return null;
            }

        }
        private void OnJoinedToRoom()
        {
            ShowStory();
            InputManager.Instance.OnAnyKeyPressd += loadGame;
        }
    }
}


