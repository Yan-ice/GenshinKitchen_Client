using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

class PrepPanelB : PrepPanelBase
{
	#region �ű��������ɵĴ���
	private Button m_btnDevConnect;
	private InputField m_inputBaut;
	private Text m_textBaut;
	private InputField m_inputPort;
	private Text m_textPort;
	/*�÷������ڳ�ʼ��ǰ��UIWindow�Զ����á�*/
	protected override void ComponentInit()
	{
		m_btnDevConnect = FindChildComponent<Button>("m_btnDevConnect");
		m_inputBaut = FindChildComponent<InputField>("baut/m_inputBaut");
		m_textBaut = FindChildComponent<Text>("baut/m_textBaut");
		m_inputPort = FindChildComponent<InputField>("port/m_inputPort");
		m_textPort = FindChildComponent<Text>("port/m_textPort");
		m_btnDevConnect.onClick.AddListener(OnClickDevConnectBtn);
	}

    protected override void onDisable()
    {
		m_inputBaut.interactable = false;
		m_inputPort.interactable = false;
		m_btnDevConnect.interactable = false;
    }

    protected override void onEnable()
    {
		m_inputBaut.interactable = true;
		m_inputPort.interactable = true;
		m_btnDevConnect.interactable = true;
		if (GlobalObj.Instance.port.IsOpen)
		{
			GlobalObj.Instance.port.Close();
		}
	}

    protected override int panelID()
    {
		return 1;
    }
    #endregion

    #region �¼�
    private void OnClickDevConnectBtn()
	{
		m_btnDevConnect.interactable = false;
		if (OpenSerialPort(m_inputPort.text, int.Parse(m_inputBaut.text)))
        {
			EventCenter.Instance.trigger(new ConfigOpenEvent(2));
        }
        else
        {
			m_btnDevConnect.interactable = true;
		}
		
	}
	#endregion


	/// <summary>
	/// �򿪴���
	/// </summary>
	/// <param name="_portName">�˿ں�</param>
	private bool OpenSerialPort(string _portName, int _baudRate)
	{
		if (GlobalObj.Instance.port.IsOpen)
		{
			GlobalObj.Instance.port.Close();
		}
		Debug.Log("Try opening serial port.");
		Parity _parity = Parity.None;
		int dataBits = 8;
		StopBits _stopbits = StopBits.One;

		try
		{
			GlobalObj.Instance.port = new SerialPort(_portName, _baudRate, _parity, dataBits, _stopbits);//�󶨶˿�
			GlobalObj.Instance.port.ReadTimeout = 3000;
			GlobalObj.Instance.port.Open();
			return true;
		}
		catch (Exception e)
		{
			Debug.Log(e);
			return false;
		}
	}

}
