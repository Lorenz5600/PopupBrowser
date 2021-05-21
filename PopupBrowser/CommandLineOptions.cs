using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PopupBrowser
{
    public enum PositionMode
    {
        Center,
        AtCursor,
        Recall
    }

    public enum StyleMode
    {
        Window,
        Fixed,
        Notification
    }

    public class CommandLineOptions
    {
        [Value(index: 0, Required = true, HelpText ="Url to open")]
        public string Url { get; set; }
        
        [Option('n', "Name", Default = "PopupBrowser")]
        public string Name { get; set; } = "PopupBrowser";
        
        [Option('p', "Position", Default = PositionMode.Center)]
        public PositionMode Position { get; set; } = PositionMode.Center;

        [Option('o', "Offset", Default ="0,0")]
        public string Offset { get; set; } = "0,0";

        [Option('t', "Style", Default = StyleMode.Window)]
        public StyleMode Style { get; set; } = StyleMode.Window;

        [Option('s', "Size", Default = "800,600")]
        public string Size { get; set; } = "800,600";
        
        [Option('a', "ShowAddressBar", Default = (bool)false)]
        public bool ShowAddressBar { get; set; } = false;
        
        [Option('c', "CloseAfter", Default = (int)0)]
        public int CloseAfter { get; set; } = 0;
        
        [Option('e',"EasyClose", Default = (bool)false)]
        public bool EasyClose { get; set; } = false;

        public Point SizePoint => strToPoint(Size);

        public Point OffsetPoint => strToPoint(Offset);
       

        public string ToHtmlHelpString()
        {
            return @"<h2>PopupBrowser</h2>
                     <p><strong>Usage</strong>: PopupBrowser.exe &lt;<em>Url</em>&gt; [options]</p>
                    <h3>Options</h3>
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
                    <p>Automatically close window after a given time. A value of 0 disables the timer.</p>";
        }

        Point strToPoint(string s)
        {
            var ar = s.Split(',').Select(Double.Parse).ToArray();
            return new Point((ar.Length >= 1) ? ar[0] : 0, (ar.Length >= 2) ? ar[1] : 0);
        }
    }
}
