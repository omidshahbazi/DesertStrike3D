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

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
                Instance = this;

            armorText.text = Chopter.Instance.currentHP.ToString();
            lifeText.text = Chopter.Instance.LifeCount.ToString();
            rescueText.text = Chopter.Instance.currentRefugeesCount.ToString();
            fuelText.text = Chopter.Instance.FuelAmount.ToString();
            hellfireText.text = ChopterPilotController.Instance.HellfireCount.ToString();
            hydraText.text = ChopterPilotController.Instance.HydraCount.ToString();
            gatlingGunText.text = ChopterPilotController.Instance.GatlingGunCount.ToString();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            EventManager.OnHealthUpdate += OnUpdateHealth;
            EventManager.OnHellfireUpdate += OnUpdateHellfire;
            EventManager.OnFuelUpdate += OnUpdateFuel;
            EventManager.OnLifeUpdate += OnUpdateLife;

        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EventManager.OnHealthUpdate -= OnUpdateHealth;
            EventManager.OnHellfireUpdate -= OnUpdateHellfire;
            EventManager.OnFuelUpdate -= OnUpdateFuel;
            EventManager.OnLifeUpdate -= OnUpdateLife;
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
            hellfireText.text = ChopterPilotController.Instance.HellfireCount.ToString();
        }

        private void OnUpdateHealth()
        {
            armorText.text = Chopter.Instance.currentHP.ToString();
        }
    }

}