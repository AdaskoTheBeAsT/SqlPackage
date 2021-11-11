# SqlPackage

Project for releasing SqlPackage in nuget.

## Intention

Ckae.SqlPackage right now is not platform agnostic. That project is created to prepare for the basis of Cake.SqlPackage addin.

[Cake.SqlPackage Issue Discussion](https://github.com/cake-contrib/Cake.SqlPackage/issues/28)
Unfortunatelly initial idea published under [Feedback Azure](https://feedback.azure.com/d365community/idea/58cd97f3-7025-ec11-b6e6-000d3a4f0da0) disappeared.
I found original idea under different link but broken [Feedback Azure 2](https://feedback.azure.com/d365community/idea/58cd97f3-7025-ec11-b6e6-000d3a4f0da0).

Anyway for now Microsoft do not plan to release official nuget package and global tool is not available. [SQL Docs Issue](https://github.com/MicrosoftDocs/sql-docs/issues/4037).

Idea was to grab zips of tool, repack to nuget to unblock further multiplatform development of Cake.SqlPackage.