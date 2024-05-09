using System;
using System.Collections.Generic;
using MonoInjectionTemplate.callableElements;
using Photon.Pun;
using UnityEngine;

namespace MonoInjectionTemplate
{
    
    
    public class DrawGUI
    {
        private static readonly List<Vector2> LastPositions = new List<Vector2>(20);
        private static readonly int LastPositionsSize = 20;
        private static readonly Vector2 StartMargins = new Vector2(50, 50);
        private static readonly Vector2 Margins = new Vector2(20, 20);
        private static readonly int columnWidth = 200;

        private static readonly int ControlHeight = 20;
        private static readonly int ControlDist = 5;
        private static readonly int ElementsMargin = 10;

        private static readonly List<CallableElement> Elements = new List<CallableElement>(100);
        private static float _scale = 1;
        
        public DrawGUI()
        {
            LastPositions.Clear();
            for (int i = 0; i < LastPositionsSize; i++)
            {
                LastPositions.Add(Vector2.zero);
            }
        }

        private void DrawColumn(int positionX, string text)
        {
            var height = CalculateColumnHeight();
            var lastPositionY = (int)(LastPositions[positionX].y + Margins.y);
            UIHelper.Begin(
                text,
                (int)((StartMargins.x + (columnWidth + Margins.x) * positionX)),
                (int)((StartMargins.y + lastPositionY)),
                columnWidth,
                height,
                ElementsMargin,
                ControlHeight,
                ControlDist,
                20,
                _scale
                );
            LastPositions[positionX] = new Vector2(positionX, lastPositionY + height);
            GUIFlush();
            
        }

        private int CalculateColumnHeight()
        {
            return (ControlHeight + (ControlHeight + ControlDist) * Elements.Count);
        }

        public CEButton AddButton(string text)
        {
            var button = new CEButton(text);
            Elements.Add(button);
            return button;
        }
        public CESlider AddSlider(CheatFieldFloat cf, float fromValue = 0, float toValue = 1f)
        {
            
            if (cf.Name != string.Empty)
            {
                Elements.Add(new StubElement());
            }
            var slider = new CESlider(fromValue, toValue, cf);
            Elements.Add(slider);
            return slider;
        }

        public void GUIFlush()
        {
            foreach (var element in Elements)
            {
                element.Draw();
            }

            Elements.Clear();
        }

        public void DrawScaler()
        {
            _scale = GUI.HorizontalSlider(new Rect(10, 10, 150, 10), _scale, 0.3f, 2f);
        }

        public void DrawMisc(int positionX)
        {
            
            AddButton("Infinity Stamina").Control(HackValues.infinityStamina);
            AddButton("Infinity Health").Control(HackValues.infinityHealth);
            AddButton("Infinity Oxygen").Control(HackValues.infinityOxygen);
            AddButton("Infinity Battery").Control(HackValues.infinityBattery);
            AddButton("Speed Hack").Control(HackValues.speedHack);

            if (HackValues.speedHack)
            {
                AddSlider(HackValues.currentSpeed, 2, 40);
            }
            else HackValues.currentSpeed.Value = 2.1f;
            
            AddButton("Gravity Hack").Control(HackValues.gravityHack);
            if (HackValues.gravityHack)
            {
                AddSlider(HackValues.currentGravity, -200, 200);
                AddButton("Set To Zero").On((c) => HackValues.currentGravity.Value = 0f);
            }
            else HackValues.currentGravity.Value = 80f;

            
            DrawColumn(positionX, "Misc");
        }

        public void DrawMeta(int positionX)
        {
            
            AddButton("Add 500 MC").On((c) => MetaProgressionHandler.RemoveMetaCoins(-500));
            AddButton("Unlock All Hats").On((c) =>  MetaProgressionHandler.UnlockAllHats());
            AddButton("Unlock All Upgrades").On((c) => RPC.UnlockAll());
            AddButton("Delete All Data").On((c) =>
            {
                SaveSystem.DeleteMetaData();
                MetaProgressionHandler.Init();
            });
            AddButton("Set 0 MC").On((c) =>
            {
                MetaProgressionHandler.SetMetaCoins(0);
            });
      
            DrawColumn(positionX, "Meta");

        }
        
        public void DrawFunny(int positionX)
        {
            AddButton("Kill All Monsters").On((c) =>
            {
                Monster.KillAll();
                HackMain.Instance.photonView.RPC("RPCKillMonsters", RpcTarget.All);
            });

            AddSlider(HackValues.droneItemsCount, 1, 100);
            AddButton("Call Drone").On((c) => RPC.RPCA_SpawnRandomDrone((int)HackValues.droneItemsCount));
            AddButton("Get $500").On((c) => SurfaceNetworkHandler.RoomStats.RemoveMoney(-500));
            DrawColumn(positionX, "Fun");

            
        }
        public void DrawCertainItems(Dictionary<string, byte> items, string name, int positionX)
        {
            foreach (var kvp in items)
            {
                AddButton(kvp.Key).On((c) => HackMain.GiveItem(kvp.Value));
            }
            DrawColumn(positionX, name);
        }

        public void DrawAllItems(int x0, int x1, int x2, int x3)
        {
            DrawCertainItems(Items.Regular, "Items: Regular", x0);
            DrawCertainItems(Items.Emotions, "Items: Emotions", x1);
            DrawCertainItems(Items.Spawnable, "Items: Spawnable", x2);
            DrawCertainItems(Items.Misc, "Items: Misc", x3);
        }

        public void DrawHost(int positionX)
        {
            AddButton("Hack Score").On((c) => SurfaceNetworkHandler.RoomStats.AddQuota(999));
            AddButton("Next Day").On((c) => SurfaceNetworkHandler.RoomStats.NextDay());
            {
                if (SurfaceNetworkHandler.RoomStats != null)
                {
                    var currentDay = SurfaceNetworkHandler.RoomStats.CurrentDay;
                    var fromDay = currentDay - 50;
                    var toDay = currentDay + 50;
                    if (fromDay < 1) fromDay = 1;
                    if (HackValues.currentDay > toDay || HackValues.currentDay < fromDay)
                    {
                        HackValues.currentDay.Value = currentDay;
                    } 
                    AddSlider(HackValues.currentDay, fromDay, toDay);
                }
               
            }
            
            AddButton("Set day").On((c) => RPC.SNH_SetCurrentDay((int)HackValues.currentDay));
            DrawColumn(positionX, "Host");

        }
        
        public void DrawSpawn(int positionX)
        {
            for (int i = 0; i < HackValues.spawns.Length; i++)
            {
                var index = i;
                AddButton(HackValues.spawns[i]).On(c =>
                {
                    HackMain.Instance.photonView.RPC("SpawnMonsterHM", RpcTarget.MasterClient, 
                        HackValues.spawns[index], HackValues.LookAtPos());

                });
            }
            DrawColumn(positionX, "Spawn Monsters");
        }
        
        public void DrawFurniture(int positionX)
        {
            foreach (var furniture in HackValues.furnituresPhoton)
            {
                AddButton(furniture.ModelName).On(c =>
                {
                    if (HackMain.Instance && HackMain.Instance.placer)
                    {
                        HackMain.Instance.placer.ShowObject(furniture);
                    } 
                });
                
            }
            DrawColumn(positionX, "Spawn Furniture");


        }
    }
}