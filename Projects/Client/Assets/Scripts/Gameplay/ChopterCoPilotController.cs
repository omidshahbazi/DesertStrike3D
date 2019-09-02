﻿//Rambo Team
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RamboTeam.Client
{
	public class ChopterCoPilotController : MonoBehaviorBase
	{
		public static ChopterCoPilotController Instance
		{
			get;
			private set;
		}

		private const float SYNC_RATE = 2;
		private const float SYNC_PERIOD = 1 / SYNC_RATE;

		[SerializeField]
		private GameObject ChopterModel;

		[SerializeField]
		public float Range = 15F;

		[SerializeField]
		public Transform FireTransform;

		[SerializeField]
		public GameObject BulletObject;

		[SerializeField]
		public float RateOfFire = 0.5F;

		private float nextShotTime;
		private float sqrRange;

		protected override void Awake()
		{
			base.Awake();

			Instance = this;

			sqrRange = Range * Range;

		}

		protected override void OnEnable()
		{
			base.OnEnable();

			NetworkCommands.OnSyncChopterShotMachinegun += OnSyncChopterShotMachinegun;

			InputManager.Instance.AddInput(KeyCode.Mouse0, Shoot);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			NetworkCommands.OnSyncChopterShotMachinegun -= OnSyncChopterShotMachinegun;

			InputManager.Instance.RemoveInput(KeyCode.Mouse0, Shoot);
		}

		protected override void Update()
		{
			base.Update();

			if (Chopter.Instance.IsDead || NetworkLayer.Instance.IsPilot)
				return;
		}

		private void Shoot()
		{
			if (Chopter.Instance.IsDead || NetworkLayer.Instance.IsPilot || Time.time < nextShotTime)
				return;

			nextShotTime = Time.time + RateOfFire;

			Vector3 pos = FireTransform.position;
			Vector3 dir = ChopterModel.transform.forward;

			ShootInternal(pos, dir);

			NetworkCommands.SyncChopterShotMachinegun(pos, dir);
		}

		private void OnSyncChopterShotMachinegun(Vector3 Position, Vector3 Direction)
		{
			if (Chopter.Instance.IsDead || !NetworkLayer.Instance.IsPilot)
				return;

			ShootInternal(Position, Direction);
		}

    

        private void ShootInternal(Vector3 Position, Vector3 Direction)
		{
			GameObject newObject = GameObject.Instantiate(BulletObject, Position, Quaternion.identity) as GameObject;

			Bullet ps = newObject.GetComponent<Bullet>();

			ps.SetParamaeters(ChopterModel.transform.forward);
		}
	}
}