#!/bin/bash
# Post-create setup script for GitHub Codespaces

set -e

echo "Building SeeSharpShell..."
dotnet build --configuration Release

echo ""
echo "========================================="
echo "  SeeSharpShell is ready!"
echo "========================================="
echo ""
echo "To run the shell interactively, use:"
echo "  ./your_program.sh"
echo ""
echo "Or run directly with dotnet:"
echo "  dotnet run"
echo ""
echo "Type 'help' to see available commands."
echo "========================================="
