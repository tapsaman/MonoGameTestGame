# MonoGameTestGame

log:
* 9.7.22 - init with walking, hitting character (link), animations with animation manager, player states with state machine + quit button
* 13.7. - tiled map, hitboxes init, map collision
* 15.7 - dialogs, static input class, sounds/music, better sprites
* 15.7 - player inherits map entity, new map, using upscaled snes resolution
* 16.7 - bitmap fonts coz spritefonts suck (blurry in small scale)
* 16.7 - event system init
* 17.7 - split map entities to characters and event triggers, zelda style bitmap font and dialog box, nicer tilemap rendering
* 18.7. - scene management and new map
* 19.7. - moving "camera" to player, float rectangles to fix collision bugs (xna rectangles use int)
* 20.7. - condition and save value events, event data storage (only for bools atm), MapObject class (for bushes) and using Tiled map object layer

roadmap:
* enemy with ai and states
* hitting enemies/bushes
* animation effects (bush leaves on hit)
* animation events (falling to hole)

small stuff todo:
* destoying bushes
* camera should (probably) pan to expected x,y before scene transition