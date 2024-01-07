using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Submit : MachineIcon
{

    protected override void onProgressEnough()
    {
    }

    protected override bool prepProgress()
    {
        return false;
    }

    protected override int totalProgress()
    {
        return 60;
    }

    protected override void onInteract()
    {
    }

    protected override void onItemModified()
    {
        Item i = popItem();
        if(i.id == 0)
        {
            return;
        }

        if (i.item_type == 2)
        {
            UIManager.Instance.GetWindow<GameScene>().putDishes(i);
        } else
        if (i.id == 6)
        {
            Warning.message("AHHHHH! So spicy!!");
        } else
        if (i.name == "hall")
        {
            Warning.message("Your customer has been poisoned. GAME OVER.");
            EventCenter.Instance.trigger(new GameStateEvent(0));
        }
        else
        {
            Warning.message("Customers don't like it!");
        }
    }
}