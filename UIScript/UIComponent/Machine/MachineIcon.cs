using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MachineIcon : UIComponent
{
	#region 脚本工具生成的代码
	private Image m_imgIcon;
	private Text m_textID;
	private GameObject m_goFocus;
	private GameObject m_goProgressBack;
	private Image m_imgProgress;
	private GameObject m_goItem1;
	private GameObject m_goItem2;
	private GameObject m_goItem3;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_imgIcon = FindChildComponent<Image>("m_imgIcon");
		m_textID = FindChildComponent<Text>("m_textID");
		m_goFocus = FindChild("m_goFocus").gameObject;
		m_goProgressBack = FindChild("m_goProgressBack").gameObject;
		m_imgProgress = FindChildComponent<Image>("m_goProgressBack/mask/m_imgProgress");
		m_goItem1 = FindChild("m_goItem1").gameObject;
		m_goItem2 = FindChild("m_goItem2").gameObject;
		m_goItem3 = FindChild("m_goItem3").gameObject;
	}
	#endregion



	public int id { get; protected set; }
	private int progress = 0, cooldown = 0;
	private Item[] item_l = new Item[3];
	private ItemIcon[] item_icon = new ItemIcon[3];
	private List<ThrowItem> throw_list = new List<ThrowItem>();

	public void setMachineId(int id)
    {
		this.id = id;
		m_textID.text = id+"";
    }

	public bool pushItem(Item i)
    {
		for(int a = 0; a < 3; a++)
        {
			if(item_l[a].id == 0)
            {
				item_l[a] = i;
				onItemModified();
				return true;
            }
        }
		return false;
	}

	public Item popItem()
	{
		for (int a = 2; a >=0 ; a--)
		{
			if (item_l[a].id != 0)
			{
				Item target = item_l[a];
				item_l[a] = Item.NOTHING;
				onItemModified();
				return target;
			}
		}
		return Item.NOTHING;
	}

	public void clearItem()
	{
		bool mod = false;
		for (int a = 0; a < 3; a++)
		{
			if(item_l[a].id != 0)
            {
				item_l[a] = Item.NOTHING;
				mod = true;
			}
		}
        if (mod)
        {
			onItemModified();
		}
	}

	protected bool containItem(int item_id)
	{
		foreach(Item i in item_l)
        {
			if(i.id == item_id)
            {
				return true;
            }
        }
		return false;
	}

	protected int getItemCount()
	{
		int cnt = 0;
		foreach (Item i in item_l)
		{
			if (i.id != 0)
			{
				cnt++;
			}
		}
		return cnt;
	}

	private void update()
    {
		//更新自己的状态。
		for(int a = 0; a < 3; a++)
        {
			item_icon[a].setItem(item_l[a]);
			item_icon[a].getGameObject().SetActive(item_l[a].id != 0);
        }
		m_goFocus.SetActive(GlobalObj.Instance.target_id == this.id);
		m_goProgressBack.SetActive(progress != 0);
		m_imgProgress.fillAmount = progress * GlobalObj.Instance.PROCESS_SPEED / totalProgress();


		//目标需要负责更新全局信号。
		if (GlobalObj.Instance.target_id == this.id && this.id != 0)
		{
			GlobalObj.Instance.target_ready = progress == 0;
			GlobalObj.Instance.target_has_item = false;
			GlobalObj.Instance.target_item_full = true;
			for (int a = 0; a < 3; a++)
			{
				if (item_l[a].id != 0)
				{
					GlobalObj.Instance.target_has_item = true;
				}
				else
				{
					GlobalObj.Instance.target_item_full = false;
				}
			}
		}

		if (!GlobalObj.Instance.GAME_START)
		{
			return;
		}

		if (cooldown > 0)
        {
			cooldown -= 1;
        }
        else
        {
			if (prepProgress())
			{
				progress++;
				if (progress >= totalProgress() / GlobalObj.Instance.PROCESS_SPEED)
				{
					progress = 0;
					cooldown = 10;
					onProgressEnough();

				}
			}
			else
			{
				progress = 0;
			}
		}
		

		//更新被扔物体的状态。
		
		for(int a = throw_list.Count -1; a>=0;a--)
        {
			ThrowItem t_item = throw_list[a];
			Vector3 delta = (m_gameObjectOuter.transform.position - t_item.getGameObject().transform.position) / 10;
			if (delta.magnitude < 0.02)
			{
				throw_list.Remove(t_item);
				t_item.DestroyImmediate();
				if(getItemCount() < 3)
                {
					pushItem(t_item.hand_item);
                }
                else
                {
					popItem();
					popItem();
					popItem();

					Warning.message("满满的置物台被一次投掷搞得一团糟！");
				}
			}
			else
			{
				float speed = delta.magnitude < GlobalObj.Instance.THROW_SPEED / 10 ? delta.magnitude : GlobalObj.Instance.THROW_SPEED / 10;
				delta = delta.normalized * speed;
				t_item.getGameObject().transform.position += delta;
			}
		}
    }

    protected override void OnInit(GameObject m_go)
    {
		m_imgIcon.sprite = GlobalObj.ICON.LoadResource<Sprite>("machine/"+this.GetType().Name+ ".png");
		MonoUpdateManager.Instance.AddUpdateListener(update);
		EventCenter.Instance.regSimpleAction<PlayerInteractEvent>(interactCallback, 3);
		item_icon[0] = CreateUIComponent<ItemIcon>(m_goItem1);
		item_icon[1] = CreateUIComponent<ItemIcon>(m_goItem2);
		item_icon[2] = CreateUIComponent<ItemIcon>(m_goItem3);

	}

	protected override void OnDestroy()
    {
		MonoUpdateManager.Instance.RemoveUpdateListener(update);
		EventCenter.Instance.unregSimpleAction<PlayerInteractEvent>(interactCallback);
	}

	private void interactCallback(PlayerInteractEvent e)
	{
		if(e.target_id == this.id)
        {
            if (!GlobalObj.Instance.GAME_START)
            {
				Warning.error("Game not started!");
				return;
			}
            switch (e.interact_type)
            {
				case 0: //get
					if (!GlobalObj.Instance.check_front())
					{
						Warning.error("Please move to the target first!");
						return;
					}
					if (!GlobalObj.Instance.player_hand_full && GlobalObj.Instance.target_has_item)
                    {
						GlobalObj.Instance.player_robot.setHandItem(popItem());
                    }
                    else
                    {
						Warning.message("Target is empty / Hand is full!");
					}
					break;
				case 1: //put
					if (!GlobalObj.Instance.check_front())
					{
						Warning.error("Please move to the target first!");
						return;
					}
					if (GlobalObj.Instance.player_hand_full && !GlobalObj.Instance.target_item_full)
					{
						if (!putItemCheck(GlobalObj.Instance.player_robot.hand_item))
						{
							Warning.message("You cannot put item into this target now.");
							return;
						}
						pushItem(GlobalObj.Instance.player_robot.hand_item);
						GlobalObj.Instance.player_robot.setHandItem(Item.NOTHING);
                    }
                    else
                    {
						Warning.error("Target is full / Hand is empty!");
					}
					break;
				case 2: //interact
					if (!GlobalObj.Instance.check_front())
					{
						Warning.error("Please move to the target first!");
						return;
					}
					onInteract();
					break;
				case 4: //throw
                    if (!this.GetType().Equals(typeof(Table)) && !this.GetType().Equals(typeof(Bin)))
                    {
						Warning.message("You can only throw items to a table/bin!");
						e.throw_target.DestroyImmediate();
						return;
					}
					if(e.throw_target != null)
                    {
						throw_list.Add(e.throw_target);
                    }
                    else
                    {
						Warning.message("Hand is empty!");
					}
					break;
			}
        }
	}


	protected abstract int totalProgress();

	protected abstract bool prepProgress();

	protected abstract void onProgressEnough();

	protected virtual void onInteract()
    {

    }
	protected virtual void onItemModified()
	{

	}

	protected virtual bool putItemCheck(Item i)
	{
		return true;
	}
}
