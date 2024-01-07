using UnityEngine;
using UnityEngine.UI;

class InputIcon : UIComponent
{
	public string inputName;

	#region 脚本工具生成的代码
	private Image m_imgBack;
	private Image m_imgIcon;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_imgBack = FindChildComponent<Image>("m_imgBack");
		m_imgIcon = FindChildComponent<Image>("m_imgIcon");
	}
    #endregion

	public void init()
    {

    }
    protected override void OnInit(GameObject m_go)
    {
		BundlePackLoader ld = new BundlePackLoader("icon_resource");
		m_imgIcon.sprite = ld.LoadResource<Sprite>("input_action");
    }


    #region 事件
    #endregion

}
