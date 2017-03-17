Import-Module '.\YammerShell.dll'

Set-YmToken abc123-def456ghi789

# crete a new group
$groupId = New-YmGroup 'Marketing' -Description 'Group of the department Marketing'

# create a new user
$userId = New-YmUser -Email 'user@company.com' -FullName 'Max Power' -DepartmentName 'Marketing'

# add user to group
Add-YmGroupMembership -GroupId $groupId -UserId $userId

# post a new message
New-YmMessage 'Welcome to Yammer!'

Get-YmGroup | ForEach-Object {
    # list all groups
    Write-Host 'All users for group:' + $_.Name

    # list all users of a group
    Get-YmUser -GroupId $_.Id
}

# list all messages
Get-YmMessage