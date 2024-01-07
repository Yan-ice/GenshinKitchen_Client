using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

[Serializable]
public class JsonSet<T>
{
    public string m_tag;
    public List<T> m_list = new List<T>();
}


public class GlobalObj : Singleton<GlobalObj>
{
    //bundle pack
    public static BundlePackLoader ICON = new BundlePackLoader("AssetBundlePack/icon_resource");
    public static BundlePackLoader DATA = new BundlePackLoader("AssetBundlePack/game_data");

    //load from json
    public static List<Item> ITEM_LIST;
    public static List<Recipe> RECIPE_LIST;

    //global config
    public bool ALLOW_KEY_INPUT = false;
    public bool GAME_READY = false;
    public bool GAME_START = false;
    public float PLAYER_SPEED = 1.5f;
    public float THROW_SPEED = 2f;
    public float PROCESS_SPEED = 1f;
    public Sprite CHARACTER = GlobalObj.ICON.LoadResource<Sprite>("chara/8.png");

    //global var
    public SerialPort port = new SerialPort();
    public Player player_robot;
    public int target_id = 1;
    public int[] move_target_id = new int[2] {1,20};
    public bool interacting = false;
    public bool interact_key = false;

    //serial var
    public bool player_ready = false;
    public bool player_hand_full = false;
    public bool target_ready = false;
    public bool target_has_item = false;
    public bool target_item_full = false;
    public bool error_found = false;

    private void gameStateListen(GameStateEvent e)
    {
        if (e.state_id == 0 && GAME_START)
        {
            GAME_START = false;
            target_id = 1;
            move_target_id = new int[2] { 1, 20 };
            interact_key = false;
            player_ready = false;
            player_hand_full = false;
            target_ready = false;
            target_has_item = false;
            target_item_full = false;
            return;
            //game stop
        }
        if (e.state_id == 1 && GAME_READY && !GAME_START)
        {
            GAME_START = true;
            return;
            //game stop
        }
        e.cancelEvent();

    }
    public GlobalObj()
    {
        MonoUpdateManager.Instance.AddUpdateListener(keyInput);
        EventCenter.Instance.regSimpleAction<GameStateEvent>(gameStateListen, 99);
        new Thread(uartInput).Start();

        UpdateLang();
    }

    public static void UpdateLang()
    {
        if (LanguageText.current_lang == "zh_CN")
        {
            TextAsset txt = GlobalObj.DATA.LoadResource<TextAsset>("json/item_list.json");
            ITEM_LIST = JsonConvert.DeserializeObject<JsonSet<Item>>(txt.text).m_list;

            txt = GlobalObj.DATA.LoadResource<TextAsset>("json/recipe_list.json");
            RECIPE_LIST = JsonConvert.DeserializeObject<JsonSet<Recipe>>(txt.text).m_list;
        }
        else
        {
            TextAsset txt = GlobalObj.DATA.LoadResource<TextAsset>("json/item_list_en.json");
            ITEM_LIST = JsonConvert.DeserializeObject<JsonSet<Item>>(txt.text).m_list;

            txt = GlobalObj.DATA.LoadResource<TextAsset>("json/recipe_list.json");
            RECIPE_LIST = JsonConvert.DeserializeObject<JsonSet<Recipe>>(txt.text).m_list;
        }

    }
    public bool check_front()
    {
        if (!player_ready)
        {
            return false;
        }
        foreach(int ids in move_target_id)
        {
            if(ids == target_id)
            {
                return true;
            }
        }
        return false;
    }

    bool uart_mode = false;
    private void uartInput()
    {
        byte[] priv_frameset = new byte[4];

        byte[] buffer = new byte[10240];

        while (true)
        {
            Thread.Sleep(20);
            try
            {
                if (port.IsOpen && !uart_mode)
                {
                    //获取数据
                    int size = port.Read(buffer, 0, 4096);
                    port.DiscardInBuffer();

                    byte priv_frame;
                    for (int a = 0; a < size; a++)
                    {

                        byte frame = buffer[a];
                        int flag = (frame & 0b11);
                        priv_frame = priv_frameset[flag];

                        if (priv_frame == frame) continue;

                        Debug.Log("receive: " + frame);
                        if (flag == 1)
                        {
                            if (frame >> 2 == 1)
                            {
                                Debug.Log("game start signal");
                                Loom.QueueOnMainThread(() =>
                                {
                                    EventCenter.Instance.trigger(new GameStateEvent(1)); // start game
                                });
                                
                            }
                            else if (frame >> 2 == 2)
                            {
                                Loom.QueueOnMainThread(() =>
                                {
                                    EventCenter.Instance.trigger(new GameStateEvent(0));
                                });
                            }
                        }

                        if ((frame & 0b11) == 2)
                        {
                            int pos_cnt = 0;

                            //处理五个上升沿信号:
                            //0: get   1: put   2: interact   3: move   4: throw
                            for (int i = 0; i < 5; i++)
                            {
                                byte bitmask = (byte)(1 << (2 + i));
                                if ((priv_frame & bitmask) == 0 && (frame & bitmask) != 0)
                                {
                                    pos_cnt++;
                                    //when move, ready should be reset to false.
                                    if (i == 3)
                                    {
                                        GlobalObj.Instance.player_ready = false;
                                    }
                                    int tmp = i;
                                    Loom.QueueOnMainThread(() =>
                                    {
                                        EventCenter.Instance.trigger(new PlayerInteractEvent(tmp, target_id));
                                    });
                                }
                                if (i == 2)
                                {
                                    interact_key = (frame & bitmask) != 0;
                                }
                            }
                            if (pos_cnt > 1)
                            {
                                Warning.error("禁止同时提供多个上升沿！");
                            }
                            
                        }


                        if ((frame & 0b11) == 3)
                        {
                            int id_temp = frame >> 2;
                            
                            if (id_temp > 20)
                            {
                                Warning.error("Invalid target ID: " + id_temp);
                            }
                            //处理ID标号
                            if (id_temp != 0)
                            {
                                target_id = id_temp;
                            }
                        }

                        priv_frameset[flag] = frame;

                    }

                    if (GAME_START)
                    {
                        //检查是否可以交互
                        interacting = interact_key && check_front();

                        //更新玩家手状态
                        player_hand_full = player_robot.hand_item.id != 0;

                        byte code = 0;
                        code += (byte)(check_front() ? 0b1 : 0);
                        code += (byte)(player_hand_full ? 0b10 : 0);
                        code += (byte)(target_ready ? 0b100 : 0);
                        code += (byte)(target_has_item ? 0b1000 : 0);
                        code += (byte)(error_found ? 0b10000 : 0);
                        if (!uart_mode)
                        {
                            port.Write(new byte[1] { (byte)((code << 2) + 1) }, 0, 1);
                            //最低位信号是port_ready，用于确认软件接收了信号。
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("UART: " + e.Message+e.StackTrace);
            }
            
        }
    }

    public void uartOutput(byte[] codes)
    {
        if (port.IsOpen)
        {
            uart_mode = true;
            int next_size = codes.Length;
            while ((next_size & 0b11) != 0b10)
            {
                next_size++;
            }
            port.DiscardOutBuffer();
            Thread.Sleep(3);
            
            port.Write(new byte[1] { (byte)next_size }, 0, 1);
            Debug.Log("script byte size:" + (byte)next_size);
            byte[] single = new byte[1];
            foreach(byte c in codes)
            {
                Thread.Sleep(2);
                single[0] = c;
                port.Write(single, 0, 1);
            }

            single[0] = 0;
            for(int a = 0;a< next_size - codes.Length+2; a++)
            {
                Thread.Sleep(2);
                port.Write(single, 0, 1);
            }
            Thread.Sleep(3);
            uart_mode = false;
        }
        
    }
    private void keyInput()
    {
        if (!ALLOW_KEY_INPUT)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            EventCenter.Instance.trigger(new GameStateEvent(1));
            return;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            EventCenter.Instance.trigger(new PlayerInteractEvent(0, target_id)); //get
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            EventCenter.Instance.trigger(new PlayerInteractEvent(1, target_id)); //put
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            EventCenter.Instance.trigger(new PlayerInteractEvent(2, target_id)); //interact
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            EventCenter.Instance.trigger(new PlayerInteractEvent(3, target_id)); //move
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            EventCenter.Instance.trigger(new PlayerInteractEvent(4, target_id)); //throw
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            target_id++;
            if(target_id > 20)
            {
                target_id = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            target_id--;
            if (target_id < 1)
            {
                target_id = 20;
            }
        }

        interact_key = Input.GetKey(KeyCode.I);
        interacting = interact_key && check_front();

    }
}
