using System.Threading;
using UnityEngine;
using UnityEngine.UI;

class SubmitPanel : UIWindow
{
	#region 脚本工具生成的代码
	private GameObject m_goGrid;
	private Text m_textTimer;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_goGrid = FindChild("back/m_goGrid").gameObject;
		m_textTimer = FindChildComponent<Text>("m_textTimer");
	}
	#endregion

	#region 事件
	#endregion

	int cnt = 0;
	public void putDishes(Item dish)
    {
		if(cnt < 3)
        {
			CreateUIComponent<ItemIcon>(m_goGrid).setItem(dish);
			cnt++;
            if (cnt == 3)
            {
				thread.Abort();
			}
		}
		
    }
	void onUpdate()
    {
		if(t_th == 0)
		{
			m_textTimer.text = "<Waiting>";
			return;
		}
		string milli = (t_th % 100).ToString().PadLeft(2,'0');
		string sec = (t_th / 100 % 60).ToString().PadLeft(2, '0');
		string min = (t_th / 6000).ToString().PadLeft(2, '0');
		m_textTimer.text = min + ":" + sec + ":" + milli;
    }

	void onGameState(GameStateEvent e)
    {
		if(e.state_id == 1)
        {
			t_th = 0;
			DestoryAllChildComponent();

        }
    }

	Thread thread;
    protected override void OnInit(GameObject m_go)
    {
		thread = new Thread(timer);
		thread.Start();
		MonoUpdateManager.Instance.AddUpdateListener(onUpdate);
		EventCenter.Instance.regSimpleAction<GameStateEvent>(onGameState, 1);
    }

    protected override void OnDestroy()
    {
		thread.Abort();
		MonoUpdateManager.Instance.RemoveUpdateListener(onUpdate);
		EventCenter.Instance.unregSimpleAction<GameStateEvent>(onGameState);
	}

    int t_th = 0;
	void timer()
    {
        while (true)
        {
            if (GlobalObj.Instance.GAME_START)
            {
				t_th++;
			}
			
			Thread.Sleep(10);
        }
    }
}
