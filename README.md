![tic-tac-godot-header](https://github.com/anbaretto/tic-tac-godot/assets/5423113/b23c3f4e-2021-4899-b627-b202279f564c)

# TicTacGodot
Hey there, welcome!

This is a small learning project I created for getting to know Godot better (using C#) and a _very specific_ game development strategy idea.
In here you will find a non-impressive tic-tac-toe game implementation, but there are layers to it hehe!

So my idea was to develop a "strategy game", or a turn-based one, in a robust way (in the lens of software development).
That means, a well-tested and validated (business) game rules, independent of game engines inner and crazy workings.

So this project has different "moving pieces":
- `TicTacToe`: a .NET project that provides a "tic-tac-toe engine", including concepts of Players, Making Moves, and Match.
- `TicTacToe.Tests`: a .Net project with [unit-tests](https://en.wikipedia.org/wiki/Unit_testing) that validate the game rules.
- `TicTacToe.ConsoleApp`: a simple windows console application to play the game.
- `Godot Project`: a game made with Godot that uses the "tic-tac-toe engine" and adds "more interesting" inputs and feedbacks.
Moreover, I implemented two "modes" in this Godot project:
  - "Basic mode", featuring simple Grid UI and big gray buttons you can click to make moves.
  - "Cannon mode", featuring -- you guessed it -- firing with cannons to make mokes in a 3D board.

In case you want to know more, I detail them further in the next section.

Otherwise, if you just want to play them now, head to [Releases](https://github.com/anbaretto/tic-tac-godot/releases) to download it!

## How it works

### TicTacToe engine/core
The TicTacToe .NET project is a non-impressive core tic-tac-toe implementation.

It provides concepts such as:
- Players (either X or O),
- Locations (top-left, middle-right, etc)
- Match (general state, current players, etc)
- Making moves (choosing a Location as a Player)
- Validating moves (checking player turn, if location is taken, etc)

### TicTacToe.Tests
This project validates the core's logic by implementing unit-tests (NUnit).

There it's validated things like win conditions, tie conditions, expected results and state changes when a Player tries to make a move.

Not much to see here, other than [checking out the code](https://github.com/anbaretto/tic-tac-godot/tree/main/TicTacToe/TicTacToe.Tests)!

### TicTacToe.ConsoleApp
The simplest implementation possible to test a game of this kind, now being able to feel the flow!

So by the time I got to make this app, the game as already tested (unit-tests) so I was 99% sure it was all good. Just one edge-case was missing, which I found out much later.

![Console_WindowsTerminal_KEo1A0Do5X-ezgif com-video-to-gif-converter (1)](https://github.com/anbaretto/tic-tac-godot/assets/5423113/2824b9fa-7c1e-467d-9dbd-886dc244712d)

To play it, you press the inputs (1 to 9) for making moves at the Locations marked by the numbers displayed in a sleek ASCII grid-ard.
And that's it! Other inputs such as start game and quitting are informed when you play.

### Godot project
The godot project (the _TicTacGodot_) uses the tic-tac-toe engine/core code by importing a dll generated from it.

No core tic-tac-toe "business rules" are done in this project. There it "only" handles user input and shows better and real-time feedbacks (the thing game engines are great for!).
Of course, there are some "managing" of states there, but it's only for things like going from one scene to another, reloading scenes, keeping some meta information and whatnot.

But more interestingly, in this project we have to modes: "Basic" and "Cannon".

#### Basic Mode
It's a straightforward mode, where all you have to do is click the big gray buttons to make a Move in the desired location.

Each time you do it, it's the next players turn.

![BasicMode_TicTacGodot-1 0_OU9cIOPHIk-ezgif com-video-to-gif-converter](https://github.com/anbaretto/tic-tac-godot/assets/5423113/2e57f452-0681-4ad6-9d78-c2201bd2101f)

It's like the Console application from before, but now you get to click the location, instead of having to count and press the number on your keyboard.


#### Cannon Mode
This whole project's _end-game_. Don't ask me why, but from the start I wanted to include cannons in tic-tac-toe.

On this mode, instead of clicking where you want to play, you have the convenience of aiming (with WASD or Arrows) and then holding to fire your cannon (with Space or Ctrl).
Where you projectile lands, it's where your move will be set! It didn't land where you wanted?! Oh well...

![CannonMode_TicTacGodot-1 0_nggMu3G1Ok-ezgif com-video-to-gif-converter](https://github.com/anbaretto/tic-tac-godot/assets/5423113/45560296-a9ad-49cd-98a9-b030572d3ce3)

With this done, I had reached 100% of my small scale scope, and since it was fun to do, during the process I came up with "Challenges" to make the Cannon Mode more... chaotic.

So whenever you **Restart**, you get a different Challenge. Things start to move, making you lose control on a very predictable game and _maybe_ have fun!

![CannonMode_TicTacGodot-1 0_rh1QmGVW5A-ezgif com-video-to-gif-converter](https://github.com/anbaretto/tic-tac-godot/assets/5423113/07951ff2-8c73-419a-b81f-c905df88a1a4)

## Final thoughts
A successful and fun (for me) little project, where I got to learn more about Godot and test out this "more robust process" on making turn-based games.

I'd like to make something similar with a more complex game, like a card game, and see if this process holds: how difficult would it be to unit-test several card effects and combinations, for example?

Definetely something that piques my interest, so let's see :)
