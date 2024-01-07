using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AutoCold : MachineIcon
{
    List<Recipe> recipe = new List<Recipe>();
    bool begin;
    Recipe target_recipe;

    protected override void onProgressEnough()
    {
        begin = false;
        Item result = GlobalObj.ITEM_LIST[target_recipe.output];
        clearItem();
        pushItem(result);
    }

    protected override bool prepProgress()
    {
        return begin;
    }

    protected override int totalProgress()
    {
        return target_recipe.time*2;
    }

    protected override void onInteract()
    {
        foreach (Recipe r in recipe)
        {
            bool checke = true;
            foreach (int item_id in r.materials)
            {
                if (!containItem(item_id))
                {
                    checke = false;
                    break;
                }
            }
            if (checke)
            {
                begin = true;
                target_recipe = r;
                return;
            }
        }
    }

    protected override void onItemModified()
    {
        begin = false;
    }
    protected override void OnInit(GameObject m_go)
    {
        base.OnInit(m_go);
        foreach (Recipe r in GlobalObj.RECIPE_LIST)
        {
            if (r.operation == "cold")
            {
                recipe.Add(r);
            }
        }
    }

    protected override bool putItemCheck(Item i)
    {
        return getItemCount() < 1;
    }
}