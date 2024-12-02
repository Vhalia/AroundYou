using AroundYou.Models;
using AroundYou.Models.Enums;
using AroundYou.Models.Modifiers;
using AroundYou.Scripts;
using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ChoosePowerUI : Control
{
    [Export]
    private Player _player;

    private static readonly string _powerControlsBasePath = "MarginContainer/VBoxContainer/HBoxContainer/";
    private static readonly string _statControlsBasePath = "MarginContainer/VBoxContainer/StatPanel/TextureRect/GridContainer/";

    private Control _powerControl1;
    private Control _powerControl2;
    private Control _powerControl3;

    private Control _healthStatControl;
    private Control _armorStatControl;
    private Control _evasionStatControl;
    private Control _movementSpeedStatControl;
    private Control _healthRegenStatControl;
    private Control _damageStatControl;
    private Control _fireRateStatControl;
    private Control _bulletSpeedStatControl;
    private Control _bulletCountStatControl;
    private Control _bulletsInMagazineStatControl;
    private Control _rangeStatControl;
    private Control _reloadSpeedStatControl;

    private List<Power> _powers;
    private Power[] _powersShown;
    private Dictionary<string, Control> _statControlPerName;
    private bool _isPrevisualizing;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        InjectPowerControls();
        InjectStatControls();

        _powers = new List<Power>()
        {
            Power.Init("Yummy !")
                .AddModifier(new StatModifier(
                    "Add +# to maximum life",
                    5,
                    nameof(StatsComponent.MaxHealth),
                    EStatCalculationType.ADD)),

            Power.Init("Reinforced")
                .AddModifier(new StatModifier(
                    "Add +#% armor",
                    5,
                    nameof(StatsComponent.Armor),
                    EStatCalculationType.ADD)),

            Power.Init("Ninja")
                .AddModifier(new StatModifier(
                    "Increase evasion by #%",
                    25,
                    nameof(StatsComponent.Evasion),
                    EStatCalculationType.INCREASE_PERCENT)),

            Power.Init("Speedy")
                .AddModifier(new StatModifier(
                    "Increase movement speed by #%",
                    25,
                    nameof(StatsComponent.MovementSpeed),
                    EStatCalculationType.INCREASE_PERCENT)),

            Power.Init("Rock")
                .AddModifier(new StatModifier(
                    "Decrease movement speed by #%",
                    -5,
                    nameof(StatsComponent.MovementSpeed),
                    EStatCalculationType.INCREASE_PERCENT))
                .AddModifier(new StatModifier(
                    "Add +#% armor",
                    10,
                    nameof(StatsComponent.Armor),
                    EStatCalculationType.ADD)),

            Power.Init("Life force")
                .AddModifier(new StatModifier(
                    "Add #% life regeneration",
                    0.5f,
                    nameof(StatsComponent.HealthRegeneration),
                    EStatCalculationType.ADD)),

            Power.Init("MORE POWEEER")
                .AddModifier(new StatModifier(
                    "Add # damage",
                    1,
                    nameof(StatsComponent.WeaponDamage),
                    EStatCalculationType.ADD)),

            Power.Init("And another mag")
                .AddModifier(new StatModifier(
                    "Add # bullets",
                    1,
                    nameof(StatsComponent.WeaponMagazineCapacity),
                    EStatCalculationType.ADD)),

            Power.Init("Shot speed")
                .AddModifier(new StatModifier(
                    "Increase shot speed by #%",
                    50,
                    nameof(StatsComponent.WeaponShotSpeed),
                    EStatCalculationType.INCREASE_PERCENT)),

            Power.Init("Bullet speed")
                .AddModifier(new StatModifier(
                    "Increase bullet speed #%",
                    50,
                    nameof(StatsComponent.WeaponBulletSpeed),
                    EStatCalculationType.INCREASE_PERCENT)),

            Power.Init("Reload speed")
                .AddModifier(new StatModifier(
                    "Increase reload speed by #%",
                    -25,
                    nameof(StatsComponent.WeaponReloadTime),
                    EStatCalculationType.INCREASE_PERCENT)),

            Power.Init("Far away")
                .AddModifier(new StatModifier(
                    "Add # range distance",
                    10,
                    nameof(StatsComponent.WeaponDistanceRange),
                    EStatCalculationType.ADD)),

            Power.Init("Bullets number")
                .AddModifier(new StatModifier(
                    "Add # bullet per shot",
                    1,
                    nameof(StatsComponent.WeaponBulletsPerShot),
                    EStatCalculationType.ADD))
        };

        _powersShown = new Power[3];

        _powerControl1.GuiInput += (@event) => PowerControlGuiInput(@event, 0);
        _powerControl2.GuiInput += (@event) => PowerControlGuiInput(@event, 1);
        _powerControl3.GuiInput += (@event) => PowerControlGuiInput(@event, 2);

        _powerControl1.MouseExited += ResetStatControls;
        _powerControl2.MouseExited += ResetStatControls;
        _powerControl3.MouseExited += ResetStatControls;
    }


    public void ShowUi()
    {
        GetTree().Paused = true;

        InitModifiers();
        InitStatPanel();

        Show();
    }

    private void InitModifiers()
    {
        List<Power> powers = SelectRandomPower();
        _powersShown = powers.ToArray();

        InitModifierControl(_powerControl1, powers[0]);
        InitModifierControl(_powerControl2, powers[1]);
        InitModifierControl(_powerControl3, powers[2]);
    }

    private void InitStatPanel()
    {
        (_healthStatControl.GetNode("Amount") as Label).Text = _player.StatsComponent.MaxHealth.ToString();
        (_armorStatControl.GetNode("Amount") as Label).Text = _player.StatsComponent.Armor.ToString();
        (_evasionStatControl.GetNode("Amount") as Label).Text = _player.StatsComponent.Evasion.ToString();
        (_movementSpeedStatControl.GetNode("Amount") as Label).Text = _player.StatsComponent.MovementSpeed.ToString();
        (_healthRegenStatControl.GetNode("Amount") as Label).Text = _player.StatsComponent.HealthRegeneration.ToString();
        (_damageStatControl.GetNode("Amount") as Label).Text = _player.Weapon.StatsComponent.WeaponDamage.ToString();
        (_fireRateStatControl.GetNode("Amount") as Label).Text = _player.Weapon.StatsComponent.WeaponShotSpeed.ToString();
        (_bulletSpeedStatControl.GetNode("Amount") as Label).Text = _player.Weapon.StatsComponent.WeaponBulletSpeed.ToString();
        (_bulletCountStatControl.GetNode("Amount") as Label).Text = _player.Weapon.StatsComponent.WeaponBulletsPerShot.ToString();
        (_bulletsInMagazineStatControl.GetNode("Amount") as Label).Text = _player.Weapon.StatsComponent.WeaponMagazineCapacity.ToString();
        (_rangeStatControl.GetNode("Amount") as Label).Text = _player.Weapon.StatsComponent.WeaponDistanceRange.ToString();
        (_reloadSpeedStatControl.GetNode("Amount") as Label).Text = _player.Weapon.StatsComponent.WeaponReloadTime.ToString();

        _statControlPerName = new()
        {
            { nameof(StatsComponent.MaxHealth), _healthStatControl },
            { nameof(StatsComponent.Armor), _armorStatControl },
            { nameof(StatsComponent.Evasion), _evasionStatControl },
            { nameof(StatsComponent.MovementSpeed), _movementSpeedStatControl },
            { nameof(StatsComponent.HealthRegeneration), _healthRegenStatControl },
            { nameof(StatsComponent.WeaponDamage), _damageStatControl },
            { nameof(StatsComponent.WeaponShotSpeed), _fireRateStatControl },
            { nameof(StatsComponent.WeaponBulletSpeed), _bulletSpeedStatControl },
            { nameof(StatsComponent.WeaponMagazineCapacity), _bulletsInMagazineStatControl },
            { nameof(StatsComponent.WeaponDistanceRange), _rangeStatControl },
            { nameof(StatsComponent.WeaponReloadTime), _reloadSpeedStatControl },
        };
    }

    private void InitModifierControl(Control controlPower, Power power)
    {
        var powerName = controlPower.GetNode("TextureRect/VBoxContainer/Name") as Label;
        powerName.Text = power.Name;

        var powerDescription = controlPower.GetNode("TextureRect/VBoxContainer/Description") as Label;
        powerDescription.Text = power.GenerateDescription();
    }

    private List<Power> SelectRandomPower()
    {
        List<Power> powersChosen = new();
        List<int> indexes = new();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = new Random().RandomBetween(0, _powers.Count - 1);
            } while (indexes.Contains(randomIndex));

            indexes.Add(randomIndex);
            powersChosen.Add(_powers[randomIndex]);
        }

        return powersChosen;
    }

    private void ChoosePower(int powerControlIndex)
    {
        var power = _powersShown[powerControlIndex];

        power.ApplyModifiers(_player.StatsComponent);
        power.ApplyModifiers(_player.Weapon.StatsComponent);

        Hide();

        GetTree().Paused = false;
    }

    private void PrevisualizeStatModifiers(int powerControlIndex)
    {
        if (_isPrevisualizing)
            return;

        var power = _powersShown[powerControlIndex];

        var statDifferencePlayer = power.CalculateDifferenceModifiers(_player.StatsComponent);
        var statDifferenceWeapon = power.CalculateDifferenceModifiers(_player.Weapon.StatsComponent);

        foreach((string statName, dynamic calculatedValue) in statDifferencePlayer
            .Where((pair) =>!pair.Key.StartsWith("Weapon")))
        {
            PrevisualizeStatLabel(statName, calculatedValue);
        }

        foreach ((string statName, dynamic calculatedValue) in statDifferenceWeapon
            .Where((pair) => pair.Key.StartsWith("Weapon")))
        {
            PrevisualizeStatLabel(statName, calculatedValue);
        }
    }

    private void PrevisualizeStatLabel(string statName, dynamic calculatedValue)
    {
        Label amountLabel = _statControlPerName[statName].GetNode("Amount") as Label;
        Label modificationLabel = _statControlPerName[statName].GetNode("Modification") as Label;

        if (float.Parse(amountLabel.Text) < calculatedValue)
        {
            modificationLabel.Text = "- ";
            modificationLabel.AddThemeColorOverride("font_color", new Color(255, 0, 0));
        }
        else if (float.Parse(amountLabel.Text) > calculatedValue)
        {
            modificationLabel.Text = "+ ";
            modificationLabel.AddThemeColorOverride("font_color", new Color(0, 255, 0));
        }
        else
        {
            modificationLabel.RemoveThemeColorOverride("font_color");
        }

        modificationLabel.Text += calculatedValue.ToString();
    }

    private void ResetStatControls()
    {
        foreach( var statControl in _statControlPerName.Values)
        {
            ResetStatControl(statControl);
        }

        _isPrevisualizing = false;
    }

    private void ResetStatControl(Control statControl)
    {
        Label amountLabel = statControl.GetNode("Modification") as Label;

        amountLabel.RemoveThemeColorOverride("font_color");

        amountLabel.Text = "";
    }

    private void InjectPowerControls()
    {
        _powerControl1 = GetNode(_powerControlsBasePath + "Power1") as Control;
        _powerControl2 = GetNode(_powerControlsBasePath + "Power2") as Control;
        _powerControl3 = GetNode(_powerControlsBasePath + "Power3") as Control;
    }

    private void InjectStatControls()
    {
        _healthStatControl = GetNode(_statControlsBasePath + "HealthStat") as Control;
        _armorStatControl = GetNode(_statControlsBasePath + "ArmorStat") as Control;
        _evasionStatControl = GetNode(_statControlsBasePath + "EvasionStat") as Control;
        _movementSpeedStatControl = GetNode(_statControlsBasePath + "MovementSpeedStat") as Control;
        _healthRegenStatControl = GetNode(_statControlsBasePath + "HealthRegenStat") as Control;
        _damageStatControl = GetNode(_statControlsBasePath + "DamageStat") as Control;
        _fireRateStatControl = GetNode(_statControlsBasePath + "FireRateStat") as Control;
        _bulletSpeedStatControl = GetNode(_statControlsBasePath + "BulletSpeedStat") as Control;
        _bulletCountStatControl = GetNode(_statControlsBasePath + "BulletCountStat") as Control;
        _bulletsInMagazineStatControl = GetNode(_statControlsBasePath + "BulletInMagazineStat") as Control;
        _rangeStatControl = GetNode(_statControlsBasePath + "RangeStat") as Control;
        _reloadSpeedStatControl = GetNode(_statControlsBasePath + "ReloadSpeedStat") as Control;
    }

    #region EVENT HANDLERS

    private void PowerControlGuiInput(InputEvent @event, int powerControlIndex)
    {
        if (@event is InputEventMouseButton mouseInput)
        {
            if (mouseInput.ButtonIndex == MouseButton.Left && mouseInput.Pressed)
            {
                ChoosePower(powerControlIndex);
            }
        }
        else if (@event is InputEventMouseMotion)
        {
            PrevisualizeStatModifiers(powerControlIndex);
            _isPrevisualizing = true;
        }
    }

    #endregion

}