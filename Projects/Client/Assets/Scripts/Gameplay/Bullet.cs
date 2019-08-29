//Rambo Team
using UnityEngine;
using RamboTeam.Client.Utilities;

namespace RamboTeam.Client
{
    public class Bullet : MonoBehaviorBase
    {
        private Chopter chopter = null;
        private Vector3 direction = Vector3.zero;
        private float endOfLifetime = 0;

        [SerializeField]
        public LayerMask LayerShouldAttack;
        [SerializeField]
        public LayerMask ChopterLayer;
        [SerializeField]
        public LayerMask enemyLayer;

        public float Speed = 10;
        public float Damage = 10;
        public float Lifetime = 20;

        public GameObject impactParticle;
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


            if (ChopterLayer.IsContains(Collider.gameObject.layer))
            {
                 targetPos = chopter.transform.position;
                chopter.ApplyDamage(Damage);
            }
            else if (enemyLayer.IsContains(Collider.gameObject.layer))
            {
                Enemy enemy = Collider.gameObject.GetComponent<Enemy>();
                if (enemy == null)
                    return;

                targetPos = enemy.transform.position;
                enemy.ApplyDamage(Damage);
            }

            Kill();
        }

        private void Kill()
        {
            if (impactParticle != null)
            {
                GameObject impactObj = GameObject.Instantiate(impactParticle, targetPos, Quaternion.identity) as GameObject;
            }

            Destroy(gameObject);
        }

        public void SetParamaeters(Vector3 Direction)
        {
            direction = Direction;

            transform.forward = direction;
        }
    }
}