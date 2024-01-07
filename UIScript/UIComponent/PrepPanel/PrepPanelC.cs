using UnityEngine;
using UnityEngine.UI;

class PrepPanelC : PrepPanelBase
{
	#region �ű��������ɵĴ���
	private Button m_btnFinishTest;
	private Button m_btnFailTest;
	private GameObject m_goTestGrid;
	/*�÷������ڳ�ʼ��ǰ��UIWindow�Զ����á�*/
	protected override void ComponentInit()
	{
		m_btnFinishTest = FindChildComponent<Button>("m_btnFinishTest");
		m_btnFailTest = FindChildComponent<Button>("m_btnFailTest");
		m_goTestGrid = FindChild("m_goTestGrid").gameObject;
		m_btnFinishTest.onClick.AddListener(OnClickFinishTestBtn);
		m_btnFailTest.onClick.AddListener(OnClickFailTestBtn);
	}
	#endregion

	#region �¼�
	private void OnClickFinishTestBtn()
	{
		EventCenter.Instance.trigger(new ConfigOpenEvent(3));
	}
	private void OnClickFailTestBtn()
	{
		EventCenter.Instance.trigger(new ConfigOpenEvent(1));
	}

    protected override void OnInit(GameObject m_go)
    {
        base.OnInit(m_go);
		CreateUIComponent<InputPanel>(m_goTestGrid);
    }
    protected override int panelID()
    {
		return 2;
    }

    protected override void onEnable()
    {
		m_btnFailTest.interactable = true;
		m_btnFinishTest.interactable = true;
    }

    protected override void onDisable()
    {
		m_btnFailTest.interactable = false;
		m_btnFinishTest.interactable = false;
	}
    #endregion

}
