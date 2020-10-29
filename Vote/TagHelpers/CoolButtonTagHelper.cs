using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Vote.TagHelpers
{
    public class CoolButtonTagHelper : TagHelper
    {
        public string Value { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";    // Replaces <link> with <a> tag
            output.Attributes.Add("value", Value);
            output.Attributes.Add("type", "submit");
            output.Attributes.Add("style", "background-color: transparent; border: 1px solid #000000");
        }
    }
}