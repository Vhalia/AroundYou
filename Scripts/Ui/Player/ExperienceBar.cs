using AroundYou.Scripts;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

public partial class ExperienceBar : Control
{
    [Export]
    private Player _player;
    [Export]
    private int _maxValuePerLevelGained = 25;

    [Node("ProgressBar")]
    private ProgressBar ProgressBar;
    [Node("Level")]
    private Label LevelLabel;

    private int _level;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        _level = 1;
        LevelLabel.Text = _level.ToString();

        _player.Pickedup += Player_Pickedup;
    }

    private void GainLevel()
    {
        _level++;
        LevelLabel.Text = _level.ToString();

        ProgressBar.MaxValue += _maxValuePerLevelGained;
        ProgressBar.Value = 0;

        _player.LevelUp(_level);
    }

    private void Player_Pickedup()
    {
        ProgressBar.Value++;
        if (ProgressBar.Value >= ProgressBar.MaxValue)
        {
            GainLevel();
        }
    }
}
