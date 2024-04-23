#if TOOLS
using Godot;
using System;

namespace WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class WCF : EditorPlugin
{
	private PackedScene m_MainScene = ResourceLoader.Load<PackedScene>("res://addons/wave_function_collapse/ui_main.tscn");

	private Control m_UIInstance;

	public override void _EnterTree()
	{
		m_UIInstance = m_MainScene.Instantiate<Control>();

		EditorInterface.Singleton.GetEditorMainScreen().AddChild(m_UIInstance);

		_MakeVisible(false);
	}

	public override void _ExitTree()
	{
		if (m_UIInstance != null)
		{
			m_UIInstance.QueueFree();
		}
	}

	public override bool _HasMainScreen()
	{
		return true;
	}

	public override void _MakeVisible(bool visible)
	{
		if (m_UIInstance != null)
		{
			m_UIInstance.Visible = visible;
		}
	}

	public override string _GetPluginName()
	{
		return "Wave Function Collapse";
	}

	public override Texture2D _GetPluginIcon()
	{
		// Must return some kind of Texture for the icon.
		return EditorInterface.Singleton.GetEditorTheme().GetIcon("Node", "EditorIcons");
	}
}
#endif
