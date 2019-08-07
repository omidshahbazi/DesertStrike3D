//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class CameraController : MonoBehaviorBase
	{
		public float Speed = 10.0F;

		[SerializeField]
		private GameObject TargetGameObject;

		[SerializeField]
		private GameObject ChopterGameObject;

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			base.Update();

			transform.position = TargetGameObject.transform.position;
			transform.LookAt(ChopterGameObject.transform);
		}
	}
}