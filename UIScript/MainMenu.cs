using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIWindow
{
	#region 脚本工具生成的代码
	private Button m_btnDevBoard;
	private Button m_btnKeyBard;
	private Button m_btnTrial;
	private Image m_imgGenshinBack;
	private Image m_imgGenshinBase;
	private Image m_imgGenshinGo;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_btnDevBoard = FindChildComponent<Button>("m_btnDevBoard");
		m_btnKeyBard = FindChildComponent<Button>("m_btnKeyBard");
		m_btnTrial = FindChildComponent<Button>("m_btnTrial");
		m_imgGenshinBack = FindChildComponent<Image>("m_imgGenshinBack");
		m_imgGenshinBase = FindChildComponent<Image>("m_imgGenshinBase");
		m_imgGenshinGo = FindChildComponent<Image>("m_imgGenshinGo");
		m_btnDevBoard.onClick.AddListener(OnClickDevBoardBtn);
		m_btnKeyBard.onClick.AddListener(OnClickKeyBardBtn);
		m_btnTrial.onClick.AddListener(OnClickTrialBtn);
		m_imgGenshinBase.enabled = false;
		m_imgGenshinGo.enabled = false;
		m_imgGenshinBack.enabled = false;
	}
	#endregion

	#region evt
	private void OnClickDevBoardBtn()
	{
		GlobalObj.Instance.ALLOW_KEY_INPUT = false;
		UIManager.Instance.ShowWindow<UartConnect>();
	}

	private void OnClickKeyBardBtn()
    {
		GlobalObj.Instance.ALLOW_KEY_INPUT = true;
		UIManager.Instance.ShowWindow<GameScene>();
	}
	private void OnClickTrialBtn()
    {
		AnimPlayer.Instance.Play(Genshin());
	}
	#endregion

	private IEnumerable<int> Genshin()
	{
		LanguageText.LoadLanguage("zh_CN");
		//UIManager.Instance.DestroyWindow<MainMenu>();
		//UIManager.Instance.ShowWindow<MainMenu>(2);
		GlobalObj.UpdateLang();
		TextReplacer.UpdateLanguage();
		MainMenu m = UIManager.Instance.GetWindow<MainMenu>();
		Color c = Color.white;
		c.a = 0;
		m.m_imgGenshinBase.enabled = true;
		m.m_imgGenshinGo.enabled = true;
		m.m_imgGenshinBack.enabled = true;
		m.m_btnTrial.interactable = false;
		for (int a = 0; a < 20; a++)
		{
			yield return 0;
		}
		for (int a = 0; a < 40; a++)
		{
			c.a = a / 40f;
			m.m_imgGenshinGo.color = c;
			yield return 0;
		}
		AudioSource ass = m.m_gameObjectOuter.GetComponent<AudioSource>();
		ass.clip = GlobalObj.DATA.LoadResource<AudioClip>("music/GenshinImpact.mp3");
		ass.Play();
		for (int a = 0; a < 80; a++)
		{
			yield return 0;
		}
		for (int a = 0; a < 31; a++)
		{
			c.a = 1 - a / 30f;
			m.m_imgGenshinGo.color = c;
			yield return 0;
		}
		for (int a = 0; a < 49; a++)
		{
			c.a = 1 - a / 50f;
			m.m_imgGenshinBase.color = c;
			yield return 0;
		}

		m.m_imgGenshinBase.enabled = false;
		m.m_imgGenshinGo.enabled = false;
	}
}


