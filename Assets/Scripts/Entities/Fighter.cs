﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{
    public enum FighterType
    {
        Bomber,
        AirFighter,
        
    }
    public static class FighterTypeExtension
    {
        public static string To2WordsString(this FighterType type)
        {
            if (type == FighterType.Bomber) return "BM";
            else if (type == FighterType.AirFighter) return "AF";
            else
            {
                throw new Exception($"FighterType donot match, the type is {type.ToString()}");
            }
        }
    }
    [Serializable]
    public class Fighter
    {
        /// <summary>
        /// 战机类型
        /// </summary>
        public FighterType Type = FighterType.Bomber;
        /// <summary>
        /// 序列化使用，为空表示没有相关的战机
        /// </summary>
        public string OperatorId = null;
        /// <summary>
        /// 战机操作员，决定战机属性。
        /// </summary>
        public Operator Operator { 
            get {
                return _Operator;
            } 
            set {
                _Operator = value;
                if(value != null)
                {
                    OperatorId = value.Id;
                }
            } 
        }

        Operator _Operator;
    }
}
