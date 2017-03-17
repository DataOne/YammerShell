# New-YmGroup

Creates a new group in yammer. Returns the ID of the new group.

## Syntax

```PowerShell
New-YmGroup [-Name] <String> [-Description <String>] [-Private]
```

## Returns
> System.Int32


## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
Name      | String | true   | The name of the new group
Description | String | false | The description of the new group.
Private   | SwitchParameter | false | Whether the new group should be private.


## Examples

### Example 1

```PowerShell
PS:> $groupId = New-YmGroup 'Business Administration' -Description 'This is a group for the team Business Administration!'
```
Creates a new group with the name _Business Administration_ and a description. The ID gets saved in $groupId.

### Example 2

```PowerShell
PS:> New-YmGroup 'YammerShell Test Group' -Private
```
Creates a new _private_ group with the name _YammerShell Test Group_.