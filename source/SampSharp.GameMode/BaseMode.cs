﻿// SampSharp
// Copyright (C) 2014 Tim Potze
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>

using System;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.Natives;
using SampSharp.GameMode.World;

namespace SampSharp.GameMode
{
    /// <summary>
    ///     Represents a SA:MP gamemode.
    /// </summary>
    public abstract class BaseMode : IDisposable
    {
        #region Constructor

        /// <summary>
        ///     Initalizes a new instance of the BaseMode class.
        /// </summary>
        protected BaseMode()
        {
            RegisterControllers();
            Console.SetOut(new LogWriter());
        }

        #endregion

        #region Methods

        private void RegisterControllers()
        {
            var controllers = new ControllerCollection();
            LoadControllers(controllers);

            foreach (IController controller in controllers)
            {
                var typeProvider = controller as ITypeProvider;
                var eventListener = controller as IEventListener;

                if (typeProvider != null)
                    typeProvider.RegisterTypes();

                if (eventListener != null)
                    eventListener.RegisterEvents(this);
            }
        }

        protected virtual void LoadControllers(ControllerCollection controllers)
        {
            controllers.Add(new CheckpointController());
            controllers.Add(new CommandController());
            controllers.Add(new DialogController());
            controllers.Add(new GlobalObjectController());
            controllers.Add(new MenuController());
            controllers.Add(new PlayerController());
            controllers.Add(new PlayerObjectController());
            controllers.Add(new PlayerTextDrawController());
            controllers.Add(new RegionController());
            controllers.Add(new TextDrawController());
            controllers.Add(new TimerController());
            controllers.Add(new VehicleController());
        }

        #endregion

        #region SA-MP methods

        /// <summary>
        ///     Set the name of the game mode, which appears in the server browser.
        /// </summary>
        /// <param name="text">GameMode name.</param>
        public void SetGameModeText(string text)
        {
            Native.SetGameModeText(text);
        }

        /// <summary>
        ///     A function that can be used in <see cref="BaseMode.OnGameModeInit" /> to enable or disable the players markers,
        ///     which would normally be shown on the radar. If you want to change the marker settings at some other point in the
        ///     gamemode, have a look at <see cref="Player.SetPlayerMarker" /> which does exactly that.
        /// </summary>
        /// <param name="mode">The mode you want to use.</param>
        public void ShowPlayerMarkers(PlayerMarkersMode mode)
        {
            Native.ShowPlayerMarkers((int) mode);
        }

        /// <summary>
        ///     Toggle the drawing of player nametags, healthbars and armor bars above players.
        /// </summary>
        /// <param name="show">False to disable, True to enable.</param>
        public void ShowNameTags(bool show)
        {
            Native.ShowNameTags(show);
        }

        /// <summary>
        ///     Sets the world time to a specific hour.
        /// </summary>
        /// <param name="hour">Which time to set.</param>
        public void SetWorldTime(int hour)
        {
            Native.SetWorldTime(hour);
        }

        /// <summary>
        ///     Set the world weather for all players.
        /// </summary>
        /// <param name="weatherid">The weather to set.</param>
        public void SetWeather(int weatherid)
        {
            Native.SetWeather(weatherid);
        }

        /// <summary>
        ///     Uses standard player walking animation (animation of CJ) instead of custom animations for every skin (e.g. skating
        ///     for skater skins).
        /// </summary>
        public void UsePlayerPedAnims()
        {
            Native.UsePlayerPedAnims();
        }

        /// <summary>
        ///     Enable friendly fire for team vehicles.
        /// </summary>
        /// <remarks>
        ///     Players will be unable to damage teammates' vehicles (<see cref="Player.Team" /> must be used!)
        /// </remarks>
        public void EnableVehicleFriendlyFire()
        {
            Native.EnableVehicleFriendlyFire();
        }

        /// <summary>
        ///     Set the maximum distance to display the names of players.
        /// </summary>
        /// <param name="distance">The distance to set.</param>
        public void SetNameTagDrawDistance(float distance)
        {
            Native.SetNameTagDrawDistance(distance);
        }

        /// <summary>
        ///     Disable all the interior entrances and exits in the game (the yellow arrows at doors).
        /// </summary>
        public void DisableInteriorEnterExits()
        {
            Native.DisableInteriorEnterExits();
        }

        /// <summary>
        ///     This function is used to change the amount of teams used in the gamemode. It has no obvious way of being used, but
        ///     can help to indicate the number of teams used for better (more effective) internal handling. This function should
        ///     only be used in the <see cref="OnGameModeInit" /> callback.
        /// </summary>
        /// <remarks>
        ///     You can pass 2 billion here if you like, this function has no effect at all.
        /// </remarks>
        /// <param name="count">Number of teams the gamemode knows.</param>
        public void SetTeamCount(int count)
        {
            Native.SetTeamCount(count);
        }

        /// <summary>
        ///     Adds a class to class selection. Classes are used so players may spawn with a skin of their choice.
        /// </summary>
        /// <param name="modelid">The skin which the player will spawn with.</param>
        /// <param name="position">The coordinate of the spawnpoint of this class.</param>
        /// <param name="zAngle">The direction in which the player should face after spawning.</param>
        /// <param name="weapon1">The first spawn-weapon for the player.</param>
        /// <param name="weapon1Ammo">The amount of ammunition for the primary spawnweapon.</param>
        /// <param name="weapon2">The second spawn-weapon for the player.</param>
        /// <param name="weapon2Ammo">The amount of ammunition for the second spawnweapon.</param>
        /// <param name="weapon3">The third spawn-weapon for the player.</param>
        /// <param name="weapon3Ammo">The amount of ammunition for the third spawnweapon.</param>
        /// <returns>
        ///     The ID of the class which was just added. 300 if the class limit (300) was reached. The highest possible class
        ///     ID is 299.
        /// </returns>
        public int AddPlayerClass(int modelid, Vector position, float zAngle, Weapon weapon1, int weapon1Ammo,
            Weapon weapon2, int weapon2Ammo, Weapon weapon3, int weapon3Ammo)
        {
            return Native.AddPlayerClass(modelid, position, zAngle, weapon1, weapon1Ammo, weapon2, weapon2Ammo, weapon3,
                weapon3Ammo);
        }

        /// <summary>
        ///     Adds a class to class selection. Classes are used so players may spawn with a skin of their choice.
        /// </summary>
        /// <param name="modelid">The skin which the player will spawn with.</param>
        /// <param name="position">The coordinate of the spawnpoint of this class.</param>
        /// <param name="zAngle">The direction in which the player should face after spawning.</param>
        /// <param name="weapon1">The first spawn-weapon for the player.</param>
        /// <param name="weapon1Ammo">The amount of ammunition for the primary spawnweapon.</param>
        /// <param name="weapon2">The second spawn-weapon for the player.</param>
        /// <param name="weapon2Ammo">The amount of ammunition for the second spawnweapon.</param>
        /// <returns>
        ///     The ID of the class which was just added. 300 if the class limit (300) was reached. The highest possible class
        ///     ID is 299.
        /// </returns>
        public int AddPlayerClass(int modelid, Vector position, float zAngle, Weapon weapon1, int weapon1Ammo,
            Weapon weapon2, int weapon2Ammo)
        {
            return Native.AddPlayerClass(modelid, position, zAngle, weapon1, weapon1Ammo, weapon2, weapon2Ammo, 0, 0);
        }


        /// <summary>
        ///     Adds a class to class selection. Classes are used so players may spawn with a skin of their choice.
        /// </summary>
        /// <param name="modelid">The skin which the player will spawn with.</param>
        /// <param name="position">The coordinate of the spawnpoint of this class.</param>
        /// <param name="zAngle">The direction in which the player should face after spawning.</param>
        /// <param name="weapon">The spawn-weapon for the player.</param>
        /// <param name="weaponAmmo">The amount of ammunition for the spawnweapon.</param>
        /// <returns>
        ///     The ID of the class which was just added. 300 if the class limit (300) was reached. The highest possible class
        ///     ID is 299.
        /// </returns>
        public int AddPlayerClass(int modelid, Vector position, float zAngle, Weapon weapon, int weaponAmmo)
        {
            return Native.AddPlayerClass(modelid, position, zAngle, weapon, weaponAmmo, 0, 0, 0, 0);
        }


        /// <summary>
        ///     Adds a class to class selection. Classes are used so players may spawn with a skin of their choice.
        /// </summary>
        /// <param name="modelid">The skin which the player will spawn with.</param>
        /// <param name="position">The coordinate of the spawnpoint of this class.</param>
        /// <param name="zAngle">The direction in which the player should face after spawning.</param>
        /// <returns>
        ///     The ID of the class which was just added. 300 if the class limit (300) was reached. The highest possible class
        ///     ID is 299.
        /// </returns>
        public int AddPlayerClass(int modelid, Vector position, float zAngle)
        {
            return Native.AddPlayerClass(modelid, position, zAngle, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        ///     Adds a class to class selection. Classes are used so players may spawn with a skin of their choice.
        /// </summary>
        /// <param name="teamid">The team you want the player to spawn in.</param>
        /// <param name="modelid">The skin which the player will spawn with.</param>
        /// <param name="position">The coordinate of the class' spawn position.</param>
        /// <param name="zAngle">The direction in which the player will face after spawning.</param>
        /// <param name="weapon1">The first spawn-weapon for the player.</param>
        /// <param name="weapon1Ammo">The amount of ammunition for the first spawnweapon.</param>
        /// <param name="weapon2">The second spawn-weapon for the player.</param>
        /// <param name="weapon2Ammo">The amount of ammunition for the second spawnweapon.</param>
        /// <param name="weapon3">The third spawn-weapon for the player.</param>
        /// <param name="weapon3Ammo">The amount of ammunition for the third spawnweapon.</param>
        /// <returns>The ID of the class that was just created.</returns>
        public int AddPlayerClass(int teamid, int modelid, Vector position, float zAngle, Weapon weapon1,
            int weapon1Ammo, Weapon weapon2, int weapon2Ammo, Weapon weapon3, int weapon3Ammo)
        {
            return Native.AddPlayerClassEx(teamid, modelid, position, zAngle, weapon1, weapon1Ammo, weapon2, weapon2Ammo,
                weapon3, weapon3Ammo);
        }

        /// <summary>
        ///     Adds a class to class selection. Classes are used so players may spawn with a skin of their choice.
        /// </summary>
        /// <param name="teamid">The team you want the player to spawn in.</param>
        /// <param name="modelid">The skin which the player will spawn with.</param>
        /// <param name="position">The coordinate of the class' spawn position.</param>
        /// <param name="zAngle">The direction in which the player will face after spawning.</param>
        /// <param name="weapon1">The first spawn-weapon for the player.</param>
        /// <param name="weapon1Ammo">The amount of ammunition for the first spawnweapon.</param>
        /// <param name="weapon2">The second spawn-weapon for the player.</param>
        /// <param name="weapon2Ammo">The amount of ammunition for the second spawnweapon.</param>
        /// <returns>The ID of the class that was just created.</returns>
        public int AddPlayerClass(int teamid, int modelid, Vector position, float zAngle, Weapon weapon1,
            int weapon1Ammo, Weapon weapon2, int weapon2Ammo)
        {
            return Native.AddPlayerClassEx(teamid, modelid, position, zAngle, weapon1, weapon1Ammo, weapon2, weapon2Ammo,
                0, 0);
        }

        /// <summary>
        ///     Adds a class to class selection. Classes are used so players may spawn with a skin of their choice.
        /// </summary>
        /// <param name="teamid">The team you want the player to spawn in.</param>
        /// <param name="modelid">The skin which the player will spawn with.</param>
        /// <param name="position">The coordinate of the class' spawn position.</param>
        /// <param name="zAngle">The direction in which the player will face after spawning.</param>
        /// <param name="weapon">The spawn-weapon for the player.</param>
        /// <param name="weaponAmmo">The amount of ammunition for the spawnweapon.</param>
        /// <returns>The ID of the class that was just created.</returns>
        public int AddPlayerClass(int teamid, int modelid, Vector position, float zAngle, Weapon weapon, int weaponAmmo)
        {
            return Native.AddPlayerClassEx(teamid, modelid, position, zAngle, weapon, weaponAmmo, 0, 0, 0, 0);
        }

        /// <summary>
        ///     Adds a class to class selection. Classes are used so players may spawn with a skin of their choice.
        /// </summary>
        /// <param name="teamid">The team you want the player to spawn in.</param>
        /// <param name="modelid">The skin which the player will spawn with.</param>
        /// <param name="position">The coordinate of the class' spawn position.</param>
        /// <param name="zAngle">The direction in which the player will face after spawning.</param>
        /// <returns>The ID of the class that was just created.</returns>
        public int AddPlayerClass(int teamid, int modelid, Vector position, float zAngle)
        {
            return Native.AddPlayerClassEx(teamid, modelid, position, zAngle, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        ///     Enables or disables stunt bonuses for all players.
        /// </summary>
        /// <param name="enable">True to enable stunt bonuses, False to disable.</param>
        public void EnableStuntBonusForAll(bool enable)
        {
            Native.EnableStuntBonusForAll(enable);
        }

        /// <summary>
        ///     Set a radius limitation for the chat. Only players at a certain distance from the player will see their message in
        ///     the chat. Also changes the distance at which a player can see other players on the map at the same distance.
        /// </summary>
        /// <param name="chatRadius">Radius limit.</param>
        public void LimitGlobalChatRadius(float chatRadius)
        {
            Native.LimitGlobalChatRadius(chatRadius);
        }

        /// <summary>
        ///     Set the player marker radius.
        /// </summary>
        /// <param name="markerRadius">The radius that markers will show at.</param>
        public void LimitPlayerMarkerRadius(float markerRadius)
        {
            Native.LimitPlayerMarkerRadius(markerRadius);
        }

        /// <summary>
        ///     Use this function before any player connects (<see cref="BaseMode.OnGameModeInit" />) to tell all clients that the
        ///     script will control vehicle engines and lights. This prevents the game automatically turning the engine on/off when
        ///     players enter/exit vehicles and headlights automatically coming on when it is dark.
        /// </summary>
        public void ManualVehicleEngineAndLights()
        {
            Native.ManualVehicleEngineAndLights();
        }

        /// <summary>
        ///     Ends the currently active gamemode.
        /// </summary>
        public void Exit()
        {
            Native.GameModeExit();
        }

        /// <summary>
        ///     Toggle whether the usage of weapons in interiors is allowed or not.
        /// </summary>
        /// <param name="allow">True to enable weapons in interiors (enabled by default), False to disable weapons in interiors.</param>
        public void AllowInteriorWeapons(bool allow)
        {
            Native.AllowInteriorWeapons(allow);
        }

        /// <summary>
        ///     With this function you can enable or disable tire popping.
        /// </summary>
        /// <param name="enable">True to enable, False to disable tire popping.</param>
        public void EnableTirePopping(bool enable)
        {
            Native.EnableTirePopping(enable);
        }

        /// <summary>
        ///     Sends an RCON command.
        /// </summary>
        /// <param name="command">The RCON command to be executed.</param>
        public void SendRconCommand(string command)
        {
            Native.SendRconCommand(command);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the <see cref="OnGameModeInit" /> callback is being called.
        ///     This callback is triggered when the gamemode starts.
        /// </summary>
        public event EventHandler<GameModeEventArgs> Initialized;

        /// <summary>
        ///     Occurs when the <see cref="OnGameModeExit" /> callback is being called.
        ///     This callback is called when a gamemode ends.
        /// </summary>
        public event EventHandler<GameModeEventArgs> Exited;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerConnect" /> callback is being called.
        ///     This callback is called when a player connects to the server.
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerConnected;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerDisconnect" /> callback is being called.
        ///     This callback is called when a player disconnects from the server.
        /// </summary>
        public event EventHandler<PlayerDisconnectedEventArgs> PlayerDisconnected;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerSpawn" /> callback is being called.
        ///     This callback is called when a player spawns.
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerSpawned;

        /// <summary>
        ///     Occurs when the <see cref="OnGameModeInit" /> callback is being called.
        ///     This callback is triggered when the gamemode starts.
        /// </summary>
        public event EventHandler<PlayerDeathEventArgs> PlayerDied;

        /// <summary>
        ///     Occurs when the <see cref="OnVehicleSpawn" /> callback is being called.
        ///     This callback is called when a vehicle respawns.
        /// </summary>
        public event EventHandler<VehicleEventArgs> VehicleSpawned;

        /// <summary>
        ///     Occurs when the <see cref="OnVehicleDeath" /> callback is being called.
        ///     This callback is called when a vehicle is destroyed - either by exploding or becoming submerged in water.
        /// </summary>
        /// <remarks>
        ///     This callback will also be called when a vehicle enters water, but the vehicle can be saved from destruction by
        ///     teleportation or driving out (if only partially submerged). The callback won't be called a second time, and the
        ///     vehicle may disappear when the driver exits, or after a short time.
        /// </remarks>
        public event EventHandler<PlayerVehicleEventArgs> VehicleDied;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerText" /> callback is being called.
        ///     Called when a player sends a chat message.
        /// </summary>
        public event EventHandler<PlayerTextEventArgs> PlayerText;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerCommandText" /> callback is being called.
        ///     This callback is called when a player enters a command into the client chat window, e.g. /help.
        /// </summary>
        public event EventHandler<PlayerTextEventArgs> PlayerCommandText;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerRequestClass" /> callback is being called.
        ///     Called when a player changes class at class selection (and when class selection first appears).
        /// </summary>
        public event EventHandler<PlayerRequestClassEventArgs> PlayerRequestClass;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerEnterVehicle" /> callback is being called.
        ///     This callback is called when a player starts to enter a vehicle, meaning the player is not in vehicle yet at the
        ///     time this callback is called.
        /// </summary>
        public event EventHandler<PlayerEnterVehicleEventArgs> PlayerEnterVehicle;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerExitVehicle" /> callback is being called.
        ///     This callback is called when a player exits a vehicle.
        /// </summary>
        /// <remarks>
        ///     Not called if the player falls off a bike or is removed from a vehicle by other means such as using
        ///     <see cref="Native.SetPlayerPos(int,Vector)" />.
        /// </remarks>
        public event EventHandler<PlayerVehicleEventArgs> PlayerExitVehicle;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerStateChange" /> callback is being called.
        ///     This callback is called when a player exits a vehicle.
        /// </summary>
        /// <remarks>
        ///     Not called if the player falls off a bike or is removed from a vehicle by other means such as using
        ///     <see cref="Native.SetPlayerPos(int,Vector)" />.
        /// </remarks>
        public event EventHandler<PlayerStateEventArgs> PlayerStateChanged;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerEnterCheckpoint" /> callback is being called.
        ///     This callback is called when a player enters the checkpoint set for that player.
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerEnterCheckpoint;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerLeaveCheckpoint" /> callback is being called.
        ///     This callback is called when a player leaves the checkpoint set for that player.
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerLeaveCheckpoint;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerEnterRaceCheckpoint" /> callback is being called.
        ///     This callback is called when a player enters a race checkpoint.
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerEnterRaceCheckpoint;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerLeaveRaceCheckpoint" /> callback is being called.
        ///     This callback is called when a player leaves the race checkpoint.
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerLeaveRaceCheckpoint;

        /// <summary>
        ///     Occurs when the <see cref="OnRconCommand" /> callback is being called.
        ///     This callback is called when a command is sent through the server console, remote RCON, or via the in-game /rcon
        ///     command.
        /// </summary>
        public event EventHandler<RconEventArgs> RconCommand;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerRequestSpawn" /> callback is being called.
        ///     Called when a player attempts to spawn via class selection.
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerRequestSpawn;

        /// <summary>
        ///     Occurs when the <see cref="OnObjectMoved" /> callback is being called.
        ///     This callback is called when an object is moved after <see cref="Native.MoveObject" /> (when it stops moving).
        /// </summary>
        public event EventHandler<ObjectEventArgs> ObjectMoved;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerObjectMoved" /> callback is being called.
        ///     This callback is called when a player object is moved after <see cref="Native.MovePlayerObject" /> (when it stops
        ///     moving).
        /// </summary>
        public event EventHandler<PlayerObjectEventArgs> PlayerObjectMoved;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerPickUpPickup" /> callback is being called.
        ///     Called when a player picks up a pickup created with <see cref="Native.CreatePickup" />.
        /// </summary>
        public event EventHandler<PlayerPickupEventArgs> PlayerPickUpPickup;

        /// <summary>
        ///     Occurs when the <see cref="OnVehicleMod" /> callback is being called.
        ///     This callback is called when a vehicle is modded.
        /// </summary>
        /// <remarks>
        ///     This callback is not called by <see cref="Native.AddVehicleComponent" />.
        /// </remarks>
        public event EventHandler<VehicleModEventArgs> VehicleMod;

        /// <summary>
        ///     Occurs when the <see cref="OnEnterExitModShop" /> callback is being called.
        ///     This callback is called when a player enters or exits a mod shop.
        /// </summary>
        public event EventHandler<PlayerEnterModShopEventArgs> PlayerEnterExitModShop;

        /// <summary>
        ///     Occurs when the <see cref="OnVehiclePaintjob" /> callback is being called.
        ///     Called when a player changes the paintjob of their vehicle (in a modshop).
        /// </summary>
        public event EventHandler<VehiclePaintjobEventArgs> VehiclePaintjobApplied;

        /// <summary>
        ///     Occurs when the <see cref="OnVehicleRespray" /> callback is being called.
        ///     The callback name is deceptive, this callback is called when a player exits a mod shop, regardless of whether the
        ///     vehicle's colors were changed, and is NEVER called for pay 'n' spray garages.
        /// </summary>
        /// <remarks>
        ///     Misleadingly, this callback is not called for pay 'n' spray (only modshops).
        /// </remarks>
        public event EventHandler<VehicleResprayedEventArgs> VehicleResprayed;

        /// <summary>
        ///     Occurs when the <see cref="OnVehicleDamageStatusUpdate" /> callback is being called.
        ///     This callback is called when a vehicle element such as doors, tires, panels, or lights get damaged.
        /// </summary>
        /// <remarks>
        ///     This does not include vehicle health changes.
        /// </remarks>
        public event EventHandler<PlayerVehicleEventArgs> VehicleDamageStatusUpdated;

        /// <summary>
        ///     Occurs when the <see cref="OnUnoccupiedVehicleUpdate" /> callback is being called.
        ///     This callback is called everytime an unoccupied vehicle updates the server with their status.
        /// </summary>
        /// <remarks>
        ///     This callback is called very frequently per second per unoccupied vehicle. You should refrain from implementing
        ///     intensive calculations or intensive file writing/reading operations in this callback.
        /// </remarks>
        public event EventHandler<UnoccupiedVehicleEventArgs> UnoccupiedVehicleUpdated;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerSelectedMenuRow" /> callback is being called.
        ///     This callback is called when a player selects an item from a menu.
        /// </summary>
        public event EventHandler<PlayerSelectedMenuRowEventArgs> PlayerSelectedMenuRow;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerExitedMenu" /> callback is being called.
        ///     Called when a player exits a menu.
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerExitedMenu;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerInteriorChange" /> callback is being called.
        ///     Called when a player changes interior.
        /// </summary>
        /// <remarks>
        ///     This is also called when <see cref="Native.SetPlayerInterior" /> is used.
        /// </remarks>
        public event EventHandler<PlayerInteriorChangedEventArgs> PlayerInteriorChanged;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerKeyStateChange" /> callback is being called.
        ///     This callback is called when the state of any supported key is changed (pressed/released). Directional keys do not
        ///     trigger this callback.
        /// </summary>
        public event EventHandler<PlayerKeyStateChangedEventArgs> PlayerKeyStateChanged;

        /// <summary>
        ///     Occurs when the <see cref="OnRconLoginAttempt" /> callback is being called.
        ///     This callback is called when someone tries to login to RCON, succesful or not.
        /// </summary>
        /// <remarks>
        ///     This callback is only called when /rcon login is used.
        /// </remarks>
        public event EventHandler<RconLoginAttemptEventArgs> RconLoginAttempt;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerUpdate" /> callback is being called.
        ///     This callback is called everytime a client/player updates the server with their status.
        /// </summary>
        /// <remarks>
        ///     This callback is called very frequently per second per player, only use it when you know what it's meant for.
        /// </remarks>
        public event EventHandler<PlayerEventArgs> PlayerUpdate;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerStreamIn" /> callback is being called.
        ///     This callback is called when a player is streamed by some other player's client.
        /// </summary>
        public event EventHandler<StreamPlayerEventArgs> PlayerStreamIn;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerStreamOut" /> callback is being called.
        ///     This callback is called when a player is streamed out from some other player's client.
        /// </summary>
        public event EventHandler<StreamPlayerEventArgs> PlayerStreamOut;

        /// <summary>
        ///     Occurs when the <see cref="OnVehicleStreamIn" /> callback is being called.
        ///     Called when a vehicle is streamed to a player's client.
        /// </summary>
        public event EventHandler<PlayerVehicleEventArgs> VehicleStreamIn;

        /// <summary>
        ///     Occurs when the <see cref="OnVehicleStreamOut" /> callback is being called.
        ///     This callback is called when a vehicle is streamed out from some player's client.
        /// </summary>
        public event EventHandler<PlayerVehicleEventArgs> VehicleStreamOut;

        /// <summary>
        ///     Occurs when the <see cref="OnDialogResponse" /> callback is being called.
        ///     This callback is called when a player responds to a dialog shown using <see cref="Native.ShowPlayerDialog" /> by
        ///     either clicking a button, pressing ENTER/ESC or double-clicking a list item (if using a list style dialog).
        /// </summary>
        public event EventHandler<DialogResponseEventArgs> DialogResponse;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerTakeDamage" /> callback is being called.
        ///     This callback is called when a player takes damage.
        /// </summary>
        public event EventHandler<PlayerDamageEventArgs> PlayerTakeDamage;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerGiveDamage" /> callback is being called.
        ///     This callback is called when a player gives damage to another player.
        /// </summary>
        /// <remarks>
        ///     One thing you can do with GiveDamage is detect when other players report that they have damaged a certain player,
        ///     and that player hasn't taken any health loss. You can flag those players as suspicious.
        ///     You can also set all players to the same team (so they don't take damage from other players) and process all health
        ///     loss from other players manually.
        ///     You might have a server where players get a wanted level if they attack Cop players (or some specific class). In
        ///     that case you might trust GiveDamage over TakeDamage.
        ///     There should be a lot you can do with it. You just have to keep in mind the levels of trust between clients. In
        ///     most cases it's better to trust the client who is being damaged to report their health/armour (TakeDamage). SA-MP
        ///     normally does this. GiveDamage provides some extra information which may be useful when you require a different
        ///     level of trust.
        /// </remarks>
        public event EventHandler<PlayerDamageEventArgs> PlayerGiveDamage;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerClickMap" /> callback is being called.
        ///     This callback is called when a player places a target/waypoint on the pause menu map (by right-clicking).
        /// </summary>
        /// <remarks>
        ///     The Z value provided is only an estimate; you may find it useful to use a plugin like the MapAndreas plugin to get
        ///     a more accurate Z coordinate (or for teleportation; use <see cref="Native.SetPlayerPosFindZ(int,Vector)" />).
        /// </remarks>
        public event EventHandler<PlayerClickMapEventArgs> PlayerClickMap;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerClickTextDraw" /> callback is being called.
        ///     This callback is called when a player clicks on a textdraw or cancels the select mode(ESC).
        /// </summary>
        /// <remarks>
        ///     The clickable area is defined by <see cref="Native.TextDrawTextSize" />. The x and y parameters passed to that
        ///     function must not be zero or negative.
        /// </remarks>
        public event EventHandler<PlayerClickTextDrawEventArgs> PlayerClickTextDraw;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerClickPlayerTextDraw" /> callback is being called.
        ///     This callback is called when a player clicks on a player-textdraw. It is not called when player cancels the select
        ///     mode (ESC) - however, <see cref="OnPlayerClickTextDraw" /> is.
        /// </summary>
        public event EventHandler<PlayerClickTextDrawEventArgs> PlayerClickPlayerTextDraw;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerClickPlayer" /> callback is being called.
        ///     Called when a player double-clicks on a player on the scoreboard.
        /// </summary>
        /// <remarks>
        ///     There is currently only one 'source' (<see cref="PlayerClickSource.Scoreboard" />). The existence of this argument
        ///     suggests that more sources may be supported in the future.
        /// </remarks>
        public event EventHandler<PlayerClickPlayerEventArgs> PlayerClickPlayer;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerEditObject" /> callback is being called.
        ///     This callback is called when a player ends object edition mode.
        /// </summary>
        public event EventHandler<PlayerEditObjectEventArgs> PlayerEditObject;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerEditAttachedObject" /> callback is being called.
        ///     This callback is called when a player ends attached object edition mode.
        /// </summary>
        /// <remarks>
        ///     Editions should be discarded if response was '0' (cancelled). This must be done by storing the offsets etc. in an
        ///     array BEFORE using EditAttachedObject.
        /// </remarks>
        public event EventHandler<PlayerEditAttachedObjectEventArgs> PlayerEditAttachedObject;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerSelectObject" /> callback is being called.
        ///     This callback is called when a player selects an object after <see cref="Native.SelectObject" /> has been used.
        /// </summary>
        public event EventHandler<PlayerSelectObjectEventArgs> PlayerSelectObject;

        /// <summary>
        ///     Occurs when the <see cref="OnPlayerWeaponShot" /> callback is being called.
        ///     This callback is called when a player fires a shot from a weapon.
        /// </summary>
        /// <remarks>
        ///     <see cref="BulletHitType.None" />: the fX, fY and fZ parameters are normal coordinates;
        ///     Others: the fX, fY and fZ are offsets from the center of hitid.
        /// </remarks>
        public event EventHandler<WeaponShotEventArgs> PlayerWeaponShot;

        /// <summary>
        ///     Occurs when the <see cref="OnIncomingConnection" /> callback is being called.
        ///     This callback is called when an IP address attempts a connection to the server.
        /// </summary>
        public event EventHandler<ConnectionEventArgs> IncomingConnection;

        /// <summary>
        ///     Occurs when the <see cref="OnTick" /> callback is being called.
        ///     This callback is called every tick(50 times per second).
        /// </summary>
        /// <remarks>
        ///     USE WITH CARE!
        /// </remarks>
        public event EventHandler<EventArgs> Tick;

        /// <summary>
        ///     Occurs when the <see cref="OnTimerTick" /> callback is being called.
        ///     This callback is called when a timer ticks.
        /// </summary>
        public event EventHandler<EventArgs> TimerTick;

        #endregion

        #region Callbacks

        /// <summary>
        ///     This callback is triggered when a timer ticks.
        /// </summary>
        /// <param name="timerid">The ID of the ticking timer.</param>
        /// <param name="args">The args object as parsed with <see cref="Native.SetTimer" />.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnTimerTick(int timerid, object args)
        {
            if (TimerTick != null)
                TimerTick(args, EventArgs.Empty);

            return true;
        }

        /// <summary>
        ///     This callback is triggered when the gamemode starts.
        /// </summary>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnGameModeInit()
        {
            var args = new GameModeEventArgs();

            if (Initialized != null)
                Initialized(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a gamemode ends.
        /// </summary>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnGameModeExit()
        {
            var args = new GameModeEventArgs();

            if (Exited != null)
                Exited(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player connects to the server.
        /// </summary>
        /// <param name="playerid">The ID of the player that connected.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerConnect(int playerid)
        {
            var args = new PlayerEventArgs(playerid);

            if (PlayerConnected != null)
                PlayerConnected(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player disconnects from the server.
        /// </summary>
        /// <param name="playerid">ID of the player that disconnected.</param>
        /// <param name="reason">The reason for the disconnection.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerDisconnect(int playerid, int reason)
        {
            var args = new PlayerDisconnectedEventArgs(playerid, (DisconnectReason) reason);

            if (PlayerDisconnected != null)
                PlayerDisconnected(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player spawns.
        /// </summary>
        /// <param name="playerid">The ID of the player that spawned.</param>
        /// <returns>Return False in this callback to force the player back to class selection when they next respawn.</returns>
        public virtual bool OnPlayerSpawn(int playerid)
        {
            var args = new PlayerEventArgs(playerid);

            if (PlayerSpawned != null)
                PlayerSpawned(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player dies.
        /// </summary>
        /// <param name="playerid">The ID of the player that died.</param>
        /// <param name="killerid">
        ///     The ID of the player that killed the player who died, or <see cref="Misc.InvalidPlayerId" /> if
        ///     there was none.
        /// </param>
        /// <param name="reason">The ID of the reason for the player's death.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerDeath(int playerid, int killerid, int reason)
        {
            var args = new PlayerDeathEventArgs(playerid, killerid, (Weapon) reason);

            if (PlayerDied != null)
                PlayerDied(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a vehicle respawns.
        /// </summary>
        /// <param name="vehicleid">The ID of the vehicle that spawned.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnVehicleSpawn(int vehicleid)
        {
            var args = new VehicleEventArgs(vehicleid);

            if (VehicleSpawned != null)
                VehicleSpawned(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a vehicle is destroyed - either by exploding or becoming submerged in water.
        /// </summary>
        /// <remarks>
        ///     This callback will also be called when a vehicle enters water, but the vehicle can be saved from destruction by
        ///     teleportation or driving out (if only partially submerged). The callback won't be called a second time, and the
        ///     vehicle may disappear when the driver exits, or after a short time.
        /// </remarks>
        /// <param name="vehicleid">The ID of the vehicle that was destroyed.</param>
        /// <param name="killerid">
        ///     The ID of the player that reported (synced) the vehicle's destruction (name is misleading).
        ///     Generally the driver or a passenger (if any) or the closest player.
        /// </param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnVehicleDeath(int vehicleid, int killerid)
        {
            var args = new PlayerVehicleEventArgs(killerid, vehicleid);

            if (VehicleDied != null)
                VehicleDied(this, args);

            return args.Success;
        }

        /// <summary>
        ///     Called when a player sends a chat message.
        /// </summary>
        /// <param name="playerid">The ID of the player who typed the text.</param>
        /// <param name="text">The text the player typed.</param>
        /// <returns>Returning False in this callback will stop the text from being sent.</returns>
        public virtual bool OnPlayerText(int playerid, string text)
        {
            var args = new PlayerTextEventArgs(playerid, text);

            if (PlayerText != null)
                PlayerText(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player enters a command into the client chat window, e.g. /help.
        /// </summary>
        /// <param name="playerid">The ID of the player that executed the command.</param>
        /// <param name="cmdtext">The command that was executed (including the slash).</param>
        /// <returns>False if the command was not processed, otherwise True.</returns>
        public virtual bool OnPlayerCommandText(int playerid, string cmdtext)
        {
            var args = new PlayerTextEventArgs(playerid, cmdtext) {Success = false};

            if (PlayerCommandText != null)
                PlayerCommandText(this, args);

            return args.Success;
        }

        /// <summary>
        ///     Called when a player changes class at class selection (and when class selection first appears).
        /// </summary>
        /// <param name="playerid">The ID of the player that changed class.</param>
        /// <param name="classid">The ID of the current class being viewed.</param>
        /// <returns>
        ///     Returning False in this callback will prevent the player from spawning. The player can be forced to spawn when
        ///     <see cref="Native.SpawnPlayer" /> is used, however the player will re-enter class selection the next time they die.
        /// </returns>
        public virtual bool OnPlayerRequestClass(int playerid, int classid)
        {
            var args = new PlayerRequestClassEventArgs(playerid, classid);

            if (PlayerRequestClass != null)
                PlayerRequestClass(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player starts to enter a vehicle, meaning the player is not in vehicle yet at the
        ///     time this callback is called.
        /// </summary>
        /// <param name="playerid">ID of the player who attempts to enter a vehicle.</param>
        /// <param name="vehicleid">ID of the vehicle the player is attempting to enter.</param>
        /// <param name="ispassenger">False if entering as driver. True if entering as passenger.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerEnterVehicle(int playerid, int vehicleid, bool ispassenger)
        {
            var args = new PlayerEnterVehicleEventArgs(playerid, vehicleid, ispassenger);

            if (PlayerEnterVehicle != null)
                PlayerEnterVehicle(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player exits a vehicle.
        /// </summary>
        /// <remarks>
        ///     Not called if the player falls off a bike or is removed from a vehicle by other means such as using
        ///     <see cref="Native.SetPlayerPos(int,Vector)" />.
        /// </remarks>
        /// <param name="playerid">The ID of the player who exited the vehicle.</param>
        /// <param name="vehicleid">The ID of the vehicle the player is exiting.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerExitVehicle(int playerid, int vehicleid)
        {
            var args = new PlayerVehicleEventArgs(playerid, vehicleid);

            if (PlayerExitVehicle != null)
                PlayerExitVehicle(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player exits a vehicle.
        /// </summary>
        /// <remarks>
        ///     Not called if the player falls off a bike or is removed from a vehicle by other means such as using
        ///     <see cref="Native.SetPlayerPos(int,Vector)" />.
        /// </remarks>
        /// <param name="playerid">The ID of the player that changed state.</param>
        /// <param name="newstate">The player's new state.</param>
        /// <param name="oldstate">The player's previous state.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerStateChange(int playerid, int newstate, int oldstate)
        {
            var args = new PlayerStateEventArgs(playerid, (PlayerState) newstate, (PlayerState) oldstate);

            if (PlayerStateChanged != null)
                PlayerStateChanged(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player enters the checkpoint set for that player.
        /// </summary>
        /// <param name="playerid">The player who entered the checkpoint.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerEnterCheckpoint(int playerid)
        {
            var args = new PlayerEventArgs(playerid);

            if (PlayerEnterCheckpoint != null)
                PlayerEnterCheckpoint(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player leaves the checkpoint set for that player.
        /// </summary>
        /// <param name="playerid">The player who left the checkpoint.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerLeaveCheckpoint(int playerid)
        {
            var args = new PlayerEventArgs(playerid);

            if (PlayerLeaveCheckpoint != null)
                PlayerLeaveCheckpoint(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player enters a race checkpoint.
        /// </summary>
        /// <param name="playerid">The ID of the player who entered the race checkpoint.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerEnterRaceCheckpoint(int playerid)
        {
            var args = new PlayerEventArgs(playerid);

            if (PlayerEnterRaceCheckpoint != null)
                PlayerEnterRaceCheckpoint(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player leaves the race checkpoint.
        /// </summary>
        /// <param name="playerid">The player who left the race checkpoint.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerLeaveRaceCheckpoint(int playerid)
        {
            var args = new PlayerEventArgs(playerid);

            if (PlayerLeaveRaceCheckpoint != null)
                PlayerLeaveRaceCheckpoint(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a command is sent through the server console, remote RCON, or via the in-game /rcon
        ///     command.
        /// </summary>
        /// <param name="command">A string containing the command that was typed, as well as any passed parameters.</param>
        /// <returns>
        ///     False if the command was not processed, it will be passed to another script or True if the command was
        ///     processed, will not be passed to other scripts.
        /// </returns>
        public virtual bool OnRconCommand(string command)
        {
            var args = new RconEventArgs(command);

            if (RconCommand != null)
                RconCommand(this, args);

            return args.Success;
        }

        /// <summary>
        ///     Called when a player attempts to spawn via class selection.
        /// </summary>
        /// <param name="playerid">The ID of the player who requested to spawn.</param>
        /// <returns>Returning False in this callback will prevent the player from spawning.</returns>
        public virtual bool OnPlayerRequestSpawn(int playerid)
        {
            var args = new PlayerEventArgs(playerid);

            if (PlayerRequestSpawn != null)
                PlayerRequestSpawn(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when an object is moved after <see cref="Native.MoveObject" /> (when it stops moving).
        /// </summary>
        /// <remarks>
        ///     SetObjectPos does not work when used in this callback. To fix it, delete and re-create the object, or use a timer.
        /// </remarks>
        /// <param name="objectid">The ID of the object that was moved.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnObjectMoved(int objectid)
        {
            var args = new ObjectEventArgs(objectid);

            if (ObjectMoved != null)
                ObjectMoved(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player object is moved after <see cref="Native.MovePlayerObject" /> (when it stops
        ///     moving).
        /// </summary>
        /// <param name="playerid">The playerid the object is assigned to.</param>
        /// <param name="objectid">The ID of the player-object that was moved.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerObjectMoved(int playerid, int objectid)
        {
            var args = new PlayerObjectEventArgs(playerid, objectid);

            if (PlayerObjectMoved != null)
                PlayerObjectMoved(this, args);

            return args.Success;
        }

        /// <summary>
        ///     Called when a player picks up a pickup created with <see cref="Native.CreatePickup" />.
        /// </summary>
        /// <param name="playerid">The ID of the player that picked up the pickup.</param>
        /// <param name="pickupid">The ID of the pickup, returned by <see cref="Native.CreatePickup" />.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerPickUpPickup(int playerid, int pickupid)
        {
            var args = new PlayerPickupEventArgs(playerid, pickupid);

            if (PlayerPickUpPickup != null)
                PlayerPickUpPickup(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a vehicle is modded.
        /// </summary>
        /// <remarks>
        ///     This callback is not called by <see cref="Native.AddVehicleComponent" />.
        /// </remarks>
        /// <param name="playerid">The ID of the driver of the vehicle.</param>
        /// <param name="vehicleid">The ID of the vehicle which is modded.</param>
        /// <param name="componentid">The ID of the component which was added to the vehicle.</param>
        /// <returns>Return False to desync the mod (or an invalid mod) from propagating and / or crashing players.</returns>
        public virtual bool OnVehicleMod(int playerid, int vehicleid, int componentid)
        {
            var args = new VehicleModEventArgs(playerid, vehicleid, componentid);

            if (VehicleMod != null)
                VehicleMod(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player enters or exits a mod shop.
        /// </summary>
        /// <param name="playerid">The ID of the player that entered or exited the modshop.</param>
        /// <param name="enterexit">1 if the player entered or 0 if they exited.</param>
        /// <param name="interiorid">The interior ID of the modshop that the player is entering (or 0 if exiting).</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnEnterExitModShop(int playerid, int enterexit, int interiorid)
        {
            var args = new PlayerEnterModShopEventArgs(playerid, (EnterExit) enterexit, interiorid);

            if (PlayerEnterExitModShop != null)
                PlayerEnterExitModShop(this, args);

            return args.Success;
        }

        /// <summary>
        ///     Called when a player changes the paintjob of their vehicle (in a modshop).
        /// </summary>
        /// <param name="playerid">The ID of the player whos vehicle is modded.</param>
        /// <param name="vehicleid">The ID of the vehicle that changed paintjob.</param>
        /// <param name="paintjobid">The ID of the new paintjob.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnVehiclePaintjob(int playerid, int vehicleid, int paintjobid)
        {
            var args = new VehiclePaintjobEventArgs(playerid, vehicleid, paintjobid);

            if (VehiclePaintjobApplied != null)
                VehiclePaintjobApplied(this, args);

            return args.Success;
        }

        /// <summary>
        ///     The callback name is deceptive, this callback is called when a player exits a mod shop, regardless of whether the
        ///     vehicle's colors were changed, and is NEVER called for pay 'n' spray garages.
        /// </summary>
        /// <remarks>
        ///     Misleadingly, this callback is not called for pay 'n' spray (only modshops).
        /// </remarks>
        /// <param name="playerid">The ID of the player that is driving the vehicle.</param>
        /// <param name="vehicleid">The ID of the vehicle that was resprayed.</param>
        /// <param name="color1">The color that the vehicle's primary color was changed to.</param>
        /// <param name="color2">The color that the vehicle's secondary color was changed to.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnVehicleRespray(int playerid, int vehicleid, int color1, int color2)
        {
            var args = new VehicleResprayedEventArgs(playerid, vehicleid, color1, color2);

            if (VehicleResprayed != null)
                VehicleResprayed(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a vehicle element such as doors, tires, panels, or lights get damaged.
        /// </summary>
        /// <remarks>
        ///     This does not include vehicle health changes.
        /// </remarks>
        /// <param name="vehicleid">The ID of the vehicle that was damaged.</param>
        /// <param name="playerid">The ID of the player who synced the damage (who had the car damaged).</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnVehicleDamageStatusUpdate(int vehicleid, int playerid)
        {
            var args = new PlayerVehicleEventArgs(playerid, vehicleid);

            if (VehicleDamageStatusUpdated != null)
                VehicleDamageStatusUpdated(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called everytime an unoccupied vehicle updates the server with their status.
        /// </summary>
        /// <remarks>
        ///     This callback is called very frequently per second per unoccupied vehicle. You should refrain from implementing
        ///     intensive calculations or intensive file writing/reading operations in this callback.
        /// </remarks>
        /// <param name="vehicleid">The vehicleid that the callback is processing.</param>
        /// <param name="playerid">The playerid that the callback is processing (the playerid affecting the vehicle).</param>
        /// <param name="passengerSeat">The passenger seat of the playerid moving the vehicle. 0 if they're not in the vehicle.</param>
        /// <param name="newX">The new X coordinate of the vehicle.</param>
        /// <param name="newY">The new y coordinate of the vehicle.</param>
        /// <param name="newZ">The new z coordinate of the vehicle.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnUnoccupiedVehicleUpdate(int vehicleid, int playerid, int passengerSeat, float newX,
            float newY, float newZ)
        {
            var args = new UnoccupiedVehicleEventArgs(playerid, vehicleid, passengerSeat, new Vector(newX, newY, newZ));

            if (UnoccupiedVehicleUpdated != null)
                UnoccupiedVehicleUpdated(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player selects an item from a menu.
        /// </summary>
        /// <param name="playerid">The ID of the player that selected an item on the menu.</param>
        /// <param name="row">The row that was selected.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerSelectedMenuRow(int playerid, int row)
        {
            var args = new PlayerSelectedMenuRowEventArgs(playerid, row);

            if (PlayerSelectedMenuRow != null)
                PlayerSelectedMenuRow(this, args);

            return args.Success;
        }

        /// <summary>
        ///     Called when a player exits a menu.
        /// </summary>
        /// <param name="playerid">The ID of the player that exited the menu.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerExitedMenu(int playerid)
        {
            var args = new PlayerEventArgs(playerid);

            if (PlayerExitedMenu != null)
                PlayerExitedMenu(this, args);

            return args.Success;
        }

        /// <summary>
        ///     Called when a player changes interior.
        /// </summary>
        /// <remarks>
        ///     This is also called when <see cref="Native.SetPlayerInterior" /> is used.
        /// </remarks>
        /// <param name="playerid">The playerid who changed interior.</param>
        /// <param name="newinteriorid">The interior the player is now in.</param>
        /// <param name="oldinteriorid">The interior the player was in.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerInteriorChange(int playerid, int newinteriorid, int oldinteriorid)
        {
            var args = new PlayerInteriorChangedEventArgs(playerid, newinteriorid, oldinteriorid);

            if (PlayerInteriorChanged != null)
                PlayerInteriorChanged(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when the state of any supported key is changed (pressed/released). Directional keys do not
        ///     trigger this callback.
        /// </summary>
        /// <param name="playerid">ID of the player who pressed/released a key.</param>
        /// <param name="newkeys">A map of the keys currently held.</param>
        /// <param name="oldkeys">A map of the keys held prior to the current change.</param>
        /// <returns>
        ///     True - Allows this callback to be called in other scripts. False - Callback will not be called in other
        ///     scripts. It is always called first in gamemodes so returning False there blocks filterscripts from seeing it.
        /// </returns>
        public virtual bool OnPlayerKeyStateChange(int playerid, int newkeys, int oldkeys)
        {
            var args = new PlayerKeyStateChangedEventArgs(playerid, (Keys) newkeys, (Keys) oldkeys);

            if (PlayerKeyStateChanged != null)
                PlayerKeyStateChanged(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when someone tries to login to RCON, succesful or not.
        /// </summary>
        /// <remarks>
        ///     This callback is only called when /rcon login is used.
        /// </remarks>
        /// <param name="ip">The IP of the player that tried to login to RCON.</param>
        /// <param name="password">The password used to login with.</param>
        /// <param name="success">False if the password was incorrect or True if it was correct.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnRconLoginAttempt(string ip, string password, bool success)
        {
            var args = new RconLoginAttemptEventArgs(ip, password, success);

            if (RconLoginAttempt != null)
                RconLoginAttempt(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called everytime a client/player updates the server with their status.
        /// </summary>
        /// <remarks>
        ///     This callback is called very frequently per second per player, only use it when you know what it's meant for.
        /// </remarks>
        /// <param name="playerid">ID of the player sending an update packet.</param>
        /// <returns>False - Update from this player will not be replicated to other clients.</returns>
        public virtual bool OnPlayerUpdate(int playerid)
        {
            var args = new PlayerEventArgs(playerid);

            if (PlayerUpdate != null)
                PlayerUpdate(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player is streamed by some other player's client.
        /// </summary>
        /// <param name="playerid">The ID of the player who has been streamed.</param>
        /// <param name="forplayerid">The ID of the player that streamed the other player in.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerStreamIn(int playerid, int forplayerid)
        {
            var args = new StreamPlayerEventArgs(playerid, forplayerid);

            if (PlayerStreamIn != null)
                PlayerStreamIn(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player is streamed out from some other player's client.
        /// </summary>
        /// <param name="playerid">The player who has been destreamed.</param>
        /// <param name="forplayerid">The player who has destreamed the other player.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerStreamOut(int playerid, int forplayerid)
        {
            var args = new StreamPlayerEventArgs(playerid, forplayerid);

            if (PlayerStreamOut != null)
                PlayerStreamOut(this, args);

            return args.Success;
        }

        /// <summary>
        ///     Called when a vehicle is streamed to a player's client.
        /// </summary>
        /// <param name="vehicleid">The ID of the vehicle that streamed in for the player.</param>
        /// <param name="forplayerid">The ID of the player who the vehicle streamed in for.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnVehicleStreamIn(int vehicleid, int forplayerid)
        {
            var args = new PlayerVehicleEventArgs(forplayerid, vehicleid);

            if (VehicleStreamIn != null)
                VehicleStreamIn(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a vehicle is streamed out from some player's client.
        /// </summary>
        /// <param name="vehicleid">The ID of the vehicle that streamed out.</param>
        /// <param name="forplayerid">The ID of the player who is no longer streaming the vehicle.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnVehicleStreamOut(int vehicleid, int forplayerid)
        {
            var args = new PlayerVehicleEventArgs(forplayerid, vehicleid);

            if (VehicleStreamOut != null)
                VehicleStreamOut(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player responds to a dialog shown using <see cref="Native.ShowPlayerDialog" /> by
        ///     either clicking a button, pressing ENTER/ESC or double-clicking a list item (if using a list style dialog).
        /// </summary>
        /// <param name="playerid">The ID of the player that responded to the dialog.</param>
        /// <param name="dialogid">
        ///     The ID of the dialog the player responded to, assigned in <see cref="Native.ShowPlayerDialog" />
        ///     .
        /// </param>
        /// <param name="response">1 for left button and 0 for right button (if only one button shown, always 1).</param>
        /// <param name="listitem">
        ///     The ID of the list item selected by the player (starts at 0) (only if using a list style
        ///     dialog).
        /// </param>
        /// <param name="inputtext">The text entered into the input box by the player or the selected list item text.</param>
        /// <returns>
        ///     Returning False in this callback will pass the dialog to another script in case no matching code were found in
        ///     your gamemode's callback.
        /// </returns>
        public virtual bool OnDialogResponse(int playerid, int dialogid, int response, int listitem, string inputtext)
        {
            var args = new DialogResponseEventArgs(playerid, dialogid, response, listitem, inputtext);

            if (DialogResponse != null)
                DialogResponse(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player takes damage.
        /// </summary>
        /// <param name="playerid">The ID of the player that took damage.</param>
        /// <param name="issuerid">The ID of the player that caused the damage. INVALID_PLAYER_ID if self-inflicted.</param>
        /// <param name="amount">The amount of dagmage the player took (health and armour combined).</param>
        /// <param name="weaponid">The ID of the weapon/reason for the damage.</param>
        /// <param name="bodypart">The body part that was hit.</param>
        /// <returns>
        ///     True: Allows this callback to be called in other scripts. False Callback will not be called in other scripts.
        ///     It is always called first in gamemodes so returning False there blocks filterscripts from seeing it.
        /// </returns>
        public virtual bool OnPlayerTakeDamage(int playerid, int issuerid, float amount, int weaponid, int bodypart)
        {
            var args = new PlayerDamageEventArgs(playerid, issuerid, amount, (Weapon) weaponid, (BodyPart) bodypart);

            if (PlayerTakeDamage != null)
                PlayerTakeDamage(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player gives damage to another player.
        /// </summary>
        /// <remarks>
        ///     One thing you can do with GiveDamage is detect when other players report that they have damaged a certain player,
        ///     and that player hasn't taken any health loss. You can flag those players as suspicious.
        ///     You can also set all players to the same team (so they don't take damage from other players) and process all health
        ///     loss from other players manually.
        ///     You might have a server where players get a wanted level if they attack Cop players (or some specific class). In
        ///     that case you might trust GiveDamage over TakeDamage.
        ///     There should be a lot you can do with it. You just have to keep in mind the levels of trust between clients. In
        ///     most cases it's better to trust the client who is being damaged to report their health/armour (TakeDamage). SA-MP
        ///     normally does this. GiveDamage provides some extra information which may be useful when you require a different
        ///     level of trust.
        /// </remarks>
        /// <param name="playerid">The ID of the player that gave damage.</param>
        /// <param name="damagedid">The ID of the player that received damage.</param>
        /// <param name="amount">The amount of health/armour damagedid has lost (combined).</param>
        /// <param name="weaponid">The reason that caused the damage.</param>
        /// <param name="bodypart">The body part that was hit.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerGiveDamage(int playerid, int damagedid, float amount, int weaponid, int bodypart)
        {
            var args = new PlayerDamageEventArgs(playerid, damagedid, amount, (Weapon) weaponid, (BodyPart) bodypart);

            if (PlayerGiveDamage != null)
                PlayerGiveDamage(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player places a target/waypoint on the pause menu map (by right-clicking).
        /// </summary>
        /// <remarks>
        ///     The Z value provided is only an estimate; you may find it useful to use a plugin like the MapAndreas plugin to get
        ///     a more accurate Z coordinate (or for teleportation; use <see cref="Native.SetPlayerPosFindZ(int,Vector)" />).
        /// </remarks>
        /// <param name="playerid">The ID of the player that placed a target/waypoint.</param>
        /// <param name="fX">The X float coordinate where the player clicked.</param>
        /// <param name="fY">The Y float coordinate where the player clicked.</param>
        /// <param name="fZ">The Z float coordinate where the player clicked (inaccurate - see note below).</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerClickMap(int playerid, float fX, float fY, float fZ)
        {
            var args = new PlayerClickMapEventArgs(playerid, new Vector(fX, fY, fZ));

            if (PlayerClickMap != null)
                PlayerClickMap(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player clicks on a textdraw or cancels the select mode(ESC).
        /// </summary>
        /// <remarks>
        ///     The clickable area is defined by <see cref="Native.TextDrawTextSize" />. The x and y parameters passed to that
        ///     function must not be zero or negative.
        /// </remarks>
        /// <param name="playerid">The ID of the player that clicked on the textdraw.</param>
        /// <param name="clickedid">The ID of the clicked textdraw. INVALID_TEXT_DRAW if selection was cancelled.</param>
        /// <returns>
        ///     Returning True in this callback will prevent it being called in other scripts. This should be used to signal
        ///     that the textdraw on which they clicked was 'found' and no further processing is needed. You should return False if
        ///     the textdraw on which they clicked wasn't found.
        /// </returns>
        public virtual bool OnPlayerClickTextDraw(int playerid, int clickedid)
        {
            var args = new PlayerClickTextDrawEventArgs(playerid, clickedid);

            if (PlayerClickTextDraw != null)
                PlayerClickTextDraw(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player clicks on a player-textdraw. It is not called when player cancels the select
        ///     mode (ESC) - however, <see cref="OnPlayerClickTextDraw" /> is.
        /// </summary>
        /// <param name="playerid">The ID of the player that selected a textdraw.</param>
        /// <param name="playertextid">The ID of the player-textdraw that the player selected.</param>
        /// <returns>
        ///     Returning True in this callback will prevent it being called in other scripts. This should be used to signal
        ///     that the textdraw on which they clicked was 'found' and no further processing is needed. You should return False if
        ///     the textdraw on which they clicked wasn't found.
        /// </returns>
        public virtual bool OnPlayerClickPlayerTextDraw(int playerid, int playertextid)
        {
            var args = new PlayerClickTextDrawEventArgs(playerid, playertextid);

            if (PlayerClickPlayerTextDraw != null)
                PlayerClickPlayerTextDraw(this, args);

            return args.Success;
        }

        /// <summary>
        ///     Called when a player double-clicks on a player on the scoreboard.
        /// </summary>
        /// <remarks>
        ///     There is currently only one 'source' (0 - CLICK_SOURCE_SCOREBOARD). The existence of this argument suggests that
        ///     more sources may be supported in the future.
        /// </remarks>
        /// <param name="playerid">The ID of the player that clicked on a player on the scoreboard.</param>
        /// <param name="clickedplayerid">The ID of the player that was clicked on.</param>
        /// <param name="source">The source of the player's click.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerClickPlayer(int playerid, int clickedplayerid, int source)
        {
            var args = new PlayerClickPlayerEventArgs(playerid, clickedplayerid, (PlayerClickSource) source);

            if (PlayerClickPlayer != null)
                PlayerClickPlayer(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player ends object edition mode.
        /// </summary>
        /// <param name="playerid">The ID of the player that edited an object.</param>
        /// <param name="playerobject">0 if it is a global object or 1 if it is a playerobject.</param>
        /// <param name="objectid">The ID of the edited object.</param>
        /// <param name="response">The type of response.</param>
        /// <param name="fX">The X offset for the object that was edited.</param>
        /// <param name="fY">The Y offset for the object that was edited.</param>
        /// <param name="fZ">The Z offset for the object that was edited.</param>
        /// <param name="fRotX">The X rotation for the object that was edited.</param>
        /// <param name="fRotY">The Y rotation for the object that was edited.</param>
        /// <param name="fRotZ">The Z rotation for the object that was edited.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerEditObject(int playerid, bool playerobject, int objectid, int response, float fX,
            float fY,
            float fZ, float fRotX, float fRotY, float fRotZ)
        {
            var args = new PlayerEditObjectEventArgs(playerid,
                playerobject ? ObjectType.PlayerObject : ObjectType.GlobalObject, objectid,
                (EditObjectResponse) response,
                new Vector(fX, fY, fZ), new Vector(fRotX, fRotY, fRotZ));

            if (PlayerEditObject != null)
                PlayerEditObject(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player ends attached object edition mode.
        /// </summary>
        /// <remarks>
        ///     Editions should be discarded if response was '0' (cancelled). This must be done by storing the offsets etc. in an
        ///     array BEFORE using EditAttachedObject.
        /// </remarks>
        /// <param name="playerid">The ID of the player that ended edition mode.</param>
        /// <param name="response">0 if they cancelled (ESC) or 1 if they clicked the save icon.</param>
        /// <param name="index">Slot ID of the attached object that was edited.</param>
        /// <param name="modelid">The model of the attached object that was edited.</param>
        /// <param name="boneid">The bone of the attached object that was edited.</param>
        /// <param name="fOffsetX">The X offset for the attached object that was edited.</param>
        /// <param name="fOffsetY">The Y offset for the attached object that was edited.</param>
        /// <param name="fOffsetZ">The Z offset for the attached object that was edited.</param>
        /// <param name="fRotX">The X rotation for the attached object that was edited.</param>
        /// <param name="fRotY">The Y rotation for the attached object that was edited.</param>
        /// <param name="fRotZ">The Z rotation for the attached object that was edited.</param>
        /// <param name="fScaleX">The X scale for the attached object that was edited.</param>
        /// <param name="fScaleY">The Y scale for the attached object that was edited.</param>
        /// <param name="fScaleZ">The Z scale for the attached object that was edited.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerEditAttachedObject(int playerid, int response, int index, int modelid, int boneid,
            float fOffsetX, float fOffsetY, float fOffsetZ, float fRotX, float fRotY, float fRotZ, float fScaleX,
            float fScaleY, float fScaleZ)
        {
            var args = new PlayerEditAttachedObjectEventArgs(playerid, (EditObjectResponse) response, index, modelid,
                boneid, new Vector(fOffsetX, fOffsetY, fOffsetZ), new Vector(fRotX, fRotY, fRotZ),
                new Vector(fScaleX, fScaleY, fScaleZ));

            if (PlayerEditAttachedObject != null)
                PlayerEditAttachedObject(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player selects an object after <see cref="Native.SelectObject" /> has been used.
        /// </summary>
        /// <param name="playerid">The ID of the player that selected an object.</param>
        /// <param name="type">The type of selection.</param>
        /// <param name="objectid">The ID of the selected object.</param>
        /// <param name="modelid">The model of the selected object.</param>
        /// <param name="fX">The X position of the selected object.</param>
        /// <param name="fY">The Y position of the selected object.</param>
        /// <param name="fZ">The Z position of the selected object.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnPlayerSelectObject(int playerid, int type, int objectid, int modelid, float fX, float fY,
            float fZ)
        {
            var args = new PlayerSelectObjectEventArgs(playerid, (ObjectType) type, objectid, modelid,
                new Vector(fX, fY, fZ));

            if (PlayerSelectObject != null)
                PlayerSelectObject(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when a player fires a shot from a weapon.
        /// </summary>
        /// <remarks>
        ///     BULLET_HIT_TYPE_NONE: the fX, fY and fZ parameters are normal coordinates;
        ///     Others: the fX, fY and fZ are offsets from the center of hitid.
        /// </remarks>
        /// <param name="playerid">The ID of the player that shot a weapon.</param>
        /// <param name="weaponid">The ID of the weapon shot by the player.</param>
        /// <param name="hittype">The type of thing the shot hit (none, player, vehicle, or (player)object).</param>
        /// <param name="hitid">The ID of the player, vehicle or object that was hit.</param>
        /// <param name="fX">The X coordinate that the shot hit.</param>
        /// <param name="fY">The Y coordinate that the shot hit.</param>
        /// <param name="fZ">The Z coordinate that the shot hit.</param>
        /// <returns> False: Prevent the bullet from causing damage. True: Allow the bullet to cause damage.</returns>
        public virtual bool OnPlayerWeaponShot(int playerid, int weaponid, int hittype, int hitid, float fX, float fY,
            float fZ)
        {
            var args = new WeaponShotEventArgs(playerid, (Weapon) weaponid, (BulletHitType) hittype, hitid,
                new Vector(fX, fY, fZ));

            if (PlayerWeaponShot != null)
                PlayerWeaponShot(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called when an IP address attempts a connection to the server.
        /// </summary>
        /// <param name="playerid">The ID of the player attempting to connect.</param>
        /// <param name="ipAddress">The IP address of the player attempting to connect.</param>
        /// <param name="port">The port of the attempted connection.</param>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnIncomingConnection(int playerid, string ipAddress, int port)
        {
            var args = new ConnectionEventArgs(playerid, ipAddress, port);

            if (IncomingConnection != null)
                IncomingConnection(this, args);

            return args.Success;
        }

        /// <summary>
        ///     This callback is called every tick.
        /// </summary>
        /// <returns>This callback does not handle returns.</returns>
        public virtual bool OnTick()
        {
            if (Tick != null)
                Tick(this, EventArgs.Empty);

            return true;
        }

        #endregion

        public void Dispose()
        {
            //Todo
        }
    }
}