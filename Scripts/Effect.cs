using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;

public partial class Effect : Node2D
{
    [Node("AnimationPlayer")]
    public AnimationPlayer AnimationPlayer;

    private string _animationName;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        AnimationPlayer.Play(_animationName);
        AnimationPlayer.AnimationFinished += AnimationPlayer_AnimationFinished;
    }

    public void Init(Node2D origin, string animName, Action action = null)
    {
        if (action != null)
            action();
        _animationName = animName;
        origin.GetMain().AddChild(this);
        this.Position = origin.Position;
    }

    private void AnimationPlayer_AnimationFinished(StringName animName)
    {
        QueueFree();
    }
}
