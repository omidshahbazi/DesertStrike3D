//Rambo Team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RamboTeam.Client
{
    public class ChoppersPicker : MonoBehaviorBase
    {
        public Transform pickerTransform;
        [SerializeField]
        private Animator LadderAnimation;

        private bool ladderIsUp = true;

        public PickUpBehaviour pickedItem
        {
            get;
            private set;
        }

        protected override void Update()
        {
            base.Update();

            if (pickedItem != null)
            {
                pickedItem.transform.position = pickerTransform.position;
            }
        }

        protected override void OnTriggerEnter(Collider Collision)
        {
            base.OnTriggerEnter(Collision);

            if (Collision.gameObject.tag == "ItemArea")
            {
                DropLadder();
            }
            else if (Collision.gameObject.tag == "Item")
            {

                if (pickedItem != null)//already picked something
                    return;

                pickedItem = Collision.GetComponent<PickUpBehaviour>();

                if (pickedItem == null)
                    return;

                pickedItem.transform.position = pickerTransform.position;

                RollUpPicker();
            }
        }

        protected override void OnTriggerExit(Collider Collision)
        {
            base.OnTriggerExit(Collision);

            if (Collision.gameObject.tag == "ItemArea")
            {
                RollUpPicker();
            }
        }

        public void DropLadder()
        {
            if (!ladderIsUp)
                return;

            if (!LadderAnimation.enabled)
                LadderAnimation.enabled = true;

            LadderAnimation.SetFloat("Direction", 1);
            LadderAnimation.Play("ChopperPick", -1, float.NegativeInfinity);

            ladderIsUp = false;
        }

        public void RollUpPicker()
        {
            if (ladderIsUp)
                return;
            LadderAnimation.SetFloat("Direction", -1);
            LadderAnimation.Play("ChopperPick", -1, float.NegativeInfinity);
            ladderIsUp = true;
        }


        public void DestroyPickedItem()
        {
            if (pickedItem == null)
                return;
            GameObject.Destroy(pickedItem.gameObject);
            pickedItem = null;
        }

    }
}