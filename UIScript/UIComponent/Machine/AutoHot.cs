using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AutoHot : MachineIcon
{
    List<Recipe> recipe = new List<Recipe>();
    Recipe target_recipe;

    bool begin = false;
    protected override void onProgressEnough()
    {
        Item result = GlobalObj.ITEM_LIST[target_recipe.output];
        clearItem();
        pushItem(result);
        begin = false;
    }

    protected override bool prepProgress()
    {
        return begin;
    }

    protected override int totalProgress()
    {
        return target_recipe.time*2;
    }

    protected override void onItemModified()
    {
        begin = false;
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
                    target_recipe.output = 0;
                    break;
                }
            }
            if (checke)
            {
                target_recipe = r;
                begin = true;
                return;
            }
        }
    }
    protected override void OnInit(GameObject m_go)
    {
        base.OnInit(m_go);
        foreach (Recipe r in GlobalObj.RECIPE_LIST)
        {
            if (r.operation == "hot")
            {
                recipe.Add(r);
            }
        }
    }

}