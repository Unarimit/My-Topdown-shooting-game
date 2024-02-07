﻿using Assets.Scripts.CombatLogic.Skill.Impactor;
using Assets.Scripts.Entities;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLua;

namespace Assets.Scripts.CombatLogic.Skill.Selector
{
    /// <summary>
    /// 利用碰撞箱的选择器，用lua指定轨迹
    /// </summary>
    internal class LuaTriggerSelector : MonoBehaviour, ISelector
    {
        List<IImpactor> _impactors;
        CombatSkill _skill;
        Action<Transform, Vector3> action;
        public void Init(List<IImpactor> impectors, Transform caster, CombatSkill skill, Vector3 aim)
        {
            _impactors = impectors;
            _skill = skill;
            action =  MyServices.LuaEnv.Global.GetInPath<Action<Transform, Vector3>>(skill.SkillSelector.Data);
            action(caster, aim);
        }
        private void OnTriggerEnter(Collider collision)
        {
            if (SkillManager.IsValidCollision(collision))
            {
                foreach (var imp in _impactors)
                {
                    imp.Impact(collision.transform);
                }
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (_skill.EffectType == SkillEffectType.ShootAndFreeze)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Collider>().enabled = false;
            }
            if (SkillManager.IsValidCollision(collision.collider))
            {
                foreach (var imp in _impactors)
                {
                    imp.Impact(collision.transform);
                }
            }
        }
    }
}
