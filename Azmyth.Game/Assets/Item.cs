using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Assets;

namespace Azmyth.Assets
{
    public class Result
    {
        public MaterialType type;
        public Materials material;
    }

    public class Item : Asset
    {


        public static Result Generate(double materialTypePercent, double materialPercent)
        {
            Result res = new Result();

            MaterialType materialType = MaterialType.Animal;

            for (int i = (int)MaterialType.Animal; i <= (int)MaterialType.Max; i++ )
            {
                if ((float)materialTypePercent >= ItemMaterial.MaterialTypeDropChances[(MaterialType)i])
                    materialType++;
            }

            res.type = materialType - 1;


            Materials materialResult = Materials.Cloth;
            var materials = ItemMaterial.MaterialList.Values.Where(item => item.MaterialType == res.type);

            foreach(ItemMaterial material in materials)
            {
                if ((float)materialPercent >= material.DropMod)
                    materialResult = material.Material;

            }

            res.material = materialResult;

            return res;

        }

        public Item()
        {

           
            
        }
    }
}
