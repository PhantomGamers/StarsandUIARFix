# StarsandUIARFix

BepInEx mod for Starsand to fix UI with aspect ratios other than 16:9

## Screenshots

### Vanilla

![image](https://user-images.githubusercontent.com/844685/140520391-0b9b22e7-b3f7-4995-bdf7-f744a52ec7fd.png)

### With the mod

![Starsand_UKewinOFlr](https://user-images.githubusercontent.com/844685/141055738-8a1fcb38-3adc-4399-9131-c244a173c17a.png)

## Installation

### Mod Manager (Recommended)

Install either [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager) or [r2modman](https://thunderstore.io/package/ebkr/r2modman/) and then install [StarsandUIARFix from Thunderstore](https://starsand.thunderstore.io/package/PhantomGamers/StarsandUIARFix/)

### Manual Installation

Follow the instructions from the [BepInEx Docs](https://docs.bepinex.dev/articles/user_guide/installation/index.html) to install BepInEx and then extract [the latest release](https://github.com/PhantomGamers/StarsandUIARFix/releases/latest) of the mod to your `BepInEx\plugins\` directory.

## Configuration

After launching the game with the mod installed, edit `BepInEx\config\StarsandUIARFix.cfg` with your text editor of choice.

## Support

For support, or to discuss and discover other mods for Starsand, check out the [Starsand Modding Discord](https://discord.gg/ZYVpC6uyY7)

## Changelog

1.1.0:

- Switched fix method to one which should be resolution agnostic. Disabled the fix on the main menu by default as this should not be needed in most cases.

1.0.7:

- Added option to automatically calculate ideal scale factor, enabled by default.

1.0.1:

- Added option to change scale factor of UI instead of stretching from 16:9, enabled by default.

1.0.0:

- Initial release
