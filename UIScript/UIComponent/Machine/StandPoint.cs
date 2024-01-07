using UnityEngine;
using UnityEngine.UI;

public class StandPoint : MachineIcon, EventListener<PlayerInteractEvent>
{

    #region 脚本工具生成的代码
    private Image m_imgIcon;
    /*该方法会在初始化前被UIWindow自动调用。*/
    protected override void ComponentInit()
    {
        m_imgIcon = FindChildComponent<Image>("m_imgIcon");
    }
    #endregion

    protected override void OnInit(GameObject m_go)
    {
        MonoUpdateManager.Instance.AddUpdateListener(update);
        EventCenter.Instance.regAction(this);
    }
    protected override void OnDestroy()
    {
        MonoUpdateManager.Instance.RemoveUpdateListener(update);
        EventCenter.Instance.unregAction(this);
    }

    private int[] stand_ids;
    public void setStandId(int stand_id)
    {
        this.id = -1;
        this.stand_ids = new int[1] { stand_id };
    }
    public void setStandId(int[] stand_ids)
    {
        this.id = -1;
        this.stand_ids = stand_ids;
    }


    private void update()
    {
        bool is_tar = false;
        if (GlobalObj.Instance.move_target_id[0] == stand_ids[0])
        {
            is_tar = true;
        }
        if (is_tar)
        {
            Vector3 delta = (m_gameObjectOuter.transform.position - GlobalObj.Instance.player_robot.getGameObject().transform.position) / 10;
            if(delta.magnitude < 0.03)
            {
                GlobalObj.Instance.player_ready = true;
                m_imgIcon.color = Color.yellow;
            }
            else
            {
                GlobalObj.Instance.player_ready = false;
                m_imgIcon.color = Color.white;
            }
            float speed = delta.magnitude < GlobalObj.Instance.PLAYER_SPEED/10 ? delta.magnitude : GlobalObj.Instance.PLAYER_SPEED/10;
            delta = delta.normalized * speed;
            GlobalObj.Instance.player_robot.getGameObject().transform.position += delta;
        }
        else
        {
            m_imgIcon.color = new Color(0,0,0,0);
        }

    }

    public int priority(PlayerInteractEvent evt)
    {
        return 1;
    }

    public void callback(PlayerInteractEvent evt)
    {
        if (evt.interact_type == 3)
        {
            foreach(int ids in stand_ids)
            {
                if (ids == evt.target_id)
                {
                    if (!GlobalObj.Instance.GAME_START)
                    {
                        Warning.error("Game not started!");
                        return;
                    }
                    GlobalObj.Instance.move_target_id = stand_ids;
                }
            }
            
            update();
        }
    }

    protected override int totalProgress()
    {
        return 0;
    }

    protected override bool prepProgress()
    {
        return false;
    }

    protected override void onProgressEnough()
    {
    }
}
