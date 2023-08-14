using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;

public partial class ReloadBar : Control
{
    [Node("AnimationPlayer")]
    public AnimationPlayer AnimationPlayer;

    public override void _Ready()
    {
        this.WireNodes();
    }
    
    public void Play(int length)
    {
        AnimationPlayer.GetAnimation("Reload").Length = length;
        AnimationPlayer.GetAnimation("Reload").TrackSetKeyTime(0, 1, length - 0.1);
        AnimationPlayer.GetAnimation("Reload").TrackSetKeyTime(1, 1, length);
        AnimationPlayer.Play("Reload");
    }

}
