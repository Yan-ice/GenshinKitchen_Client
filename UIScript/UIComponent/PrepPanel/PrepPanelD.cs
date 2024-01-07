using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

	class PrepPanelD : PrepPanelBase
{
	#region 脚本工具生成的代码
	private GameObject m_goCharas;
	private GameObject m_goCha;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_goCharas = FindChild("m_goCharas").gameObject;
		m_goCha = FindChild("m_goCha").gameObject;
	}
	#endregion

	private List<Button> btnlist = new List<Button>();
	protected override void OnInit(GameObject m_go)
    {
        base.OnInit(m_go);
		for(int a = 1; a <= 8; a++)
        {
			Button btn = GameObject.Instantiate(m_goCha, m_goCharas.transform).GetComponent<Button>();
			btn.GetComponent<Image>().sprite = GlobalObj.ICON.LoadResource<Sprite>("chara/" + a + ".png");
			btnlist.Add(btn);
			btn.interactable = false;
			int s = a;
			btn.onClick.AddListener(() => { startGame(s); });
		}

    }

	private void startGame(int character)
    {
		GlobalObj.Instance.CHARACTER = GlobalObj.ICON.LoadResource<Sprite>("chara/" + character + ".png");
		Debug.Log("Character select: " + "chara/" + character + ".png");
		UIManager.Instance.ShowWindow<GameScene>();
		UIManager.Instance.DestroyWindow<UartConnect>();
	}


	protected override void onDisable()
	{
		foreach(Button b in btnlist)
        {
			b.interactable = false;
        }
	}

	protected override void onEnable()
	{
		foreach (Button b in btnlist)
		{
			b.interactable = true;
		}
	}

	protected override int panelID()
	{
		return 3;
	}

}
