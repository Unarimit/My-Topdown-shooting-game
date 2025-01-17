﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public enum SkillReleaserType
    {
        Range, // releaser会绑定在发射物体上
        Melee // releaser会绑定在角色上
    }
    public enum SkillTargetTip
    {
        EnemySingle,
        EnemyRange,
        TeammateSingle,
        TeammateRange,
        Self,
    }
    [Serializable]
    public struct SkillSelector
    {
        public string SelectorName; // Self, Trigger
        public string Data;
    }
    [Serializable]
    public struct SkillImpactor
    {
        public string ImpectorName; // Damage, Speedup, FreezeControl
        public string Data;
    }
    /// <summary>
    /// 战斗技能
    /// </summary>
    [Serializable]
    public class CombatSkill
    {
        /// <summary>
        /// 技能类型
        /// </summary>
        public SkillType Type;
        /// <summary>
        /// 技能名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 技能Id
        /// </summary>
        public int Id;

        /// <summary>
        /// 技能描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 技能目标提示（用于AI判断和UI显示）
        /// </summary>
        public SkillTargetTip TargetTip = SkillTargetTip.EnemySingle;

        /// <summary>
        /// 射程提示（用于AI判断，实际上的射程是由释放器决定的）
        /// </summary>
        public float RangeTip = 5f;

        /// <summary>
        /// 技能冷却
        /// </summary>
        public float CoolDown;

        /// <summary>
        /// 技能持续时间
        /// </summary>
        public float Duration;

        /// <summary>
        /// 技能后摇，在这段时间内，角色不能做任何事
        /// </summary>
        public float AfterCastTime = 0;

        /// <summary>
        /// 技能播放动画Id
        /// </summary>
        public string CharacterAnimeId;

        /// <summary>
        /// 技能释放类型
        /// </summary>
        public SkillReleaserType ReleaserType; 

        /// <summary>
        /// 技能选择器
        /// </summary>
        public SkillSelector SkillSelector;
        /// <summary>
        /// 技能效果类型
        /// </summary>
        public SkillEffectType EffectType; // 附属于特定的`SkillSelector`，想想如何重新设计这一项

        /// <summary>
        /// 技能影响器
        /// </summary>
        public List<SkillImpactor> SkillImpectors;

        /// <summary>
        /// 链式技能触发，在持续时间结束后触发。如果没有下一个，设为-1
        /// </summary>
        public int NextSkillId = -1;

        /// <summary>
        /// 最大使用次数，-1表示无限
        /// </summary>
        public int Ammo = -1;


        /// <summary>
        /// 技能Prefab路径
        /// </summary>
        public string PrefabResourceUrl;

        /// <summary>
        /// 技能Icon路径
        /// </summary>
        public string IconUrl;

        public bool IsHaveNextSkill  => NextSkillId != -1;

        public bool IsGun => ReleaserType == SkillReleaserType.Range;
    }
}
