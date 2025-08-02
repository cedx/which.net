#define executable "Belin.Which.exe"
#define publisher "Belin.io"
#define name "Which for .NET"
#define version "1.0.0-rc3"

[Setup]
AppCopyright = © Cédric Belin
AppId = {{77278EEC-2915-4E3E-BF29-9385C8E9F24F}
AppName = {#name}
AppPublisher = {#publisher}
AppPublisherURL = https://belin.io
AppVersion = {#version}
ArchitecturesAllowed = x64compatible
ArchitecturesInstallIn64BitMode = x64compatible
ChangesEnvironment = yes
DefaultDirName = {autopf}\{#publisher}\Which
DisableProgramGroupPage = yes
LicenseFile = License.md
OutputBaseFilename = {#name} {#version}
OutputDir = var
PrivilegesRequired = lowest
PrivilegesRequiredOverridesAllowed = dialog
SetupIconFile = src\Program.ico
SolidCompression = yes
UninstallDisplayIcon = {app}\lib\{#executable}
WizardStyle = modern

[Files]
Source: "*.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "lib\*"; DestDir: "{app}\lib"; Excludes: "*.pdb"; Flags: ignoreversion recursesubdirs

[Tasks]
Name: addProgramToPath; Description: "Add the program to the PATH environment variable"

[Code]
{ Determines whether the end of the specified value matches the given tail. }
function EndsWith(Value, Tail: string): boolean;
begin
	Result := SameStr(Tail, Copy(Value, Length(Value) + 1 - Length(Tail), Length(Tail)));
end;

{ Gets the registry keys corresponding to the environment variables. }
procedure GetRegistryKeys(var RootKey: integer; var EnvironmentKey: string);
begin
	if IsAdminInstallMode() then begin
		RootKey := HKEY_LOCAL_MACHINE;
		EnvironmentKey := 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment';
	end
	else begin
		RootKey := HKEY_CURRENT_USER;
		EnvironmentKey := 'Environment';
	end;
end;

{ Adds the specified path to the PATH environment variable. }
procedure AddPathToEnvironment(Path: string);
var
	EnvironmentKey, Paths: string;
	RootKey: integer;
begin
	GetRegistryKeys(RootKey, EnvironmentKey);
	if not RegQueryStringValue(RootKey, EnvironmentKey, 'Path', Paths) then Paths := '';
	if Pos(';' + Lowercase(Path) + ';', ';' + Lowercase(Paths) + ';') > 0 then Exit;

	if Length(Paths) = 0 then Paths := Path
	else if EndsWith(Paths, ';') then Paths := Paths + Path
	else Paths := Paths + ';' + Path;

	RegWriteStringValue(RootKey, EnvironmentKey, 'Path', Paths)
end;

{ Procedure invoked when the current step of the wizard changes. }
procedure CurStepChanged(Step: TSetupStep);
begin
	if (Step = ssPostInstall) and WizardIsTaskSelected('addProgramToPath') then AddPathToEnvironment(ExpandConstant('{app}\lib'));
end;
