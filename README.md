# Zombie-Land - FPS Survive Game 

![intro](https://user-images.githubusercontent.com/86935394/174500943-90bf80bf-7b24-448f-9f6a-a854962d765e.png)


## How AI works in this game 

- Physics Detection
  
  When bullet touchs zombie(red zone), Ai system turn into detection mode.
 ![physics](https://user-images.githubusercontent.com/86935394/174502583-15ce688c-0ede-4635-ae22-c590cc6fd076.png)

- Vision Detection

  Give AI a field of vision angle and a vision distance.
 ![vision](https://user-images.githubusercontent.com/86935394/174502925-415bd816-a100-46e2-9461-877abcaa47d0.png)

- Sound Detection

  Set up a trigger on player and when zombie in the trigger zone, it detects the player.
  
  Tigger size changes depending on walking/running, running makes more noises lead to bigger tigger size.
  
  ![sound d](https://user-images.githubusercontent.com/86935394/174503083-b9dee47b-ba7c-4a1b-9b1a-dc83a959c338.gif)
  

## AI States

- Patrol State
  - Once AI is generated, it starts to patrol
  - Set up a patrol radius for AI and generate a random point in the patrol radius
  - When AI is close enough to the point, generate a new random point again

- Chase State
  - Once AI aware the player, it starts to chase
  - aware-> in the vision zone, being attack and hear the player near by

- Attack State
  - Once AI in the attack range, it starts to attack
  - if not in the attack range, go back to chase state

- Manic State
  - AI skips patrol state and chase the player to the death untill condition breaks

## Support AI System

- Detecting
  - Once AI Detects the player, it remains detecting and chase the player
  - Once AI lose tracking vision of the player, the system trigger and countdown timer, but at this time AI still chase the player
  - If timer goes to 0, AI lose the player and go back to the patrol state

## Error Handling

- Repath System
  - Help AI find a new patrol path if AI stucks for a few seconds

- SelfDestroy System
  - Destroy AI itself after a long time stuck.
  
  


