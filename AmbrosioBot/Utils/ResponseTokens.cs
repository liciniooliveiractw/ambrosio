using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;

namespace AmbrosioBot.Utils
{
    public class ResponseTokens : TemplateManager
    {
        public static string ReplaceToken(string original, IDictionary<string,string> tokens)
        {
            string answer = string.Copy(original);
            return tokens.Select(a => answer = answer.Replace(string.Concat("{", a.Key, "}"), a.Value)).Last();
        }

    }
}
