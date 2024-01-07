using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Warning : UIWindow
{
	#region 脚本工具生成的代码
	private Image m_imgBack;
	private Text m_textTitle;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_imgBack = FindChildComponent<Image>("m_imgBack");
		m_textTitle = FindChildComponent<Text>("m_textTitle");
	}
    #endregion

    #region 事件
    #endregion

    protected override IEnumerable<int> OnShow()
    {
		hiding = false;
		for(int a = 0; a < 10; a++)
        {
			m_imgBack.color = new Color(0, 0, 0, a/30f);
			txt_color.a = a / 10f;
			m_textTitle.color = txt_color;
			yield return 0;
        }
    }
	protected override IEnumerable<int> OnHide()
	{
		hiding = true;
		for (int a = 10; a >=0; a--)
		{
            if (!hiding)
            {
				yield break;
            }
			m_imgBack.color = new Color(0, 0, 0, a / 30f);
			txt_color.a = a / 10f;
			m_textTitle.color = txt_color;
			yield return 0;
		}
		hiding = false;
	}

	bool animing = false;
	bool hiding = false;
	int cnt = 70;
	Color txt_color = Color.white;
	private IEnumerable<int> anim()
    {
		cnt = 70;
        if (hiding)
        {
			hiding = false;
        }
		if (animing)
        {
			yield break;
        }

		animing = true;
		Show();
		while(cnt>0)
        {
			cnt--;
			yield return 0;
        }
		Hide();
		animing = false;
	}
	public static void message(string mes)
    {
		if (!GlobalObj.Instance.GAME_READY)
		{
			return;
		}
		Warning win = UIManager.Instance.GetWindow<Warning>();
		win.m_textTitle.text = mes;
		win.txt_color = Color.white;
		AnimPlayer.Instance.Play(win.anim());
    }
	public static void error(string mes)
	{
		if(!GlobalObj.Instance.GAME_READY)
        {
			return;
        }
		Warning win = UIManager.Instance.GetWindow<Warning>();
		win.m_textTitle.text = "[ERROR] "+mes;
		win.txt_color = Color.yellow;
		AnimPlayer.Instance.Play(win.anim());
	}

    protected override int SortOrder()
    {
		return 999;
    }
}
