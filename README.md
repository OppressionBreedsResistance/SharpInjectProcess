﻿# SharpInjectProcess

## O co chodzi? 
SharpInjectProcess jest programem który wstrzykuje ShellCode do dowolnie wybranego programu. 
Domyslnie SharpInjectProcess wstrzykuje ShellCode uruchamiajacy kalkulator do procesu OneDrive
Program wykorzystuje reflective function loading i posiada tylko trzy importy:
- LoadLibrary
- GetProcAddress
- Sleep

Do reszty funkcji jest znajdowany wskaźnik w danej bibliotece (najczesciej kernel32.dll) i tworzony jest nowy typ zgodny z typem danej funkcji. 

Warto polaczyc ten program z programem SharpConverter i uruchomić go w pamięci programu PowerShell z zewnętrznego zasobu - wysoki poziom AV evasion gwarantowany. 

## AV Evasion
- Xorowanie shellcode
- Xorowanie nazw WinApi calli
- Reflective Function Loading

## Sandbox Evasion
- Wykorzystanie non-emulated API Calla "VirtualAllocExNuma"
- Sprawdzenie czy sleepy są fast-forwardowane
