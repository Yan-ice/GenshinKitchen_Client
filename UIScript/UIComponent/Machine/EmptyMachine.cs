using System;
using UnityEngine;

public class EmptyMachine : MachineIcon
{
    protected override void ComponentInit()
    {
    }
    protected override void onProgressEnough()
    {
    }

    protected override bool prepProgress()
    {
        return false;
    }

    protected override int totalProgress()
    {
        return 999;
    }

    protected override void onItemModified()
    {
    }
    protected override void OnInit(GameObject m_go)
    {
    }

}