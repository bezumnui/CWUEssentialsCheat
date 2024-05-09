using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using JetBrains.Annotations;
using Photon.Pun;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Zorro.Core;

namespace CWUEssentialsCheat
{
    public class HackMain : MonoBehaviour
    {
        public PlayerController playerController;
        public Player localPlayer;
        public static HackMain Instance;
        public ObjectPlacer placer;
        
        private bool _showMenu;
        private bool cursorBefore;
        private bool afterCursor;
        private static int currentItem;

        public PhotonView photonView;
        public PingMasterClient ping;

        private void OnEnable()
        {
            Instance = this;
        }

        private void Start()
        {
            ping = PingMasterClient.Init();

            photonView = gameObject.AddComponent<PhotonView>();
            placer = gameObject.AddComponent<ObjectPlacer>();
            photonView.ViewID = 7766;
            photonView.Synchronization = ViewSynchronization.ReliableDeltaCompressed;

            Items.InitItems();
            currentItem = 35;
        }
        
        [PunRPC]
        private void SpawnMonsterHM(string monster, Vector3 groundPos)
        {
            MonsterSpawner.SpawnMonster(monster, groundPos);
        }
        
        [PunRPC]
        private void RPCA_SpawnFurniture(string name, Vector3 pos, Quaternion rotation)
        {
            PhotonNetwork.Instantiate(name, pos, rotation);
        }
        
     

        private bool canFall = true;

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.F1))
            {
                _showMenu = !_showMenu;
                localPlayer.data.hookedIntoTerminal = _showMenu;
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                RPC.SaveMonsters();
            }
         

            if (!afterCursor) cursorBefore = Cursor.visible;
            
            
            localPlayer = Player.localPlayer;
            if (!localPlayer) return;
            playerController = Player.localPlayer.refs.controller;
            
            if (Input.GetKey(KeyCode.F))
            {
                if (canFall)
                {
                    canFall = false;
                    Invoke(nameof(ResetFall), 1/ 2f);
                    CallFall(1);
                }
                // localPlayer.data.fallTime = 1f;

            }
        }

        [PunRPC]
        public void RPCKillMonsters()
        {
            Monster.KillAll();
        }
        private void ResetFall()
        {
            canFall = true;
            
        }

        private void CallFall(float seconds)
        {
            localPlayer.refs.view.RPC("RPCA_Fall", RpcTarget.All, (object) seconds);
        }

        private void RPCA_PlayerRevive()
        {
            localPlayer.data.dead = false;
            localPlayer.data.health = 30f;
            if (localPlayer.refs.view.IsMine)
            {
                Player.justDied = false;
                NetworkVoiceHandler.TalkToAlive();
            }

            if (!PlayerHandler.instance.playersAlive.Contains(localPlayer))
            {
        
                PlayerHandler.instance.playersAlive.Add(localPlayer);
            }

            if (localPlayer.refs.view.IsMine)
            {
                UI_Feedback.instance.Revive();
            }
        }

        [CanBeNull]
        private static Item GetItem(byte id)
        {
            Item item;
            if (!ItemDatabase.TryGetItemFromID(id, out item)) return null;
            item.id = id;
            return item;
        }

        public static void GiveItem(byte id)
        {
            Player.localPlayer.TryGetInventory(out var o);
            var item = GetItem(id);
            // o.TryAddItem();
            if (o.TryGetFeeSlot(out var slot) && slot.SlotID < 3)
            {
                slot.Add(new ItemDescriptor(item, new ItemInstanceData(Guid.NewGuid())));
                // return true;
            }
        } 

        private void LateUpdate()
        {
            switch (_showMenu)
            {
                case true:
                    Cursor.visible = true;
                    afterCursor = true;
                    Cursor.lockState = CursorLockMode.None;
                    break;
                case false when afterCursor:
                    afterCursor = false;
                    Cursor.visible = cursorBefore;
                    if (cursorBefore) Cursor.lockState = CursorLockMode.Locked;

                    break;
            }
            
            playerController.sprintMultiplier = HackValues.currentSpeed;
            localPlayer.refs.controller.gravity = HackValues.currentGravity;
            if (HackValues.infinityStamina)
            {
                localPlayer.data.currentStamina = 10;
            }

            if (HackValues.infinityOxygen)
            {
                localPlayer.data.remainingOxygen = localPlayer.data.maxOxygen;
            }
            if (HackValues.infinityHealth)
            {
                localPlayer.data.health = 100;
                if (localPlayer.data.dead)
                {
                    RPCA_PlayerRevive();
                }
            }

            BatteryHack();

        }
        

        public void BatteryHack()
        {
            if (!HackValues.infinityBattery) return;
            Player.localPlayer.TryGetInventory(out var o);
            if (!o) return;
            foreach (ItemDescriptor iDescriptor in o.GetItems())
            {
                foreach (var de in iDescriptor.data.m_dataEntries)
                {
                    if (!(de is BatteryEntry battery)) return;
                    battery.m_charge = battery.m_maxCharge;
                }
            }
        }
        
        
        private void OnGUI()
        {
            if (_showMenu)
            {
                var gui = new DrawGUI();
                gui.DrawScaler();
                gui.DrawMisc(0);
                gui.DrawHost(0);
                gui.DrawMeta(0);
                gui.DrawFunny(0);
                gui.DrawAllItems(1, 2, 1, 3);
                if (ping.MasterHasCheat)
                {
                    gui.DrawSpawn(4);
                    gui.DrawFurniture(5);

                };

            
                // HackValues

            }
            
        }
        
        
    }
}