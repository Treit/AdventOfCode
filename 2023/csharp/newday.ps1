param ($DayNumber)

robocopy /e Template "Day$DayNumber"
Set-Location "Day$DayNumber"
fastmod "___" "$DayNumber" --accept-all
start Benchmark.csproj
