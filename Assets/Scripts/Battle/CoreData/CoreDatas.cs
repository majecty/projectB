using System.Collections.Generic;
using System.Linq;

namespace Battle.CoreData
{
    public class Player
    {
        private int hp = 100;
        public int Hp { get { return hp; } }
        private readonly List<Card> deck;

        public Player()
        {
            var initialDeck = from i in Enumerable.Range(0, 6)
                select new Card();
            deck = initialDeck.ToList();
        }

        public void DiminishLife(float _damage)
        {
            hp -= (int)_damage;
        }

        public void ClickCard(int cardIndex)
        {
            deck[cardIndex].IsClicked = true;
        }

        public void UnClickCard(int cardIndex)
        {
            deck[cardIndex].IsClicked = false;
        }

        public bool IsClickedCard(int cardIndex)
        {
            return deck[cardIndex].IsClicked;
        }

        public void ClearAllClicked()
        {
            foreach (var card in deck)
            {
                card.IsClicked = false;
            }
        }

        public IEnumerable<Card> ClickedCardIndexes
        {
            get
            {
                return from card in deck
                    where card.IsClicked
                    select card;
            }
        }

        public string DeckToString()
        {
            return deck.DebugToString();
        }
    }

    public class Enemy
    {
        private int hp = 100;
        public int Hp { get { return hp; } }

        public void DiminishLife(float _damage)
        {
            hp -= (int)_damage;
        }
    }

    public class Card
    {
        public bool IsClicked { get; set; }
        public Skill skill;

        public Card()
        {
            skill = new Critical();
        }

        public override string ToString()
        {
            return IsClicked.ToString();
        }
    }
}
