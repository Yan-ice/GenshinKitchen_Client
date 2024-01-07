using System;
using UnityEngine;
using UnityEngine.UI;

public class ThrowItem : UIComponent
{
	#region 脚本工具生成的代码
	private GameObject m_goItem;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_goItem = FindChild("m_goItem").gameObject;
	}
	#endregion

	ItemIcon icon;
	protected override void OnInit(GameObject m_go)
	{
		icon = CreateUIComponent<ItemIcon>(m_goItem);
	}
	public Item hand_item { get; private set; }

	public void setItem(Item item)
	{
		hand_item = item;
		icon.setItem(item);
		GlobalObj.Instance.player_hand_full = hand_item.id != 0;
	}
}
