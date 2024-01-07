using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

class PrepPanelA : PrepPanelBase
{
	#region 脚本工具生成的代码
	private Button m_btnFindDev;
	private Text m_textPortTip;
	private Text m_textPortList;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_btnFindDev = FindChildComponent<Button>("m_btnFindDev");
		m_textPortTip = FindChildComponent<Text>("m_textPortTip");
		m_textPortList = FindChildComponent<Text>("m_textPortList");
		m_btnFindDev.onClick.AddListener(OnClickFindDevBtn);
	}
	#endregion

	#region 事件
	private void OnClickFindDevBtn()
	{
		EventCenter.Instance.trigger<ConfigOpenEvent>(new ConfigOpenEvent(1));
	}
	#endregion

	int cnt = 0;
	void updatei() {
		if (cnt < 10) {
			cnt += 1;
        }
        else
        {
			cnt = 0;
			string[] portList = SerialPort.GetPortNames();
			string t = "";
			foreach (string s in portList)
			{
				t += s + "\n";
			}
			m_textPortList.text = t;
		}
		
	}

    protected override void OnInit(GameObject m_go)
    {
		base.OnInit(m_go);
		MonoUpdateManager.Instance.AddUpdateListener(updatei);
		
    }
    protected override void OnDestroy()
    {
		base.OnDestroy();
		MonoUpdateManager.Instance.RemoveUpdateListener(updatei);
	}

    protected override int panelID()
    {
		return 0;
    }

    protected override void onEnable()
    {
		 m_btnFindDev.interactable = true;
	}

    protected override void onDisable()
    {
		m_btnFindDev.interactable = false;
    }
}
