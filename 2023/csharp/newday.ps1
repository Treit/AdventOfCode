param ($DayNumber)

robocopy /e Template "Day$DayNumber"
Set-Location "Day$DayNumber"
fastmod "___" "$DayNumber" --accept-all
$inputUri = https://adventofcode.com/2023/day/$DayNumber/input
$headers = @{ "Cookie" = "session=$env:AocSessionCookie" }
(Invoke-WebRequest -Uri $inputUri -Headers $headers).Content | Set-Content input.txt -NoNewline
dos2unix input.txt
"placeholder" | Set-Content testinput.txt -NoNewline
dos2unix testinput.txt
start Benchmark.csproj
