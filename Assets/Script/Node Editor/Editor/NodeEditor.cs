using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


/// <summary>
/// 自定义窗口编辑器
/// 实现数据运算
/// </summary>
public class NodeEditor : EditorWindow
{

    /// <summary>
    /// 保存所有节点信息
    /// </summary>
	private List<BaseNode> windows = new List<BaseNode>();

    /// <summary>
    /// 保存鼠标信息
    /// </summary>
	private Vector2 mousePos;

    /// <summary>
    /// 当前选择的节点信息
    /// </summary>
	private BaseNode selectednode;

    /// <summary>
    /// 是否处于连线模式
    /// </summary>
	private bool makeTransitionMode = false;


    /// <summary>
    /// 在Windows中创建打开当前编辑器的按钮
    /// </summary>
	[MenuItem("Window/Node Editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
    }

    void OnGUI()
    {
        // 获得当前编辑器事件
        Event e = Event.current;

        // 获得鼠标坐标
        mousePos = e.mousePosition;

        // 鼠标右键事件        处于连线模式
        if (e.button == 1 && !makeTransitionMode)
        {
            // 按下
            if (e.type == EventType.MouseDown)
            {
                bool clickedOnWindow = false;
                int selectIndex = -1;


                // 判断右键是否有点击到已经创建的界面
                for (int i = 0; i < windows.Count; i++)
                {
                    if (windows[i].windowRect.Contains(mousePos))
                    {
                        selectIndex = i;
                        clickedOnWindow = true;
                        break;
                    }
                }

                // GenericMenu 通用菜单
                // 用来创建自定义上下文菜单和下拉菜单。
                // AddItem 添加一个项目到菜单。
                // AddDisabledItem 添加一个禁用项目到菜单。
                // AddSeparator 添加一个分隔条项目到菜单。
                // GetItemCount 获取菜单中的项目数。
                // ShowAsContext 显示鼠标下方的菜单。
                // DropDown 在给定屏幕矩形位置显示菜单。


                // 如果没有点击到已经创建的界面
                if (!clickedOnWindow)
                {
                    // 创建一个按钮
                    GenericMenu menu = new GenericMenu();

                    // 4个子按钮，分别对应4个创建操作
                    menu.AddItem(new GUIContent("Add Input Node"), false, ContextCallback, "inputNode");
                    menu.AddItem(new GUIContent("Add Output Node"), false, ContextCallback, "outputNode");
                    menu.AddItem(new GUIContent("Add Calculation Node"), false, ContextCallback, "calcNode");
                    menu.AddItem(new GUIContent("Add Comparison Node"), false, ContextCallback, "compNode");
                    menu.AddItem(new GUIContent("Save"), false, ContextCallback, "Save");
                    menu.AddItem(new GUIContent("Load"), false, ContextCallback, "Load");

                    menu.ShowAsContext();
                    e.Use();
                }
                else
                {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, "makeTransition");
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");

                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }

        //       鼠标左键事件           按下                       连线模式
        else if (e.button == 0 && e.type == EventType.MouseDown && makeTransitionMode)
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            // 查找点击的节点
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            // 是否有点击到节点          当前点击的节点是否与以前点击的节点不同
            if (clickedOnWindow && !windows[selectIndex].Equals(selectednode))
            {
                // 设置输入操作
                windows[selectIndex].SetInput((BaseInputNode)selectednode, mousePos);

                makeTransitionMode = false;
                selectednode = null;
            }

            // 没有选择节点
            if (!clickedOnWindow)
            {
                // 非连线状态
                makeTransitionMode = false;
                // 当前选择节点为空
                selectednode = null;
            }

            e.Use();
        }

        //      鼠标左键事件             按下                          非连线
        else if (e.button == 0 && e.type == EventType.MouseDown && !makeTransitionMode)
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            // 获得当前选择节点
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow)
            {
                // 获得当前选择节点的输入节点
                BaseInputNode nodeTochange = windows[selectIndex].ClickedOnInput(mousePos);

                if (nodeTochange != null)
                {
                    // 设置当前选择的节点
                    selectednode = nodeTochange;

                    // 进入连线模式
                    makeTransitionMode = true;
                }
            }
        }

        // 连线模式                 有选择节点
        if (makeTransitionMode && selectednode != null)
        {
            // 获得点击范围
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);

            // 绘制节点曲线
            DrawNodeCurve(selectednode.windowRect, mouseRect);

            // 重新绘制
            Repaint();
        }

        // 遍历所有节点
        foreach (BaseNode n in windows)
        {
            // 绘制节点曲线
            n.DrawCurves();
        }

        BeginWindows();

        for (int i = 0; i < windows.Count; i++)
        {
            // 绘制节点窗口
            windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
        }

        EndWindows();

    }

    /// <summary>
    /// 绘制单个节点窗口
    /// </summary>
    /// <param name="id"></param>
	void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();

        // 拖拽窗口
        GUI.DragWindow();
    }

    /// <summary>
    /// 点击按钮回调统一管理
    /// </summary>
    /// <param name="obj"></param>
	void ContextCallback(object obj)
    {
        string clb = obj.ToString();

        // 点击创建输入节点
        if (clb.Equals("inputNode"))
        {
            // 新建输入节点
            InputNode inputNode = new InputNode();
            inputNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            // 添加到全局点解
            windows.Add(inputNode);
        }

        // 点击创建输出节点
        else if (clb.Equals("outputNode"))
        {
            OutputNode outputNode = new OutputNode();
            outputNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);

            windows.Add(outputNode);
        }

        // 点击创建计算节点
        else if (clb.Equals("calcNode"))
        {
            CalcNode calcNode = new CalcNode();
            calcNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);

            windows.Add(calcNode);
        }

        // 点击创建比较节点
        else if (clb.Equals("compNode"))
        {
            ComparisonNode comparisonNode = new ComparisonNode();
            comparisonNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);

            windows.Add(comparisonNode);
        }

        // 点击连线节点
        else if (clb.Equals("makeTransition"))
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            // 获得当前点击的节点
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow)
            {
                // 设置选择的几点
                selectednode = windows[selectIndex];
                // 设置连线模式
                makeTransitionMode = true;
            }
        }

        // 点击删除节点
        else if (clb.Equals("deleteNode"))
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            // 获得当前选择的节点
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow)
            {
                // 删除节点
                BaseNode selNode = windows[selectIndex];
                windows.RemoveAt(selectIndex);

                // 遍历所有节点，通知被删除节点信息
                foreach (BaseNode n in windows)
                {
                    n.NodeDeleted(selNode);
                }
            }
        }

//        else if (clb.Equals("Save"))
//        {
//            for (int i = 0; i < windows.Count; i++)
//            {
//                FileHelper.writeObjectFile(Application.persistentDataPath + "/Nodes_" + i + ".xml", windows[i], FileHelper.SerializeXML);
//            }
//        }
//
//        else if (clb.Equals("Load"))
//        {
//            for (int i = 0; i < windows.Count; i++)
//            {
//                FileHelper.readObjectFile<BaseNode>(Application.persistentDataPath + "/Nodes_" + i + ".xml",
//                    FileHelper.DeserializeXML<BaseNode>);
//            }
//        }
    }

    /// <summary>
    /// 绘制节点曲线
    /// </summary>
    /// <param name="start">起点区域</param>
    /// <param name="end">终点区域</param>
	public static void DrawNodeCurve(Rect start, Rect end)
    {
        // 计算起点
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
        // 计算终点
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);

        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;

        // 颜色
        Color shadowCol = new Color(0, 0, 0, .06f);

        for (int i = 0; i < 3; i++)
        {
            // 绘制 贝塞尔曲线将
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}
