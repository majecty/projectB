using System.Collections.Generic;

namespace Battle.CoreData
{
    public class Player
    {
        private int hp = 100;
        public int Hp { get { return hp; } }
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

    public class Deck
    {
        private List<Card> cards = new List<Card>();
        public Card GetCard(int index)
        {
            return cards[index];
        }
    }

    public class Card
    {
        private Stat stat;
        public Stat Stat { get { return stat; } }
    }

    public struct Stat
    {
        public readonly float Attack;
        public readonly float Defense;
        public readonly float Speed;

        public Stat(float attack, float defense, float speed)
        {
            Attack = attack;
            Defense = defense;
            Speed = speed;
        }
    }
}
