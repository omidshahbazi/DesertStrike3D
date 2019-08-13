﻿//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;
using System.Collections.Generic;
using UnityEngine;

namespace RamboTeam.Client
{
	public class NetworkLayer : MonoBehaviorBase
	{
		public static NetworkLayer Instance
		{
			get;
			private set;
		}

		private Client client = null;
		private List<Binary> incommingMessages = null;

		protected override void Awake()
		{
			base.Awake();

			Instance = this;

			Application.runInBackground = true;

			Connect("127.0.0.1");
		}

		protected override void Update()
		{
			base.Update();

			while (incommingMessages.Count != 0)
			{
				Binary messageInfo = incommingMessages[0];
				incommingMessages.RemoveAt(0);

				BufferStream buffer = new BufferStream(messageInfo.StreamData.byteArr);

				byte category = buffer.ReadByte();
				byte command = buffer.ReadByte();

				if (category == Commands.Category.LOBBY)
				{

				}
				else if (category == Commands.Category.ROOM)
				{
					if (command == Commands.Room.MASTER)
					{
						NetworkCommands.HandleJoinedToRoom(buffer);
						NetworkCommands.HandlePilot(buffer);
					}
					else if (command == Commands.Room.SECONDARY)
					{
						NetworkCommands.HandleJoinedToRoom(buffer);
						NetworkCommands.HandleCommando(buffer);
					}
					else if (command == Commands.Room.SYNC_CHOPTER_TRANSFORM)
					{
						NetworkCommands.HandleSyncChopterTransform(buffer);
					}
					else if (command == Commands.Room.SYNC_CHOPTER_FIRE)
					{
						byte bulletType = buffer.ReadByte();

						if (bulletType == Commands.Bullet.HELLFIRE)
							NetworkCommands.HandleSyncChopterShotHellfire(buffer);
						else if (bulletType == Commands.Bullet.HYDRA)
							NetworkCommands.HandleSyncChopterShotHydra(buffer);
						else if (bulletType == Commands.Bullet.GATLING)
							NetworkCommands.HandleSyncChopterShotGatling(buffer);
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

		public void Connect(string Host)
		{
			incommingMessages = new List<Binary>();

			client = new Client();
			client.Connect(Host);

			client.OnConnected += OnConnected;
			client.OnDisconnected += OnDisconnected;
			client.OnMessageReceived += OnMessageReceived;
		}

		private void OnConnected()
		{
			//
			// TODO: Place Holder
			//
			NetworkCommands.JoinToRoom();
		}

		private void OnDisconnected()
		{
			Debug.LogError("Disconnected");
		}

		public void Send(BufferStream Buffer)
		{
			client.Send(Buffer);
		}

		private void OnMessageReceived(NetworkingPlayer Player, Binary Frame)
		{
			incommingMessages.Add(Frame);

		}
	}
}