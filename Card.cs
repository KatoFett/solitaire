using GameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Solitaire
{
    /// <summary>
    /// A playing card.
    /// </summary>
    public class Card : Sprite
    {
        /// <summary>
        /// Creates a new instance of a <see cref="Card"/>.
        /// </summary>
        /// <param name="suit">The suit the card is a part of.</param>
        /// <param name="value">The card's value from 1 (Ace) to 14 (Joker)</param>
        public Card(CardSuit suit, int value) : base()
        {
            if (value < 1 || value > 14)
                throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} must be between 1 and 14.");
            Suit = suit;
            Value = value;
            UpdateTextureName();
        }

        /// <summary>
        /// A card suit.
        /// </summary>
        public enum CardSuit
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades
        }

        /// <summary>
        /// A card color.
        /// </summary>
        public enum CardColor
        {
            Black,
            Red
        }

        /// <summary>
        /// The time in seconds it takes for the card to move to a location.
        /// </summary>
        public const float MOVEMENT_TIME = 0.05f;

        /// <summary>
        /// Gets the size of the card's texture.
        /// </summary>
        public static Vector2 CardSize => _CardSize;
        private static Vector2 _CardSize = Vector2.Zero;

        /// <summary>
        /// The suit this card is a part of.
        /// </summary>
        public CardSuit Suit { get; }

        public CardColor Color => Suit switch
        {
            CardSuit.Diamonds => CardColor.Red,
            CardSuit.Hearts => CardColor.Red,
            _ => CardColor.Black
        };

        /// <summary>
        /// The value of the card from 1 (Ace) to 14 (Joker).
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Gets or sets whether the card is face down.
        /// </summary>
        public bool IsFaceDown
        {
            get => _IsFaceDown;
            set
            {
                _IsFaceDown = value;
                UpdateTextureName();
            }
        }
        private bool _IsFaceDown = true;

        /// <summary>
        /// Gets or sets whether the card can be picked up by the player.
        /// </summary>
        public bool CanGrab { get; set; }

        private Vector2? _MouseOffset = null;
        private int _OriginalZIndex;

        /// <summary>
        /// Gets or sets the stack containing the card.
        /// </summary>
        public CardStack? OwnerStack { get; set; }

        /// <summary>
        /// Updates the card's texture name depending on whether it is face down.
        /// </summary>
        private void UpdateTextureName()
        {
            TextureName = IsFaceDown
                ? "card_back"
                : $"card_{Suit.ToString().ToLowerInvariant()}{Value}";
        }

        /// <summary>
        /// Moves and animates the card to a new <paramref name="location"/> on the screen.
        /// </summary>
        /// <param name="location">The card's destination.</param>
        /// <param name="resetZIndex">Whether the card's z-index should be reset.</param>
        public void Move(Vector2 location, bool resetZIndex = true)
        {
            var originalCanGrab = CanGrab;
            CanGrab = false;
            var _ = new Vector2Animation
            (
                Position,
                location,
                (value) => Position = value,
                MOVEMENT_TIME,
                callback: () =>
                {
                    CanGrab = originalCanGrab;
                    if (resetZIndex)
                        ZIndex = _OriginalZIndex;
                }
            );
        }

        /// <summary>
        /// Reads the card texture and sets the <see cref="CardSize"/>.
        /// </summary>
        public static void SetSize()
        {
            var cardTexture = VideoService.GetTexture("card_back");
            _CardSize = new Vector2(cardTexture.width, cardTexture.height);
        }

        /// <inheritdoc/>
        protected internal override void Update()
        {
            if (_MouseOffset != null)
            {
                if(MouseService.IsButtonDown(Raylib_cs.MouseButton.MOUSE_BUTTON_LEFT))
                    Position = MouseService.GetMouseCoordinates() - _MouseOffset.Value;
                else
                    ReleaseCard();
            }
        }

        protected internal override void OnMouseDown()
        {
            if (IsFaceDown || !CanGrab || !((MainScene)Scene.ActiveScene).IsInitialized) return;
            if (OwnerStack != null)
            {
                var allCards = OwnerStack.Stack.ToList();
                var thisIdx = allCards.IndexOf(this);
                for (int i = thisIdx; i >= 0; i--)
                {
                    allCards[i].PickUp(100 - i);
                }
            }
            else
            {
                PickUp(100);
            }
        }

        protected internal override void OnMouseDoubleClick()
        {
            var ms = (MainScene)Scene.ActiveScene;
            if (OwnerStack?.Stack.Any() == true && OwnerStack.Stack.Peek() != this) return;
            var validStack = ms.GetSuitStack(this);
            if (validStack != null) SetDown(validStack);
        }

        /// <summary>
        /// Released the card from the player's firm and manly grip.
        /// </summary>
        private void ReleaseCard()
        {

            if (OwnerStack != null)
            {
                var allCards = OwnerStack.Stack.ToList();
                var thisIdx = allCards.IndexOf(this);
                var dropDestination = GetDropDestination(thisIdx > 0);

                if ((dropDestination ?? OwnerStack) != OwnerStack)
                    OwnerStack.RemoveCard(this);

                var cards = allCards.Take(thisIdx + 1).Reverse();
                foreach (var card in cards)
                {
                    card._MouseOffset = null;
                }
                Scene.StartCoroutine(SetStackDown(cards, dropDestination));
            }
            else
            {
                var dropDestination = GetDropDestination();
                _MouseOffset = null;
                SetDown(dropDestination);
            }
        }

        /// <summary>
        /// Picks a card up to follow the mouse.
        /// </summary>
        /// <param name="newZIndex">The new Z-Index for the card.</param>
        private void PickUp(int newZIndex)
        {
            _MouseOffset = MouseService.GetMouseCoordinates() - Position;
            _OriginalZIndex = ZIndex;
            ZIndex = newZIndex;
        }

        /// <summary>
        /// Sets a card down onto a new lane or where it originated from.
        /// </summary>
        /// <param name="dropDestination">The new destination if any.</param>
        private void SetDown(CardStack? dropDestination)
        {
            var ms = (MainScene)Scene.ActiveScene;

            if (dropDestination != null && dropDestination != OwnerStack)
            {
                if (ms.Deck.VisibleCards.Contains(this))
                {
                    // This is the top card from the drawn hand.
                    ms.Deck.RemoveTopCard();
                }
                else if (OwnerStack != null)
                {
                    // OwnerStack may be null - bottom card of a multi-card drag.
                    OwnerStack?.RemoveCard(this);
                }

                if (OwnerStack == null)
                    dropDestination.AddCard(this);
            }
            else
            {
                var destination = OwnerStack != null
                    ? OwnerStack.GetCardPosition(this)
                    : ms.Deck.GetCardPosition(this);

                Move(destination);
            }
        }

        /// <summary>
        /// Sets a stack of cards down onto a destination.
        /// </summary>
        /// <param name="cards">The cards to set down</param>
        /// <param name="dropDestination">The new destination, if any.</param>
        private IEnumerator SetStackDown(IEnumerable<Card> cards, CardStack? dropDestination)
        {
            foreach (var card in cards)
            {
                card.SetDown(dropDestination);
                yield return new WaitForSeconds(CardDeck.CARD_DELAY);
            }
        }

        /// <summary>
        /// Gets a <see cref="CardStack"/> under this card onto which this card may be dropped.
        /// </summary>
        /// <param name="isMultiStack">Whether this card is part of a stack that's being dragged.</param>
        /// <returns>A valid <see cref="CardStack"/> drop, or null if none is found.</returns>
        private CardStack? GetDropDestination(bool isMultiStack = false)
        {
            object? dropDestination = null;
            var validDrops = GetAllCollisions().Where(c =>

                // Drop on card onto lane.
                c is Card card && card.OwnerStack?.Stack.Peek() == card && card.Color != Color && card.Value == Value + 1 && !card.IsFaceDown

                // Or drop a king onto an empty lane
                || c is CardLane lane && !lane.Stack.Any() && Value == 13

                // Or drop card onto suit stack.
                || !isMultiStack && c is SuitStack suit && suit.CanAcceptCard(this)
                ).ToList();

            // If multiple valid drops, pick one closest to the card's center.
            if (validDrops.Skip(1).Any())
            {
                var center = GetCenter();
                dropDestination = validDrops.Select(body => new { body, distance = Vector2.Distance(center, body.GetCenter()) })
                    .OrderBy(c => c.distance)
                    .Select(c => c.body)
                    .First();
            }
            else dropDestination = validDrops.FirstOrDefault();

            return dropDestination is Card c
                ? c.OwnerStack
                : dropDestination is CardStack stack
                    ? stack
                    : null;
        }
    }
}
