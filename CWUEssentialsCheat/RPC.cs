using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

namespace CWUEssentialsCheat
{

    public static class RPC
    {
       
        public static void RPCA_SpawnRandomDrone(int itemsCount)
        {
            byte[] items = new byte[itemsCount];
            for (int i = 0; i < itemsCount; i++)
            {
                items[i] = (byte)Random.Range(0, 60);
            }
            CallSpawnDrone(items);
        }
        public static List<string> GetAllPrefabs()
        {
            List<string> prefabPaths = new List<string>();
            var gameObjects = Resources.LoadAll<GameObject>("");

            foreach (var obj in gameObjects)
            {
                // if (!obj.GetComponent<PhotonView>()) continue;
                // var comps = obj.GetComponents<Component>();
                var line = obj.name + " ---- ";
                // foreach (var comp in comps)
                // {
                //     line += comp.ToString() + " ";
                // }
                prefabPaths.Add(line);

            }

            return prefabPaths;
        }

        public static void SavePrefabListToFile(List<string> prefabList, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string prefabPath in prefabList)
                {
                    writer.WriteLine(prefabPath);
                }
            }

            Debug.Log("Prefab list saved to: " + filePath);
        }

        
        public static void SaveMonsters()
        {
            List<string> prefabs = GetAllPrefabs();
            
            string savePath = "F:\\PrefabList.txt";
            
            SavePrefabListToFile(prefabs, savePath);
        
        }

        public static void UnlockAll()
        {
            var boxes = Object.FindObjectsOfType<IslandUnlockBox>();
            foreach (var box in boxes)
            {
                var unlock = box.GetComponentInParent<IslandUnlock>();
                var unlocks = box.GetComponentInParent<IslandUnlocks>();
                var view = unlocks.GetComponent<PhotonView>();
                MetaProgressionHandler.UnlockIslandUpgrade(unlock);
                view.RPC("RPCA_Unlock", RpcTarget.All, (object) unlock.transform.transform.GetSiblingIndex());
            }
        }

        public static void SNH_SetCurrentDay(int day)
        {
            SurfaceNetworkHandler.RoomStats.SetCurrentDay(day);
            // SurfaceNetworkHandler.RoomStats.AddQuota(999);

        }
        
        public static void CallSpawnDrone(byte[] itemIDs)
        {
            List<Item> list = new List<Item>();
            for (int i = 0; i < itemIDs.Length; i++)
            {
                Item item;
                if (ItemDatabase.TryGetItemFromID(itemIDs[i], out item))
                {
                    list.Add(item);
                }
            }

            var rpc = HackTool.GetPrivateField<PhotonView>(ShopHandler.Instance, "m_PhotonView");
            rpc.RPC("RPCA_SpawnDrone", RpcTarget.All, itemIDs);
        }
        
    }
}