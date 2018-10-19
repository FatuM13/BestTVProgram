using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestTVProgram.TagHelpers
{
    [HtmlTargetElement("channelimage")]
    public class ChannelImageTagHelper : TagHelper
    {
        private Microsoft.Extensions.FileProviders.IFileProvider _fileProvider;

        public ChannelImageTagHelper(Microsoft.Extensions.FileProviders.IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public int ChannelId { get; set; }

        public string ChannelName { get; set; }

        public override void Process(TagHelperContext context,
                                     TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;
            

            string imagePath = $"/images/TvChannel_Logos/{ChannelId.ToString()}.gif";

            if (_fileProvider.GetFileInfo("/wwwroot" + imagePath).Exists)
            {
                output.Attributes.SetAttribute("src", imagePath);
            }
            else
            {
                output.Attributes.SetAttribute("src", "/images/TvChannel_Logos/nologo.gif");
            }
            output.Attributes.SetAttribute("alt", ChannelName);
        }
    }
}
