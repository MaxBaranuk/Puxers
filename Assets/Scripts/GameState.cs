using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameState {

    public enum GameType { Single, TwoPlayers };
    public enum StyleType { Custom, Synargy}
    public GameType gameType;
    public StyleType style;
    public Sprite currBackground;
    public int bestScore;
    public bool MusicOn;
    public bool soundsOn;
}
