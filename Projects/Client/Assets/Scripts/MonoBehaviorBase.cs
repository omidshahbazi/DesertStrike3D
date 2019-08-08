//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public abstract class MonoBehaviorBase : MonoBehaviour
	{
		protected virtual void Awake()
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
        }

        protected void SendSyncChopterTransform()
        {
            NetworkCommands.SyncChopterTransform(transform.position, transform.rotation.eulerAngles);
        }
    }
}