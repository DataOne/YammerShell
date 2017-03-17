# Get-YmNetwork

Returns a list of networks to which the current user has access to.


## Syntax

```PowerShell
Get-YmNetwork [[-Id] <int>]
```

## Returns
> YammerShell.YammerObjects.YammerNetwork

## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
Id        | int  | false    | The ID to filter the networks after.


## Examples

### Example 1

```PowerShell
PS:> Get-YmNetwork
```
Returns all networks to which the currently authenticated user has access to.

### Example 2

```PowerShell
PS:> Get-YmNetwork 495653
```
Return the network with the ID 495653 if the authenticated user as access to it.