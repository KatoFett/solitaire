using System;
using System.Linq;
using System.Numerics;

namespace Solitaire
{
    /// <summary>
    /// A stack of cards of the same suit.
    /// </summary>
    public class SuitStack : CardStack
    {
        /// <summary>
        /// Creates a new <see cref="SuitStack"/
        /// </summary>
        /// <param name="position">The position to place the stack.</param>
        public SuitStack(Vector2 position) : base(position)
        {
            TextureName = "card_placeholder";
            Tint = new(255, 255, 255, 63);
        }

        /// <inheritdoc/>
        public override void AddCard(Card card)
        {
            if(Stack.TryPeek(out var topCard))
            {
                topCard.IsVisible = false;
            }
            Stack.Push(card);
            card.CanGrab = false;
            card.Move(GetCardPosition());
        }

        /// <inheritdoc/>
        public override Vector2 GetCardPosition(Card? card = null)
        {
            return Position;
        }

        /// <inheritdoc/>
        public override void RemoveCard(Card card)
        {
            // Cards can't be removed from the stack.
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets whether a card can be accepted onto this suit.
        /// </summary>
        /// <param name="card">The card requesting to be dropped.</param>
        /// <returns>Whether dropping the card onto this stack is valid.</returns>
        public bool CanAcceptCard(Card card)
        {
            return
            // First card of any stack.
            (!Stack.Any() && card.Value == 1)

            // Next card of specific stack.
            || (Stack.TryPeek(out var topCard) && topCard.Suit == card.Suit && topCard.Value == card.Value - 1);
        }
    }
}
