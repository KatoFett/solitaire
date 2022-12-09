using System;
using System.Linq;
using System.Numerics;

namespace Solitaire
{
    /// <summary>
    /// A lane of cards spread vertically.
    /// </summary>
    public class CardLane : CardStack
    {

        /// <summary>
        /// Creates a new <see cref="CardLane"/>.
        /// </summary>
        /// <param name="position">The starting position of the card lane.</param>
        public CardLane(Vector2 position) : base(position) { }

        public const float VERTICAL_SPACING = 45f;

        /// <inheritdoc/>
        public override void AddCard(Card card)
        {
            AddCard(card, true);
        }

        /// <summary>
        /// Adds a card to the lane.
        /// </summary>
        /// <param name="card">The card to add.</param>
        /// <param name="setFaceUp">Whether the card should be flipped up.</param>
        public void AddCard(Card card, bool setFaceUp)
        {
            card.ZIndex = Stack.Select(c => c.ZIndex).DefaultIfEmpty(0).Max(z => z) + 1;
            if (setFaceUp)
            {
                card.IsFaceDown = false;
                card.CanGrab = true;
            }
            card.IsVisible = true;
            card.OwnerStack = this;

            card.Move(GetCardPosition(), resetZIndex: false);
            Stack.Push(card);
        }

        /// <inheritdoc/>
        public override Vector2 GetCardPosition(Card? card = null)
        {
            var idx = Stack.Count;
            if(card != null)
            {
                var allCards = Stack.ToList();
                if (!allCards.Contains(card)) throw new ArgumentOutOfRangeException(nameof(card));
                idx = Stack.Count - allCards.IndexOf(card) - 1;
            }
            return Position + idx * new Vector2(0, VERTICAL_SPACING);
        }

        /// <inheritdoc/>
        public override void RemoveCard(Card card)
        {
            if (!Stack.Contains(card)) throw new IndexOutOfRangeException();
            Card? poppedCard = null;
            do
            {
                poppedCard = Stack.Pop();
                poppedCard.OwnerStack = null;
            }
            while (poppedCard != card);

            if (Stack.TryPeek(out var nextCard))
            {
                nextCard.IsFaceDown = false;
                nextCard.CanGrab = true;
            }
        }
    }
}
