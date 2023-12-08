﻿using Assets.Scripts.Common;
using Assets.Scripts.Common.EscMenu;
using Assets.Scripts.Entities;
using Assets.Scripts.Services;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.HomeLogic
{
    internal class HomeContextManager : MonoBehaviour
    {
        public static HomeContextManager Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Debug.LogWarning(transform.ToString() + " try to load another Manager");
            Time.timeScale = 1;
        }

        public List<LevelRule> GetLevelRules()
        {
            return MyServices.Database.LevelRules;
        }

        public void GoToLevel(LevelRule rule)
        {

            MyServices.Database.CurLevel = LevelGenerator.GeneratorLevelInfo(rule);
            SlideUI.CreateSlideUI();
            SceneManager.LoadScene("Prepare");
        }

        private bool isEscMenu = false;
        public void OnEscMenu(InputValue value)
        {
            if (isEscMenu) return;
            isEscMenu = true;
            var ui = EscMenuUI.OpenEscMenuUI();
            ui.ReturnBtn.onClick.AddListener(() =>
            {
                isEscMenu = false;
            });
        }
    }
}
