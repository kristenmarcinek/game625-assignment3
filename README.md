# Game Development II - Assignment 3

Worked on by Kristen Marcinek, Watkin Jones, and Alethea Saliba

Issue: We wanted to implement an almost procedurally generated forest, that appears when it is within the player's camera view, and disappears when the player is no longer looking in that direction.

Chosen Pattern: Object Pool. We chose this pattern, as there are a set number of game objects within a list or pool, which saves memory and processing power. 

While our implementation still needs some refinement, we currently have objects instantiated from the object pool when the player faces a direction, and returning to the object pool and re-instantiated elsewhere when they look away.
 
