using AroundYou.Scripts.Singleton;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

public partial class MagazineUI : Control
{

    [Node("MarginContainer/BulletContainer")]
    public VBoxContainer BulletContainer;

    [Export]
    public Texture2D BulletTexture;
    [Export]
    public Texture2D EmptyBulletTexture;

    private EBulletState[] bulletsTextureState;

    public override void _Ready()
    {
        this.WireNodes();
        this.GetAutoLoad<EventsBus>().BulletsInMagazineChanged += EventsBus_BulletsInMagazineChanged;
    }

    private void EventsBus_BulletsInMagazineChanged(int bulletsCount, int maxBullets)
    {
        if (bulletsTextureState == null)
        {
            AddInitialBulletsTextures(maxBullets);
            return;
        }
        if (bulletsCount == maxBullets)
        {
            ResetFillBulletsTextures();
            return;
        }

        for (var i = bulletsTextureState.Length-1; i >= bulletsCount; i--)
        {
            if (bulletsTextureState[i] == EBulletState.Empty)
                continue;
            TextureRect texture = BulletContainer.GetChild(i) as TextureRect;
            UpdateBulletTexture(texture, $"emptybullet{i}", EmptyBulletTexture);
            bulletsTextureState[i] = EBulletState.Empty;
        }
    }

    private void ResetFillBulletsTextures()
    {
        for(var i = 0; i < bulletsTextureState.Length; i++)
        {
            TextureRect texture = BulletContainer.GetChild(i) as TextureRect;
            UpdateBulletTexture(texture, $"bullet{i}", BulletTexture);
            bulletsTextureState[i] = EBulletState.Fill;
        }
    }

    private void AddInitialBulletsTextures(int count)
    {
        bulletsTextureState = new EBulletState[count];
        for (var i = 0; i < count; i++)
        {
            bulletsTextureState[i] = EBulletState.Fill;
            TextureRect newBulletTexture = CreateBulletTexture($"bullet{i}", BulletTexture);
            BulletContainer.AddChild(newBulletTexture);
        }
    }

    private TextureRect CreateBulletTexture(string name, Texture2D texture)
    {
        TextureRect textRect = new();
        UpdateBulletTexture(textRect, name, texture);
        return textRect;
    }

    private static void UpdateBulletTexture(TextureRect textRect, string name, Texture2D texture)
    {
        textRect.Texture = texture;
        textRect.Name = name;
        textRect.StretchMode = TextureRect.StretchModeEnum.Keep;
        textRect.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
    }

    private enum EBulletState
    {
        Empty = 0,
        Fill = 1
    }
}
