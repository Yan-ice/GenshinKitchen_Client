using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerInteractEvent : Event
{
    public int interact_type { get; private set; }
    public int target_id { get; private set; }

    public ThrowItem throw_target; //only used in throw

    //0: get   1: put   2: interact   3: move   4: throw
    public PlayerInteractEvent(int type, int target)
    {
        this.interact_type = type;
        this.target_id = target;
        throw_target = null;
    }
}
