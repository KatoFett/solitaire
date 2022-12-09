# Solitaire

Solitaire is a card game where the cards are layed out before you in a randomized manner and locked behind each other.
The objective is to arrange the cards together in a specific fashion unlocking other cards until all cards are unlocked.

## Getting Started

Make sure you have installed all the required technologies. Open a command prompt window in the project workspace. Type the following command:
```
dotnet run
```
The project will be built and the game should start.
The game also runs in 1920x1040 resolution. The window does *not* support resizing so make sure it can support it!

## How to Play

### Start

When the game starts, the deck is dealt automatically. Wait for the deck to finish dealing.

### Moving Cards

#### How to Move Cards

You can move a card by clicking on it with the left mouse button and holding it down. The card will follow your mouse cursor.
You can drop the card by releasing the left mouse button. The card will automatically move to a valid position or its default position.

#### Where to Move Cards

Cards can be placed on top of other cards as long as they are

 1) of alternating color (e.g. you cannot stack red on red or black on black, but you can stack red on black and vise versa).
 2) The card is one value higher than the card you are holding (e.g. you cannot stack a 6 on an 8, but you can stack a 6 on a 7)

Kings be moved onto an empty lane.

Cards can also be placed the upper-right corner of the game. The objective is to have each suit in the upper-right corner.
 - The spaces are empty by default.
 - Aces are the first card to be placed. They can be placed in any empty space.
 - Cards afterwards must be of the same suit and one point higher.

For an example, you can place an Ace of Spades on any one of the empty spaces. But the next card in that space must be a 2 of Spades,
then a 3 of Spades and so on until the King of Spades.

Pro tip: You can double click any card and it will automatically move it to one of the spaces, if the move is valid.

### The Draw Deck

Once you have exhausted all possible moves on the board, you can click the deck on the top-left corner to draw a hand of 3 cards.
You can pull the top card and move it into a valid location on the board. If the card leaves the deck, the hand will adjust and you may
keep pulling from the hand.
If there are no available moves with the hand drawn from the deck, you may click the deck again and a new hand will be drawn.
Once you reach the end of the deck, click the deck again to move the hands back into the deck. The deck is NOT reshuffled.

### Winning

The game is won when all four of the stacks in the upper-right corner contain the cards from Ace to King.
The deck and the board will be empty.

### Losing

The game is lost when there are no possible moves. This is indicated by going through the deck to no avail.
However, be cautious as there may be tricky moves that you do not yet see.

### Starting a New Game

You can start a new game by pressing the blue NEW GAME button in the lower-left corner of the screen.

## Project Structure

The project is organized as such:
```
root
+--assets           (Folder containing images/audio/fonts specific to the game - automatically copied during build)
+--GameEngine       (Folder containing general code relevant to the game engine)
   +--Animation     (Folder containing GameEngine animation tools)
   +--CoRoutine     (Folder containing GameEngine coroutine (timed event) tools)
   +--Services      (Folder containing GameEngine services such as keyboard/audio)
   +--*.cs			(Classes relevant to the GameEngine)
+--Card.cs			(Class for the cards)
+--CardDeck.cs		(Class for the cards in the upper-left corner)
+--CardLane.cs		(Class for the cards lanes across the bottom of the board)
+--CardStack.cs		(Base class for the CardLane and SuitStack classes)
+--MainScene.cs		(The class controlling the game)
+--Program.cs       (Program entry point)
+--Raylib-cs.dll    (Raylib C# assembly)
+--raylib.dll       (Raylib assembly - automatically copied during build)
+--README.md        (Readme file)
+--Solitaire.csproj (.NET project file)
+--SuitStack.cs		(Class for the card stacks in the upper-right corner)
```
## Required Technologies

 - .NET 6.0.0 or greater

## Credits

 - Game engine derived from BYU-I CSE 210
   
   https://github.com/byui-cse/cse210-student-articulate-csharp-complete

 - Playing card images by OpenClipart-Vectors from Pixabay

 - Background image by dashu83 on Freepik

## Authors

Aaron Fox

