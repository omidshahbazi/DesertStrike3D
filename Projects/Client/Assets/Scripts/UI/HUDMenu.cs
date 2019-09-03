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

            //StartCoroutine(SendOnPilotMessage()); //Temporary 
        }

        private IEnumerator SendOnPilotMessage()//temporary to play wwithout server
        {
            yield return new WaitForSeconds(1.0F);

            NetworkCommands.HandlePilot();
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
            fuelText.text = Chopter.Instance.currentFuelAmount.ToString();
        }

        private void OnUpdateHellfire()
        {
            hellfireText.text = Chopter.Instance.currentHellfireCount.ToString();
        }

        private void OnUpdateHealth()
        {
            armorText.text = Chopter.Instance.currentHP.ToString();
        }
    }

}