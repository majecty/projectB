using System.Collections.Generic;

namespace Battle.CoreData
{
    public class Player
    {
        private int hp = 100;
        public int Hp { get { return hp; } }

        public void DiminishLife(float damage)
        {
            hp -= (int)damage;
        }
    }

    public class Enemy
    {
        private int hp = 100;
        public int Hp { get { return hp; } }

        public void DiminishLife(float damage)
        {
            hp -= (int)damage;
        }
    }
}
