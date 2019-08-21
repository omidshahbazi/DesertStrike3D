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
                Story.gameObject.SetActive(true);
                StartCoroutine(ShowStory());

            });

            QuitButton.onClick.AddListener(() => Application.Quit());
        }



        private IEnumerator ShowStory()
        {
            if (Input.anyKeyDown)
            {
                Story.gameObject.SetActive(false);
                TutorialPanel.gameObject.SetActive(true);
                StopCoroutine(ShowStory());
                StartCoroutine(ShowTutorial());
            }
            else
            {
                yield return new WaitForSeconds(0.0F);
                StartCoroutine(ShowTutorial());
                StopCoroutine(ShowStory());
                Story.gameObject.SetActive(false);
                TutorialPanel.gameObject.SetActive(true);
            }
        }

        private IEnumerator ShowTutorial()
        {
            yield return new WaitForSeconds(0.0F);
            MainMenuMusic.Stop();
            RamboSceneManager.Instance.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

    }

}