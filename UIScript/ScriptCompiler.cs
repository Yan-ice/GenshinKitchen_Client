using UnityEngine;
using UnityEngine.UI;

class ScriptCompiler : UIWindow
{
	#region 脚本工具生成的代码
	private Button m_btnCompile;
	private Button m_btnWriteUART;
	private InputField m_inputInput;
	private InputField m_inputOutput;
	private Button m_btnBack;
	/*该方法会在初始化前被UIWindow自动调用。*/
	protected override void ComponentInit()
	{
		m_btnCompile = FindChildComponent<Button>("m_btnCompile");
		m_btnWriteUART = FindChildComponent<Button>("m_btnWriteUART");
		m_inputInput = FindChildComponent<InputField>("m_inputInput");
		m_inputOutput = FindChildComponent<InputField>("m_inputOutput");
		m_btnBack = FindChildComponent<Button>("m_btnBack");
		m_btnCompile.onClick.AddListener(OnClickCompileBtn);
		m_btnWriteUART.onClick.AddListener(OnClickWriteUARTBtn);
		m_btnBack.onClick.AddListener(OnClickBackBtn);
	}
	#endregion

	byte[] codes;
	#region 事件
	private void OnClickCompileBtn()
	{
        try
        {
			string text = m_inputInput.text;
			text = text.Replace("\r\n", "\n");
			text = text.Trim('\n');
			if (!text.StartsWith("game start\n"))
            {
				text = "game start\n" + text;
            }
			if (!text.EndsWith("game end"))
			{
				text = text+"\ngame end";
			}
			m_inputInput.text = text;

			codes = MainCompiler.Compile(text);

			string output = "";

			int cnt = 0;
			foreach (byte b in codes)
			{
				output += byte2binarystr(b);
				cnt++;
				output += (cnt % 2 == 0) ? "\n" : " ";
			}
			m_inputOutput.text = output;
			m_btnWriteUART.interactable = true;
		}
        catch
        {
			m_btnWriteUART.interactable = false;
			Warning.message("Compile Error.");
			return;
        }
		
	}

	private void OnClickBackBtn()
    {
		Hide();
    }

	private void OnClickWriteUARTBtn()
    {
		if(codes.Length > 200)
        {
			Warning.message("Script is too long (must <= 100 line)!");
		}
		GlobalObj.Instance.uartOutput(codes);
    }
	#endregion


	private string byte2binarystr(byte b)
    {
		string o = "";
		for(int a = 0; a < 8; a++)
        {
			if((b & (1<<(7-a))) != 0)
            {
				o += "1";
            }
            else
            {
				o += "0";
            }
        }
		return o;
    }
    protected override int SortOrder()
    {
		return 9;
    }
}
