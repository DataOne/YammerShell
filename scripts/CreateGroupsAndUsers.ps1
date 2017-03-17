Import-Module '.\YammerShell.dll'

Set-YmToken abc123-def456ghi789

[xml]$employees =
'<xml>
  <User>
    <Email>foxmulder@company.com</Email>
    <FullName>Fox Mulder</FullName>
    <DepartmentName>Marketing</DepartmentName>
  </User>
  
  <User>
    <Email>frankblack@company.com</Email>
    <FullName>Frank Black</FullName>
    <DepartmentName>Sales</DepartmentName>
  </User>
  
  <User>
    <Email>johndoe@company.com</Email>
    <FullName>John Doe</FullName>
    <DepartmentName>Development</DepartmentName>
  </User>
</xml>'

$departments = @('Sales', 'Development', 'Marketing', 'Human Resources')
$groupIds = New-Object int[] $departments.Count

for($i=0; $i -lt $departments.Count; $i++){
    $groupIds[$i] = New-YmGroup -Name $departments[$i] -Description 'Group of the department: ' + $departments[$i]  # create a group for each department
}

for($i=0; $i -lt $employees.FirstChild.User.Count; $i++){
    $newUser = $employees.FirstChild.User[$i]  # get employee from xml
    $userID = New-YmUser -Email $newUser.Email -FullName $newUser.FullName -DepartmentName $newUser.Department  # create user for each employee

    $index = $departments.IndexOf($newUser.Department)  # get id of department group
    Add-YmGroupMembership -GroupId $groupIds[$index] -UserId $userID  # add user to group
}

New-YmMessage 'Welcome to Yammer! New groups for each department have been created and members have been added!'

$groups = Get-YmGroup

for($i=0; $i -lt $groups.Count; $i++){
    $group = $groups[$i]

    Write-Host "All users for group:" + $group.Name
    Get-YmUser -GroupId $group.Id  # list all users of a group
}

Get-YmMessage  # list all messages to see if new message was posted