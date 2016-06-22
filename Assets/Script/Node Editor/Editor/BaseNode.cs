using UnityEngine;
using UnityEditor;


/// <summary>
/// 基础节点，一个自定义的Asset
/// </summary>
public abstract class BaseNode : ScriptableObject 
{
    /// <summary>
    /// 节点显示的区域
    /// </summary>
	public Rect windowRect;

    /// <summary>
    /// 是否有输入
    /// </summary>
	public bool hasInputs = false;

    /// <summary>
    /// 标题
    /// </summary>
	public string windowTitle = "";

    /// <summary>
    /// 绘制窗口
    /// </summary>
	public virtual void DrawWindow()
	{
        // 默认绘制窗口属性
		windowTitle = EditorGUILayout.TextField("Title", windowTitle);
	}

    /// <summary>
    /// 绘制曲线
    /// </summary>
	public abstract void DrawCurves();

    /// <summary>
    /// 设置输入节点
    /// </summary>
    /// <param name="input"></param>
    /// <param name="clickPos"></param>
	public virtual void SetInput(BaseInputNode input, Vector2 clickPos)
	{

	}

    /// <summary>
    /// 删除节点
    /// </summary>
    /// <param name="node"></param>
	public virtual void NodeDeleted(BaseNode node)
	{}


    /// <summary>
    /// 点击几点
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
	public virtual BaseInputNode ClickedOnInput(Vector2 pos)
	{
		return null;
	}
}
