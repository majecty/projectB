카드 Json Format
===============

## Card object
```
{
    "name": "/* card name */",
    "active_skill": { /* Skill Data */ },
    "passive_skill": { /* Skill Data */ }
}
```
## Skill Data
```
{
    "category": /* Skill category */,
    /* Skill parameters. */
}
```
## Skill categories

### Active Skill
 * Attack
 * Defense
 * Recover
 * Buffer

### Passive Skill
 * Critical
 * 

## Skill parameters
### Attack
```
{ 
    "category": "Attack",
    "cost": /* number */,
    "power": /* number */,
    "accuracy": /* number */,
    "type": /* string. 속성 이름 */,
    "condition": /* string. 상태이상 이름? 오브젝트? */, 
    "drain": /* number. heal amount */,
    "delay": /* number. turn count */
}
```

### Defense
```
{
    "category": "Defense",
    "cost": /* number */,
    "ratio": /* number, damage reduce ratio */
    "amount": /* number, damage reduce amount */
}
```

### Recover
```
{
    "category": "Recover",
    "cost": /* number */,
    "amount": /* number 회복량 */,
    "duration": /* number 회복 지속시간 */,
    "type": /* string 회복 가능 상태이상 */,
}
```

### Buff
```
{
    "category": "Buff",
    "cost": /* number */,
    "type": /* 버프 종류 */,
}
```

### Critical
```
{
    "category": "Critical",
    "probability": /* number */,
    "multiplier": /* number */
}
```