# PowerShell script - No longer needed for WebAssembly mode!
# Since we're using InteractiveWebAssembly mode, all static files are served from Srs.Client project

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Note: Static Files Configuration" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Your application is using InteractiveWebAssembly mode." -ForegroundColor Green
Write-Host "All static files (Template, CSS, JS, images) are served directly from:" -ForegroundColor White
Write-Host "  Srs\Srs.Client\wwwroot\" -ForegroundColor Cyan
Write-Host ""
Write-Host "No file copying is needed!" -ForegroundColor Green
Write-Host ""
Write-Host "If you see missing assets (404 errors):" -ForegroundColor Yellow
Write-Host "1. Make sure files exist in Srs\Srs.Client\wwwroot\" -ForegroundColor White
Write-Host "2. Restart your application (Shift+F5, then F5)" -ForegroundColor White
Write-Host "3. Clear browser cache (Ctrl+F5)" -ForegroundColor White
Write-Host ""
Write-Host "Current static files location:" -ForegroundColor Yellow
Write-Host "  ? Template folder: Srs\Srs.Client\wwwroot\Template\" -ForegroundColor Green
Write-Host "  ? CSS files: Srs\Srs.Client\wwwroot\Template\css\" -ForegroundColor Green
Write-Host "  ? Images: Srs\Srs.Client\wwwroot\Template\img\" -ForegroundColor Green
Write-Host "  ? Fonts: Srs\Srs.Client\wwwroot\Template\font\" -ForegroundColor Green
Write-Host "  ? AAST Logo: Srs\Srs.Client\wwwroot\Template\img\AASTLogoTran.png" -ForegroundColor Green
Write-Host ""

# Verify AAST Logo exists
$logoPath = "Srs\Srs.Client\wwwroot\Template\img\AASTLogoTran.png"
if (Test-Path $logoPath) {
    Write-Host "? AAST Logo verified!" -ForegroundColor Green
} else {
    Write-Host "? Warning: AAST Logo not found at:" -ForegroundColor Red
    Write-Host "  $logoPath" -ForegroundColor Red
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
