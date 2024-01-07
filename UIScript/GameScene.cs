using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class GameScene : UIWindow
{
	#region 脚本工具生成的代码
	private GameObject m_goKitchenPanel;
	private GameObject m_itemInputPanel;
	private GameObject m_itemSubmitPanel;
	private Button m_btnReset;
	private Button m_btnRandomize;
	private Button m_btnCompiler;
	private Button m_btnKeyHelp;
	private Button m_btnCardInfo;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_goKitchenPanel = FindChild("m_goKitchenPanel").gameObject;
		m_itemInputPanel = FindChild("m_itemInputPanel").gameObject;
		m_itemSubmitPanel = FindChild("m_itemSubmitPanel").gameObject;
		m_btnReset = FindChildComponent<Button>("tidGrid/m_btnReset");
		m_btnRandomize = FindChildComponent<Button>("tidGrid/m_btnRandomize");
		m_btnCompiler = FindChildComponent<Button>("tidGrid/m_btnCompiler");
		m_btnKeyHelp = FindChildComponent<Button>("tidGrid/m_btnKeyHelp");
		m_btnCardInfo = FindChildComponent<Button>("tidGrid/m_btnCardInfo");
		m_btnReset.onClick.AddListener(OnClickResetBtn);
		m_btnRandomize.onClick.AddListener(OnClickRandomizeBtn);
		m_btnCompiler.onClick.AddListener(OnClickCompilerBtn);
		m_btnKeyHelp.onClick.AddListener(OnClickKeyHelpBtn);
		m_btnCardInfo.onClick.AddListener(OnClickCardInfoBtn);
	}
	#endregion

	#region 事件
	#endregion

	SubmitPanel submit_panel;
	public void putDishes(Item i)
    {
		submit_panel.putDishes(i);
    }
	protected override void OnInit(GameObject m_go)
    {
		GlobalObj.Instance.GAME_START = false;
		m_btnCompiler.interactable = !GlobalObj.Instance.ALLOW_KEY_INPUT;

		LoadUIComponent<InputPanel>(m_itemInputPanel);
		submit_panel = LoadUIComponent<SubmitPanel>(m_itemSubmitPanel);
		GlobalObj.Instance.player_robot = CreateUIComponent<Player>(m_gameObjectOuter);
		//line 1
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<ItemSource>(m_goKitchenPanel).setParams(1, 1, 2);
		CreateUIComponent<ItemSource>(m_goKitchenPanel).setParams(2, 2, 2);
		CreateUIComponent<ItemSource>(m_goKitchenPanel).setParams(3, 3, 2);
		CreateUIComponent<ItemSource>(m_goKitchenPanel).setParams(4, 4, 2);
		CreateUIComponent<ItemSource>(m_goKitchenPanel).setParams(5, 5, 2);
		CreateUIComponent<ItemSource>(m_goKitchenPanel).setParams(6, 6, 2);
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		//line 2
		CreateUIComponent<Bin>(m_goKitchenPanel).setMachineId(20);
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[]{1,20});
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 2 });
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 3 });
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 4 });
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 5 });
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 6,7 });
		CreateUIComponent<ManualCold>(m_goKitchenPanel).setMachineId(7);
		//line 3
		CreateUIComponent<Table>(m_goKitchenPanel).setMachineId(19);
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 19});
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 8 });
		CreateUIComponent<AutoCold>(m_goKitchenPanel).setMachineId(8);
		//line 4
		CreateUIComponent<Submit>(m_goKitchenPanel).setMachineId(18);
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 18 });
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 9});
		CreateUIComponent<Table>(m_goKitchenPanel).setMachineId(9);
		//line 5
		CreateUIComponent<Table>(m_goKitchenPanel).setMachineId(17);
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 16, 17 });
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 15 });
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 14 });
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 13 });
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 12 });
		CreateUIComponent<StandPoint>(m_goKitchenPanel).setStandId(new int[] { 10, 11 });
		CreateUIComponent<ManualHot>(m_goKitchenPanel).setMachineId(10);
		//line 6
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);
		CreateUIComponent<AutoCombine>(m_goKitchenPanel).setMachineId(16);
		CreateUIComponent<ManualCombine>(m_goKitchenPanel).setMachineId(15);
		CreateUIComponent<Table>(m_goKitchenPanel).setMachineId(14);
		CreateUIComponent<AutoHot>(m_goKitchenPanel).setMachineId(13);
		CreateUIComponent<AutoHot>(m_goKitchenPanel).setMachineId(12);
		CreateUIComponent<Table>(m_goKitchenPanel).setMachineId(11);
		CreateUIComponent<EmptyMachine>(m_goKitchenPanel);

		GlobalObj.Instance.GAME_READY = true;
		Warning.message("Game is ready to start.");
	}

    protected override IEnumerable<int> OnHide()
    {
		GlobalObj.Instance.GAME_READY = false;
		yield return 0;
	}
    private void OnClickCardInfoBtn()
    {
		UIManager.Instance.ShowWindow<Handbook>();
    }

	private void OnClickCompilerBtn()
    {
		UIManager.Instance.ShowWindow<ScriptCompiler>();
	}

	private void OnClickKeyHelpBtn()
    {
		ImageTip.imageTip(GlobalObj.ICON.LoadResource<Sprite>("keyboard_help.png"));
    }
	private void OnClickResetBtn()
	{
		EventCenter.Instance.trigger<GameStateEvent>(new GameStateEvent(0));
		Loom.QueueOnMainThread(() =>
		{
			UIManager.Instance.DestroyWindow<GameScene>();
			UIManager.Instance.ShowWindow<GameScene>();

			GlobalObj.Instance.PROCESS_SPEED = 1;
			Warning.message("Processing speed: 1x");
		});
	}

	private void OnClickRandomizeBtn()
    {
		Loom.QueueOnMainThread(() =>
		{
			UIManager.Instance.DestroyWindow<GameScene>();
			UIManager.Instance.ShowWindow<GameScene>();

			GameScene scene = UIManager.Instance.GetWindow<GameScene>();
			float rate = ((int)(Time.time*100)) % 20 / 10f + 0.1f;
			Random.InitState((int)Time.time);

			GlobalObj.Instance.PROCESS_SPEED = rate;
			Warning.message("Processing speed: " + rate + "x");
			foreach (UIComponent comp in scene.m_childComponents)
			{
				if (comp is ItemSource && Random.Range(0f, 1f) > 0.5f)
				{
					((ItemSource)comp).clearItem();
				}
				if (comp is Table)
				{
					for (int a = 0; a < 3; a++)
					{
						int rrd = (int)Random.Range(0f, 30f) + 1;
						if (rrd < 13)
						{
							((Table)comp).pushItem(GlobalObj.ITEM_LIST[rrd]);
						}
					}
				}
				if (comp is ManualCold || comp is ManualHot || comp is AutoCold || comp is AutoHot
					|| comp is ManualCombine || comp is AutoCombine)
				{
					int rrd = (int)Random.Range(0f, 20f) + 1;
					if (rrd < 11)
					{
						((MachineIcon)comp).pushItem(GlobalObj.ITEM_LIST[rrd]);
					}
				}
			}
		});
		

    }
}
