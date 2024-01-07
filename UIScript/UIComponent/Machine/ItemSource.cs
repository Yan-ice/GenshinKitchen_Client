using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemSource : MachineIcon
{
    int item_id;

    protected override void onProgressEnough()
    {
        pushItem(GlobalObj.ITEM_LIST[item_id]);
    }

    protected override bool prepProgress()
    {
        return getItemCount() < 3;
    }

    protected override int totalProgress()
    {
        return 200;
    }

    protected override void onInteract()
    {
    }

    protected override void onItemModified()
    {
    }

    public void setParams(int machine_id, int item_id, int initial_cnt)
    {
        this.setMachineId(machine_id);
        clearItem();
        this.item_id = item_id;
        for(int a = 0; a < initial_cnt; a++)
        {
            pushItem(GlobalObj.ITEM_LIST[item_id]);
        }
    }

    protected override bool putItemCheck(Item i)
    {
        return false;
    }
}