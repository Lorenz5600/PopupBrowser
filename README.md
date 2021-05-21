<img align="left" width="64" height="64" src="./PopupBrowser.png">

# PopupBrowser
A small commandline controlled WPF app with a minimalistic browser (WebView2) to quickly display web content:

 * Automatically place some monitoring dashboards on your screen(s) on system start
 * Call it from other apps with some context, e.g. configure your client management app to show content from your corporate wiki or web-based
 Active Directory querying tool with a single click.

Why not use one of the usual browsers for this purpose, you ask? I did this before, but the usual browsers
 * take longer to load
 * cause trouble to control size and position of it's window
 * have a titlebar, an addressbar, menu and other stuff you might not want
 * lack the ability to act as a notification/tooltip window that quickly close on hitting ESC, clicking outside the window or by a timer

## Usage
PopupBrowser.exe <*Url*> \[options\]
```
PopupBrowser.exe www.github.com --Size 400,300 --Style Fixed --Position AtCursor --Offset 20,-20
```
## Options
<p><strong>-n /--Name &lt;Name&gt; (Default 'PopupBrowser')</strong></p>
<p>Sets the instance name used to store window properties and to prevent multiple instances running</p>
<p><strong>-p / --Position &lt;Value&gt; (Default 'Center')</strong></p>
<p>Determines screen position:</p>
<ul>
<li>Center: Center on active screen</li>
<li>AtCursor: Topleft corner placed at mouse position</li>
<li>Recall: Restore window's position and size from last session</li>
</ul>
<p><strong>-o / --Offset &lt;x,y&gt; (Default '0,0')</strong></p>
<p>Offsets the window position</p>
<p><strong>-s / --Size &lt;width,height&gt; (Default '800,600')</strong></p>
<p>Determines the window size</p>
<p><strong>-t / --Style &lt;Value&gt; (Default 'Window')</strong></p>
<p>Determines window style:</p>
<ul>
<li>Window: resizable window</li>
<li>Fixed: non-resizable window</li>
<li>Notification: border- and titleless, non-resizable window</li>
</ul>
<p><strong>-a / --ShowAddressBar</strong></p>
<p>Show browser address bar</p>
<p><strong>-e / --EasyClose (Default: On)</strong></p>
<p>Close Window by pressing ESC or clicking outside the window</p>
<p><strong>-c / --CloseAfter &lt;Seconds&gt; (Default '0')</strong></p>
<p>Automatically close window after a given time. A value of 0 disables the timer.</p>