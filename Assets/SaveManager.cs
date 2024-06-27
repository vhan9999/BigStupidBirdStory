using System.IO;
using UnityEngine;

namespace Player.save
{
    public class SaveManager : MonoBehaviour
    {
        private void Start()
        {
            // var tmp = new CharaData();
            // tmp.name = "test";
            // tmp.hp.max = 100;
            // tmp.hp.now = 100;
            var savePath = Application.persistentDataPath + "/save.json";
            // Save(tmp,savePath);
            Clean(savePath);
            var s = Load<CharaData>(savePath);
            var saveJson = JsonUtility.ToJson(s);
            Debug.Log(saveJson);
            Debug.Log(s.name);
        }

        public static void Clean(string path)
        {
            File.WriteAllText(path, "");
        }

        public static void Save<T>(T saveData, string path)
        {
            var saveJson = JsonUtility.ToJson(saveData);
            File.WriteAllText(path, saveJson);
        }

        public static T Load<T>(string path) where T : new()
        {
            var saveJson = "";
            try
            {
                saveJson = File.ReadAllText(path);
                Debug.Log(saveJson);
            }
            catch (FileNotFoundException)
            {
                var f = File.Open(path, FileMode.Create);
                f.Close();
                var t = new T();
                Save(t, path);
                return JsonUtility.FromJson<T>(JsonUtility.ToJson(t));
            }

            return JsonUtility.FromJson<T>(saveJson);
        }
    }
}