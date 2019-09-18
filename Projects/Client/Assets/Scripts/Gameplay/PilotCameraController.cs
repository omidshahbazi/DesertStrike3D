//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
    public class PilotCameraController : RamboTeam.Client.Utilities.RamboSingelton<PilotCameraController>
    {
        public static readonly Vector3 SINGLE_PLAYER_CAMERA_OFFSET_DIRECTION = new Vector3(-1, 2, -1);
        public static readonly Vector3 MULTI_PLAYER_CAMERA_OFFSET_DIRECTION = new Vector3(0, 0.2F, -1);

        [SerializeField]
        public float Speed = 10.0F;
        public float SingleplayerBaseDistance = 150;
        public float MultiplayerBaseDistance = 100;
        public float OffsetRadius = 10;
        public float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.7f;
        public float decreaseFactor = 1.0f;

        Vector3 originalPos;
        private ChopterPilotController chopter = null;
        private Transform chopterTransform = null;
        private Transform coPilotCameraObject = null;
        private Transform chopterModelTransform = null;

        private bool isFPS = false;

        protected override void Awake()
        {
            base.Awake();

            chopter = ChopterPilotController.Instance;
            chopterTransform = chopter.transform;

            coPilotCameraObject = ChopterPilotController.Instance.transform.Find("CoPilotCamera");
            if (coPilotCameraObject != null)
                coPilotCameraObject.gameObject.SetActive(false);
            chopterModelTransform = chopter.ChopterModel.transform;
            if (decreaseFactor == 0)
                decreaseFactor = 0.5F;
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

            Vector3 forward = Vector3.zero;

            if (RamboSceneManager.IsMultiplayer)
                forward = chopterTransform.TransformPoint(MULTI_PLAYER_CAMERA_OFFSET_DIRECTION);
            else
                forward = chopterTransform.position + SINGLE_PLAYER_CAMERA_OFFSET_DIRECTION;

            forward = (forward - chopterTransform.position).normalized;
            Vector3 targetPos = Vector3.zero;

            if (RamboSceneManager.IsMultiplayer)
                targetPos = chopterTransform.position + (forward * MultiplayerBaseDistance);
            else
                targetPos = chopterTransform.position + (forward * SingleplayerBaseDistance);

            if (!RamboSceneManager.IsMultiplayer)
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

            transform.position = Vector3.Lerp(transform.position, targetPos, t);
            transform.forward = Vector3.Lerp(transform.forward, forward * -1, t);

            if (shakeDuration > 0 && decreaseFactor != 0)
            {
                transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;
                Debug.Log(shakeDuration);
                Debug.Log(decreaseFactor);
            }
        }

        public void SetCameraShake()
        {
            if (!NetworkLayer.Instance.IsPilot)
                return;

            if (shakeDuration <= 0)
            {
                shakeDuration = shakeAmount;
                originalPos = transform.localPosition;

            }
        }
    }
}