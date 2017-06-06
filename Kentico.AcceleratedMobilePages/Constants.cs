namespace Kentico.AcceleratedMobilePages
{
    public class Constants
    {
        // AMP markup
        public const string AMP_BOILERPLATE_CODE = "<style amp-boilerplate>body{-webkit-animation:-amp-start 8s steps(1,end) 0s 1 normal both;-moz-animation:-amp-start 8s steps(1,end) 0s 1 normal both;-ms-animation:-amp-start 8s steps(1,end) 0s 1 normal both;animation:-amp-start 8s steps(1,end) 0s 1 normal both}@-webkit-keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}@-moz-keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}@-ms-keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}@-o-keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}@keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}</style><noscript><style amp-boilerplate>body{-webkit-animation:none;-moz-animation:none;-ms-animation:none;animation:none}</style></noscript>";
        public const string AMP_CHARSET = "<meta charset=\"utf-8\">";
        public const string AMP_SCRIPT = "<script async src=\"{0}\"></script>";
        public const string AMP_VIEWPORT = "<meta name=\"viewport\" content=\"width=device-width,minimum-scale=1\">";
        public const string AMP_CUSTOM_STYLE = "<style amp-custom>{0}</style>";
        public const string AMP_CANONICAL_HTML_LINK = "<link rel=\"canonical\" href=\"{0}\" />";
        public const string AMP_AMP_HTML_LINK = "<link rel=\"amphtml\" href=\"{0}\" />";

        // AMP markup, importing custom elements
        public const string AMP_CUSTOM_ELEMENT_AMP_VIDEO = "<script async custom-element=\"amp-video\" src=\"{0}\"></script>";
        public const string AMP_CUSTOM_ELEMENT_AMP_AUDIO = "<script async custom-element=\"amp-audio\" src=\"{0}\"></script>";
        public const string AMP_CUSTOM_ELEMENT_AMP_IFRAME = "<script async custom-element=\"amp-iframe\" src=\"{0}\"></script>";
        public const string AMP_CUSTOM_ELEMENT_AMP_FORM = "<script async custom-element=\"amp-form\" src=\"{0}\"></script>";

        // Other
        public const string NEW_LINE = "\n";
        public const string P_HTTP = "http://";
        public const string P_HTTPS = "https://";

        // Filter states
        public const int DISABLED = 0;
        public const int ENABLED_AND_ACTIVE = 1;
        public const int ENABLED_AND_INACTIVE = 2;

        // Regular expressions

        // Html tag
        public const string HTML_TAG = "<html";
        public const string HTML_REPLACEMENT = "<html amp";
            
        // Head tag - required for finding the place where all the compulsory head markup will be inserted
        public const string REGEX_HEAD = "(?i)<[\\s]*?head(>|[\\s][\\s\\S]*?>)";

        // Internet Explorer conditional comments
        public const string REGEX_CONDITIONAL_COMMENTS = "<!(|--)\\[[^\\]]+\\]>[\\s\\S]*?<!\\[endif\\](|--)>";

        // Prohibited attributes
        public const string REGEX_ATTR_ONANY_SUFFIX = "(?i)[\\s]+on[\\S]+?[\\s]*?=[\\s]*?[\"'][\\s\\S]*?[\"']";
        public const string REGEX_ATTR_STYLE = "(?i)[\\s]+style[\\s]*?=[\\s]*?[\"'][\\s\\S]*?[\"']";
        public const string REGEX_ATTR_XML_ATTRIBUTES = "(?i)[\\s]+(xmlns|xml:lang|xml:base|xml:space)[\\s]*?=[\\s]*?[\"'][\\s\\S]*?[\"']";
        public const string REGEX_ATTR_IAMPANY_SUFFIX = "(?i)[\\s]+i-amp-[\\S]+?[\\s]*?=[\\s]*?[\"'][\\s\\S]*?[\"']";

        // Prohibited attribute's names
        public const string REGEX_NAME_CLASS = "(?i)[\\s]+class[\\s]*?=[\\s]*?[\"'](-amp-|i-amp-)[\\S]+?[\"']";
        public const string REGEX_NAME_ID = "(?i)[\\s]+id[\\s]*?=[\\s]*?[\"'](-amp-|i-amp-)[\\S]+?[\"']";

        // XPATH constants
        // translate() is a hack for lower-case() function which is not available in XPATH 1.0
        public static readonly string[] XPATH_RESTRICTED_ELEMENTS = {
            "//meta[@charset]",
            "//meta[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"viewport\"]",
            "//script[not(translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"application/ld+json\") and not(@async and @custom-element)]",
            "//input[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"image\" or translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"button\" or translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"password\" or translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"file\"]",
            "//link[translate(@rel,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"preconnect\" or translate(@rel,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"prerender\" or translate(@rel,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"prefetch\"]",
            "//meta[@http-equiv]",
            "//a[starts-with(@href,\"javascript:\")][not(contains(@target,\"_blank\"))]",
            "//style",
            "//base",
            "//frame",
            "//frameset",
            "//object",
            "//param",
            "//applet",
            "//embed"
        };

        // Remove attribute
        public const string XPATH_ATTR_STYLE = "//@style";
        public const string XPATH_ATTR_STYLE_NAME = "style";

        // Resolving more complex restrictions on specific elements
        public const string XPATH_FORM = "//form";
        public const string XPATH_FONT_STYLESHEET = "//link[@rel=\"stylesheet\"]";

        // Replacing HTML tags by AMP HTML equivalents
        public const string XPATH_IMG = "//img";
        public const string XPATH_IMG_REPLACEMENT = "amp-img";
        public const string XPATH_VIDEO = "//video";
        public const string XPATH_VIDEO_REPLACEMENT = "amp-video";
        public const string XPATH_AUDIO = "//audio";
        public const string XPATH_AUDIO_REPLACEMENT = "amp-audio";
        public const string XPATH_IFRAME = "//iframe";
        public const string XPATH_IFRAME_REPLACEMENT = "amp-iframe";
    }
}
