param ($DayNumber)

robocopy /e Template "Day$DayNumber"
Set-Location "Day$DayNumber"
fastmod "___" "$DayNumber" --accept-all
(Invoke-WebRequest -Uri https://adventofcode.com/2023/day/$DayNumber/input -Headers @{ "Cookie" = "session=$env:AocSessionCookie" }).Content | Set-Content input.txt -NoNewline

start Benchmark.csproj
