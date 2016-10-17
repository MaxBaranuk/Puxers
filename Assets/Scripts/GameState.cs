using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameState {

    public enum GameType { Single, TwoPlayers };
    public GameType gameType;
    public int bestScore;
    public bool MusicOn;
    public bool soundsOn;
}
