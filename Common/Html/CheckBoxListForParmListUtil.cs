using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Models;
namespace Common
{
    public static class CheckBoxListForParmListUtil
    {
        public static MvcHtmlString GenerateHtml(string name, Collection<Models.Parameter> codes, RepeatDirection repeatDirection, string type, object stateValue,int rowNumber=3)
        {
            TagBuilder table = new TagBuilder("table");
            int i = 0;
            bool isCheckBox = type == "checkbox";
            if (repeatDirection == RepeatDirection.Horizontal)
            {
                TagBuilder tr = new TagBuilder("tr");
                foreach (var code in codes)
                {

                    if (i > 0 && i % rowNumber == 0)
                    {
                        tr = new TagBuilder("tr");
                    }
                    i++;
                    string id = string.Format("{0}_{1}", name, i);
                    TagBuilder td = new TagBuilder("td");

                    bool isChecked = false;
                    if (isCheckBox)
                    {
                        //IEnumerable<string> currentValues = stateValue as IEnumerable<string>;
                        //isChecked = (null != currentValues && currentValues.Contains(code.ParKey));
                        if (stateValue != null)
                        {
                            string currentValues = stateValue.ToString();
                            isChecked = (null != currentValues && currentValues.Contains(code.ParKey));
                        }
                    }
                    else
                    {
                        if (stateValue != null)
                        {
                            string currentValue = stateValue as string;
                            isChecked = (null != currentValue && code.ParKey == currentValue);
                        }
                    }

                    td.InnerHtml = GenerateRadioHtml(name, id, code.ParValue, code.ParKey, isChecked, type);
                    tr.InnerHtml += td.ToString();
                    if (i > 0 && i % rowNumber == 0 || i == codes.Count)
                    {
                        table.InnerHtml += tr.ToString();
                    }
                }

            }
            else
            {
                foreach (var code in codes)
                {
                    TagBuilder tr = new TagBuilder("tr");
                    i++;
                    string id = string.Format("{0}_{1}", name, i);
                    TagBuilder td = new TagBuilder("td");

                    bool isChecked = false;
                    if (isCheckBox)
                    {
                        //IEnumerable<string> currentValues = stateValue as IEnumerable<string>;
                        //isChecked = (null != currentValues && currentValues.Contains(code.ParKey));
                        string currentValues = stateValue.ToString();
                        isChecked = (null != currentValues && currentValues.Contains(code.ParKey));
                    }
                    else
                    {
                        string currentValue = stateValue as string;
                        isChecked = (null != currentValue && code.ParKey == currentValue);
                    }

                    td.InnerHtml = GenerateRadioHtml(name, id, code.ParValue, code.ParKey, isChecked, type);
                    tr.InnerHtml = td.ToString();
                    table.InnerHtml += tr.ToString();
                }
            }
            return new MvcHtmlString(table.ToString());
        }

        private static string GenerateRadioHtml(string name, string id, string labelText, string value, bool isChecked, string type)
        {
            //var builder = new TagBuilder("input");
            //builder.MergeAttribute("type", "submit");
            //builder.MergeAttribute("value", value);
            //builder.ToString(TagRenderMode.SelfClosing);


            StringBuilder sb = new StringBuilder();

            TagBuilder label = new TagBuilder("label");
            //label.MergeAttribute("for", id);
            label.SetInnerText(labelText);

            TagBuilder input = new TagBuilder("input");
            input.GenerateId(id);
            input.MergeAttribute("name", name);
            input.MergeAttribute("type", type);
            input.MergeAttribute("value", value);
            if (isChecked)
            {
                input.MergeAttribute("checked", "checked");
            }
            string tempInput = input.ToString(TagRenderMode.SelfClosing);
            string tempLabel = label.ToString();

            int starPoint = tempLabel.IndexOf(">");
            string leftString = tempLabel.Substring(0, starPoint + 1);
            string rightString = tempLabel.Substring(starPoint + 1, tempLabel.Length - starPoint - 1);
            //sb.AppendLine(input.ToString(TagRenderMode.SelfClosing));
            //sb.AppendLine(label.ToString());
            sb.AppendLine(leftString + tempInput + rightString);

            return sb.ToString();
        }
    }
}
