using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GameLogic
{
    public static class SaveLoad {

    
        public static void SaveGame(int points)
        {
            if (points > GameManager.Settings.BestScore.Value)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
                GameManager.Settings.BestScore.Value = points;
                bf.Serialize(file, GameManager.Settings);
                file.Close();
            }

        }

        public static GameSettings LoadGame()
        {
            GameSettings settings;
            if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
                var data = (GameSettings) bf.Deserialize(file);
                file.Close();
                settings = data;
            }
            else
            {
                settings = GameSettings.CreateDefault();
                
            }
            return settings;
        }
    }
}
