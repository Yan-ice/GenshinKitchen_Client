
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public abstract class PrepPanelBase : UIComponent, EventListener<ConfigOpenEvent>
{
	protected override void OnInit(GameObject m_go)
	{
		m_go.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
		onDisable();
		EventCenter.Instance.regAction(this);
	}
	protected override void OnDestroy()
	{
		EventCenter.Instance.unregAction(this);
	}


	private bool acti = false;

	public int priority(ConfigOpenEvent evt)
	{
		return 1;
	}

	public void callback(ConfigOpenEvent evt)
	{
		if (evt.config_id == panelID() && !acti)
		{
			acti = true;
			AnimPlayer.Instance.Play(anim_enable());
		}
		if (evt.config_id != panelID() && acti)
		{
			acti = false;
			AnimPlayer.Instance.Play(anim_disable());
		}
	}

	protected abstract int panelID();
	protected abstract void onEnable();
	protected abstract void onDisable();

	public IEnumerable<int> anim_enable()
	{
		Image i = m_gameObjectOuter.GetComponent<Image>();
		for (int a = 0; a < 15; a++)
		{
			i.color = new Color(0.7f - a / 60f, 0.7f - a / 50f, 0.7f - a / 60f);
			yield return 0;
		}
		onEnable();
	}

	public IEnumerable<int> anim_disable()
	{
		onDisable();
		Image i = m_gameObjectOuter.GetComponent<Image>();
		for (int a = 15; a > 0; a--)
		{
			i.color = new Color(0.7f - a / 60f, 0.7f - a / 50f, 0.7f - a / 60f);
			yield return 0;
		}

	}
}
