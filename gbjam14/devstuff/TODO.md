# TODO

General

Game is can talk with every character but there can be only one quest at a time in order, we can start with mr toad with the !, the others can talk about general things but they have no real quest. After toad, we can continue with next and next, etc.

Next

> First Loop, Only 1 available Quest, Complete 1, Unlock next room => game ends when enter last door.
* Item highlight on interaction ready.
* Can't enter the temple without first quest

Game 

* BUG: eriq collider with vase cant move front
* One trap (arrows)
  - arrows break on hit wall
  - break vase with arrow (sfx + particles in ground?)
  - arrows shouldnt kill each other
  - detection area and/or click plate?
* Pause menu, options centered or something
  - restart room
  - quit?
  - map?
* Plate
  - toggle once and keep forever
  - is pressed but can be unpressed
  - returns to unpressed after leaving
  - returns to unpressed after some time
* Small jump
* Move blocks and vases over invisible grid?

Rooms

* Simplify room restriction, could be in ld spawn and in requirement system.
  - auto turn on if inventory changes?
* npc blocking room
* reset room
* Minimap! (not needed while game is short)

Sound Effects & Music

* Integrate vortex sfx when moving to next room
* Fire arrow
* Spikes come out
* Drag block or vase
* Vase breaks
* Arrow breaks

Pickups

* Item with no dialog
* Item auto configure depending inventory

Intro

* Show intro after main meun

# BUGS

* Game looks weird if no fullscreen in web build.

# NICE TO HAVE

* Better Palette animation
* Mind dialogs to read character mind for humor and/or for extra clues.
* Player can enter the name of the character
* Animated props & decos to make the environment feel alive.