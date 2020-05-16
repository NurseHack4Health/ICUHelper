$url = "https://api.powerbi.com/v1.0/myorg/groups/{GROUPID}/reports/{REPORTID}/GenerateToken"

$body = "{ 'accessLevel': 'View' }"

$response = Invoke-PowerBIRestMethod -Url $url -Body $body -Method Post

$json = $response | ConvertFrom-Json
$token = $json.token

$server = "serverName"
$Database = "databaseName"

$Connection = New-Object System.Data.SQLClient.SQLConnection
$Connection.ConnectionString = "server='$Server';database='$Database';User ID=USER;Password=PASS;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
$Connection.Open()
$Command = New-Object System.Data.SQLClient.SQLCommand
$Command.Connection = $Connection
$sql ="update powerbi_token set token = '$token'" 
$Command.CommandText = $sql
$Command.ExecuteReader()
$Connection.Close()

<#	Use this Block instead to initialize token

	$sql ="if not exists (select 1 from [powerbi_token] where token = '$token' ) insert powerbi_token select '$token'" 
	$Command.CommandText = $sql
	$Command.ExecuteReader()
	$Connection.Close()
#>