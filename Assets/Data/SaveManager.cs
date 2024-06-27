using System.IO;
using UnityEngine;

namespace Player.save
{
    public class SaveManager : MonoBehaviour
    {
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