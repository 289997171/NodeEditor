using UnityEngine;


/// <summary>
/// 输出节点
/// </summary>
public class OutputNode : BaseNode 
{

    /// <summary>
    /// 结果
    /// </summary>
	private string result = "";

    /// <summary>
    /// 输入节点
    /// </summary>
	private BaseInputNode inputNode;

    /// <summary>
    /// 输入节点区域
    /// </summary>
    private Rect inputNodeRect;

	public OutputNode()
	{
        // 默认标题
		windowTitle = "Output Node";
		hasInputs = true;
	}

    /// <summary>
    /// 绘制窗口
    /// </summary>
	public override void DrawWindow()
	{
        // 绘制标题
		base.DrawWindow();

        // 获得事件
		Event e = Event.current;

        // 获得输入节点内容
        string input1Title = "None" ;
        
		if(inputNode)
		{
			input1Title = inputNode.getResult();
		}

        // 显示输入节点内容
		GUILayout.Label ("Input 1: " + input1Title);

		if(e.type == EventType.Repaint)
		{
			inputNodeRect = GUILayoutUtility.GetLastRect();
		}

		GUILayout.Label("Result: " + result);
	}

    /// <summary>
    /// 绘制曲线
    /// </summary>
	public override void DrawCurves()
	{
		if(inputNode)
		{
			Rect rect = windowRect;
			rect.x += inputNodeRect.x;
			rect.y += inputNodeRect.y + inputNodeRect.height/2;
			rect.width =1;
			rect.height = 1;

			NodeEditor.DrawNodeCurve(inputNode.windowRect, rect);
		}
	}

    /// <summary>
    /// 删除节点
    /// </summary>
    /// <param name="node"></param>
	public override void NodeDeleted(BaseNode node)
	{
		if(node.Equals(inputNode))
		{
			inputNode = null;
		}
	}

	public override BaseInputNode ClickedOnInput(Vector2 pos)
	{
		BaseInputNode retVal = null;

		pos.x -= windowRect.x;
		pos.y -= windowRect.y;

		if(inputNodeRect.Contains(pos))
		{
			retVal = inputNode;
			inputNode = null;
		}

		return retVal;
	}

	public override void SetInput (BaseInputNode input, Vector2 clickPos)
	{
		clickPos.x -= windowRect.x;
		clickPos.y -= windowRect.y;

		if(inputNodeRect.Contains(clickPos))
		{
			inputNode = input;
		}
	}

}






