# Add-YmRelationship

Add an org chart relationship for a user.

## Syntax

```PowerShell
Add-YmRelationship [-UserId <int>] [-Subordinates <String[]>] [-Superiors <String[]>] [-Colleagues <String[]>]
```

## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
UserId    | int  | false    | The ID of the user to add the relationship to. If not specified the currently authenticated user gets used.
Subordinates | String[] | false | An array of email addresses of yammer users that are subordinates of the user specified by ID
Superiors | String[] | false | An array of email addresses of yammer users that are superiors of the user specified by ID
Colleagues | String[] | false | An array of email addresses of yammer users that are colleagues of the user specified by ID

## Examples

### Example 1

```PowerShell
PS:> Add-YmRelationship -UserId 1213141516 -Subordinates @('user@company.com')
```
Create a relationship for the user with the ID 1213141516 where the user with the email _user@company.com_ is a subordinate.

### Example 2

```PowerShell
PS:> Add-YmRelationship -Superiors @('user1@company.com') -Colleagues @('user2@company.com', 'user3@company.com')
```
Create a relationship for the currently authenticated user where the user _user1@company.com_ is a superior and the users _user2@company.com_ and _user3@company.com_ are colleagues.