﻿using Assets.Scripts.Entities.Save;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Common
{
    /// <summary>
    /// 存取文件助手
    /// </summary>
    internal static class FileHelper
    {
        public static readonly string SAVE_ABSTRACTS_FILENAME = "SaveAbstracts";

        public static bool HasFile(string fileName)
        {
            var path = $"{Application.dataPath}/{fileName}.dat";
            return File.Exists(path);
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        public static T LoadFile<T>(string fileName)
        {
            var path = $"{Application.dataPath}/{fileName}.dat";
            if (File.Exists(path) is false) throw new ArgumentException($"filepath not exits:{path}");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            T data = (T)bf.Deserialize(file);
            return data;
        }

        /// <summary>
        /// 保存文件，如果文件已存在且replace不为true，会中断保存并返回false
        /// </summary>
        public static bool SaveFile<T>(T data, string fileName, bool replace = false)
        {
            var path = $"{Application.dataPath}/{fileName}.dat";
            BinaryFormatter bf = new BinaryFormatter();
            if(replace is false && File.Exists(path) is true) return false;

            FileStream file = File.Create(path);
            bf.Serialize(file, data);
            file.Close();
            return true;
        }
    }
}
