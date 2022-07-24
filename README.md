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
* 21.7. - hitting bushes, animation effects, creating sign objects from map, forcing small wait time on scene change (to hide draw lag when creating a lot of map objects)
* 21.7. - scene manager uses SceneTransition class, implemented new transition type FadeToBlack
* 24.7. - guard init, creating text events from Tiled map, dialog lines shift up, sectioned dialog box for parametrized size

roadmap:
* enemy ai
* taking/giving damage
* animation events (falling to hole)

small stuff todo:
* sys timing methods
* use arrays for tilemap tiles? compare performance with timing methods

could do but prob won't:
* async loading, maybe enough for scene load methods (task lists or IEnumerator "yield" methods?)
* parametrizable sprite animation speed, e.g. to bind with walk speed
* forwards/backwards looping sprite animation 
* Character's direction prop should be named Facing?
* action fields/"callbacks" could be named uniformly (OnThing or WhenThing)