using UniRx;

namespace GameLogic
{
    public class Player
    {
        public ReactiveProperty<int> Score;
        public Type Order;

        public Player(Type order)
        {
            Score = new IntReactiveProperty(0);
            Order = order;
        }
    }
    
    public enum Type {One, Two}
}