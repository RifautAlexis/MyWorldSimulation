# Colony.Engine

## Scripts
### build.ps1
```powershell
# Build script for Colony.Engine
# This script compiles the project and generates the necessary artifacts.

# Release :
.\build.ps1 -Configuration Release

# Skip tests :
.\build.ps1 -SkipTests

# Clean :
.\build.ps1 -Clean

# Clean and Release :
.\build.ps1 -Configuration Release -Clean
```