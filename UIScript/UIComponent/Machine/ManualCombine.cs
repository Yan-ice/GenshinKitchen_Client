using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ManualCombine : MachineIcon
{
    List<Recipe> recipe = new List<Recipe>();
    Recipe target_recipe;

    protected override void onProgressEnough()
    {
        Item result = GlobalObj.ITEM_LIST[target_recipe.output];
        clearItem();
        pushItem(result);
    }

    protected override bool prepProgress()
    {
        return target_recipe.output!=0 && GlobalObj.Instance.interacting
            && GlobalObj.Instance.target_id == id;
    }

    protected override int totalProgress()
    {
        return target_recipe.time;
    }

    protected override void onItemModified()
    {
        target_recipe.output = 0;
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
                target_recipe = r;
                return;
            }
        }
    }
    protected override void OnInit(GameObject m_go)
    {
        base.OnInit(m_go);
        foreach (Recipe r in GlobalObj.RECIPE_LIST)
        {
            if (r.operation == "combine")
            {
                recipe.Add(r);
            }
        }
    }

}