using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Linq;
using System.Reflection;

public partial class StatsComponent : Node2D
{
    #region Exports

    [ExportGroup("Entity")]
    [Export]
    private int _maxHealth;
    [Export]
    private float _healthRegeneration;
    [Export]
    private int _armor;
    [Export(PropertyHint.Range, "0,1,0.01")]
    private float _evasion;
    [Export]
    private int _movementSpeed;
    [ExportGroup("Weapon")]
    [Export]
    private int _damage;
    [Export]
    private int _magazineCapacity;
    [Export]
    private int _shotSpeed;
    [Export]
    private int _bulletSpeed;
    [Export]
    private int _reloadTime;
    [Export]
    private int _distanceRange;
    [Export]
    private int _bulletsPerShot;
    [Export]
    private int _meleeRange;
    

    #endregion

    #region Attributes

    public int MaxHealth { get => _maxHealth; set => SetMaxHealth(value); }
    public float HealthRegeneration { get => _healthRegeneration; set => SetHealthRegeneration(value); }
    public int Armor { get => _armor; set => SetArmor(value); }
    public float Evasion { get => _evasion; set => SetEvasion(value); }
    public int Damage { get => _damage; set => SetDamage(value); }
    public int MovementSpeed { get => _movementSpeed; set => SetMovementSpeed(value); }
    public int MagazineCapacity { get => _magazineCapacity; set => SetMagazineCapacity(value); }
    public int ShotSpeed { get => _shotSpeed; set => SetShotSpeed(value); }
    public int BulletSpeed { get => _bulletSpeed; set => SetBulletSpeed(value); }
    public int ReloadTime { get => _reloadTime; set => SetReloadTime(value); }
    public int DistanceRange { get => _distanceRange; set => SetDistanceRange(value); }
    public int BulletsPerShot { get => _bulletsPerShot ; set => SetBulletsPerShot(value); }
    public int MeleeRange { get => _meleeRange; set => SetMeleeRange(value); }

    #endregion

    #region Signals

    [Signal]
    public delegate void MaxHealthChangedEventHandler(int maxHealth);
    [Signal]
    public delegate void HealthRegenerationChangedEventHandler(float healthRegeneration);
    [Signal]
    public delegate void ArmorChangedEventHandler(int armor);
    [Signal]
    public delegate void EvasionChangedEventHandler(float evasion);
    [Signal]
    public delegate void DamageChangedEventHandler(int damage);
    [Signal]
    public delegate void MovementSpeedChangedEventHandler(int movementSpeed);
    [Signal]
    public delegate void MagazineCapacityChangedEventHandler(int magazineCapacity);
    [Signal]
    public delegate void ShotSpeedChangedEventHandler(int shotSpeed);
    [Signal]
    public delegate void BulletSpeedChangedEventHandler(int bulletSpeed);
    [Signal]
    public delegate void ReloadTimeChangedEventHandler(int reloadTime);
    [Signal]
    public delegate void DistanceRangeChangedEventHandler(int distanceRange);
    [Signal]
    public delegate void BulletsPerShotChangedEventHandler(int bulletsPerShot);
    [Signal]
    public delegate void MeleeRangeChangedEventHandler(int meleeRange);

    #endregion

    #region Setters
    private void SetMaxHealth(int maxHealth)
    {
        if (maxHealth == _maxHealth) return;
        _maxHealth = maxHealth;
        EmitSignal(SignalName.MaxHealthChanged, maxHealth);
    }

    private void SetHealthRegeneration(float healthRegeneration)
    {
        if (healthRegeneration == _healthRegeneration) return;
        _healthRegeneration = healthRegeneration;
        EmitSignal(SignalName.HealthRegenerationChanged, _healthRegeneration);
    }

    private void SetArmor(int armor)
    {
        if (armor == _armor) return;
        _armor = armor;
        EmitSignal(SignalName.ArmorChanged, armor);
    }

    private void SetEvasion(float evasion)
    {
        if (evasion == _evasion) return;
        _evasion = evasion;
        EmitSignal(SignalName.EvasionChanged, evasion);
    }

    private void SetDamage(int damage)
    {
        if (damage == _damage) return;
        _damage = damage;
        EmitSignal(SignalName.DamageChanged, damage);
    }

    private void SetMovementSpeed(int movementSpeed)
    {
        if (movementSpeed == _movementSpeed) return;
        _movementSpeed = movementSpeed;
        EmitSignal(SignalName.MovementSpeedChanged, movementSpeed);
    }

    private void SetMagazineCapacity(int magazineCapacity)
    {
        if (magazineCapacity == _magazineCapacity) return;
        _magazineCapacity = magazineCapacity;
        EmitSignal(SignalName.MagazineCapacityChanged, magazineCapacity);
    }

    private void SetShotSpeed(int shotSpeed)
    {
        if (shotSpeed == _shotSpeed) return;
        _shotSpeed = shotSpeed;
        EmitSignal(SignalName.ShotSpeedChanged, shotSpeed);
    }

    private void SetBulletSpeed(int bulletSpeed)
    {
        if (bulletSpeed == _bulletSpeed) return;
        _bulletSpeed = bulletSpeed;
        EmitSignal(SignalName.BulletSpeedChanged, bulletSpeed);
    }

    private void SetReloadTime(int reloadTime)
    {
        if (reloadTime == _reloadTime) return;
        _reloadTime = reloadTime;
        EmitSignal(SignalName.ReloadTimeChanged, reloadTime);
    }

    private void SetDistanceRange(int distanceRange)
    {
        if (distanceRange == _distanceRange) return;
        _distanceRange = distanceRange;
        EmitSignal(SignalName.DistanceRangeChanged, distanceRange);
    }

    private void SetBulletsPerShot(int bulletsPerShot)
    {
        if (bulletsPerShot == _bulletsPerShot) return;
        _bulletsPerShot = bulletsPerShot;
        EmitSignal(SignalName.DistanceRangeChanged, bulletsPerShot);
    }

    private void SetMeleeRange(int meleeRange)
    {
        if (meleeRange == _meleeRange) return;
        _meleeRange = meleeRange;
        EmitSignal(SignalName.MeleeRangeChanged, meleeRange);
    }

    #endregion

    public void Set(string propertyName, dynamic value)
    {
        PropertyInfo property = GetType().GetProperties()
            .ToList()
            .Find(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

        property.SetValue(this, Convert.ChangeType(value, property.PropertyType));
    }

    public void Add(string propertyName, dynamic value)
    {
        PropertyInfo property = GetType().GetProperties()
            .ToList()
            .Find(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

        var currentValue = Convert.ToSingle(property.GetValue(this));
        var finalValue = currentValue + value;

        property.SetValue(this, Convert.ChangeType(finalValue, property.PropertyType));
    }

    public void AddPercent(string propertyName, dynamic value)
    {
        PropertyInfo property = GetType().GetProperties()
            .ToList()
            .Find(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

        var currentValue = Convert.ToSingle(property.GetValue(this));
        var finalValue = currentValue + ((float)value / 100);

        property.SetValue(this, Convert.ChangeType(finalValue, property.PropertyType));
    }

    public void Increase(string propertyName, dynamic value)
    {
        PropertyInfo property = GetType().GetProperties()
            .ToList()
            .Find(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

        var currentValue = Convert.ToSingle(property.GetValue(this));
        var finalValue = currentValue * value;

        property.SetValue(this, Convert.ChangeType(finalValue, property.PropertyType));
    }

    public void IncreasePercent(string propertyName, dynamic percent)
    {
        PropertyInfo property = GetType().GetProperties()
            .ToList()
            .Find(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

        var currentValue = Convert.ToSingle(property.GetValue(this));
        var finalValue = currentValue + (currentValue * ((float)percent / 100));

        property.SetValue(this, Convert.ChangeType(finalValue, property.PropertyType));
    }

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();
    }

}
