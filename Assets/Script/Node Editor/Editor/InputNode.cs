using UnityEngine;
using UnityEditor;


/// <summary>
/// 输入节点
/// </summary>
public class InputNode : BaseInputNode 
{
	private InputType inputType;

    /// <summary>
    /// 输入节点类型
    /// </summary>
	public enum InputType
	{
		Number,         // 数字
		Randomization   // 范围
	}

    // 范围内容
	private string randomFrom = "";
	private string randomTo = "";

    // 输入的内容
	private string inputValue = "";

	public InputNode()
	{
        // 默认标题
		windowTitle = "Input Node";
	}

    /// <summary>
    /// 绘制窗口
    /// </summary>
	public override void DrawWindow()
	{
        // 绘制标题
		base.DrawWindow();

        // 绘制选择类型
		inputType = (InputType) EditorGUILayout.EnumPopup("Input type : " , inputType);

		if(inputType == InputType.Number)
		{
            // 绘制Value
			inputValue = EditorGUILayout.TextField("Value", inputValue);
		}
		else if(inputType == InputType.Randomization)
		{
            // 绘制范围
			randomFrom = EditorGUILayout.TextField("From", randomFrom);
			randomTo = EditorGUILayout.TextField("To", randomTo);

            // 随机值
			if(GUILayout.Button("Calculate Random"))
			{
				calculateRandom();
			}
		}
	}

	public override void DrawCurves()
	{}
	
	private void calculateRandom ()
	{
		float rFrom = 0;
		float rTo = 0;

		float.TryParse(randomFrom, out rFrom);
		float.TryParse(randomTo, out rTo);

		int randFrom = (int)(rFrom *10);
		int randTo = (int)(rTo * 10);

		int selected = UnityEngine.Random.Range(randFrom, randTo +1);

		float selectedValue = selected / 10;

		inputValue = selectedValue.ToString();
	}

	public override string getResult ()
	{
		return inputValue.ToString();
	}

}










