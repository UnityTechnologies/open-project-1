#Overview
Menu Controller class consumes input from inputreader
- opens / closes menu

MenuSelectionHandler class acts as manager UI selection logic
- it consumes events provided by the InputReader class
- adds custom selection and component caching behaviour 
- must be present on the same transform as the SelectableUI or CC_Button components it controls

SelectableUIELement send messages to the MenuSelectionHandler when a mouse is over them or selection has been moved onto them
- they have events for mouse enter and exit
- they have events for on selection
- they find the MenuSelectionHandler component by checking the root transform for children with the MenuSelectionHandler class (can be refactored later)
- does not handle submit events (must use CC_Button if you'd like submit behaviour)

CC_Button (chop chop button) implements the same methods as SelectableUIElement, however it also inherits from UnityEngine.UI.Button implements an OnSubmitMethod which queries whether it should call base.OnSubmit() or stop the interaction early.  Used for our custom menu input handling to prevent an edgecase where mouse clicks can call submit on the wrong UI element.

## Setup

- add MenuSystem prefab to scene if not already present
    - assign references if they are not assigned
- hit escape to open and close menu
