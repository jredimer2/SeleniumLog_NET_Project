param($installPath, $toolsPath, $package, $project)
$file1 = $project.ProjectItems.Item("SeleniumLog.config")
 
$copyToOutput1 = $file1.Properties.Item("CopyToOutputDirectory")
$copyToOutput1.Value = 1