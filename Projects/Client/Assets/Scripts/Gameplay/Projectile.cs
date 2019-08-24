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
      
        [SerializeField]
        public float speed = float.MinValue;
        [SerializeField]
        public float damage = float.MinValue;
        [SerializeField]
        public float Lifetime = 20;

        private bool IsDamaging = false;
        private float endOfLifetime;
        private Vector3 Direction;

        public void InitialSetUp(Vector3 StartPosition, Vector3 Direction)
        {
            IsDamaging = false;
            this.startPosition = transform.position = StartPosition;
            this.transform.LookAt(this.Direction = Direction);
            endOfLifetime = Time.time + Lifetime;
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
            Vector3 movement = Direction * movmentDistance ;
            CheckCollision(movmentDistance);
            this.transform.Translate(movement);
            if (Time.time >= endOfLifetime)
                Destroy(this.gameObject);
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