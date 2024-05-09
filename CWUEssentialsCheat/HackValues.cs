using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class FurniturePhoton
{
    public FurniturePhoton(Quaternion rot, string name)
    {
        Position = Vector3.zero;
        Rotation = rot;
        ModelName = name;
    }
    public Vector3 Position;
    public Quaternion Rotation;
    public string ModelName;
}

namespace CWUEssentialsCheat
{
    
    public static class HackValues
    {
        
        public static CheatFieldFloat currentSpeed = new CheatFieldFloat("Current Speed", 2.5f);
        public static CheatFieldFloat currentGravity = new CheatFieldFloat("Current Gravity", 80);
        
        public static CheatFieldFloat currentDay = new CheatFieldFloat("Current Day", 1f, true);
        public static CheatFieldFloat droneItemsCount = new CheatFieldFloat("Drone Items Count", 1, true);

        
        public static CheatFieldBool speedHack = new CheatFieldBool("Speed Hack");
        public static CheatFieldBool gravityHack = new CheatFieldBool("Gravity Hack");
        public static CheatFieldBool infinityStamina = new CheatFieldBool("Infinity Stamina");
        public static CheatFieldBool infinityHealth = new CheatFieldBool("Infinity Health");
        public static CheatFieldBool infinityOxygen = new CheatFieldBool("Infinity Oxygen");
        public static CheatFieldBool infinityBattery = new CheatFieldBool("Infinity Battery");

        
        public static float scale = 1;

        public static FurniturePhoton[] furnituresPhoton =
        {
            new FurniturePhoton(Quaternion.Euler(360-90, 0, 0), "Chair_Inside"),
            new FurniturePhoton(Quaternion.Euler(360-90, 0, 0), "DeckChair"),
            new FurniturePhoton(Quaternion.Euler(360-90, 0, 0), "PodcastChair"),
            new FurniturePhoton(Quaternion.identity, "Hatshop"),
            new FurniturePhoton(Quaternion.Euler(360-90, 0, 0), "CinemaScreen"),
            new FurniturePhoton(Quaternion.Euler(360-90, 0, 0), "PoolRing"),
            new FurniturePhoton(Quaternion.identity, "Projector"),
        };


        public static string[] spawns = 
        {
            "Arms",
            "Angler",
            "AnglerMimic",
            "AnglerMimic2",
            "BarnacleBall",
            "BlackHoleBot",
            "BigSlap",
            "Bombs",
            "ButtonRobot",
            "CamCreep",
            "Dog",
            "DummyMonster",
            "Ear",
            "EyeGuy",
            "ExplodedGoop",
            "Fire",
            "Flicker",
            "Ghost",
            "Harpooner",
            "Infiltrator2",
            "Jello",
            "Knifo",
            "Mouthe",
            "Mouthe5",
            "MimicMan",
            "Mime",
            "Streamer",
            "Slurper",
            "Snatcho",
            "Spider",
            "Larva",
            "Toolkit_Wisk",
            "Toolkit_Fan",
            "Toolkit_Hammer",
            "Toolkit_Iron",
            "Toolkit_Vaccuum",
            "Puffo",
            "Worm",
            "UltraKnifo",
            "Web",
            "Weeping",
            "Wallo",
            "WalloArm",
            "Zombe",
        };

 
        
        
        
        
        
        
       

        public static Vector3 LookAtPos()
        {
            RaycastHit raycastHit = HelperFunctions.LineCheck(MainCamera.instance.transform.position, MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 30f, HelperFunctions.LayerType.TerrainProp);
            Vector3 vector3 = MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 30f;
            if ((Object) raycastHit.collider != (Object) null)
                vector3 = raycastHit.point;
            Vector3 groundPos = HelperFunctions.GetGroundPos(vector3 + Vector3.up * 1f, HelperFunctions.LayerType.TerrainProp);
            return groundPos;
        }
       
        
    }
    
   
}