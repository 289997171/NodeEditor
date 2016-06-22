using UnityEngine;
using System.Collections;


/// <summary>
/// 基础输入节点
/// </summary>
public class BaseInputNode : BaseNode {

    /// <summary>
    /// 获得结果
    /// </summary>
    /// <returns></returns>
	public virtual string getResult()
	{
		return "None";
	}

	public override void DrawCurves()
	{

	}
}
