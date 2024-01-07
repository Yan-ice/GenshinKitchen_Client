using UnityEngine;
using UnityEngine.UI;

public class Handbook : UIWindow
{
	#region 脚本工具生成的代码
	private GameObject m_goCardPanel;
	private Button m_btnBack;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_goCardPanel = FindChild("m_goCardPanel").gameObject;
		m_btnBack = FindChildComponent<Button>("m_btnBack");
		m_btnBack.onClick.AddListener(OnClickBackBtn);
	}
    #endregion

    protected override void OnInit(GameObject m_go)
    {
        base.OnInit(m_go);
		foreach(Item i in GlobalObj.ITEM_LIST)
        {
			if(i.id != 0)
            {
				CreateUIComponent<ItemInfoCard>(m_goCardPanel).setItem(i);
			}
			
        }
    }
    #region 事件
    private void OnClickBackBtn()
	{
		Destroy();
	}
    #endregion

    protected override int SortOrder()
    {
		return 10;
    }
}
