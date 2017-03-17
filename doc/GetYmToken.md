# Get-YmToken

Navigates you through the pages which are needed to **create a new application** and to get the bearer token.
Then it sets the received token. If you already registered an app you can set its token via [Set-YmToken](SetYmToken.md).

## Syntax

```PowerShell
Get-YmToken
```

## Examples

### Example 1

```PowerShell
PS:> Get-YmToken
```

This first opens the page to register an app in yammer.  
Then it lets you paste the _client id_ and the _client secret_.  
Next you get redirected and have to paste the _secret_ you will receive there.  
Now the bearer token gets automatically requested and set for this session.  
It is recommended to _save this token_ for next PowerShell sessions where you can set it with [Set-YmToken](SetYmToken.md).