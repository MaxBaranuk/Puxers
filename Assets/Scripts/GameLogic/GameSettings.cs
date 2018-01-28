using ResourcesControl;
using UniRx;

namespace GameLogic
{
   [System.Serializable]
   public class GameSettings {
      public readonly ReactiveProperty<Style.Type> CurrentStyle = new ReactiveProperty<Style.Type>();
      public readonly IntReactiveProperty BestScore = new IntReactiveProperty();
//      public int BestScore;
      public readonly BoolReactiveProperty MusicOn = new BoolReactiveProperty();
      public readonly BoolReactiveProperty SoundsOn = new BoolReactiveProperty();

      public static GameSettings CreateDefault()
      {
         var settings = new GameSettings();
         settings.CurrentStyle.Value = Style.Type.Origin;
         settings.BestScore.Value = 0;
         settings.MusicOn.Value = true;
         settings.SoundsOn.Value = true;
         return settings;
      }

//
//      public static GameRecord Record = new GameRecord();
//      public static Game CurrentGame;
   }
}
