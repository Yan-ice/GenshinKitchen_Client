using UnityEngine;
using UnityEngine.UI;

class ItemIcon : UIComponent
{
	#region 脚本工具生成的代码
	private Image m_imgBack;
	private Image m_imgItem;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_imgBack = FindChildComponent<Image>("m_imgBack");
		m_imgItem = FindChildComponent<Image>("m_imgItem");
	}
	#endregion

	#region 事件
	#endregion

	protected override void OnInit(GameObject m_go)
	{
		m_imgItem.sprite = item.getIconImg();
	}

	public Item item;
	public void setItem(Item i)
	{

		if (this.item.id != i.id)
		{
			this.item = i;
			m_imgItem.sprite = i.getIconImg();
			m_imgBack.color = i.getTypeColor();
		}
	}
}
