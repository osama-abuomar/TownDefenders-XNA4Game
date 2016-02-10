using System;
using System.Collections.Generic;
using Engine.Iso_Tile_Engine;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Components.GameLogic.Selection
{
    public class UnitSelectionManager : ISelectionManager
    {
        private Rectangle _mapViewingArea = new Rectangle(10,50,1350,570);
        private GameWorld _gameWorldRef;
        private bool _selectionEventHappened;
        bool _mouseLeftButtonClicked;
        bool _mouseLeftButtonDoubleClicked;
        bool _ctrlDown;
        bool _nothingIsCurrentlySelected = true;  // prove wrong
        public List<int> CurrentlySelectedGroups
        {
            get;
            set;
        }
        public List<ISelectable> CurrentlySelectesItems
        { get; set; }
        private readonly List<ISelectable> _currentFrameManagedItems;

        public UnitSelectionManager(GameWorld gameWorldRef)
        {
            _gameWorldRef = gameWorldRef;
            CurrentlySelectesItems = new List<ISelectable>();
            CurrentlySelectedGroups = new List<int>();
            _currentFrameManagedItems = new List<ISelectable>();
        }

        private void DeSelectAll()
        {
            foreach (var item in CurrentlySelectesItems)
            {
                deSelectItem(item);
            }
            CurrentlySelectesItems = new List<ISelectable>();
            CurrentlySelectedGroups = new List<int>();
        }

        private Point getMouseWorldPosition()
        {
            var v2ClickPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            var v2ClickWorldPosition = Camera.ScreenToWorld(v2ClickPosition);
            var clickWorldPosition = new Point((int)v2ClickWorldPosition.X, (int)v2ClickWorldPosition.Y);
            return clickWorldPosition;
        }

        private void selectItem(ISelectable item)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (!item.IsSelected)
            {
                item.Select();
            }
        }

        private void deSelectItem(ISelectable item)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (item.IsSelected)
            {
                item.DeSelect();
            }
        }

        private void flipSelectionState(ISelectable item)
        {
            if (item.IsSelected)
            {
                item.DeSelect();
            }
            else
            {
                item.Select();
            }
        }

        private void RemoveItemFromSelectedObjects(ISelectable item)
        {
            int id = item.Id;
            int index = -1;
            for (int i = 0; i < CurrentlySelectesItems.Count; i++)
            {
                if (CurrentlySelectesItems[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            // to debug
            if (index == -1) throw new Exception("item to delete not found in the list");
            deSelectItem(CurrentlySelectesItems[index]);
            CurrentlySelectesItems.RemoveAt(index);
        }

        public void Update(GameTime gameTime)
        {

            CurrentlySelectedGroups.Clear();

            if (_nothingIsCurrentlySelected && _selectionEventHappened)
            {
                var clickPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                if (_mapViewingArea.Contains(
                    (int) clickPosition.X, (int) clickPosition.Y))
                {
                    DeSelectAll();
                    Console.WriteLine("deSelectAll");
                }
            }

            _mouseLeftButtonClicked = InputManager.MouseLeftButtonClicked();
            _mouseLeftButtonDoubleClicked = InputManager.MouseLeftButtonDoubleClicked();
            if (_mouseLeftButtonDoubleClicked) _mouseLeftButtonClicked = false;
            _ctrlDown = InputManager.KeyDown(Keys.RightControl) || InputManager.KeyDown(Keys.LeftControl);
            _selectionEventHappened = _mouseLeftButtonClicked || _mouseLeftButtonDoubleClicked; // TODO or with right btn click when implemented


            _nothingIsCurrentlySelected = true;// for next frame

            _currentFrameManagedItems.Clear();



        }

        private void DeSelectAllExept(ISelectable unit)
        {
            int wantedIndex = -1;

            for (int i = 0; i < CurrentlySelectesItems.Count; i++)
            {
                if (CurrentlySelectesItems[i].Id == unit.Id)
                    wantedIndex = i;
            }
            var newList = new List<ISelectable>();
            newList.Add(CurrentlySelectesItems[wantedIndex]);

            CurrentlySelectesItems.RemoveAt(wantedIndex);

            DeSelectAll();

            CurrentlySelectesItems = newList;

        }

        private bool ItemClickedOn(ISelectable item)
        {
            Rectangle box = item.SelectBounds;
            return box.Contains(getMouseWorldPosition());
        }

        public bool SelectionHappened
        {
            get { return _selectionEventHappened; }
        }

        public void ManageSelection(ISelectable unit)
        {
            ///
            /// the calling object should know if there was even a mouse or any other input happened
            /// in this frame or not.
            ///
            if (_selectionEventHappened) // TODO and with if click position is within bounds of camera
            {
                ///
                /// this item is from now considered as managed
                /// the calling object may be needed if a double click occured
                /// if DC occured, then all previosly managed Selectable objects (stored inside currentFrameManagedItems list)
                /// in the same category as this one (the DClicked one) must be also selected
                /// (we don't need the currentFrameManagedItems list after that a DC happened)
                ///
                _currentFrameManagedItems.Add(unit);

                /// 
                /// the calling selectable item is the item that was clicked on
                ///
                if (ItemClickedOn(unit))
                {

                    ///
                    /// this flag is used to say: well, ok, this selectable object is clicked on. so now i am sure that 
                    /// the user didn't click on an empty area. (continuos in the update method)
                    ///
                    _nothingIsCurrentlySelected = false;

                    #region MouseLeftButtonClicked
                    if (_mouseLeftButtonClicked)
                    {
                        if (_ctrlDown)
                        {
                            flipSelectionState(unit);
                            if (unit.IsSelected) // became selected
                                CurrentlySelectesItems.Add(unit);
                            else
                                RemoveItemFromSelectedObjects(unit);
                        }
                        else
                        {
                            CurrentlySelectedGroups.Clear();
                            if (!unit.IsSelected)
                            {
                                DeSelectAll();
                                selectItem(unit);
                                CurrentlySelectesItems.Add(unit);
                            }
                            else if (unit.IsSelected)
                            {
                                DeSelectAllExept(unit);
                            }
                        }
                    }
                    #endregion

                    #region MouseLeftButtonDoubleClicked
                    if (_mouseLeftButtonDoubleClicked)
                    {
                        if (_ctrlDown)
                        {
                            CurrentlySelectedGroups.Add(unit.SelectionGroup);
                            // select all items in currentFrameManagedItems list
                            foreach (var item in _currentFrameManagedItems)
                            {
                                if (item.SelectionGroup == unit.SelectionGroup)
                                {
                                    selectItem(item);
                                    CurrentlySelectesItems.Add(item);
                                }
                            }
                            _currentFrameManagedItems.Clear();
                        }
                        else
                        {
                            DeSelectAll();
                            CurrentlySelectedGroups.Clear();
                            CurrentlySelectedGroups.Add(unit.SelectionGroup);
                            // select all items in currentFrameManagedItems list
                            foreach (var item in _currentFrameManagedItems)
                            {
                                if (item.SelectionGroup == unit.SelectionGroup)
                                {
                                    selectItem(item);
                                    CurrentlySelectesItems.Add(item);
                                }
                            }
                            _currentFrameManagedItems.Clear();
                        }
                    }
                    #endregion
                }

             

                if (IsWithinSelectedGroups(unit))
                {
                    ///
                    /// in the same frame update, a unit is DC on (multiple selection with same group number)
                    /// if this selectable lies under the same group, then automatically select it
                    ///
                    if (!unit.IsSelected)
                    {
                        selectItem(unit);
                        CurrentlySelectesItems.Add(unit);
                    }
                }
            }

        }

        private bool IsWithinSelectedGroups(ISelectable unit)
        {
            return
                CurrentlySelectedGroups.Contains(unit.SelectionGroup);
        }
    }
}
