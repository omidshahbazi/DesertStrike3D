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
        private AudioSource audioSource;

        public PickUpBehaviour pickedItem
        {
            get;
            private set;
        }

        protected override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            base.Update();

            if (pickedItem != null)
            {
                pickedItem.transform.position = pickerTransform.position;
            }



        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (LadderAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 && LadderAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.time *= LadderAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    audioSource.Play();
                }
            }

            if (LadderAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 || LadderAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                audioSource.Stop();

        }

        protected override void OnTriggerEnter(Collider Collision)
        {
            base.OnTriggerEnter(Collision);

            if (Collision.gameObject.tag == "ItemArea" && pickedItem == null)
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
                pickedItem.Picked();
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

            if (!audioSource.isPlaying)
                audioSource.Play();

            if (!LadderAnimation.enabled)
                LadderAnimation.enabled = true;

            float normTime = Mathf.Clamp01(LadderAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime);

            Debug.Log("Ladder Going Down");

            LadderAnimation.SetFloat("Direction", 1);
            LadderAnimation.Play("ChopperPick", 0, normTime);

            ladderIsUp = false;
        }

        public void RollUpPicker()
        {
            if (ladderIsUp)
                return;

            if (!audioSource.isPlaying)
                audioSource.Play();
            Debug.Log("Ladder Going UP");

            float normTime = Mathf.Clamp01(LadderAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime);

            LadderAnimation.SetFloat("Direction", -1);
            LadderAnimation.Play("ChopperPick", 0, normTime);
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