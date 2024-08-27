# MeBatifolage - scanning

 > Huh, these barcode reader can scan pretty far away ! You could do some laser tag with this professional equipement ...

 Maui mobile application used on Zebra TC27 barcode scanner to shoot people and have fun while doing so :)

<p float="left">
  <img src="/images/preview.PNG" width="100" />
  <img src="/images/live.gif" width="100" /> 
</p>

## Description

Register an Id by scanning it, shoot some other player QR code, score !

- Real time score system
- Real time player state
- Connection lifecycle handling (connection, deconnection, ...)

## Getting Started

### Dependencies

- Created with Visual studio Community 2022 (64 bits) Version 17.8.4
- Require *.NET Desktop development* workload installed
- Require *.NET Maui* workload installed

### Executing program

- Start the project `MeBatifolage.Api` and expose it on the network.
- Adapt the url of the api in `MeBatifolage.App.MauiProgram.cs` :
```cs
...
builder.Services.AddSingleton<GameHubClient>((provider) => new GameHubClient("https://recette-wms.3magroup.com/game"));
...
```
- Start `MeBatifolage.App` on a Zebra device that allow scanning.

## Acknowledgments

Use :
* [Fontawesome](https://fontawesome.com/) icons
* [PropertyChanged.Fody](https://www.nuget.org/packages/PropertyChanged.Fody) for boilerplate
* [Plugin.Maui.Audio](https://github.com/jfversluis/Plugin.Maui.Audio) to play audio
