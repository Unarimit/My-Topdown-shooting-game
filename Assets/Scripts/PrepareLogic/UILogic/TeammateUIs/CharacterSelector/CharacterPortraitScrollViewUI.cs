using Assets.Scripts.PrepareLogic.PrepareEntities;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.PrepareLogic.UILogic.TeammateUIs.CharacterSelector
{
    /// <summary>
    /// ��PortraitsScrollView
    /// </summary>
    public class CharacterPortraitScrollViewUI : MonoBehaviour
    {

        public GameObject CharacterPortraitPrefab;
        public Transform PortraitsContentTrans;
        private PrepareContextManager _context => PrepareContextManager.Instance;
        private List<CharacterPortraitUI> _characterPortraits;
        private TeammateUI _teammateUI;
        public void Inject(TeammateUI teammateUI)
        {
            _teammateUI = teammateUI;
        }
        public void GeneratePortrait()
        {
            var ops = _context.data;
            _characterPortraits = new List<CharacterPortraitUI>();
            foreach (var op in ops)
            {
                var go = Instantiate(CharacterPortraitPrefab, PortraitsContentTrans);

                // set content
                var cp = go.GetComponent<CharacterPortraitUI>();
                cp.Inject(op, TeammatePortraitPage.ChoosePage, this);

                _characterPortraits.Add(cp);
            }
        }
        public void ChangePage(TeammatePortraitPage page)
        {
            foreach (var cp in _characterPortraits)
            {
                cp.ChangePage(page);
            }
            if(page == TeammatePortraitPage.EditPage) // Ĭ��ѡ�е�һ����ɫ
            {
                InEditSelect(_characterPortraits[0], _context.data[0]);
                _characterPortraits[0].SetChoose(true);
            }
        }

        /// <summary>
        /// ���������ʾ�Լ�ǰ����
        /// </summary>
        public void InEditSelect(CharacterPortraitUI portraitUI, PrepareOperator model)
        {
            foreach (var cp in _characterPortraits)
            {
                if(cp != portraitUI) cp.SetChoose(false);
            }
            _teammateUI.ShowEditCharacter(model);
        }

    }

}