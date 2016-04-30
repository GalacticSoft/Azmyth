using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Assets;
using Azmyth.Stats;

namespace Azmyth.Assets
{
    public class Item : Asset
    {
        private Rarity m_rarity;

        private Quality m_quality;

        private ItemClass m_itemClass;

        private MaterialType m_material;

        private MaterialClass m_materialType;

        private ItemType m_itemType;

        public Rarity Rarity
        {
            get { return m_rarity; }
            set { m_rarity = value; }
        }
        public Quality Quality
        {
            get { return m_quality; }
            set { m_quality = value; }
        }
            
        public ItemClass ItemClass
        {
            get { return m_itemClass; }
            set { m_itemClass = value; }
        }

        public MaterialType MaterialType
        {
            get { return m_material; }
            set { m_material = value; }
        }

        public MaterialClass MaterialClass
        {
            get { return m_materialType; }
            set { m_materialType = value; }
        }

        public ItemType ItemType
        {
            get { return m_itemType; }
            set { m_itemType = value; }
        }

        public static Item Generate(ItemClass itemClass = ItemClass.Any, Rarity rarity = Rarity.Any, Quality quality = Quality.Any, MaterialClass materialClass = MaterialClass.Any, MaterialType materialType = MaterialType.Any)
        {
            Item item = new Item();

            if (rarity == Rarity.Any)
            {
                double rarityPercent = Azmyth.Numbers.Percent() * 100;

                rarity = Rarity.None;

                for (int i = (int)Rarity.None; i < (int)Rarity.Max; i++)
                {
                    if ((float)rarityPercent >= ItemTables.RarityDropChances[i])
                        rarity++;
                }

                 rarity--;
            }

            item.Rarity = rarity;


            if (quality == Quality.Any)
            {
                double qualityPercent = Azmyth.Numbers.Percent() * 100;

                quality = Quality.Poor;

                for (int i = (int)Quality.Poor; i < (int)Quality.Max; i++)
                {
                    if ((float)qualityPercent >= ItemTables.QualityDropChances[i])
                        quality++;
                }

                quality--;
            }

            item.Quality = quality;

            if (materialClass == MaterialClass.Any)
            {
                double materialTypePercent = Azmyth.Numbers.Percent() * 100;

                materialClass = MaterialClass.None;

                for (int i = (int)MaterialClass.None; i < (int)MaterialClass.Max; i++)
                {
                    if ((float)materialTypePercent >= ItemTables.MaterialTypeDropChances[i])
                        materialClass++;
                }

                materialClass--;
            }

            item.MaterialClass = materialClass;

            if (materialType == MaterialType.Any)
            {
                double materialPercent = Azmyth.Numbers.Percent() * 100;

                materialType = MaterialType.Cloth;
                var materials = ItemTables.Materials.Values.Where(i => i.MaterialType == item.MaterialClass);

                foreach (Material m in materials)
                {
                    if ((float)materialPercent >= m.DropMod)
                        materialType = m.MaterialID;
                }
            }

            item.MaterialType = materialType;

            if (itemClass == ItemClass.Any)
            {
                double itemTypePercent = Azmyth.Numbers.Percent() * 100;

                itemClass = ItemClass.None;

                for (int i = (int)ItemClass.None; i < (int)ItemClass.Max; i++)
                {
                    if ((float)itemTypePercent >= ItemTables.ItemTypeDropChances[i])
                        itemClass++;
                }

                itemClass--;
            }

            item.ItemClass = itemClass;

            var itemTypes = ItemTables.ItemTypes.Where(i => i.ItemClass == item.ItemClass);

            int num = Numbers.NumberRange(0, itemTypes.Count<ItemType>()-1);
            int count = 0;

            foreach(ItemType t in itemTypes)
            {
                if(count == num)
                {
                    item.ItemType = t;
                    break;
                }

                count++;
            }

            string[] names = item.ItemType.Names.Split('|');

            num = Numbers.NumberRange(0, names.Count<string>()-1);


            item.Name = quality + " quality " + rarity +" " + materialType + " " + names[num];

            item.Name = Azmyth.Strings.Article(item.Name) + " " + item.Name;

            return item;
        }
    }
}
