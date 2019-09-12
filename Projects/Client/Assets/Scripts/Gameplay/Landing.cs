//Rambo Team
using UnityEngine;
using System.Collections;
using System;

namespace RamboTeam.Client
{
    public class Landing : MonoBehaviorBase
    {
        public static  Landing Instance
        {
            get;
            set;
        }
        public enum State
        {
            Landed,
            Fly
        }

        [SerializeField]
        public float SetMaxHeight;
        [SerializeField]
        public float SetMinHeight;
        [SerializeField]
        public float Range;
        private ChopterPilotController chinstance;
        private float sqrRange;

        public static State state;
        private bool isProcess;




        protected override void Start()
        {
            base.Start();
            Instance = this;
            chinstance = ChopterPilotController.Instance;
            sqrRange = Range * Range;
            state = State.Fly;
         
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.F))
                SetOperation();
            if (!isProcess)
                return;
    
            switch(state)
            {
                case State.Landed:
                    chinstance.transform.position = Vector3.Slerp(chinstance.transform.position, new Vector3(chinstance.transform.position.x, SetMaxHeight, chinstance.transform.position.z), Time.deltaTime *10);
                    break;
                case State.Fly:
                    chinstance.transform.position = Vector3.Slerp(chinstance.transform.position, new Vector3(chinstance.transform.position.x, SetMinHeight, chinstance.transform.position.z), Time.deltaTime*10);

                    break;
            }
            if (chinstance.transform.position.y == SetMaxHeight || chinstance.transform.position.y == SetMinHeight)
            {
                isProcess = false;
                state = state == State.Fly ? State.Landed : State.Fly;



            }

        }

        private void SetOperation()
        {
            if (isProcess)
                return;
            isProcess = true;
            Vector3 orgin = this.transform.position;
            Vector3 target = chinstance.transform.position;
            orgin.y = target.y = 0;
            Vector3 diff = orgin - target;
            if (diff.magnitude > sqrRange)
            {
                isProcess = false;
                return;
            }

      
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.DrawWireSphere(transform.position, Range *Range);
        }

    }
}