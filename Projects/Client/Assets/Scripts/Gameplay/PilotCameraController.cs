//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
    public class PilotCameraController : MonoBehaviorBase
    {
        public static PilotCameraController Instance
        {
            get;
            private set;
        }

        public float Speed = 10.0F;
        public float BaseDistance = 30;
        public float OffsetRadius = 10;
        public float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.7f;
        public float decreaseFactor = 1.0f;

        Vector3 originalPos;
        private ChopterPilotController chopter = null;
        private Transform chopterTransform = null;
        private Transform coPilotCameraObject = null;

        private bool isFPS = false;

        public Vector3 PanOffset
        {
            get;
            set;
        }

        protected override void Awake()
        {
            base.Awake();

            Instance = this;

            chopter = ChopterPilotController.Instance;
            chopterTransform = chopter.transform;

            coPilotCameraObject = ChopterPilotController.Instance.transform.Find("CoPilotCamera");
            if (coPilotCameraObject != null)
                coPilotCameraObject.gameObject.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            coPilotCameraObject.gameObject.SetActive(!NetworkLayer.Instance.IsPilot);
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (!NetworkLayer.Instance.IsPilot)
                return;

            Vector3 forward = chopterTransform.position + new Vector3(-1, 2, -1);
            forward = (forward - chopterTransform.position).normalized;
            Vector3 targetPos = chopterTransform.position + (forward * BaseDistance);

            if (chopter.IsMoving)
            {
                Vector3 chopterForward = chopterTransform.forward;
                chopterForward.y = 0;

                float angle = Vector3.Angle(chopterForward, Vector3.right);

                if (chopterForward.z < 0)
                    angle = 360 - angle;

                angle *= Mathf.Deg2Rad;

                targetPos.x += OffsetRadius * Mathf.Cos(angle) * (Screen.width / (float)Screen.height);
                targetPos.z += OffsetRadius * Mathf.Sin(angle) * (Screen.width / (float)Screen.height);
            }

            float t = Time.deltaTime * Speed;


            transform.position = Vector3.Lerp(transform.position, targetPos + PanOffset, t);
            transform.forward = Vector3.Lerp(transform.forward, forward * -1, t);


            transform.forward = Vector3.Lerp(transform.forward, forward * -1, t);
            if (shakeDuration > 0)
            {
                transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;

            }
        }

        public void SetCameraShake()
        {
            if (!NetworkLayer.Instance.IsPilot)
                return;

            if (shakeDuration <= 0)
            {
                shakeDuration = 0.3F;
                originalPos = transform.localPosition;
            }
        }
    }
}