# BERT Scouting Application 2024

In Project settings, Android, Options, turn off Fast Deployment. Causes errors.

## Notes

After deploying to Kindle, long-click on app icon and select "App Info". Select "Permissions" and turn on "Storage".

For any settings, use this format to have different values by platform:
	FontSize="{OnPlatform WinUI=Small, Android=Large}"
	WidthRequest="{OnPlatform WinUI=100, Android=160}"

To get the database from the Kindle to Windows, open the Android Adp Command prompt and use something like:
	adb pull /sdcard/Documents/BertScout2024.db3 C:\Temp
This may also work in a batch file. Use this to find the path:
	which adb

## Airtable

Create a new Airtable base for the new year, e.g. BertScout2024.
Rename default table to TeamMatch.
Start adding fields for the year with these first:
	Id (rename from Name) - Primary field - Number, 1 (0), no thousands sep, no neg
	(delete other generated fields)
	Uuid - Single line text
	TeamNumber - Number, 1 (0), no thousands sep, no neg
	MatchNumber - Number, 1 (0), no thousands sep, no neg
	ScoutName - Single line text
	Comments - Single line text
	... all fields for this year ...
Click on Help, API Documentation.
	In AirtableService.cs, update the constants:
		AIRTABLE_BASE = The ID of this base
		AIRTABLE_TABLE = The id for TeamMatch
	Click on the link in there to "/create/tokens".
	Create a new Personal Access Token for the new Base and Table.
	Add the following scopes:
		data.records:read
		data.records:write
		schema.bases:read
	Copy the value right away (it is not available after creation!!!)
	Paste into AIRTABLE_TOKEN (and save elsewhere, such as emailing it to team members)
