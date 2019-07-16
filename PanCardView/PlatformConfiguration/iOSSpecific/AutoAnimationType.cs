using System;

namespace PanCardView.PlatformConfiguration.iOSSpecific
{
    [Flags]
    public enum AutoAnimationType
    {
        FromProcessors = 0,
        Flip = 1,
        Curl = 2,
        FlipAndCurl = Flip | Curl
    }
}
