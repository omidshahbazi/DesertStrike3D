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
        public Button CreditsButton;
        public Button SingleButton;
        public Button CoOpButton;
        public Button QuitButton;
        public Button PilotButton;
        public Button CoPilotButton;
        public GameObject MissionBrief;
        public GameObject Story;
        public GameObject TutorialPanel;
        public GameObject loadingScreen;
        public GameObject CoopMenu;
        public GameObject CreditsObj;
        public Text Text;
        public Image Image;
        private AudioSource MainMenuMusic;
        private bool isKeyGet;
        private float progress;

		private bool isMultiplayer = false;

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
				isMultiplayer = false;

				NetworkLayer.Instance.IsPilot = true;
                ShowStory();
                InputManager.Instance.OnAnyKeyPressd += ShowTutorial;
            });
            CoOpButton.onClick.AddListener(() =>
			{
				isMultiplayer = true;

				NetworkCommands.JoinToRoom();
               
            });

            CreditsButton.onClick.AddListener(() =>
            {
                CreditsObj.gameObject.SetActive(true);
                InputManager.Instance.OnAnyKeyPressd += CloseCreditPanel;
            });

            PilotButton.onClick.AddListener(() =>
            {
				NetworkCommands.BecomePilot();
			});

            CoPilotButton.onClick.AddListener(() =>
			{
				NetworkCommands.BecomeCoPilot();
			});

            QuitButton.onClick.AddListener(() => Application.Quit());

            NetworkCommands.OnConnected += NetworkCommands_OnConnected;
            NetworkCommands.OnConnectionLost += NetworkCommands_OnDisconnected;

            NetworkCommands.OnJoinedToRoom += OnJoinedToRoom;

			NetworkCommands.OnBecomePilot += OnBecomePilot;
			NetworkCommands.OnBecomeCoPilot += OnBecomeCoPilot;
			NetworkCommands.OnPilotReserved += OnPilotReserved;
			NetworkCommands.OnPilotReleased += OnPilotReleased;
			NetworkCommands.OnCoPilotReserved += OnCoPilotReserved;
			NetworkCommands.OnCoPilotReleased += OnCoPilotReleased;


			CoOpButton.interactable = false;
            InputManager.Instance.AddInput(KeyCode.Backspace, OnBackSpaceClick);
        }

		private void OnBecomePilot()
		{
			PilotButton.interactable = false;
		}

		private void OnBecomeCoPilot()
		{
			CoPilotButton.interactable = false;
		}

		private void OnPilotReserved()
		{
			PilotButton.interactable = false;
		}

		private void OnPilotReleased()
		{
			PilotButton.interactable = true;
		}

		private void OnCoPilotReserved()
		{
			CoPilotButton.interactable = false;
		}

		private void OnCoPilotReleased()
		{
			CoPilotButton.interactable = true;
		}

		private void OnBackSpaceClick()
        {
            CoopMenu.gameObject.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            CoOpButton.interactable = NetworkLayer.Instance.IsConnected;
        }

        protected  void OnDestroy()
        {
            InputManager.Instance.RemoveInput(KeyCode.Backspace, OnBackSpaceClick);
        }

        private void NetworkCommands_OnConnected()
        {
            CoOpButton.interactable = true;
        }

        private void NetworkCommands_OnDisconnected()
        {
            CoOpButton.interactable = false;
        }

        private void ShowStory()
        {
            CoOpButton.gameObject.SetActive(false);
            Story.gameObject.SetActive(true);
        }

        private void ShowTutorial()
        {
            TutorialPanel.gameObject.SetActive(true);
            InputManager.Instance.OnAnyKeyPressd -= ShowTutorial;

            InputManager.Instance.OnAnyKeyPressd += ShowMissionBreif;

        }

        private void ShowCoopMenu()
        {
            CoopMenu.gameObject.SetActive(true);
        }

        private void ShowMissionBreif()
        {
            MissionBrief.gameObject.SetActive(true);
            InputManager.Instance.OnAnyKeyPressd -= ShowMissionBreif;
            InputManager.Instance.OnAnyKeyPressd += loadGame;

        }



        private void loadGame()
        {
            InputManager.Instance.OnAnyKeyPressd -= loadGame;
            MainMenuMusic.Stop();

            loadingScreen.SetActive(true);
            StartCoroutine(LoadAsync());
        }


        private IEnumerator LoadAsync()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync("FinalScene001", LoadSceneMode.Single);
            RamboSceneManager.Instance.SetLoadSceneParameters("FinalScene001", isMultiplayer);

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
			ShowCoopMenu();

			//ShowStory();
   //         InputManager.Instance.OnAnyKeyPressd += loadGame;
        }

        private void CloseCreditPanel()
        {
            InputManager.Instance.OnAnyKeyPressd -= CloseCreditPanel;
            CreditsObj.gameObject.SetActive(false);
        }

    }
}


