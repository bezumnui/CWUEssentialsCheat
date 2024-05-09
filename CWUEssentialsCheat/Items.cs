using System.Collections.Generic;
using Zorro.Core;

namespace CWUEssentialsCheat
{
    public static class Items
    {
        public static Dictionary<string, byte> Regular;
        public static Dictionary<string, byte> Emotions;
        public static Dictionary<string, byte> Spawnable;
        public static Dictionary<string, byte> Upgrades;
        public static Dictionary<string, byte> Misc;
        
        public static void InitItems()
        {

            Regular = new Dictionary<string, byte>();
            Emotions = new Dictionary<string, byte>();
            Spawnable = new Dictionary<string, byte>();
            Upgrades = new Dictionary<string, byte>();
            Misc = new Dictionary<string, byte>();
            
            
            foreach (var item in SingletonAsset<ItemDatabase>.Instance.Objects)
            {
                var itemName = item.displayName;
                if (itemName.Equals(string.Empty)) itemName = item.name;
                if (item.emoteInfo.animationName != "")
                {
                    Emotions.Add(item.id + ": " + itemName, item.id);
                    continue;
                }
                if (item.purchasable)
                {
                    switch (item.Category)
                    {
     
                        case ShopItemCategory.Gadgets:
                            Regular.Add(item.id + ": " + itemName, item.id);
                            continue;
                        case ShopItemCategory.Upgrades:
                            Upgrades.Add(item.id + ": " + itemName, item.id);
                            break;
                    }
                }

                if (item.spawnable)
                {
                    Spawnable.Add(item.id + ": " + itemName, item.id);
                    continue;
                }
                Misc.Add(item.id + ": " + itemName, item.id);
                
                
            }

        }

    }
}