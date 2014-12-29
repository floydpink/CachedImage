[CachedImage](http://floydpink.github.io/CachedImage/) [![NuGet version](https://badge.fury.io/nu/CachedImage.png)](http://badge.fury.io/nu/CachedImage) [![Build status](https://ci.appveyor.com/api/projects/status/6tb8p301yio5fmh4)](https://ci.appveyor.com/project/floydpink/cachedimage)
===========

<a href="http://floydpink.github.io/CachedImage/"><img src="http://floydpink.github.io/CachedImage/images/logo.png" alt="logo" width="300px" /></a>

A WPF control that wraps the Image control to enable file-system based caching.

### Background
If we use the native WPF `Image` control for displaying images over the HTTP protocol (by setting the `Source` to an http url), the image will be downloaded from the server every time the control is loaded. 

In its `Dedicated` mode (see `Cache Mode` below), the `Image` control present in this `CachedImage` library, wraps the native `Image` control to add a local file-system based caching capability. This control creates a local copy of the image on the first time an image is downloaded; to a configurable cache folder (defaults to `<current-user/appdata/roaming>\AppName\Cache`). All the subsequent loads of the control (or the page, window or app that contains the control), will display the image from the local file-system and will not download it from the server.

In its `WinINet` mode, the `Image` control uses the Temporary Internet Files directory that IE uses for the cache.

### Cache Mode
We provide two cache mode: `WinINet` mode and `Dedicated` mode.
* `WinINet`: This is the default mode and it takes advantage of `BitmapImage.UriCachePolicy` property and uses the Temporary Internet Files directory of IE to store cached images. The image control will have the same cache policy of IE.
* `Dedicated`: Another url-based cache implementation. You can set your own cache directory. The cache will never expire unless you delete the cache folder manually.

### Usage
1. Install the NuGet package named `CachedImage` on the WPF project 
2. Add a namespace reference to the `CachedImage` assembly on the Window/Usercontrol `xmlns:cachedImage="clr-namespace:CachedImage;assembly=CachedImage"` as in the example `Window` below:
  ```xml
  <Window x:Class="MyWindow1"
          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
          xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
          xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
          xmlns:cachedImage="clr-namespace:CachedImage;assembly=CachedImage">
  
  </Window>
  ```
3. Use the control and set or bind the `ImageUrl` attribute:
  ```xml
  
      <cachedImage:Image ImageUrl="{Binding LargeImageUrl}">  </cachedImage:Image>
  ```
4. As it is only a wrapper, all the XAML elements that could be used with the `Image` control are valid here as well:
  ```xml
  
    <cachedImage:Image ImageUrl="{Binding LargeImageUrl}">
        <Image.ToolTip>This image gets cached to the file-system the first time it is downloaded</Image.ToolTip>
    </cachedImage:Image>
  ```
5. To change cache mode, set FileCache.AppCacheMode like this:
  ```csharp
  
    CachedImage.FileCache.AppCacheMode = CachedImage.FileCache.CacheMode.Dedicated; // The default mode is WinINet
  ```
6. To change the cache folder location of the dedicated cache mode, set the static string property named `AppCacheDirectory` of the `FileCache` class like this:
  ```csharp
  
    CachedImage.FileCache.AppCacheDirectory = string.format("{0}\\MyCustomCacheFolder\\",
                                  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
  ```
6. Please note that the dedicated cache mode does not consider `Cache-Control` or `Expires` headers. Unless the cache folder (or specific files in it) gets deleted, the control will not fetch the file again from the server. The application could let the end-user empty the cache folder as done in the [flickr downloadr](https://github.com/flickr-downloadr/flickr-downloadr) application that uses this control.

### Thanks
All of the code in this library is from the answers on a Stack Overflow question:

[How do I cache images on the client for a WPF application?](http://stackoverflow.com/questions/1878060/how-do-i-cache-images-on-the-client-for-a-wpf-application). 

Thanks to:

1. [Simon Hartcher](http://stackoverflow.com/users/459159/simon-hartcher), who answered his own question [with the solution](http://stackoverflow.com/questions/1878060/how-do-i-cache-images-on-the-client-for-a-wpf-application/1893173#1893173)

2. [Jeroen van Langen](http://stackoverflow.com/users/641271/jeroen-van-langen) for [the wonderful refacoring](http://stackoverflow.com/questions/1878060/how-do-i-cache-images-on-the-client-for-a-wpf-application/5175424#5175424) of Simon's solution

3. [Ivan Leonenko](http://stackoverflow.com/users/367287/ivan-leonenko) for [the tweaks to make the control bindable](http://stackoverflow.com/questions/1878060/how-do-i-cache-images-on-the-client-for-a-wpf-application/12638859#12638859).

### License

[MIT License](https://raw.github.com/floydpink/CachedImage/master/LICENSE)
