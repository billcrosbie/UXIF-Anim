See below for instructions on how to explore the project and see how the animation setup / system works

ui_mainMenu
--Run the scene and select this object and flip the "on" bool parameter on and off in the Animator Window to see the menu turn on  / off
--Select the object in the scene and look at the UI Anim Helper script inspector to see all the messages setup on this animator that talk to its children

kit_popup
--Run the scene and the "window" object to turn the animator on / off
--you can see the messages to the overlay object from the "window" object's UI Anim Helper script in the inspector

ui_cardEvolve
--Run the scene, select this object, and set the "on" bool parameter to true in the animator window
--from there you can manually change the "card selected" and "resourceSelected" bool parameters in the editor
--OR you can click a card in the left panel and then the right panel, there is a dummy script setup to set the parameters accordingly in code
--also if you click the "+" a handful of times the dummy script will detect the cost is too great and set the button to disabled,
which triggers the warning bubble message to turn on
--Select the ui_cardEvolve object in the scene and look at the UI Anim Helper inspector to see the animator messages it has setup to control its children


Prefabs
--Drag any of the kit prefabs from the Prefabs > Kit folder into the scene under the canvas and explore its animator parameters to see how it works

--kit_tab shows an example of an override of the kit_btn controller as well as messages to control the visibility of the ribbon based on the selected parameter

item_cardEntry
--the cards in the left panel of the card Evolve menu show an example of toggling on a child object when selected (using messages from the selected layer)

item_resourceEntry
--the cards in the right panel of the card Evolve menu show an example of changing the visual state of an object when locked (using the locked layer and animator messages)


if you have any specific questions, feel free to reach out to me neil@uxisfine.com