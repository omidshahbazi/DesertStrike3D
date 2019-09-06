//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;
using System.Collections.Generic;
using UnityEngine;

namespace RamboTeam.Client
{
	public class NetworkLayer : MonoBehaviorBase
	{
		private const byte ON_CONNECTION_CATEGORY = byte.MaxValue;
		private const byte ON_CONNECTED_COMMAND = byte.MaxValue;
		private const byte ON_CONNECTION_LOST_COMMAND = byte.MaxValue - 1;
		private const byte ON_CONNECTION_RESTORED_COMMAND = byte.MaxValue - 2;

		private static NetworkLayer instance = null;
		public static NetworkLayer Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameObject = new GameObject("NetworkLayer");
					DontDestroyOnLoad(gameObject);
					instance = gameObject.AddComponent<NetworkLayer>();
				}

				return instance;

			}
		}

		private Client client = null;
		private List<BufferStream> incommingMessages = null;

		public bool IsConnected
		{
			get { return (client == null ? false : client.IsConnected); }
		}

        public bool IsPilot
        {
            get;
            set;
        } = true;

		protected override void Awake()
		{
			base.Awake();

			Application.runInBackground = true;

			Connect();
		}

		protected override void Update()
		{
			base.Update();

			while (incommingMessages.Count != 0)
			{
				BufferStream buffer = incommingMessages[0];
				incommingMessages.RemoveAt(0);

				byte category = buffer.ReadByte();
				byte command = buffer.ReadByte();

				if (category == ON_CONNECTION_CATEGORY)
				{
					if (category == ON_CONNECTED_COMMAND)
					{
						NetworkCommands.HandleConnected();
					}
					else if (category == ON_CONNECTION_LOST_COMMAND)
					{
						NetworkCommands.HandleConnectionLost();
					}
					else if (category == ON_CONNECTION_RESTORED_COMMAND)
					{
						NetworkCommands.HandleConnectionRestored();
					}
				}
				else if (category == Commands.Category.LOBBY)
				{

				}
				else if (category == Commands.Category.ROOM)
				{
					if (command == Commands.Room.MASTER)
					{
						IsPilot = true;
						NetworkCommands.HandleJoinedToRoom();
						NetworkCommands.HandlePilot();
					}
					else if (command == Commands.Room.SECONDARY)
					{
						IsPilot = false;
						NetworkCommands.HandleJoinedToRoom();
						NetworkCommands.HandleCoPilot();
					}
					else if (command == Commands.Room.SYNC_CHOPTER_TRANSFORM)
					{
						NetworkCommands.HandleSyncChopterTransform(buffer);
					}
					else if (command == Commands.Room.END_GAME)
					{
						NetworkCommands.HandleEndGame();
					}
					else if (command == Commands.Room.SYNC_PILOT_FIRE)
					{
						byte bulletType = buffer.ReadByte();

						if (bulletType == Commands.Bullet.HELLFIRE)
							NetworkCommands.HandleSyncChopterShotHellfire(buffer);
						else if (bulletType == Commands.Bullet.HYDRA)
							NetworkCommands.HandleSyncChopterShotHydra(buffer);
						else if (bulletType == Commands.Bullet.GATLING)
							NetworkCommands.HandleSyncChopterShotGatling(buffer);
					}
					else if (command == Commands.Room.SYNC_CO_PILOT_FIRE)
					{
						byte bulletType = buffer.ReadByte();

						if (bulletType == Commands.Bullet.MACHINEGUN)
							NetworkCommands.HandleSyncChopterShotMachinegun(buffer);
					}
					else if (command == Commands.Room.SYNC_ENEMY_FIRE)
					{
						byte bulletType = buffer.ReadByte();

						if (bulletType == Commands.Enemy.ANTI_AIRCRAFT)
							NetworkCommands.HandleSyncAntiAircraftShot(buffer);
						else if (bulletType == Commands.Enemy.M3VDA)
							NetworkCommands.HandleSyncM3VDAShot(buffer);
						else if (bulletType == Commands.Enemy.MISSLE_LAUNCHER)
							NetworkCommands.HandleSyncMissleLauncherShot(buffer);
						else if (bulletType == Commands.Enemy.RIFLE_MAN)
							NetworkCommands.HandleSyncRifleManShot(buffer);
						else if (bulletType == Commands.Enemy.RPG_MAN)
							NetworkCommands.HandleSyncRPGManShot(buffer);
					}
				}
			}
		}

		public void Connect()
		{
			incommingMessages = new List<BufferStream>();

			client = new Client();
			client.Connect();

			client.OnConnected += OnConnected;
			client.OnConnectionLost += OnConnectionLost;
			client.OnConnectionRestored += OnConnectionRestored;
			client.OnMessageReceived += OnMessageReceived;
		}

		private void OnConnected()
		{
			incommingMessages.Add(new BufferStream(new byte[] { ON_CONNECTION_CATEGORY, ON_CONNECTED_COMMAND }));

			Debug.Log("OnConnected");
		}

		private void OnConnectionLost()
		{
			incommingMessages.Add(new BufferStream(new byte[] { ON_CONNECTION_CATEGORY, ON_CONNECTION_LOST_COMMAND }));

			Debug.LogError("OnConnectionLost");
		}

		private void OnConnectionRestored()
		{
			incommingMessages.Add(new BufferStream(new byte[] { ON_CONNECTION_CATEGORY, ON_CONNECTION_RESTORED_COMMAND }));

			Debug.LogError("OnConnectionRestored");
		}

		public void Send(BufferStream Buffer)
		{
			client.Send(Buffer);
		}

		private void OnMessageReceived(NetworkingPlayer Player, Binary Frame)
		{
			incommingMessages.Add(new BufferStream(Frame.StreamData.byteArr));

		}
	}
}