﻿using Assets.Scripts.CombatLogic.Skill.Impactor;
using Assets.Scripts.CombatLogic.Skill.Releaser;
using Assets.Scripts.Entities;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CombatLogic.Skill.Selector
{
    internal class TriggerSelector : MonoBehaviour, ISelector
    {
        List<IImpactor> _impactors;
        CombatSkill _skill;
        float _time; // -1代表不销毁，0表示碰到就销毁(暂时未对trigger实现)，除此之外表示按时销毁
        public void Init(List<IImpactor> impectors, BaseReleaser releaser)
        {
            // reset碰撞系统
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            _impactors = impectors;
            _skill = releaser.Skill;
            _time = float.Parse(_skill.SkillSelector.Data);
            if (_time > 0) DOVirtual.DelayedCall(_time, () => GetComponent<Collider>().enabled = false);

            if (_skill.EffectType == SkillEffectType.PaticalPrefab)
            {
                transform.eulerAngles = Vector3.zero;
                GetComponent<ParticleSystem>().Play();
            }
            else if (_skill.EffectType == SkillEffectType.Shoot || _skill.EffectType == SkillEffectType.ShootAndFreeze)
            {
                GetComponent<Rigidbody>().velocity = (releaser.Aim - transform.position).normalized * 20 / 0.2f;
            }
            else if (_skill.EffectType == SkillEffectType.Throw)
            {
                GetComponent<Rigidbody>().velocity = releaser.Aim - transform.position;
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (SkillManager.IsValidCollision(collision))
            {
                foreach(var imp in _impactors)
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
