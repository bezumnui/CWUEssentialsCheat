using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

namespace MonoInjectionTemplate
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
            RPCA_SpawnDrone(items);
        }
        public static List<string> GetAllPrefabs()
        {
            List<string> prefabPaths = new List<string>();

            PhotonView[] gameObjects = Resources.LoadAll<PhotonView>("");

            foreach (PhotonView obj in gameObjects)
            {
                // if (!obj.GetComponent<PhotonView>()) continue;
                prefabPaths.Add(obj.name + ";");
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
        
        public static void RPCA_SpawnDrone(byte[] itemIDs)
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

            var rpc = ShopHandler.Instance.gameObject.GetComponent<PhotonView>();
            rpc.RPC("RPCA_SpawnDrone", RpcTarget.All, itemIDs);
            // Object.Instantiate(ShopHandler.Instance.droneObject, Vector3.zero, Quaternion.Euler(new Vector3(0f, -30f, 0f))).GetComponent<Drone>().items = list.ToArray();
        }
        
    }
}