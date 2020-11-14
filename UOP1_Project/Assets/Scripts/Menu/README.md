#Overview
MenuInput class acts as manager for menu input logic
- it consumes events provided by the InputReader class
- adds custom selection and component caching behaviour 
- must be present on the same transform as the SelectableUI components it controls

SelectableUIELement send messages to the MenuInput when a mouse is over them
- they have events for mouse enter and exit
- they find the MenuInput component by checking the root transform for children with the MenuInput class (can be refactored later)

##TODO
- mouse click activates the currently selected UI element (even if the mouse is not over it)
- if the keyboard or gamepad navigates the selection away from a UI element which the mouse is over, you must move the mouse out of the game object's bounds and re-enter to select it again.