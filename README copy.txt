#Gold
1. Restrict player from upgrading claw for more than 1 time.
2. Modify conveyor belt so that no left or right arrow will be shown. Add compost icons for the compost bin. Make the trash processor to spit the trash out if not sorted.
3. Modify Intro Menu so that the level selection panel is much more smaller. Spin the robot.
4. Remove Battery and Goal Bar. Replace Warning bar with 3 warning signs. Rearrange UI to make it less cluttered.
5. Modify tutorial to make it happens with events.
6. Add sound for bumping into things.

List of assets:
https://docs.google.com/spreadsheets/d/1nDwrej17rh-kuaAA5ipnH8yi4yncGrHYVwIS5nn62lk/edit?usp=sharing



#Beta
1. Fix bugs according to play testing feedback
2. Add trash sorting mechanic for Hard mode. Renew the conveyor belt. Enable player to choose an easy mode without the conveyor belt.
3. Adjust the price of tools in store
4. Add tips to all UI components.
5. Adjust control to eliminate the lagging of robot movement
6. Change game name to Sweeping Robot MOMO.
7. Enable player to earn coins from trash that need to be wiped.
8. Show store item list by default since many players did not notice the stuff in store.
9. Credit scene will be showed if player wins second level and did not choose to restart and exit in 5 seconds.



# Alpha
1. Add car horn when player collide with car
2. Enable different sounds to be played for collecting different trash
3. Change Time count down from text to a progress circle
4. Add Intro Menu
5. Add skybox
6. Adjust the littering rate of citizens. Citizens will throw trash at a faster rate as time passes.
7. Click on placed trash bin will show trash bin range circle along with move and delete button. Trash bins can now be moved and deleted with 0.8 of the original cost returned.
8. Add sound effect when claw hit citizens
9. Add particle effect for the area where warning is issued (for too much trash)
10. Add more trash type
11. Add trash processor and conveyor belt. Sending recyclable trash to non-recyclable processor will not get coin rewards.

Game Rule:
1. Earn 500 to win at Level 1. Earn 1000 to win at Level 2
2. Supervisor will conduct sanitation check every 100s. It will issue warning if there are too much trash in a region. 3 warnings cause game over. Robot is fired.
3. There are 3 types of trash:
	- Recyclable trash that by collection can earn coins
	- Trash that need wiping
	- Normal trash
4. Player need to send recyclable trash to recyclable trash processor
4. Tools can be bought at store to help collect trash




# POC
1. Build Level 1
2. Create 6 trash types (3 recyclable, 1 need wiping, 2 other)
3. Create 2 citizens
4. Implement Store/Inventory System. Add 3 trash bins. Add detergent (speed up wiping)
5. Add driving cars
6. Add animation for citizens and robot
7. Enable Player to move, wipe and shoot claw

# Prototype 1
1. Build Level 2
2. Add 3 more trash types (1 recyclable, 1 need wiping, 1 other)
4. Add 1 citizen - troublemaker
	- Throw trash at faster rate
	- Knock down trash bin at a random chance
	- Throw trash bomb at a random chance ( trash bomb explodes in 60s and release a random amount of trash (2-5) )
5. Add more items to the store
	- Upgrade Claw (Expand claw shoot range)
	- AI sweeper (collect trash and wipe automatically)
6. Add Robot Supervisor (start inspection every 200s, issue warning if the trash density of a region is too high, after 3 warnings, sweeping robot is fired, player loses the game)
7. Add win mechanics: player accomplish level 1 if he gain 500 coins; player accomplish level 2 if he gain 1000 coins
8. Add cheat code: Press C to rise to level 2
9. Press M to get a larger map

# Prototype 2
1. Add animation for robot wiping
2. Add Battery System. Player slows down if battery is low. Player speed up if battery is full. Player can charge battery within the range of a change station.
3. Add beginning cutscene for Level 1 and Level 2
4. Enable trash bin/AI sweeper to snap. Restrict the location where trash bin/AI weeper can be placed. Add a circle around the object to show range
5. Change music after game over
6. Add air wall to prevent player go out of bound

