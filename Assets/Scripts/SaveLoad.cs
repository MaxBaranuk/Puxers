using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad {

    
    public static void saveGame(int points)
    {
        if (points > GameSettings.state.bestScore)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
            GameSettings.state.bestScore = points;
            bf.Serialize(file, GameSettings.state);
            file.Close();
        }

    }

    public static void loadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
            GameState data = (GameState) bf.Deserialize(file);
            file.Close();
            GameSettings.state = data;
        }
    }
}
