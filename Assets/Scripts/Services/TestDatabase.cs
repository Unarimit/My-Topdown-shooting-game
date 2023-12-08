﻿using Assets.Scripts.Common;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Mechas;
using Assets.Scripts.Services.Interface;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Services.MyConfig;

namespace Assets.Scripts.Services
{
    internal class TestDatabase : IGameDatabase
    {
        public List<Operator> Operators { get; private set; }

        public List<MechaBase> Mechas { get; private set; }

        public List<LevelRule> LevelRules { get; private set; }

        public LevelInfo CurLevel { get; set; }

        public List<string> ModelList { get; private set; }

        public TestDatabase()
        {
            ModelList = new List<string> { "Hoshino", "Shiroko", "Aru", "Karin", "Mashiro" };
            LevelRules = generateTestLevel();
            Operators = generateTestOperators();
            Mechas = generateTestMechas();
            registerDatabind();
        }

        private List<LevelRule> generateTestLevel()
        {
            // 演习作战（聚集分布敌人，进攻性强）、攻城作战（互动单位，中范围聚集分布敌人，进攻性弱）、三角定位（互动单位，和随机位置分布敌人，进攻性弱）
            return new List<LevelRule>()
            {
                new LevelRule
                {
                    LevelName = "演习作战",
                    Description = "达到击杀数就是胜利！可能会出现8-10个水平相当的敌人",
                    MapSize = MapSize.Middle,
                    TeamSpawn = new RectInt(5, 5, 5, 5),
                    EnemySpawn = new RectInt(25, 25, 5, 5),
                    OperatorPrefabs = new OperatorPrefab[]
                    {
                        new OperatorPrefab
                        {
                            OpInfo = getRandomCA(),
                            MinAmount = 5,
                            MaxAmount = 8,
                            UseRandomCModel = true,
                            MechaRandomUpgradeFactor = 0,
                            AiAgressive = true,
                            InitPosition = InitPosition.EnemySpawnScatter,
                            Dropouts = new Dropout[]{
                                new Dropout(ItemHelper.GetItem(DropoutTable.Sphere.ToString()))
                            }
                        },
                        new OperatorPrefab
                        {
                            OpInfo = getRandomCV(),
                            MinAmount = 1,
                            MaxAmount = 2,
                            UseRandomCModel = true,
                            MechaRandomUpgradeFactor = 0,
                            AiAgressive = true,
                            InitPosition = InitPosition.EnemySpawnScatter,
                            Dropouts = new Dropout[]{
                                new Dropout(ItemHelper.GetItem(DropoutTable.Red.ToString())),
                                new Dropout(ItemHelper.GetItem(DropoutTable.Purple.ToString()))
                            }
                        }
                    },
                    WinCondition = new Condition[]{
                        new Condition
                        {
                            ItemName =  DropoutTable.KillEnemy.ToString(),
                            Amount = 15,
                            Description = "击杀{0}个敌人"
                        }
                    },
                    LossCondition = new Condition[]{
                        new Condition
                        {
                            ItemName =  DropoutTable.KillTeam.ToString(),
                            Amount = 15,
                            Description = "被击杀{0}个队友"
                        }
                    },
                    AllowRespawn = true,
                    TeamAttackThreshold = 0,
                    EnemyAttackThreshold = 0
                },
                new LevelRule
                {
                    LevelName = "攻城作战",
                    Description = "深入敌营，破坏敌人重军防守的主要目标！可能会出现12-14个水平相当的敌人",
                    MapSize = MapSize.Middle,
                    TeamSpawn = new RectInt(5, 5, 5, 5),
                    EnemySpawn = new RectInt(25, 25, 5, 5),
                    OperatorPrefabs = new OperatorPrefab[]
                    {
                        new OperatorPrefab
                        {
                            OpInfo = getRandomCA(),
                            MinAmount = 5,
                            MaxAmount = 8,
                            UseRandomCModel = true,
                            MechaRandomUpgradeFactor = 0,
                            AiAgressive = true,
                            InitPosition = InitPosition.EnemySpawnScatter,
                        },
                        new OperatorPrefab
                        {
                            OpInfo = getRandomCV(),
                            MinAmount = 1,
                            MaxAmount = 2,
                            UseRandomCModel = true,
                            MechaRandomUpgradeFactor = 0,
                            AiAgressive = true,
                            InitPosition = InitPosition.EnemySpawnScatter,
                        }
                    },
                    InteractablePrefabs = new InteractablePrefab[]
                    {
                        new InteractablePrefab
                        {
                            ObjectId = "secret",
                            InteractTip = "破坏",
                            Duration = 3,
                            MinAmount = 1,
                            MaxAmount = 1,
                            InitPosition = InitPosition.EnemySpawnCenter,
                            Dropouts = new Dropout[]{
                                new Dropout(ItemHelper.GetItem(DropoutTable.Key.ToString()))
                            },
                            ModelUrl = "Objects/Key",
                        }
                    },
                    WinCondition = new Condition[]{
                        new Condition
                        {
                            ItemName =  DropoutTable.Key.ToString(),
                            Amount = 1,
                            Description = "摧毁{0}个红色方块"
                        }
                    },
                    LossCondition = new Condition[]{
                        new Condition
                        {
                            ItemName =  DropoutTable.Time.ToString(),
                            Amount = 120,
                            Description = "时间经过{0}秒"
                        }
                    },
                    AllowRespawn = true,
                    TeamAttackThreshold = 0.5f,
                    EnemyAttackThreshold = 0.5f
                },
                new LevelRule
                {
                    LevelName = "三角定位",
                    Description = "激活地图上的三个红色方块！小心散落在地图上的敌人",
                    MapSize = MapSize.Middle,
                    TeamSpawn = new RectInt(5, 5, 5, 5),
                    EnemySpawn = new RectInt(15, 15, 5, 5),
                    OperatorPrefabs = new OperatorPrefab[]
                    {
                        new OperatorPrefab
                        {
                            OpInfo = getRandomCA(),
                            MinAmount = 5,
                            MaxAmount = 8,
                            UseRandomCModel = true,
                            MechaRandomUpgradeFactor = 0,
                            AiAgressive = true,
                            InitPosition = InitPosition.MapScatter,
                        },
                        new OperatorPrefab
                        {
                            OpInfo = getRandomCV(),
                            MinAmount = 1,
                            MaxAmount = 2,
                            UseRandomCModel = true,
                            MechaRandomUpgradeFactor = 0,
                            AiAgressive = true,
                            InitPosition = InitPosition.MapScatter,
                        }
                    },
                    InteractablePrefabs = new InteractablePrefab[]
                    {
                        new InteractablePrefab
                        {
                            ObjectId = "secret",
                            InteractTip = "激活",
                            Duration = 3,
                            MinAmount = 3,
                            MaxAmount = 3,
                            InitPosition = InitPosition.MapScatter,
                            Dropouts = new Dropout[]
                            {
                                new Dropout(ItemHelper.GetItem(DropoutTable.Key.ToString()))
                            },
                            ModelUrl = "Objects/Key",
                        }
                    },
                    WinCondition = new Condition[]{
                        new Condition
                        {
                            ItemName =  DropoutTable.Key.ToString(),
                            Amount = 3,
                            Description = "激活{0}个红色方块"
                        }
                    },
                    LossCondition = new Condition[]{
                        new Condition
                        {
                            ItemName =  DropoutTable.Time.ToString(),
                            Amount = 120,
                            Description = "时间经过{0}秒"
                        }
                    },
                    AllowRespawn = true,
                    TeamAttackThreshold = 0.5f,
                    EnemyAttackThreshold = 0.5f
                }
            };
        }

        int opId = 0;

        private List<Operator> generateTestOperators()
        {
            // TODO: these just for test
            return new List<Operator>() {
                new Operator { Name = "hoshino", ModelResourceUrl = "Hoshino", WeaponSkillId = 4, Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Type = OperatorType.CV,
                    WeaponSkillId = 6,
                    Fighters = new List<Fighter>{
                        new Fighter { Operator = new Operator { Name = "ho", ModelResourceUrl = "Hoshino", Id = (++opId).ToString() } },
                        new Fighter { Operator = new Operator { Name = "shi", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() } }
                    },
                    Id = (++opId).ToString()
                },
                new Operator { Name = "aru", ModelResourceUrl = "Aru",Id = (++opId).ToString()  },
                new Operator { Name = "akrin", ModelResourceUrl = "Karin", Id = (++opId).ToString() },
                new Operator { Name = "mashiro", ModelResourceUrl = "Mashiro",Id = (++opId).ToString()  },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString()},
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
                new Operator { Name = "shiroko", ModelResourceUrl = "Shiroko", Id = (++opId).ToString() },
            };
        }
        private List<MechaBase> generateTestMechas()
        {
            return new List<MechaBase>
            {
                MechaBody.DefaultMecha(),
                MechaHead.DefaultMecha(),
                MechaLeg.DefaultMecha(),
                new MechaHead("Head II", "head2", 10, 10),
                new MechaBody("Body II", "body2", 15, 2),
                new MechaLeg(name: "Leg II", "leg2", 4, 10),
            };
        }
        private Operator getRandomCA()
        {
            return new Operator
            {
                Name = "random test",
                ModelResourceUrl = ModelList[Random.Range(0, ModelList.Count)],
                WeaponSkillId = 4,
                Type = OperatorType.CA,
                Id = (++opId).ToString()
            };
        }
        private Operator getRandomCV()
        {
            return new Operator
            {
                Name = "random test",
                ModelResourceUrl = ModelList[Random.Range(0, ModelList.Count)],
                WeaponSkillId = 6,
                Type = OperatorType.CV,
                Fighters = new List<Fighter>{
                    new Fighter { Operator = new Operator { Name = "r1", ModelResourceUrl = ModelList[Random.Range(0, ModelList.Count)] } },
                    new Fighter { Operator = new Operator { Name = "r2", ModelResourceUrl = ModelList[Random.Range(0, ModelList.Count)] } }
                },
                Id = (++opId).ToString()
            };
        }

        /// <summary>
        /// 注册数据绑定，面向1对1或1对多关系
        /// </summary>
        private void registerDatabind()
        {
            foreach(var op in Operators)
            {
                op.MechaChangeEventHandler += opMechaChangeEventHandler;
            }
        }

        private void opMechaChangeEventHandler(Operator @this, MechaBase oldMehca, MechaBase newMehca)
        {
            if (oldMehca.IsDefaultMecha() is false) oldMehca.DataBind(null);
            newMehca.DataBind(@this);
        }
    }
}
