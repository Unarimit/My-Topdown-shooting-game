﻿using Assets.Scripts.BulletLogic;
using StarterAssets;
using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
using Random = UnityEngine.Random;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace Assets.Scripts.CombatLogic.Characters.Player
{
    [RequireComponent(typeof(OperatorController))]

    public class PlayerController : MonoBehaviour
    {

        public delegate void InteractEvent(Transform trans);
        public event InteractEvent InteractEventHandler;

        #region component
        private OperatorController _controller;
        private StarterAssetsInputs _input;
        private CombatContextManager _context => CombatContextManager.Instance;
        private DestructiblePersonController _destructiblePersonController;
        #endregion

        private GameObject _cameraFlowing;
        private void Awake()
        {
            // 注册其他组件
            // 相机追踪
            _cameraFlowing = Instantiate(ResourceManager.Load<GameObject>("Characters/CameraFlowing"), transform);
            _context.m_Camera.Follow = _cameraFlowing.transform;
            _context.CombatVM.PlayerTrans = transform;
        }
        private void OnDestroy()
        {
            // 注销组件
            Destroy(_cameraFlowing);
        }
        private void Start()
        {
            _controller = GetComponent<OperatorController>();
            _input = StarterAssetsInputs.Instance;
            _destructiblePersonController = GetComponent<DestructiblePersonController>();

        }
        private void Update()
        {
            Move();
            Skill();
            AimAndShoot();
            DoTriggerAction();
        }
        private void DoTriggerAction()
        {
            _controller.SetSprint(_input.sprint);
            if (_input.jump)
            {
                _controller.Jump();
                _input.jump = false;
            }
            else if (_input.slide) _controller.Slide();
            else if (_input.reloading) _controller.Reload();
            else if (_input.interact)
            {
                if(InteractEventHandler != null) InteractEventHandler.Invoke(transform);
            }
        }
        #region 按键逻辑
        private void Move()
        {
            _controller.Move(_input.move);
            _context.Operators[transform].Speed = _controller.Speed;
        }

        private float GunLine = 0.95f; // 枪线，应该在准星上
        private void AimAndShoot()
        {
            // 瞄准和射击的动作控制
            if (_input.aim && _controller.Aim(true, getMouseAiming()))
            {
                _context.CombatVM.IsPlayerAimming = true;
                // 开始射击
                if (_input.shoot)
                {
                    _controller.Shoot(getMouseAiming());
                }
            }
            else
            {
                _context.CombatVM.IsPlayerAimming = false;
                _controller.Aim(false, Vector3.zero);
            }
        }
        private void Skill()
        {
            if (_input.skill1 || _input.skill2)
            {
                if (_input.skill1) _controller.Skill(0, getMouseAiming());
                if (_input.skill2) _controller.Skill(1, getMouseAiming());
            }
        }

        private Vector3 getMouseAiming()
        {
            var mouse = Mouse.current.position.ReadValue();
            var ray = Camera.main.ScreenPointToRay(mouse);
            var hits = Physics.RaycastAll(ray, 100f, LayerMask.GetMask(new string[] { "Ground" }));
            var aim = new Vector3(); // 射击目标
            if (hits.Length == 1)
            {
                var temp = hits[0].point;
                aim = new Vector3(
                    temp.x + GunLine / ray.direction.y * ray.direction.x,
                    GunLine,
                    temp.z + GunLine / ray.direction.y * ray.direction.z
                );
            }
            else
            {
                Debug.Log("can not hit ground " + hits.Length);
            }
            return aim;
        }
        #endregion
    }
}