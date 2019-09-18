//Rambo Team
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RamboTeam.Client.UI
{
    public class HUDMenu : MonoBehaviorBase
    {
        public static HUDMenu Instance
        {
            get;
            private set;
        }

        public Text lifeText;
        public Text armorText;
        public Text fuelText;
        public Text rescueText;
        public Text hellfireText;
        public Text hydraText;
        public Text gatlingGunText;
        public Text MissionStatusText;
        public GameObject HealthObj;
        public Image healthImage;
        public GameObject missionPanel;
        public GameObject misssionItem;
        public RectTransform missionsItemsPanel;
        public GameObject mapPanel;
        public Text lowFuelText;
        public Text lowwArmorText;
        public GameObject missionName;
        public GameObject missionVictoryPanel;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
                Instance = this;

            armorText.text = Chopter.Instance.currentHP.ToString();
            lifeText.text = Chopter.Instance.currentLifeCount.ToString();
            rescueText.text = Chopter.Instance.currentRefugeesCount.ToString();
            fuelText.text = Chopter.Instance.currentFuelAmount.ToString();
            hellfireText.text = Chopter.Instance.currentHellfireCount.ToString();
            hydraText.text = Chopter.Instance.currentHydraCount.ToString();
            gatlingGunText.text = Chopter.Instance.currentGatlingGunCount.ToString();
            MissionStatusText.gameObject.SetActive(false);
            HealthObj.gameObject.SetActive(false);
            missionPanel.SetActive(false);
            InputManager.Instance.AddInput(KeyCode.M, ShowHideMissionPanel);
            InputManager.Instance.AddInput(KeyCode.Tab, ShowMapPanel);


            //StartCoroutine(SendOnPilotMessage()); //Temporary 
        }

        private IEnumerator SendOnPilotMessage()//temporary to play wwithout server
        {
            yield return new WaitForSeconds(1.0F);

            NetworkCommands.HandleBecomePilot();
        }

        protected override void Update()
        {
            base.Update();

            if (lowFuelText.gameObject.activeSelf)
            {
                float t = Mathf.PingPong(Time.time * 1.0F, 1.0F);
                lowFuelText.color = Color.Lerp(Color.yellow, Color.red, t);
            }
            if (lowwArmorText.gameObject.activeSelf)
            {
                float t = Mathf.PingPong(Time.time * 1.0F, 1.0F);
                lowwArmorText.color = Color.Lerp(Color.yellow, Color.red, t);
            }

            if (Landing.state == Landing.State.Landed && MissionManager.Instance.AreAllTasksDone && !missionPanel.activeSelf)
            {
                UpdateMissions();
                CheckVictoryState();
                missionPanel.SetActive(true);
                missionPanel.transform.SetAsLastSibling();
                Time.timeScale = 0; 
            }
        }

        private void CheckVictoryState()
        {
            bool isVictory = MissionManager.Instance.AreAllTasksDone;
            missionName.SetActive(!isVictory);
            missionVictoryPanel.SetActive(isVictory);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            EventManager.OnHealthUpdate += OnUpdateHealth;
            EventManager.OnHellfireUpdate += OnUpdateHellfire;
            EventManager.OnFuelUpdate += OnUpdateFuel;
            EventManager.OnLifeUpdate += OnUpdateLife;
            EventManager.OnRefugeeUpdate += OnUpdateRefugee;
            EventManager.OnHydraUpdate += OnUpdateHydra;
            EventManager.OnGatlingGunUpdate += OnUpdateGatlingGun;

            EventManager.OnMissionComplete += OnMissionCompleted;
            EventManager.OnAllMissionsComplete += OnAllMissionsCompleted;
            EventManager.OnEnemyDeath += OnEnemyDeath;
            missionPanel.SetActive(false);
        }

        public void QuitToMainMenu()
        {
            RamboSceneManager.Instance.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            Time.timeScale = 1;
        }

        private void ShowHideMissionPanel()
        {
            if (mapPanel.activeSelf || missionPanel.activeSelf || PauseMenu.Instance.isGamePaused)
                return;

            UpdateMissions();
            CheckVictoryState();
            missionPanel.SetActive(true);
            missionPanel.transform.SetAsLastSibling();
            Time.timeScale = 0;
            InputManager.Instance.OnKeyRealeased += OnkeyCode;
        }

        private void ShowMapPanel()
        {
            if (mapPanel.activeSelf || missionPanel.activeSelf || PauseMenu.Instance.isGamePaused)
                return;

            mapPanel.SetActive(true);
            mapPanel.transform.SetAsLastSibling();
            Time.timeScale = 0;
            InputManager.Instance.OnKeyRealeased += OnkeyCode;
        }

        private void OnkeyCode(KeyCode KeyCode)
        {
            if (KeyCode == KeyCode.M)
            {
                missionPanel.SetActive(false);
                Time.timeScale = 1;
                InputManager.Instance.OnKeyRealeased -= OnkeyCode;
                return;
            }
            if (KeyCode == KeyCode.Tab)
            {
                mapPanel.SetActive(false);
                Time.timeScale = 1;
                InputManager.Instance.OnKeyRealeased -= OnkeyCode;
                return;
            }

        }

        private void UpdateMissions()
        {
            for (int i = 0; i < missionsItemsPanel.childCount; ++i)
            {
                Destroy(missionsItemsPanel.GetChild(i).gameObject);
            }

            List<Mission> Missions = MissionManager.Instance.Missions;

            for (int i = 0; i < Missions.Count; ++i)
            {
                Mission mis = Missions[i];
                GameObject missionItem = GameObject.Instantiate(misssionItem);
                missionItem.transform.SetParent(missionsItemsPanel, false);
                missionItem.transform.SetAsLastSibling();
                MissionItem item = missionItem.GetComponent<MissionItem>();
                item.SetData((i + 1).ToString() + " " + mis.Description, mis.IsDone);
            }

        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EventManager.OnHealthUpdate -= OnUpdateHealth;
            EventManager.OnHellfireUpdate -= OnUpdateHellfire;
            EventManager.OnFuelUpdate -= OnUpdateFuel;
            EventManager.OnLifeUpdate -= OnUpdateLife;
            EventManager.OnRefugeeUpdate -= OnUpdateRefugee;
            EventManager.OnHydraUpdate -= OnUpdateHydra;
            EventManager.OnGatlingGunUpdate -= OnUpdateGatlingGun;

            EventManager.OnMissionComplete -= OnMissionCompleted;
            EventManager.OnAllMissionsComplete -= OnAllMissionsCompleted;
            EventManager.OnEnemyDeath -= OnEnemyDeath;
            InputManager.Instance.RemoveInput(KeyCode.Tab, ShowHideMissionPanel);
        }

        private void OnMissionCompleted(Mission Mission)
        {
            if (MissionManager.Instance.AreAllTasksDone)
            {
                OnAllMissionsCompleted();
                return;
            }

            MissionStatusText.text = "Mission " + Mission.Title + " Accomplished";
            if (!MissionStatusText.gameObject.activeSelf)
            {
                MissionStatusText.gameObject.SetActive(true);
                StopCoroutine(DisableMissionStatus());
                StartCoroutine(DisableMissionStatus());
            }
        }

        public void SetEnemyHealth(Enemy Enemy)
        {
			if (healthImage == null)
				return;

            if (Enemy == null)
            {
                healthImage.fillAmount = 1;
                HealthObj.gameObject.SetActive(false);
                return;
            }
            float val = (Enemy.currentHP / Enemy.HP);
            if (val == 1)
                HealthObj.gameObject.SetActive(false);
            else
                HealthObj.gameObject.SetActive(true);

            healthImage.fillAmount = val;

        }


        private void OnEnemyDeath(Enemy DeadEnemy)
        {
            HealthObj.gameObject.SetActive(false);
        }


        private IEnumerator DisableMissionStatus()
        {
            yield return new WaitForSeconds(3);

            MissionStatusText.gameObject.SetActive(false);
        }

        private void OnAllMissionsCompleted()
        {
            MissionStatusText.text = "All the Missions Accomplished. Return to Base.";
            if (!MissionStatusText.gameObject.activeSelf)
            {
                MissionStatusText.gameObject.SetActive(true);
            }
        }

        private void OnUpdateRefugee()
        {
            rescueText.text = Chopter.Instance.currentRefugeesCount.ToString();
        }

        private void OnUpdateHydra()
        {
            hydraText.text = Chopter.Instance.currentHydraCount.ToString();
        }

        private void OnUpdateGatlingGun()
        {
            gatlingGunText.text = Chopter.Instance.currentGatlingGunCount.ToString();
        }

        private void OnUpdateLife()
        {
            lifeText.text = Chopter.Instance.currentLifeCount.ToString();
        }

        private void OnUpdateFuel()
        {
            fuelText.text = Chopter.Instance.currentFuelAmount.ToString("0");

            if (Chopter.Instance.currentFuelAmount <= Chopter.Instance.LowFuelAmount)
            {
                lowFuelText.gameObject.SetActive(true);
            }
            else
            {
                lowFuelText.gameObject.SetActive(false);
            }
        }

        private void OnUpdateHellfire()
        {
            hellfireText.text = Chopter.Instance.currentHellfireCount.ToString();
        }

        private void OnUpdateHealth()
        {
            armorText.text = Chopter.Instance.currentHP.ToString();


            if (Chopter.Instance.currentHP <= Chopter.Instance.LeftArmDestructionHP || Chopter.Instance.currentHP <= Chopter.Instance.RightArmDestructionHP)
            {
                lowwArmorText.gameObject.SetActive(true);
                //Do Blink
            }
            else
            {
                lowwArmorText.gameObject.SetActive(false);
            }
        }
    }

}