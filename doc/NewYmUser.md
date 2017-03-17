# New-YmUser

Creates a new user. Current user should be a verified admin to perform this action. Returns the ID of the new user.


## Syntax

```PowerShell
New-YmUser [-Email] <String> [-FullName <String>] [-JobTitle <String>] [-DepartmentName <String>] [-Location <String>] [-WorkTelephone <String>]
```

## Returns
> System.Int32

## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
Email     | String | true   | The email address of the new user.
FullName  | String | false  | Pre- and surname of the new user separated by a space.
JobTitle  | String | false  | The job title of the new user.
DepartmentName | String | false | The name of the department of the new user.
Location | String | false | The location where the new user works at.
WorkTelephone | String | false | The work phone number of the new user. Must contain only numbers.


## Examples

### Example 1

```PowerShell
PS:> New-YmUser 'user@company.com' -FullName 'Test User' -DepartmentName 'Sales'
```
Creates a new user with the email _user@company.com_, the name _Test User_ and the department _Sales_. Returns the ID of the new user.