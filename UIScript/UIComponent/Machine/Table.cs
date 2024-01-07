using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Table : MachineIcon
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
        return 200;
    }

    protected override void onInteract()
    {
    }

}