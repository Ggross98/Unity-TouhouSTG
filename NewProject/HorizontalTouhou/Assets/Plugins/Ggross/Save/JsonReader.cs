using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ggross.Save{
    public class JsonReader
    {

        /// <summary>
        /// 将指定json文件读取为对象
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadFromJson<T>(string path)
        {   
            // string prefix = asset ? Application.streamingAssetsPath : System.Environment.CurrentDirectory;
            // string path = prefix + "/" + _path;
            // Debug.Log(path);

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string readdata = reader.ReadToEnd();

                    if (readdata.Length > 0)
                    {
                        // Debug.Log(readdata);
                        T data = JsonUtility.FromJson<T>(readdata);
                        return data;
                    }
                }
            }
            Debug.Log("Not read");
            return default(T);
        }

        /// <summary>
        /// 将对象保存为json文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        public static void SaveAsJson<T>(string path, T obj){

            // 删除已有文件
            if(File.Exists(path)){
                File.Delete(path);
            }

            // 保存文件
            string json = JsonUtility.ToJson(obj);
            Debug.Log(json);
            StreamWriter sw = new StreamWriter(path);
            sw.Write(json);
            sw.Close();


        }
    }
}


