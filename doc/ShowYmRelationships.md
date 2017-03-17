# Show-YmRelationships

## Syntax

```PowerShell
Show-YmRelationships [[-UserId] <int>]
```

## Returns
> YammerShell.YammerObjects.YammerRelationship

## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
UserId    | int  | false    | The ID of a user to show to relationships of


## Examples

### Example 1

```PowerShell
PS:> Show-YmRelationships
```
Return the relationships for the current authenticated user.

### Example 2

```PowerShell
PS:> Show-YmRelationships -UserId 1213141516
```
Return the relationships for the user with the ID 1213141516.