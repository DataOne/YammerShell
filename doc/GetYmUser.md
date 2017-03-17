# Get-YmUser

Returns all users in the user's Yammer network or a specific user.

## Syntax

```PowerShell
Get-YmUser [-Id] <int>
```

```PowerShell
Get-YmUser -Current
```

```PowerShell
Get-YmUser -GroupId <int> [-Limit <int>] [-Reverse]
```

```PowerShell
Get-YmUser -Email <String> [-Limit <int>] [-Reverse]
```

```PowerShell
Get-YmUser -StartLetter <Char> [-Limit <int>] [-Reverse]
```

## Returns
> YammerShell.YammerObjects.YammerUser


## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
Id        | int  | true     | The ID of a user to return
Current   | SwitchParameter | true | Returns the currently authenticated user if set
Limit     | int  | false    | The Maximum of returned users. Accepts values between 1 and 1000.
Reverse   | SwitchParameter | false | Returns the users in reverse alphabetical order
Email     | String | true   | The email of one or more profiles to return
StartLetter | Char | true   | The start letter of users to return



## Examples

### Example 1

```PowerShell
PS:> Get-YmUser -GroupId 1234567 -Limit 7
```
Returns the first seven users of the group with the ID 1234567

### Example 2

```PowerShell
PS:> Get-YmUser -Email 'user@company.com' -Reverse
```
Returns all user profiles with the email _user@company.com_ in reverse alphabetical order