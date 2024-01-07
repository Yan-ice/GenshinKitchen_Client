using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bin : MachineIcon
{

    protected override void onProgressEnough()
    {
        popItem();
    }

    protected override bool prepProgress()
    {
        return getItemCount()>0;
    }

    protected override int totalProgress()
    {
        return 60;
    }

    protected override void onInteract()
    {
    }


}