//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class Chopter : MonoBehaviorBase
	{
		public static Chopter Instance
		{
			get;
			private set;
		}

		private float currentHP = 0;
		private bool isPilot = false;

		public float HP = 100;

		protected override void Awake()
		{
			base.Awake();

			Instance = this;

			currentHP = HP;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			NetworkCommands.OnPilot += OnPilot;
			NetworkCommands.OnCommando += OnCommando;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			NetworkCommands.OnPilot -= OnPilot;
			NetworkCommands.OnCommando -= OnCommando;
		}

		private void OnPilot()
		{
			isPilot = true;
		}

		private void OnCommando()
		{
			isPilot = false;
		}

		public void ApplyDamage(float Damage)
		{
			if (!isPilot)
				return;

			currentHP = Mathf.Clamp(currentHP - Damage, 0, HP);

			if (currentHP == 0)
			{
				Debug.Log("Dead");
			}
		}
	}
}