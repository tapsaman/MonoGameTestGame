# Zelda Adventure 666

MonoGame testing game project

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
* 25.7. - dialog can be skipped again, wood shadow overlay, direction methods, guard states, player takes damage
* 29.7. - shaders!!!, new rendering utility class, enemy class, new enemy bat, enemy death animation, heart display, inputcontroller class
* 31.7. - static music class, mild noise shader, characters collidingx/y fields update on entity colliding, new map c1, touch trigger events, gamepad support, game menu init
* 1.8. - global game states, menu state, using global game object renamed ZeldaAdventure666, states with draw method "render states"
* 5.8. - slider input, sfx/music volume sliders, select input, resolution setting, wait event, game states cutscene, start over and game over, animation class and game over animations, dialog questions, static img class, global event manager replaced with event system, spotlight shader, interfaces init, renamed StaticData to Static and Animation(Manager) to SAnimation(Manager)
* 5.8. - moogle, c1 start event init, animation events, jump and walk animations
* 10.8. - event manager not waiting and waiting for id, enemies bubble bari biri, moogle start event done, dialog questions and ask event, new link sprites, game over animations finished, state args for state enter
* 17.8. - new b1 forest scene, fixed bats, excecution timer utility, diagonal map collsion with collision type on each tile (CollisionTileMap), dynamic maps with world loaded from tiled world json, dev utils and dev view, trasure chests, doorways, wait/teleport/remove/run events, cave room and  klaus, items and heart, animated life hud, opening with title + start menu, renderer revamp and renderresolution class
18.8. - jump.to animation, rupee hud, moogle scene done ig, event managers by default get removed on scene change, xml data saving

roadmap:
* fuck shit up -> every death from falling to hole should break something in the game

small stuff todo:
* bush with hole shouldn't leave leafs
* use arrays for tilemap tiles? compare performance with timing methods
* better text highlight shader
* abstract class UIInput from Button
* could clean SAnimation constructor overload
* separate generic code to own namespace and folder like TapsasEngine or smth for future games
    - TileMap to (I)Map -> TileMap

could do but prob won't:
* if any velocity becomes NaN everything breaks, could force numeric values
* walk animation issue: collision map may block character movement before the distance is traveled. faster speed moves more by each update and is more likely to be blocked?
    possible fixes:
    - when blocked by collisin map move the charcter to the final possible x/y (by reducing velocity?)
    - check in walk animation if remaining distance is smaller than what character would move with the velocity
* harder issue with current character collision: diagonal y collision changes x velocity and vice versa. changed velocity should then be re-evaluated.  
* rename events to commands
* switch event based on datastore int
* async loading, maybe enough for scene load methods (task lists or IEnumerator "yield" methods?)
* parametrizable sprite animation speed, e.g. to bind with walk speed
* forwards/backwards looping sprite animation 
* Character's direction prop should be named Facing?
* action fields/"callbacks" could be named uniformly (OnThing or WhenThing)
* rename Content to Assets
* could use milliseconds instead of seconds for updates because ints take less space than floats
* circle/polygon collision shapes (???)
* ~~there's shitload of managers + manageables (animations, dialog, ui, events), could maybe have Manager + Manageable interfaces to enforce uniformity~~
e.g.
    IManager<TKey, IManageable>  ->
        Dictionary<TKey, IManageable> Lookup/Children/Collection/Stages?
        float ElapsedStageTime
        void Update
        void GoTo(TKey)
        void Draw?
        void SetToRemove
    IManageable ->
        IManager Manager
        bool IsDone
        bool CanReEnter
        void Update
        bool Paused?
        void Draw?
        bool DisableDrawing?
* ok nah all mmanagers work too differently to force anything except bare base interfaces, like IManageable with `bool IsDone { get; }` property