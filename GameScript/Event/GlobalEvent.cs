using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigOpenEvent : Event
{
    public int config_id { get; private set; }

    public ConfigOpenEvent(int id)
    {
        config_id = id;
    }
}

public class GameStateEvent : Event
{
    public int state_id { get; private set; }

    //1: start game   0: end game
    public GameStateEvent(int state_id)
    {
        this.state_id = state_id;
    }
}