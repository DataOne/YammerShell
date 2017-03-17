# Send-YmInvitation

Send an invitation to the yammer network to the specified emails.

## Syntax

```PowerShell
Send-YmInvitation [-Emails] <String[]>
```

## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
Emails    | String[] | true | The email addresses to send an invitation to.


## Examples

### Example 1

```PowerShell
PS:> Send-YmInvitation @('user1@example.com', 'user2@example.com')
```
Sends invitations to the emails _user1@example.com_ and _user2@example.com_.