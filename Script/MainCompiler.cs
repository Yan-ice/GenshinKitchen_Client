using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MainCompiler
{
    public static byte[] Compile(string str)
    {

        string[] scripts_ori = str.Split('\n');

        List<string> scripts = new List<string>();

        foreach(string line in scripts_ori)
        {
            if (!line.StartsWith("#") && line.Trim().Length > 2) scripts.Add(line);
        }
        byte[] codes = new byte[scripts.Count * 2];

        for(int a = 0;a<scripts.Count;a++)
        {
            int code = analyze_line(scripts[a]);
            codes[a * 2] = (byte)(code);
            codes[a * 2 + 1] = (byte)(code >> 8);
        }

        return codes;
    }

    private static int analyze_line(string line)
    {
        string[] param = line.Split(' ');
        int instruction_type_2_0 = 0;
        int instruction_data_15_3 = 0;

        switch (param[0])
        {
            case "put":
            case "get":
            case "interact":
            case "throw":
                instruction_type_2_0 = 1;
                instruction_data_15_3 = analyze_action(param);
                break;

            case "jumpif":
            case "jumpifnot":
                instruction_type_2_0 = 2;
                instruction_data_15_3 = analyze_if(param);
                break;

            case "wait":
            case "waituntil":
                instruction_type_2_0 = 3;
                instruction_data_15_3 = analyze_wait(param);
                    //int.Parse(param[1]);
                break;
            case "game":
                instruction_type_2_0 = 4;
                instruction_data_15_3 = analyze_gatestate(param);
                break;
            default:
                throw new ArgumentException();
        }
        return instruction_type_2_0 | (instruction_data_15_3 << 3);
    }

    private static int analyze_gatestate(string[] param)
    {
        int function_code = 0b00;
        switch (param[1])
        {
            case "start":
                function_code = 0b01;
                break;
            case "end":
                function_code = 0b10;
                break;
            default:
                throw new ArgumentException();
        }
        return function_code;
    }
    /// <summary>
    /// 仅返回12bit指令。
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private static int analyze_action(string[] param)
    {
        int function_code = 0b00;
        int target = int.Parse(param[1]);
        switch (param[0])
        {
            case "get":
                function_code = 0b00;
                break;
            case "put":
                function_code = 0b01;
                break;
            case "interact":
                function_code = 0b10;
                break;
            case "throw":
                function_code = 0b11;
                break;
            default:
                throw new ArgumentException();
        }
        return function_code | (target << 5);
    }

    /// <summary>
    /// 仅返回12bit指令。
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private static int analyze_wait(string[] param)
    {
        int function_code = 0b00;
        int check_signal = 0;
        int data = 0;
        switch (param[0])
        {
            case "wait":
                function_code = 0b00;
                data = int.Parse(param[1]);
                break;
            case "waituntil":
                function_code = 0b01;
                switch (param[1])
                {
                    case "player_ready":
                        check_signal = 0b000;
                        break;
                    case "player_hasitem":
                        check_signal = 0b001;
                        break;
                    case "target_ready":
                        check_signal = 0b010;
                        break;
                    case "target_hasitem":
                        check_signal = 0b011;
                        break;
                    default:
                        throw new ArgumentException();
                }
                break;
            default:
                throw new ArgumentException();
        }
        return function_code | (check_signal << 2) | (data << 5);
    }

    /// <summary>
    /// 仅返回12bit指令。
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private static int analyze_if(string[] param)
    {
        int function_code = 0;
        int check_signal = 0;
        int data = 0;
        switch (param[0])
        {
            case "jumpif":
                function_code = 0b00;
                break;
            case "jumpifnot":
                function_code = 0b01;
                break;
            default:
                throw new ArgumentException();
        }

        switch (param[1])
        {
            case "player_ready":
                check_signal = 0b000;
                break;
            case "player_hasitem":
                check_signal = 0b001;
                break;
            case "target_ready":
                check_signal = 0b010;
                break;
            case "target_hasitem":
                check_signal = 0b011;
                break;
            default:
                throw new ArgumentException();
        }
        if (param.Length > 2)
        {
            data = int.Parse(param[2]);
        }
        return function_code | (check_signal << 2) | (data << 5);
    }
}