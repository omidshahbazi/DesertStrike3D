//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public abstract class MonoBehaviorBase : MonoBehaviour
	{
		protected NetworkSender NetworkSender
		{
			get;
			private set;
		}

		protected virtual void Awake()
		{
		}

		protected virtual void Start()
		{
			NetworkSender = new NetworkSender();
		}

		protected virtual void Update()
		{
		}

		protected void SendPosition()
		{
			NetworkSender.BeginSend();
			NetworkSender.PutVector3(transform.position);
			NetworkSender.EndSend();
		}
	}
}