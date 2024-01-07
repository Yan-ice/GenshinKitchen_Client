using UnityEngine;
using UnityEngine.UI;

public class InputPanel : UIWindow, EventListener<PlayerInteractEvent>
{
	#region 脚本工具生成的代码
	private Image m_imgBack1;
	private Image m_imgBack2;
	private Image m_imgBack3;
	private Image m_imgBack4;
	private Image m_imgBack5;
	private Image m_imgBack;
	private Text m_textTargetIndex;

    public void callback(PlayerInteractEvent evt)
    {
        switch (evt.interact_type)
        {
			case 0:
				m_imgBack1.enabled = true;
				break;
			case 1:
				m_imgBack2.enabled = true;
				break;
			case 2:
				m_imgBack3.enabled = true;
				break;
			case 3:
				m_imgBack4.enabled = true;
				break;
			case 4:
				m_imgBack5.enabled = true;
				break;
		}
		cnt = 0;
    }

    public int priority(PlayerInteractEvent evt)
    {
		return 50;
    }

	int cnt = 0;
	void update()
    {
		cnt++;
        if (cnt > 4)
        {
			cnt = 0;
			m_imgBack3.enabled = GlobalObj.Instance.interact_key;
			m_imgBack1.enabled = false;
			m_imgBack2.enabled = false;
			m_imgBack4.enabled = false;
			m_imgBack5.enabled = false;
			m_textTargetIndex.text = "#"+GlobalObj.Instance.target_id.ToString();
		}
		
	}

    /*该方法会在初始化前被UIWindow自动调用。*/
    protected override void ComponentInit()
	{
		m_imgBack1 = FindChildComponent<Image>("InputIconA/m_imgBack1");
		m_imgBack2 = FindChildComponent<Image>("InputIconB/m_imgBack2");
		m_imgBack3 = FindChildComponent<Image>("InputIconC/m_imgBack3");
		m_imgBack4 = FindChildComponent<Image>("InputIconD/m_imgBack4");
		m_imgBack5 = FindChildComponent<Image>("InputIconE/m_imgBack5");
		m_imgBack = FindChildComponent<Image>("InputIndex/m_imgBack");
		m_textTargetIndex = FindChildComponent<Text>("InputIndex/m_textTargetIndex");
	}
	#endregion

	#region 事件
	#endregion

	protected override void OnInit(GameObject m_go)
	{
		EventCenter.Instance.regAction(this);
		MonoUpdateManager.Instance.AddUpdateListener(update);
	}

    protected override void OnDestroy()
    {
		EventCenter.Instance.unregAction(this);
		MonoUpdateManager.Instance.RemoveUpdateListener(update);
	}

}
