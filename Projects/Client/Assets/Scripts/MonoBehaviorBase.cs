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

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

		}

		protected virtual void Update()
		{
		}

		protected virtual void LateUpdate()
		{
		}

		protected virtual void OnDrawGizmosSelected()
		{
			
		}

		protected virtual void OnTriggerEnter(Collider Collision)
		{
			
		}

        protected virtual void OnTriggerExit (Collider Collision)
        {

        }
    }
}