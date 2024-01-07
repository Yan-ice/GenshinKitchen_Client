using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

class UartConnect : UIWindow, EventListener<ConfigOpenEvent>
{
	SerialPort sp;
	PrepPanelBase[] panels = new PrepPanelBase[4];

	#region 脚本工具生成的代码
	private GameObject m_goPanelGrid;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_goPanelGrid = FindChild("m_goPanelGrid").gameObject;
	}
	#endregion

	protected override void OnInit(GameObject m_go)
    {
		panels[0] = CreateUIComponent<PrepPanelA>(m_goPanelGrid);
		panels[1] = CreateUIComponent<PrepPanelB>(m_goPanelGrid);
		panels[2] = CreateUIComponent<PrepPanelC>(m_goPanelGrid);
		panels[3] = CreateUIComponent<PrepPanelD>(m_goPanelGrid);
		EventCenter.Instance.trigger(new ConfigOpenEvent(0));
	}

    public int priority(ConfigOpenEvent evt)
    {
		return 0;
    }

    public void callback(ConfigOpenEvent evt)
    {
        if(evt.config_id >= 0 && evt.config_id < 4)
        {
			panels[evt.config_id].Show();
        }
    }
}
