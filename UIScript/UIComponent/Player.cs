using UnityEngine;
using UnityEngine.UI;

public class Player : UIComponent, EventListener<PlayerInteractEvent>
{
    #region 脚本工具生成的代码
    private Image m_imgPlayer;
    private GameObject m_goItem;
    /*该方法会在初始化前被UIWindow自动调用。*/
    protected override void ComponentInit()
    {
        m_imgPlayer = FindChildComponent<Image>("m_imgPlayer");
        m_goItem = FindChild("m_goItem").gameObject;
    }
    #endregion

    ItemIcon icon;
    protected override void OnInit(GameObject m_go)
    {
		icon = CreateUIComponent<ItemIcon>(m_goItem);
        icon.Hide();
        m_imgPlayer.sprite = GlobalObj.Instance.CHARACTER;
        EventCenter.Instance.regAction(this);
    }
    protected override void OnDestroy()
    {
        EventCenter.Instance.unregAction(this);
    }

    public Item hand_item { get; private set; }

	public void setHandItem(Item item)
    {
        hand_item = item;
        if (item.id == 0)
        {
            icon.Hide();
        }
        else
        {
            icon.setItem(item);
            icon.Show();
        }
		GlobalObj.Instance.player_hand_full = hand_item.id != 0;  
    }

    public int priority(PlayerInteractEvent evt)
    {
        return 99;
    }

    public void callback(PlayerInteractEvent evt)
    {
        if(hand_item.id != 0 && evt.interact_type == 4)
        {
            ThrowItem thr = CreateUIComponent<ThrowItem>(m_gameObjectOuter);
            thr.SetParent(m_parent);
            thr.setItem(hand_item);
            setHandItem(Item.NOTHING);
            evt.throw_target = thr;
        }
        else
        {
            evt.throw_target = null;
        }
    }
}
