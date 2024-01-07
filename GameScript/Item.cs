using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public struct Item
{

    public static Item NOTHING = new Item(0, "nothing", "NONE");

    public int id;
    public string name;
    public string display;
    public int item_type;

    public Item(int id, string name, string display)
    {
        this.id = id;
        this.name = name;
        this.display = display;
        this.item_type = 0;
    }

    public Sprite getIconImg()
    {
        if (id == 0) return null;

        return GlobalObj.ICON.LoadResource<Sprite>("items/" + name+".png");
    }
    public Color getTypeColor()
    {
        switch (item_type)
        {
            case 0: return new Color(0.9f, 0.9f, 0.9f);
            case 1: return new Color(0.8f, 0.8f, 1f);
            case 2: return Color.yellow;
            case 3: return Color.black;
        }
        return Color.white;
    }

}
