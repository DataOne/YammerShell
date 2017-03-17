# Remove-YmGroupMembership


## Syntax

```PowerShell
Remove-YmGroupMembership [-GroupId] <int> [-UserId <int>]
```

## Returns
> System.Int32

## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
GroupId   | int  | true     | The ID of the group where a user gets removed from
UserId    | int  | false    | The ID of the user to leave the group


## Examples

### Example 1

```PowerShell
PS:> Remove-YmGroupMembership -GroupId 1234567
```
Removes the currently authenticated user from the group with the ID 1234567.

### Example 2

```PowerShell
PS:> Remove-YmGroupMembership 1234567 -UserId 2345678901
```
Removes the user with the ID 2345678901 from the group with the ID 1234567.