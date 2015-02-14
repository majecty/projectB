using Battle.CoreData;

namespace Battle
{
    public class State
    {
        public readonly Player player;
        public readonly Enemy enemy;

        public State()
        {
            player = new Player();
            enemy = new Enemy();
        }

        public override string ToString()
        {
            var _log = "Player: " + player.Hp;
            _log += "\nEnemy: " + enemy.Hp;

            return _log;
        }
    }
}
