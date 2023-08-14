using Godot;

public partial class FloatingLabel : Label
{


    /*public async override void _ExitTree()
    {
        await ToSignal(GetTree().CreateTimer(5f), "timeout");
        base._ExitTree();
    }*/

    public override void _Ready()
    {
        base._Ready();
        TreeExiting += FloatingLabel_TreeExiting;
    }

    private async void FloatingLabel_TreeExiting()
    {
        _ = await ToSignal(GetTree().CreateTimer(5f), "timeout");
    }
}
