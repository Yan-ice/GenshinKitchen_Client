using UnityEngine;
using UnityEngine.UI;

class ItemInfoCard : UIWindow
{
	#region 脚本工具生成的代码
	private Image m_imgBack;
	private Image m_imgItem;
	private Text m_textName;
	private Button m_btnInfo;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_imgBack = FindChildComponent<Image>("m_imgBack");
		m_imgItem = FindChildComponent<Image>("middle/m_imgItem");
		m_textName = FindChildComponent<Text>("m_textName");
		m_btnInfo = FindChildComponent<Button>("m_btnInfo");
		m_btnInfo.onClick.AddListener(OnClickInfoBtn);
	}
	#endregion

	#region 事件
	private void OnClickInfoBtn(){
		Sprite rec = GlobalObj.ICON.LoadResource<Sprite>("recipes/r_" + item.name + ".png");
		if(rec == null)
        {
			rec = GlobalObj.ICON.LoadResource<Sprite>("recipes/r_nothing.png");
		}
		ImageTip.imageTip(rec, "Checking Recipe", true);
    }
	#endregion

	private Item item;
	public void setItem(Item i)
    {
		m_imgItem.sprite = i.getIconImg();
		m_imgBack.color = i.getTypeColor();
		m_textName.text = i.display;
		if(LanguageText.current_lang == "en_US")
        {
			m_textName.fontSize = 22;
        }
		this.item = i;
	}
}
