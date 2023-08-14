using AroundYou.Utils.Extensions;
using Godot;
using Godot.Collections;

public partial class AnimationComponent : Node2D
{
    public string CurrentAnimationName { get; set; }

    private Dictionary<string, Node> AnimationsHandler;

    [Signal]
    public delegate void AnimationFinishedEventHandler(string animName);

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        AnimationPlayer animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        AnimatedSprite2D reloadAnimatedSprite = GetNode("ReloadBar") as AnimatedSprite2D;

        animationPlayer.AnimationFinished += AnimationPlayer_AnimationFinished;
        reloadAnimatedSprite.AnimationFinished += AnimatedSprite_AnimationFinished;

        AnimationsHandler = new Dictionary<string, Node>()
        {
            {"Hurt",  animationPlayer},
            {"Reload", reloadAnimatedSprite }
        };
    }

    public void Play(string name, float speedScale = 1)
    {
        if (AnimationsHandler.TryGetValue(name, out Node animationHandler))
        {
            if (animationHandler is AnimatedSprite2D animatedSprite2D)
            {
                animatedSprite2D.Play(name);
                animatedSprite2D.SpeedScale = speedScale;
            }
            else if (animationHandler is AnimationPlayer animationPlayer)
            {
                animationPlayer.Play(name);
                animationPlayer.SpeedScale = speedScale;
            }

            CurrentAnimationName = name;
        }
    }

    public void Stop(string name)
    {
        if (AnimationsHandler.TryGetValue(name, out Node animationHandler))
        {
            if (animationHandler is AnimatedSprite2D animatedSprite2D)
            {
                animatedSprite2D.Stop();
            }
            else if (animationHandler is AnimationPlayer animationPlayer)
            {
                animationPlayer.Stop();
            }
        }
    }

    public void Pause(string name)
    {
        if (AnimationsHandler.TryGetValue(name, out Node animationHandler))
        {
            if (animationHandler is AnimatedSprite2D animatedSprite2D)
            {
                animatedSprite2D.Pause();
            }
            else if (animationHandler is AnimationPlayer animationPlayer)
            {
                animationPlayer.Pause();
            }
        }
    }

    private void AnimatedSprite_AnimationFinished()
    {
        _ = EmitSignal(nameof(AnimationComponent.AnimationFinishedEventHandler), CurrentAnimationName);
        CurrentAnimationName = null;
    }

    private void AnimationPlayer_AnimationFinished(StringName animName)
    {
        _ = EmitSignal(nameof(AnimationComponent.AnimationFinishedEventHandler), animName);
        CurrentAnimationName = null;
    }
}
