using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var playButton = GetNode<Button>("VBoxContainer/PlayButton");
		var exitButton = GetNode<Button>("VBoxContainer/ExitButton");

		playButton.Pressed += OnPlayPressed;
		exitButton.Pressed += OnExitPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPlayPressed()
	{
		GD.Print("Play");
	}

	private void OnExitPressed()
	{
		GD.Print("Exit");
		GetTree().Quit();
	}
}
