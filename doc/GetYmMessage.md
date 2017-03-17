# Get-YmMessage

Shows messages of the user's Yammer network.


## Syntax

```PowerShell
Get-YmMessage [-Id] <int>
```

```PowerShell
Get-YmMessage -Sent [-Limit <int>] [-OlderThan <int>]
```

```PowerShell
Get-YmMessage -Received [-Private] [-Limit <int>] [-OlderThan <int>]
```

```PowerShell
Get-YmMessage -Topic <int>
```

```PowerShell
Get-YmMessage -Top -Following
```

```PowerShell
Get-YmMessage -Top
```

```PowerShell
Get-YmMessage -Following
```

## Returns

> YammerShell.YammerObjects.YammerMessage
> YammerShell.YammerObjects.YammerMessage[]


## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
Id        | int  | false    | The ID of a message to get.
Sent      | SwitchParameter | false | Only sent messages get returned if set.
Limit     | int  | false    | The Maximum of returned messages.
OlderThan | int  | false    | The ID of a message. Specify this to return messages older than this message.
Received  | SwitchParameter | false | All received messages get returned if set.
Private   | SwitchParameter | false | Set this to receive only private messages.
Topic     | SwitchParameter | false | The ID of a specific topic.
Top       | SwitchParameter | false | Return top messages if set.
Following | SwitchParameter | false | Return messages you follow if set.


## Examples

### Example 1

```PowerShell
PS:> Get-YmMessage -Id 806862287
```
Return the message with the ID 806862287.

### Example 2

```PowerShell
PS:> Get-YmMessage -Received -Private -Limit 5
```
Return the first five received private messages of the authenticated user.

### Example 3

```PowerShell
PS:> Get-YmMessage -Sent -OlderThan 806862287
```
Return messages sent by the authenticated user older than the message with the ID 806862287

### Example 4

```PowerShell
PS:> Get-YmMessage -Topic 18629862
```
Return messages for the topic with the ID 18629862.

### Example 5

```PowerShell
PS:> Get-YmMessage -Top -Following
```
Return the authenticated user’s message feed, based on the selection made between _Following_ and _Top_ conversations.

### Example 6

```PowerShell
PS:> Get-YmMessage -Top
```
Return the algorithmic feed for the authenticated user that corresponds to _Top_ conversations. The Top conversations feed is the feed currently shown in the Yammer mobile apps.

### Example 7

```PowerShell
PS:> Get-YmMessage -Following
```
Return the _Following_ feed which is conversations involving people, groups and topics that the authenticated user is following.
