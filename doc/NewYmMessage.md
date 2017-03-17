# New-YmMessage

Posts a new message or announcement to the network or a group. Returns the id of that message.

## Syntax

```PowerShell
New-YmMessage [-Body] <String> [-GroupId <String>] [-AnnouncementTitle <String>]
```

```PowerShell
New-YmMessage [-Body] <String> -RepliedToId <int>
```

```PowerShell
New-YmMessage [-Body] <String> -DirectToUserIds <int[]>
```

## Returns
> System.Int32


## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
Body      | String | true   | The text body of the message.
GroupId   | int  | false    | The ID of a group where the message gets posted.
AnnouncementTitle | String | false | The title of the announcement. If this parameter is set the message will be of type announcement.
RepliedToId | int | false   | The ID of an other message to which the new message is a reply.
DirectToUserIds | int[] | false | IDs of users to send the message to. If this parameter is set the message will be private.


## Examples

### Example 1

```PowerShell
PS:> New-YmMessage 'Hello World!' -GroupId 1234567
```
Creates a new message in the group with the ID 1234567. Returns the ID of this message.

### Example 2

```PowerShell
PS:> New-YmMessage 'This is a private message!' -DirectToUserIds @(1234567890, 2345678901)
```
Sends a new private message to the users with the IDs 1234567890 and 2345678901.