using GameEngine;
using Raylib_cs;
using System.Collections;
using System.Linq;
using System.Numerics;

namespace Solitaire
{
    public class MainScene : Scene
    {
        public MainScene() : base("Solitaire", new Vector2(1920, 1020), Color.BLACK, 60)
        {
            _IsInitialized = false;
            TextObject.DefaultFont = "Kanit-Regular";
            Card.SetSize();
            var cardSpacing = new Vector2(30);
            var boardLength = 7 * Card.CardSize.X + 6 * cardSpacing.X;
            var cardX = Size.X / 2 - boardLength / 2;
            var cardY = Card.CardSize.Y + cardSpacing.Y * 2;

            _Background = VideoService.GetTexture("background");
            _NewGameButton = new Button("button", () => ResetGame())
            {
                Position = new Vector2(20f, 926f),
                Text = new TextObject("NEW GAME")
                {
                    FontSize = 30,
                    HorizontalAlignment = TextObject.Alignment.Center,
                    VerticalAlignment = TextObject.Justification.Center,
                },
                ColorPalette = new()
                {
                    Regular = new Color(0, 41, 138, 255),
                    Hover = new Color(9, 143, 253, 255),
                    Click = new Color(197, 97, 0, 255),
                    Disabled = new Color(127, 127, 127, 127),
                },
                TextColorPalette = new()
                {
                    Disabled = new Color(127, 127, 127, 127),
                }
            };

            _Deck = new CardDeck(new Vector2(cardX, cardSpacing.Y));
            _SuitStacks = new SuitStack[4]
            {
                new SuitStack(new Vector2(cardX + (Card.CardSize.X + cardSpacing.X) * 3, cardSpacing.Y)),
                new SuitStack(new Vector2(cardX + (Card.CardSize.X + cardSpacing.X) * 4, cardSpacing.Y)),
                new SuitStack(new Vector2(cardX + (Card.CardSize.X + cardSpacing.X) * 5, cardSpacing.Y)),
                new SuitStack(new Vector2(cardX + (Card.CardSize.X + cardSpacing.X) * 6, cardSpacing.Y)),
            };

            // Initialize lanes.
            _Lanes = new CardLane[LANE_COUNT];
            for (int i = 0; i < LANE_COUNT; i++)
            {
                var lane = new CardLane(new Vector2(cardX, cardY));
                _Lanes[i] = lane;
                cardX += Card.CardSize.X + cardSpacing.X;
            }

            InitGame();
        }

        public const int LANE_COUNT = 7;

        private readonly Texture2D _Background;
        private readonly CardLane[] _Lanes;
        private readonly SuitStack[] _SuitStacks;
        private readonly Button _NewGameButton;

        /// <summary>
        /// Gets whether the scene is initialized.
        /// </summary>
        public bool IsInitialized => _IsInitialized;
        private bool _IsInitialized;

        /// <summary>
        /// Gets the deck of cards in the top-left corner.
        /// </summary>
        public CardDeck Deck => _Deck;
        private readonly CardDeck _Deck;

        public override void Update()
        {
            Raylib.DrawTexture(_Background, 0, 0, Color.GREEN);
            base.Update();
        }

        /// <summary>
        /// Deals the deck onto the board.
        /// </summary>
        IEnumerator DealDeck()
        {
            // Each lane gets i + 1 cards.
            for (int col = 0; col < LANE_COUNT; col++)
            {
                for (int row = 0; row < LANE_COUNT; row++)
                {
                    if (row >= col)
                    {
                        var card = _Deck.DrawCard();
                        _Lanes[row].AddCard(card, row == col);
                        yield return new WaitForSeconds(0.05f);
                    }
                }
            }

            _IsInitialized = true;
            _NewGameButton.IsEnabled = true;
        }

        /// <summary>
        /// Resets the game and starts a new one.
        /// </summary>
        public void ResetGame()
        {
            StopAllCoroutines();
            Animation.StopAllAnimations();

            foreach (var lane in _Lanes)
            {
                lane.Clear();
            }

            foreach (var lane in _SuitStacks)
            {
                lane.Clear();
            }

            _Deck.Clear();
            InitGame();
        }

        /// <summary>
        /// Starts a game.
        /// </summary>
        public void InitGame()
        {
            _NewGameButton.IsEnabled = false;
            _Deck.InitDeck();
            StartCoroutine(DealDeck());
        }

        /// <summary>
        /// Gets a suit stack that the card can be dropped onto.
        /// </summary>
        /// <param name="card">The card requesting to be dropped.</param>
        /// <returns>A valid suit stack, or null if the card can't be legally dropped onto one.</returns>
        public SuitStack? GetSuitStack(Card card)
        {
            return _SuitStacks.FirstOrDefault(s => s.CanAcceptCard(card));
        }
    }
}
