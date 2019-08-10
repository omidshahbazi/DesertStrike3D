//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class ChopterPilotController : MonoBehaviorBase
	{
		public static ChopterPilotController Instance
		{
			get;
			private set;
		}

		private const float SYNC_PERIOD = 1.0F;

		private float nextSyncTime = 0.0F;
		private Vector3 lastPosition = Vector3.zero;
		private Quaternion lastRotation = Quaternion.identity;

		[SerializeField]
		private Transform cameraPositionTransform = null;

		[SerializeField]
		private Transform cameraTargetTransform = null;

		public float MovementSpeed = 10;
		public float RotationSpeed = 10;

		public bool IsPilot
		{
			get;
			private set;
		}

		public bool IsMoving
		{
			get;
			private set;
		}

		public Transform CameraPositionTransform
		{
			get { return cameraPositionTransform; }
		}

		public Transform CameraTargetTransform
		{
			get { return cameraTargetTransform; }
		}

		protected override void Awake()
		{
			base.Awake();

			Instance = this;
		}

		protected override void Start()
		{
			base.Start();

			NetworkCommands.OnPilot += OnPilot;
			NetworkCommands.OnCommando += OnCommando;
			NetworkCommands.OnSyncChopterTransform += OnSyncChopterTransform;
		}

		private void OnPilot()
		{
			IsPilot = true;
		}

		private void OnCommando()
		{
			IsPilot = false;
		}

		private void OnSyncChopterTransform(Vector3 Position, Vector3 Rotation)
		{
			if (IsPilot)
				return;

			lastPosition = Position;
			lastRotation = Quaternion.Euler(Rotation);

			nextSyncTime = Time.time + SYNC_PERIOD;
		}

		protected override void Update()
		{
			base.Update();

			if (IsPilot)
			{
				IsMoving = false;

				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				{
					transform.Translate(transform.forward * Time.deltaTime * MovementSpeed, Space.World);
					IsMoving = true;
				}
				else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				{
					transform.Translate(transform.forward * Time.deltaTime * MovementSpeed * -1, Space.World);
					IsMoving = true;
				}

				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				{
					transform.Rotate(0, Time.deltaTime * RotationSpeed * -1, 0, Space.World);
				}
				else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				{
					transform.Rotate(0, Time.deltaTime * RotationSpeed, 0, Space.World);
				}

				if (Time.time >= nextSyncTime)
				{
					NetworkCommands.SyncChopterTransform(transform.position, transform.rotation.eulerAngles);

					nextSyncTime = Time.time + SYNC_PERIOD;
				}
			}
			else
			{
				float t = SYNC_PERIOD - (nextSyncTime - Time.time);

				IsMoving = (t != 1.0F);

				transform.position = Vector3.Lerp(transform.position, lastPosition, t);
				transform.rotation = Quaternion.Lerp(transform.rotation, lastRotation, t);
			}
		}
	}
}