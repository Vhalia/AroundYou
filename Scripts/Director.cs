using AroundYou.Models;
using AroundYou.Models.Enums;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = AroundYou.Models;

namespace AroundYou.Scripts
{
    public partial class Director : Node
    {
        [ExportCategory("Debug")]
        private bool _enableSpawn = true;

        [ExportCategory("Director")]
        [Export(PropertyHint.Range, "0,9999,1")]
        public int Units;
        [Export(PropertyHint.Range, "0,9999,1")]
        private int _unitsGainedPerSecond;
        [Export(PropertyHint.Range, "0,9999,1")]
        private int _spawnDistanceFromPlayer;
        [Export]
        private Player _player;
        [Export]
        private string _basePath;
        [Export(PropertyHint.Range, "0,1,0.1")]
        private float _difficultyScaleRate;
        [Export(PropertyHint.Range, "0,1,0.01")]
        private float _chanceToSpawnEnemies;

        private int _secondsPassed;

        [Node("Timer")]
        private Timer Timer;

        private List<Model.Entity> _entities = new();
        private Dictionary<string, int> _currentEntitiesSpawnedCount;

        private RandomNumberGenerator _random = new();

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
            PopulateEntities();

            SpawnEntities();

            Timer.Timeout += Timer_Timeout;
        }

        

        private void PopulateEntities()
        {
            var walkerEntity = new Model.Entity("walker", EEnemyType.WALKER, 10, 50, GD.Load<PackedScene>(_basePath + "walkerEnemy.tscn"));
            var shooterEntity = new Model.Entity("shooter", EEnemyType.SHOOTER, 50, 10, GD.Load<PackedScene>(_basePath + "shooterEnemy.tscn"));
            _entities = new()
            {
               walkerEntity,shooterEntity
            };
            _currentEntitiesSpawnedCount = new()
            {
                { walkerEntity.Name, 0 },
                {shooterEntity.Name, 0 },
            };
        }

        private void SpawnEntities()
        {
            if (!_enableSpawn) return;
            var possibleEntitiesToSpawn = _entities.Where(e => e.Cost <= Units).ToList();
            while (possibleEntitiesToSpawn.Any())
            {
                if (!_random.CanDoAction(_chanceToSpawnEnemies))
                    continue;

                var randomIndex = _random.RandiRange(0, possibleEntitiesToSpawn.Count - 1);
                SpawnEntity(possibleEntitiesToSpawn.ElementAt(randomIndex));
                possibleEntitiesToSpawn = possibleEntitiesToSpawn.Where(e => e.Cost <= Units).ToList();
            }
        }

        private void SpawnEntity(Model.Entity entity)
        {
            Units -= entity.Cost;
            var entityInstance = entity.Scene.Instantiate<Enemy>();
            entityInstance.Init(GenerateRandomPositionAroundPlayer());
            GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, entityInstance);
            _currentEntitiesSpawnedCount[entity.Name]++;
        }

        private void GainUnits()
        {
            GD.Print("DIRECTOR :: Units gained: ", (int)_unitsGainedPerSecond * _secondsPassed * _difficultyScaleRate);
            Units += (int) (_unitsGainedPerSecond * _secondsPassed * _difficultyScaleRate);
        }

        private Vector2 GenerateRandomPositionAroundPlayer()
        {
            float randomAngle = _random.RandfRange(0f, Mathf.Tau);
            return _player.GlobalPosition + new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * _spawnDistanceFromPlayer;
        }

        #region EVENT HANDLERS

        private void Timer_Timeout()
        {
            _secondsPassed += (int)Timer.WaitTime;
            SpawnEntities();
            GainUnits();
        }

        #endregion

    }
}
