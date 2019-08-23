//Rambo Team
using UnityEngine;

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

            Debug.Log("Collide");
            if (Collider.gameObject.layer != LayerShouldAttack)
                return;

            if (Collider.gameObject.layer == ChopterLayer)
                chopter.ApplyDamage(Damage);
            else if (Collider.gameObject.layer == enemyLayer)
            {
                Enemy enemy = Collider.gameObject.GetComponent<Enemy>();
                if (enemy == null)
                    return;

                enemy.ApplyDamage(Damage);
            }

            Kill();
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