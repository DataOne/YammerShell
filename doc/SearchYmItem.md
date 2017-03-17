# Search-YmItem

Search messages, users, topics and groups.

## Syntax

```PowerShell
Search-YmItem [-SearchItem] <String> [-Page <int>] [-Limit <int>]
```

## Returns
> YammerShell.YammerObjects.YammerSearchResult

## Parameters

Parameter | Type | Required | Description
----------|------|----------|------------
SearchItem | String | true  | The string to search items with.
Page      | int  | false    | The number of the result page. Default is 1.
Limit     | int  | false    | The maximum of result items for each result page. Between 1 and 20. Default is 20.


## Examples

### Example 1

```PowerShell
PS:> Search-YmItem 'welcome'
```
Searches messages, users, topics and groups for the word _welcome_. Returns an object containing message, user, topic, files and group items.

### Example 2

```PowerShell
PS:> Search-YmItem 'welcome' -Page 1 -Limit 3
```
Searches yammer for the word _welcome_. Returns the first three results from result page one.