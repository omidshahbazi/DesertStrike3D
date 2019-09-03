//Rambo Team
using UnityEngine;
using RamboTeam.Client.Utilities;
using System;

namespace RamboTeam.Client
{
    public class Bullet : MonoBehaviorBase
    {
        public enum ImpactPosition
        {
            Target,
            Bullet
        }

        private Chopter chopter = null;
        private Vector3 direction = Vector3.zero;
        private float endOfLifetime = 0;

        [SerializeField]
        public LayerMask LayerShouldAttack;
        [SerializeField]
        public LayerMask ChopterLayer;
        [SerializeField]
        public LayerMask enemyLayer;
        [SerializeField]
        public LayerMask terrainLayer;
        [SerializeField]
        public LayerMask waterLayer;

        public float Speed = 10;
        public float Damage = 10;
        public float Lifetime = 20;

        public GameObject impactToBuildingParticle;
        public GameObject impactToTerrainParticle;
        public GameObject impactToWaterParticle;

        private Vector3 targetPos;

        protected override void Start()
        {
            base.Start();

            chopter = Chopter.Instance;

            endOfLifetime = Time.time + Lifetime;
        }

        protected override void Update()
        {
            base.Update();

            transform.Translate(direction * Speed * Time.deltaTime, Space.World);

            if (Time.time >= endOfLifetime)
                Kill();
        }

        protected override void OnTriggerEnter(Collider Collider)
        {
            base.OnTriggerEnter(Collider);

            if (!LayerShouldAttack.IsContains(Collider.gameObject.layer))
                return;
            Debug.Log("Collide");

            GameObject impactPart = null;


            if (ChopterLayer.IsContains(Collider.gameObject.layer))
            {
                chopter.ApplyDamage(Damage);
                //PilotCameraController.Instance.SetCameraShake();
                targetPos = transform.position;
                impactPart = impactToBuildingParticle;
            }
            else if (enemyLayer.IsContains(Collider.gameObject.layer))
            {
                Enemy enemy = Collider.gameObject.GetComponent<Enemy>();
                if (enemy == null)
                    return;

                enemy.ApplyDamage(Damage);
                PilotCameraController.Instance.SetCameraShake();

                impactPart = impactToBuildingParticle;
                targetPos = transform.position;

            }
            else if (terrainLayer.IsContains(Collider.gameObject.layer)) //impact to terrain
            {
                impactPart = impactToTerrainParticle;
                targetPos = transform.position;

            }
            else if (waterLayer.IsContains(Collider.gameObject.layer))//impact to water
            {
                impactPart = impactToWaterParticle;
                targetPos = transform.position;
            }

            PlayImpactParticle(impactPart, targetPos);

            Kill();
        }

        private void PlayImpactParticle(GameObject impactPart, Vector3 targetPos)
        {
            if (impactPart != null)
            {
                GameObject impactObj = GameObject.Instantiate(impactPart, targetPos, Quaternion.identity) as GameObject;
            }
        }

        private void Kill()
        {
            Destroy(gameObject);
        }

        public void SetParamaeters(Vector3 Direction)
        {
            direction = Direction;

            transform.forward = direction;
        }
    }
}