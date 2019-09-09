//Rambo Team
using RamboTeam.Client.UI;
using System.Collections.Generic;
using UnityEngine;

namespace RamboTeam.Client
{
    public class ChopterPilotController : MonoBehaviorBase
    {
        public static ChopterPilotController Instance
        {
            get;
            private set;
        }

        [SerializeField]
        public float RangeDetect = 15F;

        [SerializeField]
        public GameObject MissleLuncher;
        [SerializeField]
        public Transform RightMissleLuncher;
        [SerializeField]
        public Transform LeftMissleLuncher;
        [SerializeField]
        public float MissleLuncherRateOfShot;

        [SerializeField]
        public float GaltingGunRateOfShot;
        [SerializeField]
        public Transform GaltingPosition;
        [SerializeField]
        public GameObject GaltingBulletPrefab;


        [SerializeField]
        public GameObject AirCraftLuncher;
        [SerializeField]
        public Transform RightAirCraft;
        [SerializeField]
        public Transform LeftAirCraft;
        [SerializeField]
        public float AirCraftRateOfShot;

        private float nextShotTime;
        private float nextAirCraftShotTime;

        private const float SYNC_RATE = 2;
        private const float SYNC_PERIOD = 1 / SYNC_RATE;

        private float nextSyncTime = 0.0F;
        private Vector3 lastPosition = Vector3.zero;
        private Quaternion lastRotation = Quaternion.identity;

        private float verticalRoation = 0;
        private float horizontalRoation = 0;

        public float MovementSpeed = 10;
        public float RotationSpeed = 10;
        public float VerticalRotation = 15;
        public float HorizontalRotation = 10;

        [SerializeField]
        private GameObject ChopterModel;
        private bool nextPos;
        private bool nextAirCraftPos;

        private List<Enemy> enemiesList = new List<Enemy>();

        public bool IsMoving
        {
            get;
            private set;
        }
        public float sqrRange { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Instance = this;

            lastPosition = transform.position;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            NetworkCommands.OnSyncChopterTransform += OnSyncChopterTransform;
            NetworkCommands.OnEndGame += OnEndGame;
            NetworkCommands.OnSyncChopterShotGatling += OnSyncChopterShotGatling;
            NetworkCommands.OnSyncChopterShotHellfire += OnSyncChopterShotHellfire;
            NetworkCommands.OnSyncChopterShotHydra += OnSyncChopterShotHydra;
            InputManager.Instance.AddInput(KeyCode.Z, ShootHellFireMissle);
            InputManager.Instance.AddInput(KeyCode.X, ShootAirCraft);
            InputManager.Instance.AddInput(KeyCode.C, MachineGunShoot);
            QueryEnemies();
            Enemy.OnEnemyDead += RemoveEnemyFromList;
            sqrRange = RangeDetect * RangeDetect;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            NetworkCommands.OnSyncChopterTransform -= OnSyncChopterTransform;
            NetworkCommands.OnEndGame -= OnEndGame;
            NetworkCommands.OnSyncChopterShotGatling -= OnSyncChopterShotGatling;
            NetworkCommands.OnSyncChopterShotHellfire -= OnSyncChopterShotHellfire;
            NetworkCommands.OnSyncChopterShotHydra -= OnSyncChopterShotHydra;
            InputManager.Instance.RemoveInput(KeyCode.Z, ShootHellFireMissle);
            InputManager.Instance.RemoveInput(KeyCode.X, ShootAirCraft);
            InputManager.Instance.RemoveInput(KeyCode.C, MachineGunShoot);
            Enemy.OnEnemyDead -= RemoveEnemyFromList;
        }

        private void OnSyncChopterTransform(Vector3 Position, Vector3 Rotation)
        {
            if (NetworkLayer.Instance.IsPilot)
                return;

            lastPosition = Position;
            lastRotation = Quaternion.Euler(Rotation);

            nextSyncTime = Time.time + SYNC_PERIOD;
        }

        protected override void Update()
        {
            if (Landing.Instance.state == Landing.State.Landed)
                return;

            base.Update();

            float t = Time.deltaTime;

            bool isControlDown = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
            if (isControlDown)
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    PilotCameraController.Instance.PanOffset += Vector3.forward;
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    PilotCameraController.Instance.PanOffset -= Vector3.forward;
                }

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    PilotCameraController.Instance.PanOffset += Vector3.right;
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    PilotCameraController.Instance.PanOffset -= Vector3.right;
                }
            }

            if (NetworkLayer.Instance.IsPilot)
            {
                IsMoving = false;

                if (!isControlDown && !Chopter.Instance.IsDead)
                {
                    verticalRoation = 0;
                    horizontalRoation = 0;

                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                    {
                        IsMoving = true;
                        verticalRoation = VerticalRotation;

                        if (Input.GetKey(KeyCode.Space) && Chopter.Instance.isEngineBoostEquipted && Chopter.Instance.currentFuelAmount > Chopter.Instance.LowFuelAmount)
                            transform.Translate(transform.forward * Time.deltaTime * (MovementSpeed * 2), Space.World);
                        else
                            transform.Translate(transform.forward * Time.deltaTime * MovementSpeed, Space.World);

                    }
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    {
                        IsMoving = true;
                        verticalRoation = VerticalRotation * -1;

                        transform.Translate(transform.forward * Time.deltaTime * MovementSpeed * -1, Space.World);

                    }

                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    {
                        IsMoving = true;

                        horizontalRoation = HorizontalRotation;

                        transform.Rotate(0, Time.deltaTime * RotationSpeed * -1, 0, Space.World);
                    }
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        IsMoving = true;

                        horizontalRoation = HorizontalRotation * -1;

                        transform.Rotate(0, Time.deltaTime * RotationSpeed, 0, Space.World);
                    }

                    ChopterModel.transform.localRotation = Quaternion.Lerp(ChopterModel.transform.localRotation, Quaternion.Euler(verticalRoation, 0, horizontalRoation), t);
                }

                if (Time.time >= nextSyncTime)
                {
                    NetworkCommands.SyncChopterTransform(transform.position, transform.rotation.eulerAngles);

                    nextSyncTime = Time.time + SYNC_PERIOD;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, lastPosition, t);
                transform.rotation = Quaternion.Lerp(transform.rotation, lastRotation, t);

                IsMoving = ((lastPosition - transform.position).sqrMagnitude > 1);
            }
        }

        private void ShootHellFireMissle()
        {
            if (Chopter.Instance.IsDead || !NetworkLayer.Instance.IsPilot || Chopter.Instance.currentHellfireCount == 0 || Time.time < nextShotTime)
                return;

            nextShotTime = Time.time + MissleLuncherRateOfShot;
            nextPos = !nextPos;

            Vector3 pos = nextPos ? RightMissleLuncher.position : LeftMissleLuncher.position;
            (Enemy en, Vector3 dir) = SearchClosetTarge();

            ShootHellFireMissleInternal(pos, en == null ? GetBottomDirection() : dir);

            NetworkCommands.SyncChopterShotGatling(pos, en == null ? GetBottomDirection() : dir);
        }

        private void MachineGunShoot()
        {
            Debug.Log("key triggered");
            if (Chopter.Instance.IsDead || !NetworkLayer.Instance.IsPilot || Chopter.Instance.currentGatlingGunCount == 0 || Time.time < nextShotTime)
                return;

            nextShotTime = Time.time + GaltingGunRateOfShot;

            (Enemy en, Vector3 dir) = SearchClosetTarge();

            MachineGunShootInternal(GaltingPosition.position, en == null ? GetBottomDirection() : dir);

            NetworkCommands.SyncChopterShotHellfire(GaltingPosition.position, en == null ? GetBottomDirection() : dir);
        }

        private void ShootAirCraft()
        {
            if (Chopter.Instance.IsDead || !NetworkLayer.Instance.IsPilot || Chopter.Instance.currentHydraCount == 0 || Time.time < nextAirCraftShotTime)
                return;

            nextAirCraftShotTime = Time.time + AirCraftRateOfShot;
            nextAirCraftPos = !nextAirCraftPos;

            Vector3 pos = nextAirCraftPos ? RightAirCraft.position : LeftAirCraft.position;
            (Enemy en, Vector3 dir) = SearchClosetTarge();

            ShootAirCraftInternal(pos, en == null ? GetBottomDirection() : dir);

            NetworkCommands.SyncChopterShotHydra(pos, en == null ? GetBottomDirection() : dir);
        }

        private void OnSyncChopterShotGatling(Vector3 Position, Vector3 Direction)
        {
            if (Chopter.Instance.IsDead || NetworkLayer.Instance.IsPilot)
                return;

            ShootHellFireMissleInternal(Position, Direction);
        }

        private void OnSyncChopterShotHellfire(Vector3 Position, Vector3 Direction)
        {
            if (Chopter.Instance.IsDead || NetworkLayer.Instance.IsPilot)
                return;

            MachineGunShootInternal(Position, Direction);
        }

        private void OnSyncChopterShotHydra(Vector3 Position, Vector3 Direction)
        {
            if (Chopter.Instance.IsDead || NetworkLayer.Instance.IsPilot)
                return;

            ShootAirCraftInternal(Position, Direction);
        }

        private void ShootHellFireMissleInternal(Vector3 Position, Vector3 Direction)
        {
            GameObject newObject = GameObject.Instantiate(MissleLuncher, Position, Quaternion.identity) as GameObject;
            Bullet ps = newObject.GetComponent<Bullet>();
            ps.SetParamaeters(Direction);

            Chopter.Instance.TriggerHellfireShot();
        }

        private void MachineGunShootInternal(Vector3 Position, Vector3 Direction)
        {
            GameObject newObject = GameObject.Instantiate(GaltingBulletPrefab, Position, Quaternion.identity) as GameObject;
            Bullet ps = newObject.GetComponent<Bullet>();
            ps.SetParamaeters(Direction);

            Chopter.Instance.TriggerGaltingfireShot();
        }

        private void ShootAirCraftInternal(Vector3 Position, Vector3 Direction)
        {
            GameObject newObject = GameObject.Instantiate(AirCraftLuncher, Position, Quaternion.identity) as GameObject;
            Bullet ps = newObject.GetComponent<Bullet>();
            ps.SetParamaeters(Direction);

            Chopter.Instance.TriggerHydraShot();
        }

        private (Enemy, Vector3) SearchClosetTarge()
        {
            Enemy findTarget = null;
            Vector3 dir = Vector3.zero;
            float closetPoint = float.MaxValue;
            EnemyPriority enemyType = EnemyPriority.none;
            for (int i = 0; i < enemiesList.Count; ++i)
            {
                Enemy en = enemiesList[i];
                if (en == null || en.gameObject == null)
                    continue;
                Vector3 orgin = this.transform.position;
                Vector3 target = en.transform.position;
                orgin.y = target.y = 0;
                Vector3 diff = orgin - target;
                float mag = diff.sqrMagnitude;

                if (mag > sqrRange ||
                   Vector3.Angle((target - orgin), transform.forward) >= 45)
                    continue;
                if (enemyType != EnemyPriority.none && en.EnemyType < enemyType)
                    continue;

                if (mag < closetPoint)
                {
                    closetPoint = diff.sqrMagnitude;
                    enemyType = en.EnemyType;
                    findTarget = en;
                    //They Are looking each other
                    dir = (this.transform.position - en.transform.position).normalized;
                }


            }

            HUDMenu.Instance.SetEnemyHealth(findTarget);
            return (findTarget, -1 * dir);
        }

        private Vector3 GetBottomDirection()
        {
            return ((transform.forward - transform.up / 5)).normalized;
        }

        private void RemoveEnemyFromList(Enemy Enemy)
        {
            if (enemiesList.Contains(Enemy))
                enemiesList.Remove(Enemy);
        }

        private void QueryEnemies()
        {
            enemiesList.AddRange(FindObjectsOfType<Enemy>());
        }

        private void OnEndGame()
        {
            UI.GameOver.Instance.Open();
        }
    }
}