using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects.Characters;
using Engine.Components.GameObjects.Structures.Buildings;
using Microsoft.Xna.Framework;

namespace Engine.Utilities.Controls.Panels.Dock.Manager
{
    public class DockManager
    {
        private readonly ControlManager _controlManager;
        private static readonly Vector2 UnifiedDockPosition
            = new Vector2(100,644);

        /// <summary>
        /// defining the possible docks (they are limited)
        /// </summary>
        private readonly CampDock _campDock;
        private readonly StableDock _stableDock;
        private readonly HouseDock _houseDock;
        private readonly FarmerDock _farmerDock;
        private readonly KnightDock _knightDock;
        private readonly SwordsmanDock _swordsmanDock;
        private readonly HorsemanDock _horsemanDock;

        private DisplayDock _currentDock;


        public DockManager(ControlManager controlManager)
        {
            _controlManager = controlManager;

            _campDock = new CampDock(UnifiedDockPosition, _controlManager);
            _stableDock = new StableDock(UnifiedDockPosition, _controlManager);
            _houseDock = new HouseDock(UnifiedDockPosition, _controlManager);
            _farmerDock = new FarmerDock(UnifiedDockPosition, _controlManager);
            _knightDock = new KnightDock(UnifiedDockPosition, _controlManager);
            _swordsmanDock = new SwordsmanDock(UnifiedDockPosition, _controlManager);
            _horsemanDock = new HorsemanDock(UnifiedDockPosition, _controlManager);

            _campDock.Hide();
            _stableDock.Hide();
            _horsemanDock.Hide();
            _farmerDock.Hide();
            _knightDock.Hide();
            _swordsmanDock.Hide();
            _houseDock.Hide();

            _currentDock = null;
        }

        public void HideCurrentDock()
        {
            if (_currentDock != null) _currentDock.Hide();
            _currentDock = null;
        }

        public void ShowStableDock(Stable context, GamePlayer player)
        {
            if (_currentDock != null) _currentDock.Hide();
            _stableDock.UpdateContext(context, player);
            _currentDock = _stableDock;
            _currentDock.Show();
        }

        public void ShowHouseDock(ResidentialHouse context, GamePlayer player)
        {
            if (_currentDock != null) _currentDock.Hide();
            _houseDock.UpdateContext(context, player);
            _currentDock = _houseDock;
            _currentDock.Show();
        }

        public void ShowFarmerDock(Farmer context, GamePlayer player)
        {
            if (_currentDock != null) _currentDock.Hide();
            _farmerDock.UpdateContext(context, player);
            _currentDock = _farmerDock;
            _currentDock.Show();
        }

        public void ShowKnightDock(Knight context, GamePlayer player)
        {
            if (_currentDock != null) _currentDock.Hide();
            _knightDock.UpdateContext(context, player);
            _currentDock = _knightDock;
            _currentDock.Show();
        }

        public void ShowSwordsmanDock(Swordsman context, GamePlayer player)
        {
            if (_currentDock != null) _currentDock.Hide();
            _swordsmanDock.UpdateContext(context, player);
            _currentDock = _swordsmanDock;
            _currentDock.Show();
        }

        public void ShowHorsemanDock(Horseman context, GamePlayer player)
        {
            if (_currentDock != null) _currentDock.Hide();
            _horsemanDock.UpdateContext(context, player);
            _currentDock = _horsemanDock;
            _currentDock.Show();
        }

        public void ShowCampDock(TrainingCamp context, GamePlayer player)
        {
            if (_currentDock != null) _currentDock.Hide();
            _campDock.UpdateContext(context, player);
            _currentDock = _campDock;
            _currentDock.Show();
        }
    }
}
