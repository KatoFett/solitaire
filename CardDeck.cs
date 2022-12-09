using GameEngine;
using Raylib_cs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Solitaire
{
    /// <summary>
    /// The deck of cards sitting in the top-left corner of the game.
    /// </summary>
    public class CardDeck : Sprite
    {
        /// <summary>
        /// Creates a new <see cref="CardDeck"/>.
        /// </summary>
        /// <param name="position">The position for the deck.</param>
        public CardDeck(Vector2 position) : base()
        {
            Position = position;
            InitDeck();
        }

        /// <summary>
        /// The amount of cards drawn at a time.
        /// </summary>
        public const int HAND_SIZE = 3;

        /// <summary>
        /// The horizontal spacing between drawn cards.
        /// </summary>
        public const float CARD_SPACING = 30f;

        /// <summary>
        /// The delay in seconds between clicks.
        /// </summary>
        public const float CLICK_DELAY = 0.1f;

        /// <summary>
        /// The card animation delay.
        /// </summary>
        public const float CARD_DELAY = 0.05f;

        /// <summary>
        /// Gets the cards in the deck.
        /// </summary>
        public List<Card> Cards { get; private set; }

        /// <summary>
        /// Gets the cards that are shown to the player.
        /// </summary>
        public List<Card> VisibleCards { get; } = new List<Card>();

        private bool _CanClick = true;

        /// <summary>
        /// Draws and removes the top card from the deck.
        /// </summary>
        /// <returns>The drawn card.</returns>
        public Card DrawCard()
        {
            var card = Cards.Last();
            Cards.Remove(card);
            return card;
        }

        protected internal override void OnMouseDown()
        {
            if (!((MainScene)Scene.ActiveScene).IsInitialized || !_CanClick || !Cards.Any()) return;

            _CanClick = false;

            if (VisibleCards.Count == Cards.Count)
            {
                Scene.StartCoroutine(BringVisibleCardsBack());
            }
            else
            {
                VisibleCards.TakeLast(HAND_SIZE).ToList().ForEach(c =>
                {
                    c.IsVisible = false;
                    c.CanGrab = false;
                });

                var cardCount = Math.Min(HAND_SIZE, Cards.Count - VisibleCards.Count);
                for (int i = 0; i < cardCount; i++)
                {
                    var card = Cards[VisibleCards.Count];
                    VisibleCards.Add(card);
                }

                Scene.StartCoroutine(MoveTopCards());

                if (VisibleCards.Count == Cards.Count)
                {
                    TextureName = "card_placeholder";
                    Tint = new Color(255, 255, 255, 63);
                }
            }
        }

        /// <summary>
        /// Removes the top card from the visible cards drawn.
        /// </summary>
        public void RemoveTopCard()
        {
            var topCard = VisibleCards.Last();
            VisibleCards.Remove(topCard);
            Cards.Remove(topCard);
            Scene.StartCoroutine(MoveTopCards(isShifting: true));

            // Cards won't always be shifted, may need to manually set CanGrab.
            if (VisibleCards.Any() )
                VisibleCards.Last().CanGrab = true;
        }

        /// <summary>
        /// Clears the deck and disposes the cards.
        /// </summary>
        public void Clear()
        {
            VisibleCards.Clear();
            foreach (var card in Cards)
            {
                card.Dispose();
            }
            Cards.Clear();
        }

        /// <summary>
        /// Initializes the deck with a new deck of cards.
        /// </summary>
        public void InitDeck()
        {
            SetTextureToDefault();
            var cards = new List<Card>();
            for (int suit = 0; suit < 4; suit++)
            {
                for (int value = 0; value < 13; value++)
                {
                    var card = new Card((Card.CardSuit)suit, value + 1)
                    {
                        IsVisible = false,
                        IsFaceDown = true,
                        Position = Position
                    };

                    cards.Add(card);
                }
            }

            Cards = cards.OrderBy(c => Random.Shared.Next()).ToList();
        }

        /// <summary>
        /// Moves the top cards from the deck onto the discard pile.
        /// </summary>
        /// <param name="isShifting">Whether the cards are already on the board and being shifted over one space.</param>
        private IEnumerator MoveTopCards(bool isShifting = false)
        {
            var lastCards = VisibleCards.TakeLast(HAND_SIZE).ToArray();
            var cardCount = Math.Min(HAND_SIZE, VisibleCards.Count);

            for (int i = 0; i < cardCount; i++)
            {
                var card = lastCards[isShifting ? lastCards.Length - i - 1 : i];

                card.IsVisible = true;
                card.IsFaceDown = false;
                card.ZIndex = isShifting ? HAND_SIZE - i : i;

                card.Move(GetCardPosition(card), false);

                yield return new WaitForSeconds(CARD_DELAY);
            }

            yield return new WaitForSeconds(CLICK_DELAY);
            _CanClick = true;

            if(VisibleCards.Any())
                VisibleCards.Last().CanGrab = true;
        }

        /// <summary>
        /// Gets the position for a visible card.
        /// </summary>
        /// <param name="card">The card whose position should be determined.</param>
        /// <returns>A <see cref="Vector2"/> that is the position for the card.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The card is not visible in the discard pile.</exception>
        public Vector2 GetCardPosition(Card card)
        {
            if (!VisibleCards.Contains(card)) throw new ArgumentOutOfRangeException(nameof(card));
            // Default position is far right.
            var cardX = Position.X + Card.CardSize.X + (Math.Min(HAND_SIZE, VisibleCards.Count) + 1) * CARD_SPACING;
            cardX -= (CARD_SPACING * (VisibleCards.Count - VisibleCards.IndexOf(card)));
            return new Vector2(cardX, Position.Y);
        }

        /// <summary>
        /// Returns all visible cards to the deck.
        /// </summary>
        private IEnumerator BringVisibleCardsBack()
        {
            SetTextureToDefault();
            var lastCards = VisibleCards.TakeLast(HAND_SIZE).Reverse().ToArray();
            var cardCount = Math.Min(HAND_SIZE, VisibleCards.Count);

            for (int i = 0; i < cardCount; i++)
            {
                var card = lastCards[i];
                card.IsVisible = true;
                card.IsFaceDown = true;
                card.ZIndex = 100 + i;
                card.CanGrab = false;
                card.Move(Position);
                yield return new WaitForSeconds(CARD_DELAY);
                card.IsVisible = false;
            }

            foreach (var card in VisibleCards)
            {
                card.IsFaceDown = true;
                card.IsVisible = false;
                card.ZIndex = 0;
                card.Position = Position;
            }

            VisibleCards.Clear();

            yield return new WaitForSeconds(CLICK_DELAY);
            _CanClick = true;
        }

        /// <summary>
        /// Sets the deck's texture to the default.
        /// </summary>
        private void SetTextureToDefault()
        {
            TextureName = "card_back";
            Tint = new Color(255, 255, 255, 255);
        }
    }
}
