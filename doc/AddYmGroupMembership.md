# Add-YmGroupMembership

Adds a user to a group specified by ID or email. If both is omitted the user that is currently authenticated will be added to the group

## Syntax

```PowerShell
Add-YmGroupMembership [-GroupId] <int> [-UserId <int>]
```

```PowerShell
Add-YmGroupMembership [-GroupId] <int> [-Email <String>]
```

## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
GroupId   | int  | true     | The ID of the group where to add the user.
UserId    | int  | false    | The ID of the user to add.
Email     | String | false  | The email of the user to add. If the email does not correspond to an existing user then the user will be invited to join the network (if you are network admin).


## Examples

### Example 1

```PowerShell
PS:> Add-YmGroupMembership -GroupId 1234567
```
Adds the currently authenticated user to the group with the ID 1234567

### Example 2

```PowerShell
PS:> Add-YmGroupMembership 1234567 -UserId 1213141516
```
Adds the user with the ID 1213141516 to the group with the ID 1234567

### Example 3

```PowerShell
PS:> Add-YmGroupMembership 1234567 -Email user@company.com
```
Adds the user with the email _user@company.com_ to the group with the ID 1234567 or sends an invitation if user is not yet registered on yammer.