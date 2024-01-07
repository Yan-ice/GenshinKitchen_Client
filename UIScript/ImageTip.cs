using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ImageTip : UIWindow
{
	#region 脚本工具生成的代码
	private Image m_imgBack;
	private Image m_imgShow;
	private Text m_textTitle;
	private Button m_btnClick;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_imgBack = FindChildComponent<Image>("m_imgBack");
		m_imgShow = FindChildComponent<Image>("m_imgShow");
		m_textTitle = FindChildComponent<Text>("m_textTitle");
		m_btnClick = FindChildComponent<Button>("m_btnClick");
		m_btnClick.onClick.AddListener(OnClickClickBtn);
	}
	#endregion

	#region 事件
	private void OnClickClickBtn()
	{
		m_btnClick.interactable = false;
		if (canClick)
		{
			Hide();
		}
	}
	#endregion

	public static void imageTip(Sprite img, string mess = "", bool canClick = true)
	{
		ImageTip win = UIManager.Instance.GetWindow<ImageTip>();
		win.m_imgShow.sprite = img;
		win.m_textTitle.text = mess;
		win.canClick = canClick;
		win.Show();
	}


	private bool canClick = false;
	protected override IEnumerable<int> OnShow()
	{
		for (int a = 0; a < 10; a++)
		{
			m_imgBack.color = new Color(1, 1, 1, a / 15f);
			m_imgShow.color = new Color(1, 1, 1, a / 10f);
			m_textTitle.color = new Color(0, 0, 0, a / 10f);
			yield return 0;
		}
		m_btnClick.interactable = true;
	}
	protected override IEnumerable<int> OnHide()
	{
		m_btnClick.interactable = false;
		for (int a = 10; a >= 0; a--)
		{
			m_imgBack.color = new Color(1, 1, 1, a / 15f);
			m_imgShow.color = new Color(1, 1, 1, a / 10f);
			m_textTitle.color = new Color(0, 0, 0, a / 10f);
			yield return 0;
		}
		
	}

    protected override int SortOrder()
    {
		return 99;
    }
}
