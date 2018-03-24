using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

namespace Assets.Script.Component
{
    public class FileData
    {
        public void SaveUserData(User userData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/userData.dat");
            bf.Serialize(file, userData);
            file.Close();
        }

        public User LoadUserData()
        {
            if (File.Exists(Application.persistentDataPath + "/userData.dat"))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + "/userData.dat", FileMode.Open);
                    User data = (User)bf.Deserialize(file);
                    
                    file.Close();
                    return data;
                }
                catch (Exception e)
                {
                    Setting data = new Setting();
                    SaveSetting(data);

                    Scene scene = SceneManager.GetActiveScene();
                    if (scene.name != "selectLanguage" && scene.name != "login")
                    {
                        SceneManager.LoadScene(0);
                    }
                  return null;
                }
            }
            return null;
        }

        public void SaveMatchData(Match userData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/matchData.dat");
            bf.Serialize(file, userData);
            file.Close();
        }

        public Match LoadMatchData()
        {
            if (File.Exists(Application.persistentDataPath + "/matchData.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/matchData.dat", FileMode.Open);
                Match data = (Match)bf.Deserialize(file);
                file.Close();
                return data;
            }
            return null;
        }

        public void SaveSetting(Setting settingData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/setting.dat");
            bf.Serialize(file, settingData);
            file.Close();
        }

        public Setting LoadSetting()
        {
            Setting data;
            if (File.Exists(Application.persistentDataPath + "/setting.dat"))
            {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + "/setting.dat", FileMode.Open);
                    data = (Setting)bf.Deserialize(file);
                    file.Close();
                    return data;
            }
            else
            {
                data = new Setting();
                SaveSetting(data);
                return data;
            }
        }
    }
}

