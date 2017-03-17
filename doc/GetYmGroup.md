# Get-YmGroup

Returns all groups or a specific group selected by id.

## Syntax

```PowerShell
Get-YmGroup [[-Id] <int>]
```

## Returns

> YammerShell.YammerObjects.YammerGroup
> YammerShell.YammerObjects.YammerGroup[]


## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
Id        | int  | false    | The ID of a group to return.


## Examples

### Example 1

```PowerShell
PS:> Get-YmGroup
```
Returns all groups of the yammer network.

### Example 2

```PowerShell
PS:> Get-YmGroup 1234567
```
Return the group with the ID 1234567