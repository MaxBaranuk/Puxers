using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad {

    public static int bestScore;
    public static void save(int points)
    {

        if (points > bestScore)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
            bestScore = points;
            bf.Serialize(file, bestScore);
            file.Close();
        }

    }

    public static int load()
    {
        int score = 0;
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            int data = (int)bf.Deserialize(file);
            file.Close();
            score = data;
        }
        return score;
    }
}
