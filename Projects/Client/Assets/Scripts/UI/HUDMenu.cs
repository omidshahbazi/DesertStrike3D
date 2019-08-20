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

        }

        public void UpdateHP()
        {
            armorText.text = Chopter.Instance.currentHP.ToString();
        }
    }

}