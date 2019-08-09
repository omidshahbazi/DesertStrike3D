using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RamboTeam.Client;
using RamboTeam.Client.Utilities;

namespace RamboTeam.Client.GamePlayLogic
{
    public class Projectile : MonoBehaviorBase
    {
        public delegate void ApplyDamage(GameObject GameObject,float Damage);
        public static event ApplyDamage OnApplyDamage;

        [SerializeField]
        public LayerMask Layers;

        private Vector3 startPosition = Vector3.zero;

        private float range = float.MinValue;

        private float speed = float.MinValue;

        private float damage = float.MinValue;

        private bool IsDamaging = false;
  
        public void InitialSetUp(Vector3 StartPosition, float Range, float Speed, float Damage)
        {
            IsDamaging = false;
            this.startPosition = transform.position = StartPosition;
            this.range = Range;
            this.speed = Speed;
            this.damage = Damage;
            this.transform.LookAt(Vector3.forward);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void Update()
        {
            base.Update();
            if (IsDamaging)
                return;
            float movmentDistance = Time.deltaTime*speed;
            Vector3 movement = Vector3.forward *movmentDistance ;
            CheckCollision(movmentDistance);
            this.transform.Translate(movement);
        }
        
        private void CheckCollision(float MovmentValue)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, MovmentValue + 0.1F, Layers, QueryTriggerInteraction.Collide))
                OnBulletHit(raycastHit.collider.gameObject, damage);
            
        }

        private void OnBulletHit(GameObject GameObject, float Damage)
        {
            IsDamaging = true;
            OnApplyDamage?.Invoke(GameObject, Damage);
            Destroy(this.gameObject);
        }

    }
}