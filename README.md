# Buffman-Jumpman

A 2D platformer created for the 2020 United Game Jam. Play as the dynamic duo of Jumpman and Buffman, two heroes each with their own skills and strengths, that must work together to solve the dungeon's puzzles and reach the goal. Inspired by Portal 2 and Ice Climbers, the game was developed to fit the theme "Two."

I led a team of three in developing the game. I was responsible for programming, gameplay design, locating assets, and infrastructure/project management. I worked with my brother who was responsible for level design and programming, and Andres Chamat, who did the music and a few of the sound effects.

Windows executable is available (download the archive file and unzip). Playable build is available at https://grahamewatt.com/buffman-jumpman. 

This repo contains the full unityproject file as well as the folder containing the scripts. To view the full project, download the unityproject file and open in Unity.  

Game Jam Entry page: https://itch.io/jam/united-game-jam-2020/rate/683110

## Devlog
A 48 hour jam is a challenge by any stretch of the imagination. I would have loved to have 48 hours to build this game. Due to timezone differences (the jam was on Indian Standard time), the jam began at 9am on Friday, and was scheduled to finish 9am Sunday. Both I and my brother had a full day of work to do before we could begin to start getting to the game development. Essentially, we ended up with ~6 hours on Friday evening to work, plus the day Saturday and a bit Sunday morning to cut a build and submit. Andres had paid commissions to tend to, so he was only sporadically available and had to compose blind.

We had a couple of interesting initial ideas, including a twin-stick type shooter where each stick controlled a character, and a game where you only got two actions (not counting moving). We settled on the two-character platformer as we both loved portal two and puzzle crafting for a team-type game really made us excited. Plus, platformer mechanics are fairly straightforward to implement, which means we could spend a bit more time on polish...in theory.

The initial mechanics were as follows:

**Jumpman**

* Fast move speed
* Can jump
* Can toggle switches
* Can kill certain enemies by stomping them
* *Can lower a rope for Buffman* - was not implemented

**Buffman**

* Slower move speed
* Cannot jump
* Can punch obstacles and enemies
* Can grab and throw things (i.e. Jumpman)

We did not have an artist, so we used assets from the Unity store. The tilemap feature in Unity was a lifesaver in terms of getting down a prototype scene; it meant we could drop a few platforms and focus on getting the mechanics installed. My brother programmed the enemies, and then took on the mechanic of lowering a rope; I dealt with the player and the various interactive objects in the game.

Our biggest mistake as a team came as a result of trying to implement the rope mechanic. Rusty with Unity (he hadn't used it in a couple of years), my brother implemented an interesting rope install mechanic over a couple of hours...then realized it wasn't going to work due to a quirk of how colliders behave with Unity's line renderer component. I missed a couple of check-ins for both jam related reasons (trying to implement unnecessary interfaces and getting myself tangled up in the code) and for reasons outside the jam, and so evening rolled around to realize that we were off-track and hadn't designed any levels yet--plus we still had to implement sound, animations, and build a menu UI. It was poor timing for a breakdown in communication--you tend to assume small teams will nearly always be on the same page, but this was a stark reminder that teamwork and communication are probably the most important part of any project. We had twelve hours to go and a lot to do, so we agreed to ditch the rope mechanic; my brother took charge of building levels and I went ahead and began implementing Andres' music and sound effects.

By late Saturday evening, we had a working game minus a menu screen. I whipped up a custom punch animation for Buffman (the only custom in-game art), and called it a night. Sunday morning we slapped together a menu screen and cut a build...though in our rush to upload the whole executable did not make it to itch.io, just the .exe (we were missing the data files). I set up a mirror on my personal website so folks could play the game. 

We got a few positive reviews of our game; the general sense was that it was a bit unpolished but had a fun concept. Given our breakdown in internal communication and the compressed timeline (well, more compressed), I'm pleased with what we ultimately shipped, and I took away a far deeper understanding of what game design and game dev management actually entails.



