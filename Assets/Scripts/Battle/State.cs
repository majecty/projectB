using Battle.CoreData;

namespace Battle
{
    public class State
    {
        public readonly Player Player;
        public readonly Enemy Enemy;

        public State()
        {
            Player = new Player();
            Enemy = new Enemy();
        }

        public override string ToString()
        {
            var log = "Player: " + Player.Hp;
            log += "\nEnemy: " + Enemy.Hp;

            return log;
        }
    }
}
