# Remove-YmMessage

Deletes a message per ID.

## Syntax

```PowerShell
Remove-YmMessage [-Id] <int>
```

## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
Id        | int  | true     | The ID of the message to remove.


## Examples

### Example 1

```PowerShell
PS:> Remove-YmMessage 123456789
```
Removes the message with the ID 123456789.