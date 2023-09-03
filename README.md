# SubclassMod
A game modification for SCP-Secret Laboratory based on the EXILED framework. This modification will allow you to create your own subclasses and customize existing roles. __Required version of EXILED for modification: 8.0.1__

## Features
- Setting up custom subclasses/private player's characters.
- Customize basic game roles.
- Creating roleplay names with required type of naming like: Signs/Firstname/Full (Firstname + Secondname)
- Comfortable commands for interacting with plugin logic

## Commands
``.force <playerId> <subclassId>`` - Forcing player as selected subclass. __[RA COMMAND]__
<br>
``.character-select <characterId>`` - Forcing yourself as your character. Your steamid must be presented in character's config to spawn.

## Configs setup guide
### Customization of basic game role
```yml
    ClassD:
    # Prefix that will be placed before nickname
      name_prefix: ''
      # Postfix that will be placed after nickname
      name_postfix: ''
      # Custom info of overridden role
      custom_info: 'Class D personnel'
      # Is inventory overridden
      inventory_overridden: false
      # Naming method for this role
      naming_method: Signs 
      # Overwritten items list
      inventory_overwrite: []
      # Add any EXILED custom items by id
      inventory_custom_items: []
```

### Creating your own subclass
```yml
  - id: 0 # Unique subclass ID
    max_players: 2 # Maximum players allowed for that subclass
    health: 100 # Subclass health
    spawn_percent: 20 # Chance to spawn as this subclass
    name: 'Уборочный персонал класса D' # Name that will be appear when player spawn
    description: 'N/A' # Description that will be appear when player spawn
    name_prefix: '' # Name prefix
    name_postfix: '' # Name postfix
    custom_info: 'Уборщик класса D' # Subclassed player custom info
    forceclass_only: false # Disable automatic spawning as that subclass
    base_role: ClassD # The base role of the current subclass
    spawn_method: SpawnZone # Current spawn method
    naming_method: Signs # Current naming method
    ammo: # Subclass ammo pack
      Nato9: 0 
      Nato556: 0
      Nato762: 0
    custom_items: [] # List of subclass custom items
    items: # List of subclass items
    - KeycardJanitor
    spawn_zones: # List of zones allowed to spawn 
    - LightContainment
    spawn_rooms: [] # List of rooms allowed to spawn
    spawn_positions: [] # List of Vector3 position allowed to spawn
```

### Creating private player's character
```yml
  - id: 2 # Unique character's id
    scale: 1 # Player's model scale 
    name: 'Эдд' # Character DisplayName
    info: 'Мужчина | 30 лет | Крепкое телосложение | На плече большая, дорогая кожаная сумка | Брюнет, волосы короткие.' # Character CustomInfo
    base_role: Tutorial # Character base role
    spawn_zone: Entrance # Character spawn zone
    allowed_users: # Users that will be allowed to spawn as character
    - '76561199149088368@steam'
    inventory_override: # Character's inventory
    - Flashlight
    - Radio
    - KeycardContainmentEngineer
    - Coin
    inventory_custom_items: [] # Character's custom items
```
