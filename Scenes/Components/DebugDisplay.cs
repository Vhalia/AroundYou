using AroundYou.Scripts;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using Godot.Collections;
using System;

public partial class DebugDisplay : Control
{
    [Node("GridContainer")]
    public GridContainer GridContainer;

    [Export]
    public LabelSettings LabelSettings;

    private Dictionary<string, Label> _valuePerTitle;

    [Export]
    public Player Player;
    [Export]
    public Director Director;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();
        PopulateValues();
        CreateLabels();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        UpdateValues();
    }

    private void UpdateValues()
    {
        _valuePerTitle["PlayerPosition"].Text =
            new Vector2(
                (float)Math.Round(Player.GlobalPosition.X, 2),
                (float)Math.Round(Player.GlobalPosition.Y, 2))
            .ToString();

        _valuePerTitle["DirectorUnits"].Text = Director.Units.ToString();

        _valuePerTitle["Armor"].Text = Player.StatsComponent.Armor.ToString();

        _valuePerTitle["Evasion"].Text = Player.StatsComponent.Evasion.ToString();

        _valuePerTitle["Movement Speed"].Text = Player.StatsComponent.MovementSpeed.ToString();
    }

    private void PopulateValues()
    {
        _valuePerTitle = new()
        {
            {"PlayerPosition", InitLabel("")},
            {"DirectorUnits", InitLabel("")},
            {"Armor", InitLabel("")},
            {"Evasion", InitLabel("")},
            {"Movement Speed", InitLabel("")},
        };
    }

    private void CreateLabels()
    {
        foreach ((string title, Label value) in _valuePerTitle)
        {
            GridContainer.AddChild(InitLabel(title + ": "));
            GridContainer.AddChild(value);
        }
    }

    private Label InitLabel(string value)
    {
        Label label = new()
        {
            Text = value,
            LabelSettings = LabelSettings
        };

        return label;
    }
}
